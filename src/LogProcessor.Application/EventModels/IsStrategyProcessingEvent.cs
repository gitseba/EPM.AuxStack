using LogProcessor.Shared.Events;

namespace LogProcessor.Application.EventModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class IsStrategyProcessingEvent : EventGeneric<IsStrategyProcessingEvent>
{

    /// <summary>
    /// Flag representing a strategy that is processing 
    /// </summary>
    public bool IsProcessing { get; set; }

}
