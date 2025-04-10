using LogProcessor.Strategy.Metadata.Extractors;

namespace LogProcessor.Strategy.Metadata;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public static class RegisteredExtractors
{
    public static IList<IMetadataExtractor> Get() =>
        [
            new RegisteredInstanceExtractor(),
            new BuildVersionMetadata(),
            new StartLogExtractor(),
        ];
}
