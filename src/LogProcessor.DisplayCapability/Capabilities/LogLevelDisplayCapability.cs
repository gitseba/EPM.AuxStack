using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.DisplayCapability.Formats;
using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Capabilities;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class LogLevelDisplayCapability : BaseCapability, IDisplayCapabilityService
{

    public LogLevelDisplayCapability(FlowDocument input)
             : base(input)
    {

    }

    public void GuideWizard(int stepIndex, string message)
    {
        throw new NotImplementedException();
    }

    public void ClearPanel()
    {
        _flowDocument.Blocks.Clear();
        _flowDocument?.Blocks.Add(new Paragraph(new Run("No option is being selected...")));
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

        var logLevels = (string[])input.SelectedOption;
        DefaultDisplay($"> {input.Strategy.GetType().Name} : {string.Join(" | ", logLevels)}\n");

        int idx = 0;

        await Task.Delay(1);
        foreach (var line in input?.ProcessedLines)
        {
            ColorLogLevelCapability($"[#{line.LineNumber}]: {line.Line} \n");
            idx++;
        }

        foreach (var paragraph in _flowDocument.Blocks.OfType<Paragraph>())
        {
            paragraph.LineHeight = 2;
        }

        return _flowDocument;
    }

    public FlowDocument? Display(string message)
    {
        throw new NotImplementedException();
    }
}
