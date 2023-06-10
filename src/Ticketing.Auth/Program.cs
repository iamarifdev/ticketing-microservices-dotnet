using Ticketing.Auth.Application.Extensions;
using Ticketing.Auth.Application.Handlers;
using Ticketing.Auth.Persistence;
using Ticketing.Common.Extensions;
using Ticketing.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.AddSwagger(serviceName: "Auth");

// Configure services
builder.AddMapping();
builder.AddDbContext<AuthDbContext>();
builder.AddMediatR<SignupCommandHandler>();
builder.RegisterServices();
builder.ConfigureRoute();
builder.AddFluentValidation();

var app = builder.Build();

app.MigrateDatabase<AuthDbContext>();

app.UseStaticFiles();
app.UseCustomSwagger();

app.UseMiddleware<CurrentUserMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapRoutes();

app.Run();