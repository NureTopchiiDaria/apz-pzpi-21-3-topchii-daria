using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataClasses;
using Core.DTOs;
using Core.Models;
using NetTopologySuite.Geometries;

namespace BLL.Abstractions.Interfaces
{
    public interface ITrackingDataService
    {
        Task<IEnumerable<TrackingDataModel>> GetTrackingDataByUserId(Guid userId);

        Task<TrackingDataModel> AddTrackingData(TrackingDataModel trackingData);

        Task<OptionalResult<TrackingDataModel>> UpdateTrackingData(TrackingDataModel trackingData);

        Task<ExceptionalResult> DeleteTrackingData(Guid id);

        Task<StatisticsDto> GetStatistics(Guid userId, Guid roomId);

        Task<LatestTrackingDataDto> GetLatestTrackingData(Guid userId, Guid roomId);

        Task<DistanceAndSpeedDto> GetTotalDistanceAndAverageSpeed(Guid userId, Guid roomId);
    }
}