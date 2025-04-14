using LogProcessor.Shared.Events;
using LogProcessor.Strategy.Abstractions;

namespace LogProcessor.Application.EventModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class StrategySelectedEvent : EventGeneric<StrategySelectedEvent>
{
    public IStrategy? ActiveStrategy { get; set; }
    public object? ActiveStrategyValue { get; set; }
}
