using LogProcessor.Strategy.Abstractions;
using LogProcessor.Strategy.Models;
using LogProcessor.Strategy.Utils;

namespace LogProcessor.Strategies;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class TimeGapStrategy : IStrategy
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

        return await Task.Run(() =>
        {
            TimeSpan? previousTime = null;
            string previousLine = string.Empty;
            var model = new ProcessedModel();

            for (int index = 0; index < textLines.Count; index++)
            {
                var line = textLines[index];
                if (string.IsNullOrWhiteSpace(line) || line.Length < 12) continue;

                string timeStampPart = line.Substring(0, 12);
                if (TimeSpan.TryParseExact(timeStampPart, @"hh\:mm\:ss\.fff", null, out TimeSpan currentTime))
                {
                    if (previousTime.HasValue)
                    {
                        var diff = currentTime - previousTime.Value;

                        if (diff.TotalSeconds >= thresholdSeconds)
                        {
                            model?.ProcessedLines?.Add(new ContentModel
                            {
                                Line = line,
                                Difference = diff.TotalSeconds > 59
                                    ? TimeUtils.TotalMinutesFormat(diff)
                                    : TimeUtils.TotalSecondsFormat(diff),
                                PreviousLine = previousLine,
                                LineNumber = index.ToString()
                            });
                        }
                    }

                    previousTime = currentTime;
                    previousLine = line;
                }
            }

            return model;
        }, ctsToken);
    }
}
