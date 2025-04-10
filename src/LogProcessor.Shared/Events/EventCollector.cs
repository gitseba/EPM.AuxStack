namespace LogProcessor.Shared.Events;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class EventCollector
{

    private readonly Dictionary<Type, object> _events = new();

    public EventGeneric<T> Event<T>() where T : class, new()
    {
        var type = typeof(T);
        if (!_events.ContainsKey(type))
        {
            _events[type] = new EventGeneric<T>();
        }
        return (EventGeneric<T>)_events[type];
    }
}

