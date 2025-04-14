using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.DisplayCapability.Capabilities.Factories;
using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Services;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class DisplayCapabilityService : IDisplayCapabilityService
{
    public FlowDocument? Display(string message)
    {
        var capability = DisplayFactory.Create(null);
        return capability?.Display(message);
    }

    public async Task<FlowDocument?> DisplayAsync(ProcessedModel input)
    {
        var capability = DisplayFactory.Create(input);
        var flowDocument = await capability?.DisplayAsync(input);

        return flowDocument;
    }

    public void GuideWizard(int stepIndex, string message)
    {
        // ToDo: add user steps wizard guide
        throw new NotImplementedException();
    }
}
