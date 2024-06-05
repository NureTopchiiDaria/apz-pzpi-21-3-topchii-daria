using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Core.Models.RoomModels
{
    public class RoomModel : BaseModel
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        public bool IsApproved { get; set; }

        public Point StartLocation { get; set; }

        public Point EndLocation { get; set; }

        public float TripLength { get; set; }

        public DateTime DateTime { get; set; }

        public string Information { get; set; }
    }
}
