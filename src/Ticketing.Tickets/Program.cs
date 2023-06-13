using Ticketing.Tickets.Application.Extensions;
using Ticketing.Tickets.Persistence;
using Ticketing.Common.Extensions;
using Ticketing.Common.Middlewares;
using Ticketing.Tickets.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.AddSwagger(serviceName: "Ticket");

// Configure services
builder.AddMapping();
builder.AddDbContext<TicketDbContext>();
builder.AddMediatR<CreateTicketCommandHandler>();
builder.RegisterServices();
builder.AddMassTransit(endpoint => endpoint.RegisterEventConsumers());
builder.RegisterEventPublishers();
builder.ConfigureRoute();
builder.AddFluentValidation();

var app = builder.Build();

app.MigrateDatabase<TicketDbContext>();

app.UseStaticFiles();
app.UseCustomSwagger();

app.UseMiddleware<CurrentUserMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapRoutes();

app.Run();