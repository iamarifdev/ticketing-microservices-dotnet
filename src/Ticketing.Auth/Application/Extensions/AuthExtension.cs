using Ticketing.Auth.Application.DTO;
using Ticketing.Common;
using Ticketing.Common.DTO;
using Ticketing.Common.Services;

namespace Ticketing.Auth.Application.Extensions;

public static class AuthExtension
{
    public static AuthResponse WithJwtToken(this AuthResponse response, JwtConfig jwtConfig)
    {
        var userPayload = new UserPayload(response.Id, response.Email);
        var token = JwtTokenService.GenerateToken(userPayload, jwtConfig);
        return response with { Token = token };
    }
    
    public static AuthResponse SetJwtTokenCookie(this AuthResponse response, IHttpContextAccessor httpContextAccessor)
    {
        httpContextAccessor.SetCookie(Constants.Jwt, response.Token);
        return response;
    }
    
    public static void RemoveJwtTokenCookie(this IHttpContextAccessor httpContextAccessor)
    {
        httpContextAccessor.RemoveCookie(Constants.Jwt);
    }
    
    public static UserPayload GetCurrentUser(this IHttpContextAccessor httpContextAccessor)
    {
        object? userPayload = null;
        httpContextAccessor.HttpContext?.Items.TryGetValue(Constants.User, out userPayload);
        
        if (userPayload is null)
            throw new UnauthorizedAccessException("Invalid credentials");
        
        return (UserPayload)userPayload;
    }
}