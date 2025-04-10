namespace LogProcessor.Strategy.Abstractions;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IStrategyOrchestrator
{
    Task HandleAsync(IStrategy logStrategy, object input, CancellationToken ctsToken = default);
}
