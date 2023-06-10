using FluentValidation;
using Ticketing.Tickets.Application.Commands;
using Ticketing.Tickets.Application.Mapping;
using Ticketing.Tickets.Application.Validators;
using Ticketing.Tickets.Persistence.Repositories;
using Ticketing.Tickets.Presentation.Routes;

namespace Ticketing.Tickets.Application.Extensions;

public static class AppExtension
{
    public static void AddMapping(this WebApplicationBuilder builder)
    {
        MappingConfig.Configure();
    }

    public static void AddFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<CreateTicketCommand>, CreateTicketCommandValidator>();
    }

    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
    }
    
    public static void MapRoutes(this WebApplication app)
    {
        var tickets = app.MapGroup("/tickets")
            .WithTags("Tickets Service")
            .WithName("Tickets Service");
        
        tickets.MapPost("/", TicketRouteHandlers.CreateTicket).WithName("CreateTicket");
        // todo: add get all
        // todo: add new
        // todo: show ticket
        // todo: update
    }
}