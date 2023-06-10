using Mapster;
using MediatR;
using Ticketing.Common.Extensions;
using Ticketing.Tickets.Application.Commands;
using Ticketing.Tickets.Application.DTO;
using Ticketing.Tickets.Domain.Entities;
using Ticketing.Tickets.Persistence.Repositories;

namespace Ticketing.Tickets.Application.Handlers;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketResponse>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateTicketCommandHandler(ITicketRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _ticketRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TicketResponse> Handle(CreateTicketCommand dto, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.GetCurrentUser();
        var ticket = await _ticketRepository.CreateAsync(new Ticket
        {
            Price = dto.Price,
            Title = dto.Title,
            UserId = currentUser.Id.ToString(),
            Version = 1
        }, cancellationToken);

        return ticket.Adapt<TicketResponse>();
    }
}