namespace BLL.Abstractions.Interfaces.RoomInterfaces;

public interface IRoomPointsService
{
    Task<float> CalculateRouteLength(Guid roomId);
}