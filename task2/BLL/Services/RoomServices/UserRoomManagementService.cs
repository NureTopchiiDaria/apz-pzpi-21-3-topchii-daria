using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Abstractions.Interfaces.RoomInterfaces;
using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.DataClasses;
using Core.Models;
using Core.Models.RoomModels;
using Core.Models.UserModels;
using DAL.Abstractions.Interfaces;

namespace BLL.Services.RoomServices
{
    internal class UserRoomManagementService : IUserRoomManagementService
    {
        private readonly IGenericStorageWorker<UserRoomModel> userRoomStorage;
        private readonly IRoomService roomService;
        private readonly IUserService userService;

        public UserRoomManagementService(
            IGenericStorageWorker<UserRoomModel> userRoomStorage,
            IRoomService roomService,
            IUserService userService)
        {
            this.userRoomStorage = userRoomStorage;
            this.roomService = roomService;
            this.userService = userService;
        }

        public async Task<ExceptionalResult> AddUserToRoom(Guid userId, Guid roomId, bool isAdmin = false)
        {
            var user = await this.userService.GetUserById(userId);
            var room = await this.roomService.GetRoomById(roomId);

            if (user == null || room == null)
            {
                return new ExceptionalResult(false, "User or Room does not exist.");
            }

            var userRoom = new UserRoomModel
            {
                UserId = userId,
                RoomId = roomId,
                IsAdmin = isAdmin,
            };

            await this.userRoomStorage.Create(userRoom);

            return new ExceptionalResult();
        }

        public async Task<ExceptionalResult> RemoveUserFromRoom(Guid userId, Guid roomId)
        {
            var userRoom = await this.userRoomStorage.GetByCondition(ur => ur.UserId == userId && ur.RoomId == roomId);

            if (userRoom == null)
            {
                return new ExceptionalResult(false, "User is not part of this room.");
            }

            await this.userRoomStorage.Delete(userRoom);

            return new ExceptionalResult();
        }

        public async Task<ExceptionalResult> MakeUserAdminInRoom(Guid userId, Guid roomId)
        {
            var userRoom = await this.userRoomStorage.GetByCondition(ur => ur.UserId == userId && ur.RoomId == roomId);

            if (userRoom == null)
            {
                return new ExceptionalResult(false, "User is not part of this room.");
            }

            userRoom.IsAdmin = true;

            await this.userRoomStorage.Update(userRoom);

            return new ExceptionalResult();
        }

        public async Task<ExceptionalResult> RevokeAdminStatus(Guid userId, Guid roomId)
        {
            var userRoom = await this.userRoomStorage.GetByCondition(ur => ur.UserId == userId && ur.RoomId == roomId);

            if (userRoom == null)
            {
                return new ExceptionalResult(false, "User is not part of this room.");
            }

            userRoom.IsAdmin = false;

            await this.userRoomStorage.Update(userRoom);

            return new ExceptionalResult();
        }
    }
}
