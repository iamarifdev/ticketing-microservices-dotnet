using Mapster;
using MediatR;
using Ticketing.Common.Events;
using Ticketing.Common.Extensions;
using Ticketing.Tickets.Application.Commands;
using Ticketing.Tickets.Application.DTO;
using Ticketing.Tickets.Application.Exceptions;
using Ticketing.Tickets.Domain.Events.Publishers;
using Ticketing.Tickets.Persistence.Repositories;

namespace Ticketing.Tickets.Application.Handlers;

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, TicketResponse>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TicketUpdatedPublisher _ticketUpdatedPublisher;

    public UpdateTicketCommandHandler(
        ITicketRepository userRepository, 
        IHttpContextAccessor httpContextAccessor, 
        TicketUpdatedPublisher ticketUpdatedPublisher)
    {
        _ticketRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _ticketUpdatedPublisher = ticketUpdatedPublisher;
    }

    public async Task<TicketResponse> Handle(
        UpdateTicketCommand dto, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(dto.Id, cancellationToken);
        
        if (ticket is null) throw new TicketNotFoundException(dto.Id);
        if (ticket.OrderId is not null) throw new TicketAlreadyReservedException();
        
        var currentUser = _httpContextAccessor.GetCurrentUser();
        if (ticket.UserId != currentUser.Id.ToString()) 
            throw new UnauthorizedTicketEditException();

        ticket.Title = dto.Title;
        ticket.Price = dto.Price;
        ticket.Version += 1;
        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        
        await _ticketUpdatedPublisher.Publish(new TicketUpdatedEvent(
            ticket.Id.ToString(),
            ticket.Version.ToString(),
            ticket.Title,
            ticket.Price,
            ticket.UserId),
            cancellationToken);

        return ticket.Adapt<TicketResponse>();
    }
}