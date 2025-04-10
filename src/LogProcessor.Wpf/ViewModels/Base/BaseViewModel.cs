using LogProcessor.Wpf.Notifiers;

namespace LogProcessor.Wpf.ViewModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public abstract class BaseViewModel
    : NotifierObject
{
    /// <summary>
    /// Notify the UI that a busy action is performed
    /// </summary>
    public bool IsLoading { get; set; }

    /// <summary>
    /// Notify UI if any component should be enabled/disabled
    /// </summary>
    public bool IsEnabled { get; set; }
}
