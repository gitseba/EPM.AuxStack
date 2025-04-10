using LogProcessor.Strategy.Enums;
using LogProcessor.Strategy.Utils;
using System.Text.RegularExpressions;

namespace LogProcessor.Strategy.Metadata.Extractors;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class StartLogExtractor : IMetadataExtractor
{
    public (LogMetadataEnum, string) Extract(IEnumerable<string> textLines)
    {
        string pattern = @"\b\d{2}:\d{2}:\d{2}\.\d{3}\b"; // Regex for HH:MM:SS.mmm format

        foreach (string line in textLines)
        {
            if (Regex.IsMatch(line, pattern))
            {
                var result = TimeUtils.ExtractTime(line);
                return (LogMetadataEnum.LogStart, result);
            }
        }

        return (LogMetadataEnum.LogStart, string.Empty);
    }
}
