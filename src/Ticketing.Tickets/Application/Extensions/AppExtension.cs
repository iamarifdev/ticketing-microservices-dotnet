using FluentValidation;
using MassTransit;
using Ticketing.Tickets.Application.Commands;
using Ticketing.Tickets.Application.Mapping;
using Ticketing.Tickets.Application.Validators;
using Ticketing.Tickets.Domain.Events.Publishers;
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
        builder.Services.AddScoped<IValidator<UpdateTicketCommand>, UpdateTicketCommandValidator>();
    }

    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
    }
    
    public static void RegisterEventPublishers(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<TicketCreatedPublisher>();
        builder.Services.AddScoped<TicketUpdatedPublisher>();
    }
    
    public static void RegisterEventConsumers(this IRabbitMqReceiveEndpointConfigurator endpoint)
    {
        // Configure Consumer here
        // endpoint.Consumer<>();
    }
    
    public static void MapRoutes(this WebApplication app)
    {
        var tickets = app.MapGroup("/tickets")
            .WithTags("Tickets Service")
            .WithName("Tickets Service")
            .RequireAuthorization();
        
        tickets.MapGet("/", TicketRouteHandlers.GetTickets)
            .WithName("GetTickets");
        tickets.MapGet("/{id:int}", TicketRouteHandlers.GetTicketById)
            .WithName("GetTicketById");
        tickets.MapPost("/", TicketRouteHandlers.CreateTicket)
            .WithName("CreateTicket");
        tickets.MapPut("/{id:int}", TicketRouteHandlers.UpdateTicket)
            .WithName("UpdateTicket");
    }
}