using FluentValidation;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Mapping;
using Ticketing.Auth.Application.Validators;
using Ticketing.Auth.Persistence.Repositories;
using Ticketing.Auth.Presentation.Routes;

namespace Ticketing.Auth.Application.Extensions;

public static class AppExtension
{
    public static void AddMapping(this WebApplicationBuilder builder)
    {
        MappingConfig.Configure();
    }

    public static void AddFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<SignupRequest>, SignupCommandValidator>();
        builder.Services.AddScoped<IValidator<SignInCommand>, SignInCommandValidator>();
    }

    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }
    
    public static void MapRoutes(this WebApplication app)
    {
        var auth = app.MapGroup("/auth")
            .WithTags("Auth Service")
            .WithName("Auth Service");
        
        auth.MapGet("/currentuser", AuthRouteHandlers.GetCurrentUser)
            .WithName("CurrentUser")
            .RequireAuthorization();
        auth.MapPost("/signup", AuthRouteHandlers.Signup).WithName("Signup");
        auth.MapPost("/signin", AuthRouteHandlers.SignIn).WithName("Signin");
        auth.MapPost("/signout", AuthRouteHandlers.SignOut)
            .WithName("SignOut")
            .RequireAuthorization();
    }
}