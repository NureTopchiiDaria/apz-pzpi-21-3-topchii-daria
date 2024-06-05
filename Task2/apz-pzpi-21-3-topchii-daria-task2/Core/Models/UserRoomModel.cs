using Core.Models.UserModels;

namespace Core.Models;
using Core.Models.RoomModels;

public class UserRoomModel : BaseModel
{
    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }

    public bool IsAdmin { get; set; }

    public DateTime JoinDate { get; set; }

    public UserModel User { get; set; }

    public RoomModel Room { get; set; }
}