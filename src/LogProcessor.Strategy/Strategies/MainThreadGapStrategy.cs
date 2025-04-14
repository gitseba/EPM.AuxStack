using LogProcessor.Strategy.Abstractions;
using LogProcessor.Strategy.Models;
using LogProcessor.Strategy.Utils;
using System.Text.RegularExpressions;

namespace LogProcessor.Strategies;

/// <summary>
/// Purpose: Strategy to figure out how much Time in logs the Main Thread didn't logged 
/// Created by: tseb
/// </summary>
public class MainThreadGapStrategy : IStrategy
{
    public async Task<ProcessedModel?> ProcessAsync(object input, IList<string> textLines, CancellationToken ctsToken = default)
    {
        // Validate input
        if (input is not string inputStr ||
            string.IsNullOrWhiteSpace(inputStr) ||
            !int.TryParse(inputStr, out int thresholdSeconds) ||
            textLines == null || textLines.Count == 0)
        {
            return null;
        }

        // Initialize the result model
        ProcessedModel model = new();

        return await Task.Run(() =>
        {
            string logPattern = @"(\d{2}:\d{2}:\d{2}\.\d{3}) .*(?:TID|ThreadId)\s*[:]?\s*1\b";
            int lineCount = textLines.Count();

            if (textLines is null || lineCount == 0) return null;

            var dict = new Dictionary<int, int>();
            bool isBlocked = false;

            List<string> lines = textLines as List<string> ?? textLines.ToList(); // Convert to List for O(1) indexing
            int firstPointer = 0, secondPointer = 1;

            while (secondPointer < lineCount)
            {
                bool firstMatch = Regex.IsMatch(lines[firstPointer], logPattern);
                bool secondMatch = Regex.IsMatch(lines[secondPointer], logPattern);

                if (firstMatch && !isBlocked && firstMatch != secondMatch)
                {
                    isBlocked = true;
                }

                if (isBlocked)
                {
                    while (++secondPointer < lineCount && !Regex.IsMatch(lines[secondPointer], logPattern))
                    {
                        // Skip lines that don't match the pattern
                    }

                    if (secondPointer >= lineCount) break;

                    dict[firstPointer] = secondPointer;
                    isBlocked = false;
                    firstPointer = secondPointer - 1;
                }

                firstPointer++;
                secondPointer = firstPointer + 1;
            }

            foreach (var (key, value) in dict)
            {
                if (key >= lines.Count || value >= lines.Count) continue; // Safety check

                string firstLine = lines[key];
                string secondLine = lines[value];

                if (firstLine.Length < 12 || secondLine.Length < 12) continue; // Skip invalid lines

                // Extract timestamps
                string firstIndex = firstLine[..12];  // Equivalent to Substring(0, 12)
                string secondIndex = secondLine[..12];

                if (TimeSpan.TryParseExact(firstIndex, @"hh\:mm\:ss\.fff", null, out TimeSpan currentTime1) &&
                    TimeSpan.TryParseExact(secondIndex, @"hh\:mm\:ss\.fff", null, out TimeSpan currentTime2))
                {
                    TimeSpan difference = currentTime2 - currentTime1;

                    if (input != null && difference.TotalSeconds >= int.Parse(input.ToString()))
                    {
                        model?.ProcessedLines?.Add(new ContentModel
                        {
                            Line = secondLine,
                            Difference = difference.TotalSeconds > 59
                                 ? TimeUtils.TotalMinutesFormat(difference)
                                    : TimeUtils.TotalSecondsFormat(difference),
                            PreviousLine = firstLine,
                            LineNumber = $"Lines between {key + 1} and {value + 1}"
                        });
                    }
                }
            }

            return model;
        });
    }
}
