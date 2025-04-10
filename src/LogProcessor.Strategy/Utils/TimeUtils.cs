using System.Text.RegularExpressions;

namespace LogProcessor.Strategy.Utils;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public static class TimeUtils
{
    public static string ExtractTime(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        Match match = Regex.Match(input, @"\d{2}:\d{2}:\d{2}\.\d{3}");
        if (match.Success)
        {
            string timeSegment = match.Value; // E.g. ("20:40:59.200")
            if (string.IsNullOrEmpty(timeSegment))
            {
                Console.WriteLine("Failed to parse time.");
                return string.Empty;
            }

            return timeSegment;
        }
        else
        {
            Console.WriteLine("No valid time found in text line.");
            return string.Empty;
        }
    }

    /// <summary>
    /// Display TimeSpan as seconds format
    /// </summary>
    public static string TotalSecondsFormat(TimeSpan input)
    {
        return $"{input.Seconds.ToString()}s (+{input.Milliseconds}ms)";
    }

    /// <summary>
    /// Display TimeSpan as minutes format
    /// </summary>
    public static string TotalMinutesFormat(TimeSpan input)
    {
        return $"{input.Minutes.ToString()} min (+{input.Seconds})";
    }
}