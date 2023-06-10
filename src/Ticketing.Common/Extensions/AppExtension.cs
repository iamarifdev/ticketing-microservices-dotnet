using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Ticketing.Common.DTO;

namespace Ticketing.Common.Extensions;

public static class AppExtension
{
    public static void AddMediatR<T>(this WebApplicationBuilder builder) where T : class
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(T).Assembly));
    }

    public static void ConfigureRoute(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
    }

    public static UserPayload GetCurrentUser(this IHttpContextAccessor httpContextAccessor)
    {
        object? userPayload = null;
        httpContextAccessor.HttpContext?.Items.TryGetValue(Constants.User, out userPayload);

        if (userPayload is null)
            throw new UnauthorizedAccessException("Invalid credentials");

        return (UserPayload)userPayload;
    }

    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }
}