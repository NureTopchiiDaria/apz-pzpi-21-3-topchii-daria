namespace Core.Models;

public class UserRoomModel : BaseModel
{
    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }

    public bool IsAdmin { get; set; }
}