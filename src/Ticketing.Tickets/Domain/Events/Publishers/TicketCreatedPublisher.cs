using MassTransit;
using Ticketing.Common.Events;

namespace Ticketing.Tickets.Domain.Events.Publishers
{
    public class TicketCreatedPublisher : IPublisher<TicketCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TicketCreatedPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }


        public async Task Publish(TicketCreatedEvent ticketCreatedEvent)
        {
            await _publishEndpoint.Publish(ticketCreatedEvent);
        }
    }
}