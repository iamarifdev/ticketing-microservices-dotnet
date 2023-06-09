namespace Ticketing.Common.Events;

public record TicketCreatedEventData(
    string Id, string Version, string Title, decimal Price, string UserId);

public class TicketCreatedEvent : IEvent<TicketCreatedEventData>
{
    public TicketCreatedEvent(TicketCreatedEventData data)
    {
        Subject = Subjects.TicketCreated;
        Data = data;
    }

    public Subjects Subject { get; }
    public TicketCreatedEventData Data { get; }
}