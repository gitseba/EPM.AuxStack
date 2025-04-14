using LogProcessor.Shared.Events;

namespace LogProcessor.Application.EventModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class RootPathSelectedEvent : EventGeneric<RootPathSelectedEvent>
{
    public string Path { get; set; } = string.Empty;
}
