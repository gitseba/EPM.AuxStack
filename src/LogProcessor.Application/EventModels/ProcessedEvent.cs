using LogProcessor.Shared.Events;

namespace LogProcessor.Application.EventModels;

/// <summary>
/// Purpose: Represents results after a process (E.g. strategy results)
/// Created by: tseb
/// </summary>
public class ProcessedEvent : EventGeneric<ProcessedEvent>
{
    public object? Results { get; set; }
}
