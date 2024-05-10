#pragma warning disable CA1704
namespace Core.DTOs;
#pragma warning restore CA1704

public class UserHeightDTO
{
    public float Height { get; set; }

    public bool IsInFeet { get; set; } // Флаг для указания, задается ли рост в футах
}