namespace Ticketing.Common.Events;

public record TicketUpdatedEvent(
    string Id, string Version, string Title, decimal Price, string UserId, string? OrderId) : IEvent
{
    public Subjects Subject => Subjects.TicketUpdated;
}