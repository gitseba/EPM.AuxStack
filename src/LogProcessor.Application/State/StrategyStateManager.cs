using LogProcessor.Application.EventModels;
using LogProcessor.Shared.Events;
using LogProcessor.Strategy.Abstractions;

namespace LogProcessor.Application.State;

/// <summary>
/// Purpose: Keep the global state for strategies actions
/// Created by: tseb
/// </summary>
public class StrategyStateManager
{
    public Dictionary<StrategyStateEnum, List<Action<object>>> StateManager { get; } = [];

    public StrategyStateManager()
    {
        RegisterSubscriptionEvents();
    }

    /// <summary>
    /// Subscribe to events coming from Mediator
    /// </summary>
    private void RegisterSubscriptionEvents()
    {
        // Event subscriptions from Mediator
        EventGlobalInstance.Instance.Event<StrategySelectedEvent>()
            .Subscribe(async(ev) =>
            {
                ActiveStrategy = ev.ActiveStrategy;
                ActiveStrategyValue = ev.ActiveStrategyValue;
            });

        EventGlobalInstance.Instance.Event<IsStrategyProcessingEvent>()
            .Subscribe(async(ev) => IsProcessing = ev.IsProcessing );

        EventGlobalInstance.Instance.Event<ProcessedEvent>()
            .Subscribe(async(ev) => ProcessedResults = ev.Results);

        EventGlobalInstance.Instance.Event<FilesClearEvent>()
            .Subscribe(async(ev) => Reset());
    }

    /// <summary>
    /// Subscribe to a state
    /// </summary>
    /// <param name="stateOption" <see cref="FileStateOptions"/>></param>
    /// <param name="action"> Action/Method that will be triggered on state updated</param>
    public void StateSubscription(StrategyStateEnum stateOption, Action<object> action)
    {
        if (!StateManager.ContainsKey(stateOption))
            StateManager[stateOption] = [];

        StateManager[stateOption].Add(action);
    }

    /// <summary>
    /// Generic method to update state and notify subscribers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Action identifier key</param>
    /// <param name="property">Property that holds the value state</param>
    /// <param name="value">Value</param>
    private void UpdateState<T>(StrategyStateEnum key, ref T property, T value)
    {
        property = value;
        if (StateManager.TryGetValue(key, out var actions))
        {
            foreach (var action in actions)
            {
                action?.Invoke(value);
            }
        }
    }

    /// <summary>
    /// Reset state to initial default values
    /// </summary>
    public void Reset()
    {
        ActiveStrategy = null;
        ActiveStrategyValue = null;
    }

    /// <summary>
    /// Represents strategy in processing state
    /// </summary>
    private bool _isProcessing;
    public bool IsProcessing
    {
        get => _isProcessing;
        set => UpdateState(StrategyStateEnum.IsProcessing, ref _isProcessing, value);
    }

    /// <summary>
    /// Represents active feature selected (TimeFreeze, LogLevels etc)
    /// </summary>
    private IStrategy? _activeStrategy;
    public IStrategy? ActiveStrategy
    {
        get => _activeStrategy;
        set => UpdateState(StrategyStateEnum.ActiveStrategy, ref _activeStrategy, value);
    }


    /// <summary>
    /// Represents active feature's value
    /// </summary>
    private object? _activeStrategyValue;
    public object? ActiveStrategyValue
    {
        get => _activeStrategyValue;
        set => UpdateState(StrategyStateEnum.ActiveStrategyValue, ref _activeStrategyValue, value);
    }

    /// <summary>
    /// Represents results after processing strategy
    /// </summary>
    private object? _processedResults;
    public object? ProcessedResults
    {
        get => _processedResults;
        set => UpdateState(StrategyStateEnum.ProcessedResults, ref _processedResults, value);
    }
}