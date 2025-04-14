using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.DisplayCapability.Formats;
using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Capabilities;

/// <summary>
/// Purpose: Display om console capability for Time gap strategy
/// Created by: tseb
/// </summary>
public class TimeGapDisplayCapability : BaseCapability, IDisplayCapabilityService
{
    public TimeGapDisplayCapability(FlowDocument flowDocument)
        : base(flowDocument)
    {
    }

    public void ClearPanel()
    {
        _flowDocument.Blocks.Clear();
        _flowDocument?.Blocks.Add(new Paragraph(new Run("Please select value for active strategy...")));
    }

    public FlowDocument Display(string message)
    {
        throw new NotImplementedException();
    }

    public async Task<FlowDocument?> DisplayAsync(ProcessedModel input)
    {
        ClearEntriesCapability();

        DefaultDisplay($"> {input?.Strategy.GetType().Name}: {input?.SelectedOption} seconds");
        DefaultDisplay($"> Description: Process text lines to find timestamp differences\n");

        if (input is null || input?.ProcessedLines?.Count == 0)
        {
            base.DefaultDisplay("No results!");
            return _flowDocument;
        }

        await Task.Delay(1);
        foreach (var line in input.ProcessedLines)
        {
            base.ColorLogLevelCapability(line.PreviousLine);
            ColorLineDisplay($" [#{line.LineNumber}] GAP => {line.Difference}", brushColor: FormatText.ConsoleColorToBrush(ConsoleColor.Red));
            base.ColorLogLevelCapability(line.Line + "\n");
        }

        foreach (var paragraph in _flowDocument.Blocks.OfType<Paragraph>())
        {
            paragraph.LineHeight = 2;
        }

        return _flowDocument;
    }
}
