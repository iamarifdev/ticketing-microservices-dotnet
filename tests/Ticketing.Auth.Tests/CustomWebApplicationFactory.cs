using Microsoft.AspNetCore.Mvc.Testing;

namespace Ticketing.Auth.Tests;

public static class CustomWebApplicationFactory<T> where T : class
{
    public static WebApplicationFactory<T> Create()
    {
        return new WebApplicationFactory<T>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                context.HostingEnvironment.EnvironmentName = "Test";
            });
        });
    }
}