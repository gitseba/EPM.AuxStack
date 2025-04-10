using System.Windows;

namespace LogProcessor.Wpf.Attached;

/// <summary>
/// Purpose:The IsBusy attached property for a anything that wants to flag if the control is busy/loading/updating ...
/// Created by: tseb
/// </summary>
public class IsBusyProperty : BaseAttachedProperty<IsBusyProperty, bool>
{
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        base.OnValueChanged(sender, e);
    }

    public override void OnValueUpdated(DependencyObject sender, object value)
    {
        base.OnValueUpdated(sender, value);
    }
}
