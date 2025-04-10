using LogProcessor.Strategy.Enums;

namespace LogProcessor.Strategy.Metadata.Extractors;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IMetadataExtractor
{
    (LogMetadataEnum, string) Extract(IEnumerable<string> textLines);
}
