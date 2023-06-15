using Ticketing.Auth.Application.Extensions;
using Ticketing.Auth.Application.Handlers;
using Ticketing.Auth.Persistence;
using Ticketing.Common.Extensions;
using Ticketing.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.AddJwtAuthentication();
builder.Services.AddAuthorization();

builder.AddMapping();
builder.AddDbContext<AuthDbContext>();
builder.RegisterServices();

builder.Services.AddHttpContextAccessor();
builder.AddMediatR<SignupCommandHandler>();
builder.ConfigureRoute();
builder.AddFluentValidation();
builder.AddSwagger(serviceName: "Auth");

var app = builder.Build();

app.MigrateDatabase<AuthDbContext>();

app.UseAuthentication();

app.UseCustomSwagger();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.MapRoutes();

app.Run();