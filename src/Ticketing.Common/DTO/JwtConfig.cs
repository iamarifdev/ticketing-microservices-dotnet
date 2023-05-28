namespace Ticketing.Common.DTO;

public class JwtConfig
{
    public JwtConfig()
    {
    }

    public JwtConfig(string issuer, string audience, int expiryMinutes)
    {
        Issuer = issuer;
        Audience = audience;
        ExpiryMinutes = expiryMinutes;
    }

    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpiryMinutes { get; set; }

    public static string JwtKey =>
        Environment.GetEnvironmentVariable(Constants.JwtKey) ?? "supersecretkeythatis128bitlong";
}