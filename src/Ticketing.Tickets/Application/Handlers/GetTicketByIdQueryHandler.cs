using Mapster;
using MediatR;
using Ticketing.Tickets.Application.DTO;
using Ticketing.Tickets.Application.Exceptions;
using Ticketing.Tickets.Application.Queries;
using Ticketing.Tickets.Persistence.Repositories;

namespace Ticketing.Tickets.Application.Handlers;

public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketResponse>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketByIdQueryHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<TicketResponse> Handle(GetTicketByIdQuery query, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(query.Id, cancellationToken);
        if (ticket is null) throw new TicketNotFoundException(query.Id);

        return ticket.Adapt<TicketResponse>();
    }
}