using NetTopologySuite.Geometries;

namespace Core.Models
{
    public class TrackingDataModel : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }

        public Point Location { get; set; }

        public int Pulse { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}