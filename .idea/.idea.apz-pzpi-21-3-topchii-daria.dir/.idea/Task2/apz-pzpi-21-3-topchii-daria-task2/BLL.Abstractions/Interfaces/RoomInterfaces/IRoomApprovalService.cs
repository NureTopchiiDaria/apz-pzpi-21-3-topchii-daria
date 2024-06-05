using Core.DataClasses;

namespace BLL.Abstractions.Interfaces.RoomInterfaces;

public interface IRoomApprovalService
{
    Task<ExceptionalResult> ApproveRoom(Guid id);
}