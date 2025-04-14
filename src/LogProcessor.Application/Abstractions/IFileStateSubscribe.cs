using LogProcessor.Application.State;

namespace LogProcessor.Application.Abstractions;

/// <summary>
/// Purpose: Contract for applying option of any class to subscribe to state management 
/// Created by: tseb
/// </summary>
public interface IFileStateSubscribe
{
    void Subscribe(FilesStateEnum stateOption, Action<object> action);
}
