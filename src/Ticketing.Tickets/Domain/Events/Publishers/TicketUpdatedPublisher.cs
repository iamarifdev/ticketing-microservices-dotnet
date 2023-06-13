using MassTransit;
using Ticketing.Common.Events;

namespace Ticketing.Tickets.Domain.Events.Publishers;

public class TicketUpdatedPublisher : IPublisher<TicketUpdatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public TicketUpdatedPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Publish(
        TicketUpdatedEvent ticketUpdatedEvent, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(ticketUpdatedEvent, cancellationToken);
    }
}