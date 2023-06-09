namespace Ticketing.Common.Events;

public interface IEvent<out T>
{
    Subjects Subject { get; }
    T Data { get; }
}