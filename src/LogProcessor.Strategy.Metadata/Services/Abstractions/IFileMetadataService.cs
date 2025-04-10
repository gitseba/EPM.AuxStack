using LogProcessor.Strategy.Enums;

namespace LogProcessor.Strategy.Metadata.Services;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IFileMetadataService
{
    Task<Dictionary<LogMetadataEnum, string>> ProcessAsync(string filePath,
        CancellationToken ctsToken = default);
}
