using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.DisplayCapability.Formats;
using LogProcessor.Strategy.Models;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Capabilities;

/// <summary>
/// Purpose: 
/// Created by: sebde
/// </summary>
public class FilterDisplayCapability : BaseCapability, IDisplayCapabilityService
{
    public FilterDisplayCapability(FlowDocument flowDocument)
        : base(flowDocument)
    {
    }

    public void ClearPanel()
    {
        _flowDocument.Blocks.Clear();
        _flowDocument?.Blocks.Add(new Paragraph(new Run("No option is being selected...")));
    }

    public FlowDocument? Display(string message)
    {
        throw new NotImplementedException();
    }

    public async Task<FlowDocument?> DisplayAsync(ProcessedModel input)
    {
        ClearEntriesCapability();

        if (input is null) return null;

        if (input?.ProcessedLines?.Count == 0)
        {
            ColorLineDisplay("No lines were found! Please select another option.", brushColor: FormatText.ConsoleColorToBrush(ConsoleColor.Red));
            return _flowDocument;
        }

        await Task.Delay(1);
        int idx = 0;
        foreach (var line in input?.ProcessedLines)
        {
            idx++;
            // ToDo: Even 5000 is much for analysis, this needs to be reconsiderate
            if (idx == 5000) 
            {
                break;
            }
            // Hide the source path of the Classes
            string pattern = @"\(([^)]+\.[^)]+)\)";
            string replacement = "(...)";

            string output = Regex.Replace(line.Line, pattern, replacement);
            base.ColorLogLevelCapability($"[#{line.LineNumber}] - " + output);
        }

        foreach (var paragraph in _flowDocument.Blocks.OfType<Paragraph>())
        {
            paragraph.LineHeight = 2;
        }

        return _flowDocument;
    }

    public void GuideWizard(int stepIndex, string message)
    {
        throw new NotImplementedException();
    }
}
