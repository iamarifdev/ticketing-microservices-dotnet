using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Ticketing.Common.Extensions;

public static class SwaggerExtension
{
    public static void AddSwagger(this WebApplicationBuilder builder, string serviceName)
    {
        builder.Services.AddEndpointsApiExplorer().AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = $"{serviceName} Service", Version = "v1" });
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }   
            });
            config.ExampleFilters();
        });
        builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
    }

    public static void UseCustomSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var jsInterceptor = @"
                function(response) {
                    const token = response.obj?.token || response.obj?.data?.token;
                    if (token) {
                        setTimeout(() => {
                            const authDefinitions = ui.authSelectors.definitionsToAuthorize();
                            const payload = {
                                Bearer: {
                                    name: 'Bearer',
                                    value: token,
                                    schema: authDefinitions.get(0).get('Bearer')
                                }
                            };
                            ui.authActions.authorizeWithPersistOption(payload);
                        });
                    }
                    return response;
                }
            ".Replace("\r\n", "").Replace("\n", "");
            
            options.Interceptors.ResponseInterceptorFunction = jsInterceptor;
            options.EnablePersistAuthorization();
        });
    }
}