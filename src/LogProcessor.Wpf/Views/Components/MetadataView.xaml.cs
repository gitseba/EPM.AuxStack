using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LogProcessor.Wpf.Views;

/// <summary>
/// Interaction logic for MetadataView.xaml
/// </summary>
public partial class MetadataView : UserControl
{
    public MetadataView()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty MetadataProperty =
    DependencyProperty.Register(nameof(Metadata), typeof(ObservableCollection<KeyValuePair<string, string>>),
        typeof(MetadataView),
        new PropertyMetadata(null, OnMetadataChanged));

    public new ObservableCollection<KeyValuePair<string, string>> Metadata
    {
        get => (ObservableCollection<KeyValuePair<string, string>>)GetValue(MetadataProperty);
        set => SetValue(MetadataProperty, value);
    }

    private static void OnMetadataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MetadataView listView)
        {
            listView.SetValue(ItemsControl.ItemsSourceProperty, e.NewValue);
        }
    }
}
