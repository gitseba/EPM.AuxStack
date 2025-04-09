using System.Windows.Input;

namespace LogProcessor.Wpf.Commands;

/// <summary>
/// Purpose: Generic version for commands with parameters
/// Created by: tseb
/// </summary>
public class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || (parameter is T t && _canExecute(t));
    }

    public void Execute(object? parameter)
    {
        if (parameter is T t)
        {
            _execute(t);
        }
        else if (parameter == null && typeof(T).IsClass)
        {
            _execute((T)parameter!);
        }
    }
}
