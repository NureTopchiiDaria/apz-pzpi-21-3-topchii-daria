using BLL.Abstractions.Interfaces.RoomInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class AdministrationController : ControllerBase
    {
        private readonly IRoomApprovalService roomApprovalService;

        public AdministrationController(IRoomApprovalService roomApprovalService)
        {
            this.roomApprovalService = roomApprovalService;
        }

        [HttpPost("{id}/approve")]
       public async Task<IActionResult> ApproveRoom(Guid id)
        {
            var result = await roomApprovalService.ApproveRoom(id);

            if (result.IsSuccess)
            {
                return Ok("Room approved successfully.");
            }
            else
            {
                return StatusCode(500, result.ExceptionMessage);
            }
        }
    }
}