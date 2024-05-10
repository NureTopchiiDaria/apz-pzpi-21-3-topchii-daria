using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel userData)
        {
            try
            {
                var result = await loginService.Login(userData);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.ExceptionMessage);
                }
                return Ok(new { token = result.Value });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}