using System.Windows;

namespace LogProcessor.Wpf.Attached;

/// <summary>
/// Purpose: Attached properties for Textbox elements (Placeholder text)
/// Created by: tseb
/// </summary>
public class PlaceholderProperty : BaseAttachedProperty<PlaceholderProperty, string>
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
