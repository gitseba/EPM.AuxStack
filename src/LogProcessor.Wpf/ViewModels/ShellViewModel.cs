using System.Windows;
using System.Windows.Input;
using LogProcessor.Wpf.Commands;

namespace LogProcessor.Wpf.ViewModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class ShellViewModel : BaseViewModel
{
    public ShellViewModel()
    {
        // Window commands
        MinimizeCommand = new RelayCommand((p) => { WindowState = WindowState.Minimized; });
        MaximizeCommand = new RelayCommand((p) =>
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        });
        ExitCommand = new RelayCommand((p) => (p as Window)?.Close());
    }

    #region Binding properties
    public WindowState WindowState { get; set; } = WindowState.Normal;
    #endregion

    #region ICommands

    public ICommand MinimizeCommand { get; set; }
    public ICommand MaximizeCommand { get; set; }
    public ICommand ExitCommand { get; set; }

    #endregion
}
