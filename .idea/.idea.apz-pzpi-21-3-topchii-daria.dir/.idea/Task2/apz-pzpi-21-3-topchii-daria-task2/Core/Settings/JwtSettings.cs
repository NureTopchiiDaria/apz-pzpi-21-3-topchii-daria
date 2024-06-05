namespace Core.Settings;

#pragma warning disable CA1704
public class JwtSettings
#pragma warning restore CA1704
{
    public string ValidAudience { get; set; } = string.Empty;

    public string ValidIssuer { get; set; } = string.Empty;

    public string Secret { get; set; } = string.Empty;

    public int TokenValidityInMinutes { get; set; } = 5;

    public int RefreshTokenValidityInDays { get; set; } = 7;
}