using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Abstractions;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IDisplayCapabilityService
{
    Task<FlowDocument?> DisplayAsync(ProcessedModel input);
    FlowDocument? Display(string message);
}
