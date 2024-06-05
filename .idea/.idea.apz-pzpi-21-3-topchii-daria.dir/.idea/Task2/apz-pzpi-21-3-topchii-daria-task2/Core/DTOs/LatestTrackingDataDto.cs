#pragma warning disable CA1704
namespace Core.DTOs;
#pragma warning restore CA1704

#pragma warning disable CA1704
public class LatestTrackingDataDto
#pragma warning restore CA1704
{
    public DateTime Timestamp { get; set; }

    public LocationDto Location { get; set; }

    public int Pulse { get; set; }
}