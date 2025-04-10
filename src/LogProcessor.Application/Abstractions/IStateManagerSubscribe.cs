using LogProcessor.Application.State;

namespace LogProcessor.Application.Abstractions;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IStateManagerSubscribe
{
    void StateManagerSubscription(FilesStateEnum stateOption, Action<object> action);
}
