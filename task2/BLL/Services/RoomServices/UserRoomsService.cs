using BLL.Abstractions.Interfaces.RoomInterfaces;
using Core.Models;
using DAL.Abstractions.Interfaces;

namespace BLL.Services.RoomServices;

public class UserRoomsService : IUserRoomsService
{
    private readonly IGenericStorageWorker<UserRoomModel> userRoomStorage;

    public UserRoomsService(IGenericStorageWorker<UserRoomModel> userRoomStorage)
        {
        this.userRoomStorage = userRoomStorage;
        }

    public async Task Create(Guid userId, Guid roomId)
        {
            var userRoom = new UserRoomModel
            {
                UserId = userId,
                RoomId = roomId,
                IsAdmin = true, // Пользователь, создающий комнату, будет администратором
            };

            await this.userRoomStorage.Create(userRoom);
        }
}
