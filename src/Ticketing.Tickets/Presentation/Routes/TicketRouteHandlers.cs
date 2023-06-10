using Ticketing.Tickets.Application.DTO;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Ticketing.Tickets.Application.Commands;

namespace Ticketing.Tickets.Presentation.Routes
{
    public static class TicketRouteHandlers
    {
        public static async Task<Results<Ok<TicketResponse>, ValidationProblem>> CreateTicket(
            IValidator<CreateTicketCommand> validator,
            IMediator mediator,
            CreateTicketCommand command)
        {
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());

            var result = await mediator.Send(command);

            return TypedResults.Ok(result);
        }
    }
}