using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.UserModels;
using NetTopologySuite.Geometries;

namespace Core.Models.RoomModels
{
    public class RoomCreateModel
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }

        public Point StartLocation { get; set; }

        public Point EndLocation { get; set; }

        public float TripLength { get; set; }

        public DateTime DateTime { get; set; }

        public string Information { get; set; }
    }
}
