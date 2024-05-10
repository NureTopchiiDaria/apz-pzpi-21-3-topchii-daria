using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces.RoomInterfaces;
using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.DataClasses;
using Core.Enums;
using Core.Models;
using Core.Models.RoomModels;
using Core.Models.UserModels;
using DAL.Abstractions.Interfaces;

namespace BLL.Services.RoomServices
{
    internal class UserRoomService : IUserRoomService
    {
        private readonly IGenericStorageWorker<UserRoomModel> storage;

        private readonly IRoomService roomService;

        private readonly IUserService userService;

        private readonly IRoomValidationService validationService;

        private readonly ITransactionsWorker transactionsWorker;

        public UserRoomService(
            IRoomService roomService,
            IUserService userService,
            IRoomValidationService validationService,
            ITransactionsWorker transactionsWorker,
            IGenericStorageWorker<UserRoomModel> storage)
        {
            this.transactionsWorker = transactionsWorker;
            this.roomService = roomService;
            this.userService = userService;
            this.validationService = validationService;
            this.storage = storage;
        }

        public async Task<ExceptionalResult> CreateRoomForUser(UserModel user, RoomCreateModel createModel)
        {
            return await this.CreateRoom(user, createModel);
        }

        public async Task<ExceptionalResult> DeleteRoomByUser(UserModel user, int roomId)
        {
            var room = await GetRoomAndCheckAdmin(user, roomId);
            if (!room.IsSuccess)
            {
                return room;
            }

            return await this.roomService.Delete(room.Value.Id);
        }

        public async Task<ExceptionalResult> DeleteUserFromRoom(UserModel user, RoomModel room, UserModel userToDelete)
        {
            // Проверяем, есть ли переданный пользователь в комнате
            var userRoom = await this.storage.GetUserRoom(user, room);
            if (userRoom is null)
            {
                return new ExceptionalResult(false, $"User with id {user.Id} does not belong to room with id {room.Id}");
            }

            // Проверяем, является ли пользователь, выполняющий действие, админом
            if (!userRoom.IsAdmin)
            {
                return new ExceptionalResult(false, $"User with id {user.Id} does not have rights to perform this action");
            }

            // Проверяем, есть ли пользователь, которого нужно удалить, в комнате
            var userToDeleteRoom = await this.storage.GetUserRoom(userToDelete, room);
            if (userToDeleteRoom is null)
            {
                return new ExceptionalResult(false, $"User with id {userToDelete.Id} does not belong to room with id {room.Id}");
            }

            // Если пользователь, которого нужно удалить, является последним администратором комнаты, возвращаем ошибку
            if (userToDeleteRoom.IsAdmin && await this.storage.IsLastAdminInRoom(userToDelete, room))
            {
                return new ExceptionalResult(false, "Room must have at least one admin. Make someone else admin first.");
            }

            // Удаляем пользователя из комнаты
            await this.storage.DeleteUserFromRoom(userToDelete, room);

            // Создаем запись в журнале аудита
            var record = new CreateAuditRecordModel()
            {
                ActionType = ActionType.DeleteUserFromRoom,
                Actor = user,
                Room = room,
                UserUnderAction = userToDelete,
            };
            var auditResult = await this.auditService.CreateAuditRecord(record);
            if (!auditResult.IsSuccess)
            {
                // В случае неудачи создания записи в журнале аудита, возвращаем соответствующий результат
                return auditResult;
            }

            // Возвращаем успешный результат
            return new ExceptionalResult();
        }

        public async Task<ExceptionalResult> DeleteUserAndRooms(UserModel user)
        {
            // Удаление всех комнат пользователя
            var rooms = user.Rooms.ToList();
            foreach (var room in rooms)
            {
                if (room.Users.Count == 1)
                {
                    // Пользователь последний в комнате, удаляем комнату
                    await roomService.Delete(room.Id);
                }
                else
                {
                    // Удаляем пользователя из комнаты
                    room.Users.Remove(user);
                    await roomService.UpdateRoomUsers(room);
                }
            }

            // Удаляем пользователя
            return await roomService.Delete(user.Id);
        }

        public async Task<ExceptionalResult> AddUserToRoom(string email, RoomModel room, UserModel actor)
        {
            var user = (await userService.GetActiveUsers(x => x.Email == email)).FirstOrDefault();
            if (user == null)
            {
                return new ExceptionalResult(false, $"User with email {email} does not exist");
            }

            // Добавляем пользователя в комнату
            room.Users.Add(user);
            await roomService.UpdateRoomUsers(room);

            // Предполагается, что AddRoleForUserAndRoom устанавливает админство, если пользователь первый в комнате
            return await roomService.AddRoleForUserAndRoom(user, room, RoleType.Member.ToString(), false);
        }

        public async Task<ExceptionalResult> UpdateRoomForUser(UserModel user, RoomEditModel editModel)
        {
            var roomResult = await GetRoomAndCheckAdmin(user, editModel.Id);
            if (!roomResult.IsSuccess)
            {
                return roomResult;
            }

            var room = roomResult.Value;
            var validationResult = this.validationService.ValidateUpdateModel(editModel);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var updateResult = await this.roomService.Update(editModel);
            if (!updateResult.IsSuccess)
            {
                return updateResult;
            }

            return new ExceptionalResult();
        }

        private async Task<ExceptionalResult> CreateRoom(UserModel user, RoomCreateModel createModel)
        {
            var validationResult = this.validationService.ValidateCreateModel(createModel);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            createModel.Users = new List<UserModel>() { user };
            var roomResult = await this.roomService.Create(createModel);
            if (!roomResult.IsSuccess)
            {
                return roomResult;
            }

            var room = roomResult.Value;

            // Предполагается, что первый пользователь в комнате становится админом
            room.Users.First().IsAdmin = true;
            await this.roomService.UpdateRoomUsers(room);

            return new ExceptionalResult();
        }

       /* private async Task<ExceptionalResult> GetRoomAndCheckAdmin(UserModel user, int roomId)
        {
            var room = await this.roomService.GetRoomById(roomId);
            if (room == null)
            {
                return new ExceptionalResult(false, $"Room with id {roomId} does not exist");
            }

            var userRoom = room.Users.FirstOrDefault(u => u.Id == user.Id);
            if (userRoom == null || !userRoom.IsAdmin)
            {
                return new ExceptionalResult(false, $"User with id {user.Id} is not an admin in room with id {roomId}");
            }

            return new ExceptionalResult<RoomModel>(room);
        }
        */
    }
}
