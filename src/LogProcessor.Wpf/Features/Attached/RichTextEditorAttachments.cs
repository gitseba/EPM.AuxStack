using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LogProcessor.Wpf.Attached;

/// <summary>
/// Purpose:  The IsBusy attached property for a anything that wants to flag if the control is busy
/// Created by: tseb
/// </summary>
public class BoundTextProperty : BaseAttachedProperty<BoundTextProperty, FlowDocument>
{
    public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        base.OnValueChanged(sender, e);

        if (sender is RichTextBox richTextBox && e.NewValue is FlowDocument newDoc)
        {
            // Defer setting document to the UI thread with lower priority
            richTextBox.Document = newDoc;
        }
    }

    public override void OnValueUpdated(DependencyObject sender, object value)
    {
        base.OnValueUpdated(sender, value);
    }
}