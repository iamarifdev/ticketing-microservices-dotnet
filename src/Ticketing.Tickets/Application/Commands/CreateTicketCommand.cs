using MediatR;
using Ticketing.Tickets.Application.DTO;

namespace Ticketing.Tickets.Application.Commands;

public record CreateTicketCommand(string Title, decimal Price) : IRequest<TicketResponse>;