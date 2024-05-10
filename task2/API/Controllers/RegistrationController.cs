using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserRegistrationModel userData)
        {
            try
            {
                var result = await registrationService.Register(userData);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.ExceptionMessage);
                }
                return Ok("Registration successful. Please check your email for activation instructions.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}