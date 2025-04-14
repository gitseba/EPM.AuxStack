using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.Strategies;
using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.DisplayCapability.Capabilities.Factories;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class DisplayFactory
{
    // ToDo: To be thinking if the FlowDocument object should be created and provided from this Factory
    public static IDisplayCapabilityService? Create(ProcessedModel processedModel)
    {

        var strategyType = processedModel?.Strategy?.GetType();

        return strategyType switch
        {
            Type t when t == typeof(TimeGapStrategy) => new TimeGapDisplayCapability(new FlowDocument()),
            Type t when t == typeof(MainThreadGapStrategy) => new MainThreadGapCapability(new FlowDocument()),
            Type t when t == typeof(LogLevelsStrategy) => new LogLevelDisplayCapability(new FlowDocument()),
            Type t when t == typeof(FilterStrategy) => new FilterDisplayCapability(new FlowDocument()),
            _ => new DefaultCapability(new FlowDocument())
        };
    }
}