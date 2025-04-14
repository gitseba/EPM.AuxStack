namespace LogProcessor.Application.State;

/// <summary>
/// Purpose: Represent options that other classes can subscribe to state manager
/// Created by: tseb
/// </summary>
public enum StrategyStateEnum
{
    IsProcessing, // Strategy is in process state
    ActiveStrategy, // Selected active strategy
    ActiveStrategyValue, // Selected value of the active strategy
    ProcessedResults
}
