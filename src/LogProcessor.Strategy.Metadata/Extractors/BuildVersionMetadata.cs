using LogProcessor.Strategy.Enums;
using System.Text.RegularExpressions;

namespace LogProcessor.Strategy.Metadata.Extractors;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class BuildVersionMetadata : IMetadataExtractor
{
    public (LogMetadataEnum, string) Extract(IEnumerable<string> textLines)
    {
        var buildLine = textLines?.FirstOrDefault(x => x.Contains("Build:"));
        if (buildLine is null)
        {
            return (LogMetadataEnum.BuildVersion, string.Empty);
        }
        string pattern = @"\d+\.\d+\.\d+(\.\d+)?";  // Regex to match version numbers
        Match match = Regex.Match(buildLine, pattern);
        return (LogMetadataEnum.BuildVersion, match.Success ? match.Value : "No build version found.");
    }
}
