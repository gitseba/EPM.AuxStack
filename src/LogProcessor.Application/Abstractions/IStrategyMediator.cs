using LogProcessor.Strategy.Abstractions;

namespace LogProcessor.Application.Abstractions;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IStrategyMediator : IFileStateSubscribe, IStrategyStateSubscribe
{
    Task HandleAsync(IStrategy logStrategy, object input, CancellationToken ctsToken = default);
}
