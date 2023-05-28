using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticketing Auth Service", Version = "v1" });
    config.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer <token>\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        }
    );
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Configure services
builder.AddMapping();
builder.AddDbContext();
builder.AddMediatR();
builder.RegisterServices();
builder.ConfigureRoute();
builder.AddFluentValidation();

var app = builder.Build();

app.MigrateDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGroup("/auth")
    .WithTags("Auth Service")
    .WithName("Auth Service")
    .MapPost("/signup", async (IValidator<SignupRequest> validator, IMediator mediator, SignupRequest dto) =>
    {
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
        var result = await mediator.Send(new SignupCommand(dto));
        return Results.Ok(result);
    });

app.Run();