using LogProcessor.Shared.Events;

namespace LogProcessor.Application.EventModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class FileMetadataUpdateEvent : EventGeneric<FileMetadataUpdateEvent>
{
    public Dictionary<string, string> Metadata { get; set; } = [];
}
