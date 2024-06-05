using System.Linq.Expressions;
using BLL.Abstractions.Interfaces.RoomInterfaces;
using Core.Models;
using Core.Models.RoomModels; // Ensure this is the correct namespace
using DAL.Abstractions.Interfaces;

namespace BLL.Services.RoomServices
{
    public class UserRoomsService : IUserRoomsService
    {
        private readonly IGenericStorageWorker<UserRoomModel> userRoomStorage;
        private readonly IGenericStorageWorker<RoomModel> roomStorage;

        public UserRoomsService(IGenericStorageWorker<UserRoomModel> userRoomStorage, IGenericStorageWorker<RoomModel> roomStorage)
        {
            this.userRoomStorage = userRoomStorage;
            this.roomStorage = roomStorage;
        }

        public async Task AddAdminUserToRoom(Guid userId, Guid roomId)
        {
            var userRoom = new UserRoomModel
            {
                UserId = userId,
                RoomId = roomId,
                IsAdmin = true,
                JoinDate = DateTime.UtcNow,
            };

            await this.userRoomStorage.Create(userRoom);
        }

        public async Task AddUserToRoom(UserRoomCreateModel userRoomCreate)
        {
            var userRoom = new UserRoomModel
            {
                UserId = userRoomCreate.UserId,
                RoomId = userRoomCreate.RoomId,
                IsAdmin = false,
                JoinDate = DateTime.UtcNow,
            };

            await this.userRoomStorage.Create(userRoom);
        }

        public async Task<UserRoomModel> GetUserRoomByRoomId(Guid roomId)
        {
            return await this.userRoomStorage.GetByCondition(ur => ur.RoomId == roomId && ur.IsAdmin);
        }

        public async Task<UserRoomModel> GetAdminInfoByRoomId(Guid roomId)
        {
            return await this.userRoomStorage.GetByCondition(ur => ur.RoomId == roomId && ur.IsAdmin);
        }

        public async Task<IEnumerable<UserRoomModel>> GetRoomsForUser(Guid userId)
        {
            var userRooms = await this.userRoomStorage.GetByConditions(new Expression<Func<UserRoomModel, bool>>[] { ur => ur.UserId == userId });

            foreach (var userRoom in userRooms)
            {
                userRoom.Room = await this.roomStorage.GetById(userRoom.RoomId);
            }

            return userRooms;
        }
    }
}
