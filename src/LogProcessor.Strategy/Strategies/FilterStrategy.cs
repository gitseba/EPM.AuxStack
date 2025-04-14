using LogProcessor.Strategy.Abstractions;
using LogProcessor.Strategy.Models;
using System.Text.RegularExpressions;

namespace LogProcessor.Strategies;

/// <summary>
/// Purpose: 
/// Created by: sebde
/// </summary>
public class FilterStrategy : IStrategy
{
    public async Task<ProcessedModel?> ProcessAsync(object input, IList<string> textLines, CancellationToken ctsToken = default)
    {
        if (input is not string inputText || inputText == "-" || textLines == null || textLines.Count == 0)
        {
            return null;
        }

        return await Task.Run(() =>
        {
            var model = new ProcessedModel();

            // Exact word match using word boundary \b
            string pattern = $@"\b{Regex.Escape(inputText)}\b";
            var regex = new Regex(pattern, RegexOptions.Compiled);

            for (int i = 0; i < textLines.Count; i++)
            {
                ctsToken.ThrowIfCancellationRequested();

                var line = textLines[i];
                if (regex.IsMatch(line))
                {
                    model.ProcessedLines.Add(new ContentModel
                    {
                        Line = line,
                        LineNumber = (i + 1).ToString()
                    });
                }
            }

            return model;
        }, ctsToken);
    }

}
