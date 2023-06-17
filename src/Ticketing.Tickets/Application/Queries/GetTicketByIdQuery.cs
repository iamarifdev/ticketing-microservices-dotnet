using MediatR;
using Ticketing.Tickets.Application.DTO;

namespace Ticketing.Tickets.Application.Queries;

public record GetTicketByIdQuery(int Id): IRequest<TicketResponse>;