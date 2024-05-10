using BLL.Abstractions.Interfaces.RoomInterfaces;
using Core.DataClasses;

namespace BLL.Services.RoomServices
{
    public class RoomApprovalService : IRoomApprovalService
    {
        private readonly IRoomService roomService;

        public RoomApprovalService(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        public async Task<ExceptionalResult> ApproveRoom(Guid id)
        {
            try
            {
                var result = await this.roomService.ApproveRoom(id);

                if (result.IsSuccess)
                {
                    return new ExceptionalResult();
                }
                else
                {
                    return new ExceptionalResult(false, result.ExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                return new ExceptionalResult(false, $"Internal server error: {ex.Message}");
            }
        }
    }
}