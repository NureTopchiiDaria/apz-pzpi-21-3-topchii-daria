using System.Linq.Expressions;
using BLL.Abstractions.Interfaces.RoomInterfaces;
using Core.Models.RoomModels;
using DAL.Abstractions.Interfaces;

namespace BLL.Services.RoomServices;

internal class RoomPointsService : IRoomPointsService
{
    private readonly IGenericStorageWorker<RoomPointsModel> roomPointsStorage;

    public RoomPointsService(IGenericStorageWorker<RoomPointsModel> roomPointsStorage)
    {
        this.roomPointsStorage = roomPointsStorage;
    }

    public async Task<IEnumerable<RoomPointsModel>> GetByConditions(params Expression<Func<RoomPointsModel, bool>>[] conditions)
    {
        return await this.roomPointsStorage.GetByConditions(
            conditions,
            r => r.RoomId);
    }

    public async Task<float> CalculateRouteLength(Guid roomId)
    {
        var roomPoints = await this.roomPointsStorage.GetByConditions(new Expression<Func<RoomPointsModel, bool>>[] { }, new Expression<Func<RoomPointsModel, object>>[] { });

        if (roomPoints == null || !roomPoints.Any())
        {
            return 0;
        }

        float routeLength = 0;

        for (int i = 0; i < roomPoints.Count() - 1; i++)
        {
            routeLength += this.CalculateDistance(roomPoints.ElementAt(i), roomPoints.ElementAt(i + 1));
        }

        return routeLength;
    }

    private float CalculateDistance(RoomPointsModel point1, RoomPointsModel point2)
    {
        float distanceSquared = (float)(Math.Pow(point2.Latitude - point1.Latitude, 2) + Math.Pow(point2.Longitude - point1.Longitude, 2));

        distanceSquared += (float)Math.Pow(point2.Elevation - point1.Elevation, 2);

        return (float)Math.Sqrt(distanceSquared);
    }
}
