namespace BLL.Abstractions.Interfaces.RoomInterfaces;

public interface IUserRoomsService
{
    Task Create(Guid userId, Guid roomId);
}