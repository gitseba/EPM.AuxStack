
using LogProcessor.Strategy.Enums;
using System.Text.RegularExpressions;

namespace LogProcessor.Strategy.Metadata.Extractors;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class RegisteredInstanceExtractor : IMetadataExtractor
{
    public (LogMetadataEnum, string) Extract(IEnumerable<string> textLines)
    {
        var buildLine = textLines?.FirstOrDefault(x => x.Contains("Instance registered"));
        if (buildLine is null)
        {
            return (LogMetadataEnum.InstanceRegistration, string.Empty);
        }
        string pattern = @"(?<=Instance registered as:\s)(Global\\[\w\-]+)";
        Match match = Regex.Match(buildLine, pattern);
        return (LogMetadataEnum.InstanceRegistration, match.Success ? match.Value : "\"No instance found.");
    }
}
