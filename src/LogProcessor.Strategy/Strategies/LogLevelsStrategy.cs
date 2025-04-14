using LogProcessor.Strategy.Abstractions;
using LogProcessor.Strategy.Models;

namespace LogProcessor.Strategies;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class LogLevelsStrategy : IStrategy
{
    public async Task<ProcessedModel?> ProcessAsync(object input, IList<string> textLines, CancellationToken ctsToken = default)
    {
        // Validate input
        if (input is not string[] targetWords || textLines == null || !textLines.Any())
        {
            return default; // Return a new instance of the specified type (T)
        }

        // Initialize the result model
        ProcessedModel model = Activator.CreateInstance<ProcessedModel>();

        return await Task.Run(() =>
        {
            string previousLine = string.Empty;

            // Use LINQ to filter matched lines based on target words
            var matchedLines = textLines
                .Select((line, index) => new { Line = line, LineNumber = index + 1 }) // Add line number (1-based)
                .Where(entry => targetWords.Any(word => entry.Line.Contains(word, StringComparison.Ordinal))) // Match words
                .ToList();

            // Process each matched line and add it to the result model
            foreach (var line in matchedLines)
            {
                model?.ProcessedLines?.Add(new ContentModel
                {
                    Line = line.Line.ToString(),
                    PreviousLine = previousLine,
                    LineNumber = line.LineNumber.ToString()
                });

                previousLine = line.Line;
            }

            return model;
        });
    }
}
