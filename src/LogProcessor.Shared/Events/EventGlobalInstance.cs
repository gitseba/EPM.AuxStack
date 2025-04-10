namespace LogProcessor.Shared.Events;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public static class EventGlobalInstance
{
    public static EventCollector Instance { get; } = new EventCollector();
}
