using MediatR;
using Ticketing.Tickets.Application.DTO;

namespace Ticketing.Tickets.Application.Commands;

public record UpdateTicketCommand(int Id, string Title, decimal Price) : IRequest<TicketResponse>;