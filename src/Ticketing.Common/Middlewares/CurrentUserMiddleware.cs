using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Ticketing.Common.DTO;

namespace Ticketing.Common.Middlewares;

public class CurrentUserMiddleware
{
    private const string JWT_KEY = "JWT_KEY";
    private readonly ILogger<CurrentUserMiddleware> _logger;
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next, ILogger<CurrentUserMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            await _next(context);
            return;
        }

        var token = authHeader.ToString().Replace("Bearer ", string.Empty);
        
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable(JWT_KEY) 
                                            ?? throw new InvalidOperationException($"{JWT_KEY} is not set"))),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            var user = handler.ValidateToken(token, tokenValidationParameters, out _);

            if (user.Identity is null)
            {
                _logger.LogError("User identity claims not found");
                throw new InvalidOperationException("User identity claims not found");
            }
            
            var claims = ((ClaimsIdentity)user.Identity).Claims.ToList();

            var userId = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var email = claims.First(c => c.Type == ClaimTypes.Email).Value;
            
            var userPayload = new UserPayload(userId, email);

            context.Items["User"] = userPayload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        await _next(context);
    }
}
