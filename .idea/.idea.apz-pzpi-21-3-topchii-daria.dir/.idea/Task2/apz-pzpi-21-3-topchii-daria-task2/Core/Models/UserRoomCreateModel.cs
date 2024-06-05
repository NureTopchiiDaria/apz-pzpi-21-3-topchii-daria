using Core.Models.UserModels;

namespace Core.Models;
using Core.Models.RoomModels;

public class UserRoomCreateModel : BaseModel
{
    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }
}