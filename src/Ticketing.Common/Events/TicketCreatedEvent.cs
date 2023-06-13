namespace Ticketing.Common.Events;

public record TicketCreatedEvent(
    string Id, string Version, string Title, decimal Price, string UserId) : IEvent
{
    public Subjects Subject => Subjects.TicketCreated;
}