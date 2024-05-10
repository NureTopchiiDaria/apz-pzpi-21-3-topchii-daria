using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces.RoomInterfaces;
using BLL.Requests;
using Core.Models.RoomModels;
using Core.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomModel>>> GetRooms()
        {
            try
            {
                var rooms = await _roomService.GetByConditions();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomModel>> GetRoomById(Guid id)
        {
            try
            {
                var room = await _roomService.GetRoomById(id);
                if (room == null)
                {
                    return NotFound($"Room with id {id} not found");
                }
                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoomModel>> CreateRoom(RoomCreateModel room)
        {
            try
            {
                var result = await _roomService.Create(room);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.ExceptionMessage);
                }
                return CreatedAtAction(nameof(GetRoomById), new { id = result.Value.Id }, result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(Guid id)
        {
            try
            {
                var result = await _roomService.Delete(id);
                if (!result.IsSuccess)
                {
                    return NotFound(result.ExceptionMessage);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<RoomModel>> UpdateRoom(RoomUpdateModel roomModel)
        {
            try
            {
                var result = await _roomService.Update(roomModel);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.ExceptionMessage);
                }
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
