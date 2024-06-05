#pragma warning disable CA1704
namespace Core.DTOs;
#pragma warning restore CA1704

#pragma warning disable CA1704
public class DistanceAndSpeedDto
#pragma warning restore CA1704
    {
        public double TotalDistance { get; set; }

        public double AverageSpeed { get; set; }
    }