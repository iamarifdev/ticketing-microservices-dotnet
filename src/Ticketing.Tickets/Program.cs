using Ticketing.Tickets.Application.Extensions;
using Ticketing.Tickets.Persistence;
using Ticketing.Common.Extensions;
using Ticketing.Common.Middlewares;
using Ticketing.Tickets.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.AddJwtAuthentication();
builder.Services.AddAuthorization();

builder.AddMapping();
builder.AddDbContext<TicketDbContext>();
builder.RegisterServices();

builder.AddMassTransit(endpoint => endpoint.RegisterEventConsumers());
builder.RegisterEventPublishers();

builder.Services.AddHttpContextAccessor();
builder.AddMediatR<CreateTicketCommandHandler>();
builder.ConfigureRoute();
builder.AddFluentValidation();
builder.AddSwagger(serviceName: "Ticket");

var app = builder.Build();

app.MigrateDatabase<TicketDbContext>();

app.UseAuthentication();

app.UseCustomSwagger();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.MapRoutes();

app.Run();