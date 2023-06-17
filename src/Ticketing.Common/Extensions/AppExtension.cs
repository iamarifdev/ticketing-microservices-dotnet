using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Ticketing.Common.DTO;
using Ticketing.Common.Exceptions;
using Ticketing.Common.Middlewares;

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
            throw new UnauthorizedException("Invalid credentials");

        return (UserPayload)userPayload;
    }

    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }

    public static Task HandleExceptionAsync(this HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errorMessage = "An error occurred.";

        if (ex is HttpException httpException)
        {
            statusCode = (HttpStatusCode)httpException.StatusCode;
            errorMessage = httpException.Message;
        }

        var errorResponse = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = errorMessage
        };

        var json = JsonSerializer.Serialize(errorResponse);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(json);
    }

    public static void UseExceptionHandler(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}