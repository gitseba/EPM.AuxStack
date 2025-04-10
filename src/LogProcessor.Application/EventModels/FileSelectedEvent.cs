using LogProcessor.Shared.Events;

namespace LogProcessor.Application.EventModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class FileSelectedEvent : EventGeneric<FileSelectedEvent>
{
    public string FilePath { get; set; } = string.Empty;
}
