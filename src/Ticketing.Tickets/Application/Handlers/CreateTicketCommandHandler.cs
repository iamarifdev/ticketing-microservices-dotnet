using Mapster;
using MediatR;
using Ticketing.Common.Events;
using Ticketing.Common.Extensions;
using Ticketing.Tickets.Application.Commands;
using Ticketing.Tickets.Application.DTO;
using Ticketing.Tickets.Domain.Entities;
using Ticketing.Tickets.Domain.Events.Publishers;
using Ticketing.Tickets.Persistence.Repositories;

namespace Ticketing.Tickets.Application.Handlers;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketResponse>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TicketCreatedPublisher _ticketCreatedPublisher;

    public CreateTicketCommandHandler(
        ITicketRepository userRepository, 
        IHttpContextAccessor httpContextAccessor, 
        TicketCreatedPublisher ticketCreatedPublisher)
    {
        _ticketRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _ticketCreatedPublisher = ticketCreatedPublisher;
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

        await _ticketCreatedPublisher.Publish(
            new TicketCreatedEvent(
                ticket.Id.ToString(),
                ticket.Version.ToString(),
                ticket.Title,
                ticket.Price,
                ticket.UserId));

        return ticket.Adapt<TicketResponse>();
    }
}