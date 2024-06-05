using Core.Models;

namespace BLL.Abstractions.Interfaces.RoomInterfaces;

public interface IUserRoomsService
{
    Task AddAdminUserToRoom(Guid userId, Guid roomId);

    Task AddUserToRoom(UserRoomCreateModel userRoomCreate);

    Task<UserRoomModel> GetUserRoomByRoomId(Guid roomId);

    Task<UserRoomModel> GetAdminInfoByRoomId(Guid roomId);

    Task<IEnumerable<UserRoomModel>> GetRoomsForUser(Guid userId);
}