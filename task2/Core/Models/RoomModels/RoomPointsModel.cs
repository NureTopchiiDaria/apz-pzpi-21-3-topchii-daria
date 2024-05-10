namespace Core.Models.RoomModels;

public class RoomPointsModel : BaseModel
{
    public Guid RoomId { get; set; }

    public float Latitude { get; set; }

    public float Longitude { get; set; }

    public float Elevation { get; set; }
}
