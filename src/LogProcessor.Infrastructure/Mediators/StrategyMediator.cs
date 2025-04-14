using LogProcessor.Application.Abstractions;
using LogProcessor.Application.EventModels;
using LogProcessor.Application.State;
using LogProcessor.Shared.Events;
using LogProcessor.Strategy.Abstractions;

namespace LogProcessor.Infrastructure.Mediators;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class StrategyMediator : IStrategyMediator
{
    private readonly StrategyStateManager _strategyStateManager;
    private readonly FilesStateManager _fileStateManager;

    public StrategyMediator(
        StrategyStateManager strategyStateManager,
        FilesStateManager fileStateManager)
    {
        _strategyStateManager = strategyStateManager;
        _fileStateManager = fileStateManager;
    }

    /// <summary>
    /// Ability to subscribe to strategy state manager
    /// </summary>
    public void Subscribe(StrategyStateEnum stateOption, Action<object> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        _strategyStateManager.StateSubscription(stateOption, action);
    }

    /// <summary>
    /// Ability to subscribe to file state manager
    /// </summary>
    public void Subscribe(FilesStateEnum stateOption, Action<object> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        _fileStateManager.StateSubscription(stateOption, action);
    }

    public async Task HandleAsync(IStrategy strategy, object input, CancellationToken ctsToken = default)
    {
        if (strategy is null || input is null) return;

         EventGlobalInstance.Instance.Event<StrategySelectedEvent>()
              .Publish(new StrategySelectedEvent() { ActiveStrategy = strategy, ActiveStrategyValue = input });
        try
        {
             EventGlobalInstance.Instance.Event<IsStrategyProcessingEvent>()
                 .Publish(new IsStrategyProcessingEvent { IsProcessing = true });

            var lines = File.ReadLines(_fileStateManager.SelectedFilePath);
            var results = await strategy.ProcessAsync(input, lines.ToArray(), ctsToken);
            if (results is not null)
            {
                results.Strategy = strategy;
                results.SelectedOption = input;
            }

             EventGlobalInstance.Instance.Event<ProcessedEvent>()
                .Publish(new ProcessedEvent { Results = results });

        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            await Task.Delay(1, ctsToken);
             EventGlobalInstance.Instance.Event<IsStrategyProcessingEvent>()
                 .Publish(new IsStrategyProcessingEvent { IsProcessing = false });
        }
        return;
    }
}
