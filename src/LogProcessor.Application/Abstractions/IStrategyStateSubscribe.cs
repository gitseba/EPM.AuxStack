using LogProcessor.Application.State;

namespace LogProcessor.Application.Abstractions;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IStrategyStateSubscribe
{
    void Subscribe(StrategyStateEnum stateOption, Action<object> action);
}
