#pragma warning disable CA1704
namespace Core.DTOs;
#pragma warning restore CA1704

#pragma warning disable CA1704
public class LocationDto
#pragma warning restore CA1704
{
    public DateTime Timestamp { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}