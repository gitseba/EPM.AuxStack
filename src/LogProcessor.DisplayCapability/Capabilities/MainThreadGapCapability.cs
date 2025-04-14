using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.DisplayCapability.Formats;
using LogProcessor.Strategy.Abstractions;
using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Capabilities;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class MainThreadGapCapability : BaseCapability, IDisplayCapabilityService
{

    public MainThreadGapCapability(FlowDocument input)
             : base(input)
    {

    }

    public void ClearPanel()
    {
        _flowDocument.Blocks.Clear();
        _flowDocument?.Blocks.Add(new Paragraph(new Run("No option is being selected...")));
    }

    public async Task<FlowDocument?> DisplayAsync(ProcessedModel input)
    {
        ClearEntriesCapability();

        DefaultDisplay($"> {input?.Strategy.GetType().Name} : {input?.SelectedOption} seconds");
        DefaultDisplay($"> Description: Process text lines to find Mainthread [Th1] gap in logs (Meaning: when Th1 didn't appear in logs) \n");

        if (input is null || input?.ProcessedLines?.Count == 0)
        {
            base.DefaultDisplay("No results!");
            return _flowDocument;
        }
        
        await Task.Delay(1);
        foreach (var line in input.ProcessedLines)
        {
            base.ColorLogLevelCapability(line.PreviousLine);
            ColorLineDisplay($"  GAP => {line.Difference}", brushColor: FormatText.ConsoleColorToBrush(ConsoleColor.Red));
            base.ColorLogLevelCapability(line.Line + "\n");
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
