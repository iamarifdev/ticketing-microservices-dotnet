using Ardalis.SmartEnum;

namespace Ticketing.Common.Events;

public class Subjects : SmartEnum<Subjects>
{
    public static readonly Subjects TicketCreated = new("ticket:created", 0);
    public static readonly Subjects TicketUpdated = new("ticket:updated", 1);
    public static readonly Subjects OrderCreated = new("order:created", 2);
    public static readonly Subjects OrderCancelled = new("order:cancelled", 3);
    public static readonly Subjects ExpirationComplete = new("expiration:complete", 4);
    public static readonly Subjects PaymentCreated = new("payment:created", 5);

    private Subjects(string name, int value) : base(name, value)
    {
    }
}