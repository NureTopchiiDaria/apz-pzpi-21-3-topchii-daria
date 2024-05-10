using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.DTOs;
using Core.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1;

 [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateModel userModel)
        {
            var result = await _userService.CreateNonActiveUser(userModel);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ExceptionMessage);
            }
            return CreatedAtAction(nameof(GetUserById), new { id = result.Value.Id }, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ExceptionMessage);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserHeightDTO userHeightDTO)
        {
            var userModel = new UserUpdateModel
            {
                Id = id,
                Height = userHeightDTO.Height,
                // Добавьте логику для обработки информации о росте в футах или метрах
            };

            var result = await _userService.Update(userModel, userHeightDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ExceptionMessage);
            }
            return Ok(result.Value);
        }


        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            var result = await _userService.ActivateUser(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ExceptionMessage);
            }
            return Ok(result.Value);
        }
    }