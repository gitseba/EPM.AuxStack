using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.DisplayCapability.Formats;
using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Capabilities;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class DefaultCapability : BaseCapability, IDisplayCapabilityService
{
    public DefaultCapability(FlowDocument flowDocument)
        : base(flowDocument)
    {
    }

    public void ClearPanel()
    {
        base.ClearEntriesCapability();
    }

    public FlowDocument Display(string msg)
    {
        ClearPanel();
        return base.DefaultDisplay(msg);
    }

    public async Task<FlowDocument> DisplayAsync(ProcessedModel input)
    {
        ClearEntriesCapability();
        await Task.Delay(1);
        ColorLineDisplay("No lines were found! Please select another option.", brushColor: FormatText.ConsoleColorToBrush(ConsoleColor.Red));
        return _flowDocument;
    }
}
