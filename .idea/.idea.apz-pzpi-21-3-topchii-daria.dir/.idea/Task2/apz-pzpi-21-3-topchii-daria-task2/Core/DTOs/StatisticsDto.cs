#pragma warning disable CA1704
namespace Core.DTOs;
#pragma warning restore CA1704

#pragma warning disable CA1704
public class StatisticsDto
#pragma warning restore CA1704
{
    public double AveragePulse { get; set; }

    public List<LocationDto> Locations { get; set; }
}
