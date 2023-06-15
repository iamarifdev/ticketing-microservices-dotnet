using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ticketing.Common.DTO;
using Ticketing.Common.Exceptions;

namespace Ticketing.Common.Extensions;

public static class AuthExtension
{
    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.JwtKey)),
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = (context) => context.SetCurrentAuthUser(),
                    OnChallenge = async (context) =>
                    {
                        context.HandleResponse();
                        await context.HttpContext.HandleExceptionAsync(
                            context.AuthenticateFailure ?? 
                            new UnauthorizedException("Unauthorized access"));
                    },
                    OnAuthenticationFailed = async (context) =>
                    {
                        await context.HttpContext.HandleExceptionAsync(context.Exception);
                    }
                };
            });
    }

    private static Task SetCurrentAuthUser(this TokenValidatedContext context)
    {
        if (context.Principal?.Identity is null)
        {
            throw new UnauthorizedException("User identity claims not found");
        }

        var claims = ((ClaimsIdentity)context.Principal.Identity).Claims.ToList();

        var userId = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var email = claims.First(c => c.Type == ClaimTypes.Email).Value;

        var userPayload = new UserPayload(int.Parse(userId), email);

        context.HttpContext.Items[Constants.User] = userPayload;

        return Task.CompletedTask;
    }
}