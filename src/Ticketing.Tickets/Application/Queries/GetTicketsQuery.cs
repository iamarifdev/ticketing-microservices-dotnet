using MediatR;
using Ticketing.Tickets.Application.DTO;

namespace Ticketing.Tickets.Application.Queries;

public record GetTicketsQuery: IRequest<List<TicketResponse>>;