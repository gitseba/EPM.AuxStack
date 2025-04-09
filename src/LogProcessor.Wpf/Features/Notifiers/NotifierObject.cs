using System.ComponentModel;

namespace LogProcessor.Wpf.Notifiers;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class NotifierObject : INotifyPropertyChanged
{
    /// <summary>
    /// The event that is fired when any child property changes its value
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

    /// <summary>
    /// Call this to fire a <see cref="PropertyChanged"/> event
    /// </summary>
    /// <param name="name">property name</param>
    public void OnPropertyChanged(string name)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
}