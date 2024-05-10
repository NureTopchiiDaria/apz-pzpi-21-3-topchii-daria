using BLL.Abstractions.Interfaces.RoomInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomPointsController : ControllerBase
    {
        private readonly IRoomPointsService _roomPointsService;

        public RoomPointsController(IRoomPointsService roomPointsService)
        {
            _roomPointsService = roomPointsService;
        }

        [HttpGet("{roomId}/route-length")]
        public async Task<IActionResult> CalculateRouteLength(Guid roomId)
        {
            try
            {
                var routeLength = await _roomPointsService.CalculateRouteLength(roomId);
                return Ok(routeLength);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}