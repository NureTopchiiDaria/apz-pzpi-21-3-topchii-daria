using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core.DataClasses;
using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingDataController : ControllerBase
    {
        private readonly ITrackingDataService trackingDataService;

        public TrackingDataController(ITrackingDataService trackingDataService)
        {
            this.trackingDataService = trackingDataService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<TrackingDataModel>> GetTrackingDataByUserId(Guid userId)
        {
            return await this.trackingDataService.GetTrackingDataByUserId(userId);
        }

        [HttpPost]
        public async Task<ActionResult<TrackingDataModel>> AddTrackingData([FromBody] TrackingDataModel trackingData)
        {
            var createdTrackingData = await this.trackingDataService.AddTrackingData(trackingData);
            return CreatedAtAction(nameof(GetTrackingDataByUserId), new { userId = createdTrackingData.UserId }, createdTrackingData);
        }

        [HttpPut]
        public async Task<ActionResult<OptionalResult<TrackingDataModel>>> UpdateTrackingData([FromBody] TrackingDataModel trackingData)
        {
            var result = await this.trackingDataService.UpdateTrackingData(trackingData);
            if (!result.IsSuccess)
            {
                return NotFound(result.ExceptionMessage);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ExceptionalResult>> DeleteTrackingData(Guid id)
        {
            var result = await this.trackingDataService.DeleteTrackingData(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.ExceptionMessage);
            }
            return Ok(result);
        }

        [HttpGet("statistics/user/{userId}/room/{roomId}")]
        public async Task<ActionResult<StatisticsDto>> GetStatistics(Guid userId, Guid roomId)
        {
            var statistics = await this.trackingDataService.GetStatistics(userId, roomId);
            return Ok(statistics);
        }

        [HttpGet("latest/user/{userId}/room/{roomId}")]
        public async Task<ActionResult<LatestTrackingDataDto>> GetLatestTrackingData(Guid userId, Guid roomId)
        {
            var latestTrackingData = await this.trackingDataService.GetLatestTrackingData(userId, roomId);

            if (latestTrackingData == null)
            {
                return NotFound("No tracking data found for the specified user and room.");
            }

            return Ok(latestTrackingData);
        }
    }
}
