namespace LogProcessor.Shared.Events;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class EventGeneric<T>
{
    private readonly List<Action<T>> _subscribers = new();

    public void Subscribe(Action<T> action)
    {
        if (!_subscribers.Contains(action))
        {
            _subscribers.Add(action);
        }
    }

    public void Unsubscribe(Action<T> action)
    {
        _subscribers.Remove(action);
    }

    public void Publish(T message)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Invoke(message);
        }
    }
}