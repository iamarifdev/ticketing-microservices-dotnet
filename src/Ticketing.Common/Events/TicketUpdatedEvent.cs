namespace Ticketing.Common.Events;

public record TicketUpdatedEventData(
    string Id, string Version, string Title, decimal Price, string UserId, string? OrderId);

public class TicketUpdatedEvent : IEvent<TicketUpdatedEventData>
{
    public TicketUpdatedEvent(TicketUpdatedEventData data)
    {
        Subject = Subjects.TicketUpdated;
        Data = data;
    }

    public Subjects Subject { get; }
    public TicketUpdatedEventData Data { get; }
}