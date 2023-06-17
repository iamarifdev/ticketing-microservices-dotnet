using Mapster;
using MediatR;
using Ticketing.Tickets.Application.DTO;
using Ticketing.Tickets.Application.Queries;
using Ticketing.Tickets.Persistence.Repositories;

namespace Ticketing.Tickets.Application.Handlers;

public class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, List<TicketResponse>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketsQueryHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }
    
    public async Task<List<TicketResponse>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.GetListAsync(cancellationToken);
        return tickets.Adapt<List<TicketResponse>>();
    }
}