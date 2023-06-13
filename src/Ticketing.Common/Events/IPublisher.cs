namespace Ticketing.Common.Events;

public interface IPublisher<in TEvent> where TEvent : IEvent
{
    Task Publish(TEvent @event);
}
