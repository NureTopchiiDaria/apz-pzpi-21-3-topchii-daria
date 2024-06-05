using Core.Models.RoomModels;
using Core.Models.UserModels;

namespace BLL.Requests;

public class RoomCreateRequest
{
    public RoomCreateModel RoomModel { get; set; }

    public UserModel UserModel { get; set; }
}
