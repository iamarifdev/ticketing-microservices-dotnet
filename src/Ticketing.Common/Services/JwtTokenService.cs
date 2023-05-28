using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ticketing.Common.DTO;

namespace Ticketing.Common.Services;

public static class JwtTokenService
{
    public static string GenerateToken(UserPayload userPayload, JwtConfig config)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userPayload.Id.ToString()),
            new(ClaimTypes.Email, userPayload.Email)
        };

        // todo: use RSA cryptography to sign the token instead of HMAC (private/public key)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            config.Issuer,
            config.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(config.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}