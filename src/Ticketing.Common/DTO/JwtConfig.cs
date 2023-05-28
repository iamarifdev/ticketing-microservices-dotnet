namespace Ticketing.Common.DTO;

public record JwtConfig(string Issuer, string Audience, int ExpiryMinutes);