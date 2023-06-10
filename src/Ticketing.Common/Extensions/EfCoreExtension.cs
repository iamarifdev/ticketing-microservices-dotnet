using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Common.Extensions;

public static class EfCoreExtension
{
    public static void AddDbContext<TContext>(this WebApplicationBuilder builder) where TContext : DbContext
    {
        if (builder.Environment.IsEnvironment("Test"))
            builder.Services.AddDbContext<TContext>(options =>
                options.UseInMemoryDatabase(nameof(TContext)));
        else
            builder.Services.AddDbContext<TContext>(
                options => options.UseNpgsql(
                    builder.Configuration.GetConnectionString(typeof(TContext).Name)));
    }

    public static void MigrateDatabase<TContext>(this WebApplication? app) where TContext : DbContext
    {
        if (app is null) return;
        if (app.Environment.IsEnvironment("Test")) return;

        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var context = serviceProvider.GetRequiredService<TContext>();
        context.Database.Migrate();
    }
}