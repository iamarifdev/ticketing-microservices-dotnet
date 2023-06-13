namespace Ticketing.Common.Events;

public interface IEvent
{
    Subjects Subject { get; }
}