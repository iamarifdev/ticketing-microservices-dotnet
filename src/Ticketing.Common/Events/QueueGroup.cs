using Ardalis.SmartEnum;

namespace Ticketing.Common.Events
{
    public class QueueGroup : SmartEnum<QueueGroup>
    {
        public static readonly QueueGroup OrderService = new("order-service", 0);
        public static readonly QueueGroup ExpirationService = new("expiration-service", 1);
        public static readonly QueueGroup PaymentService = new("payment-service", 2);
        public static readonly QueueGroup TicketService = new("ticket-service", 3);

        private QueueGroup(string name, int value) : base(name, value)
        {
        }
    }
}