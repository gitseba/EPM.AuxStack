using System.Windows.Input;

namespace LogProcessor.Wpf.Commands;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class RelayCommand : ICommand
{
    /// <summary>
    /// The action to run
    /// </summary>
    private readonly Action<object> _action;
    private readonly Func<bool> _canExecute;

    /// <summary>
    /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
    /// </summary>
    public event EventHandler? CanExecuteChanged = (sender, e) => { };

    public RelayCommand(Action action) 
        : this(obj => action())  
    {
      
    }

    public RelayCommand(Action<object> action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
        _canExecute = () => true;  // Default: always executable
    }

    public RelayCommand(Action<object> action, Func<bool> canExecute)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
        _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    /// <summary>
    /// Determines whether the action is available to be executed 
    /// (e.g., whether a button is enabled or disabled).
    /// </summary>
    public bool CanExecute(object parameter) => _canExecute();
    
    /// <summary>
    /// Executes the commands Action
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(object? parameter) => _action(parameter);
    
}

