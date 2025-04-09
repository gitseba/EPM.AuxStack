using System.Globalization;
using System.Windows;

namespace LogProcessor.Wpf.Converters;

/// <summary>
/// Purpose: Converters used to enabling/disabling controls
/// Created by: tseb
/// </summary>
public class ContentNotNullVisibilityConverter
    : BaseValueConverter<ContentNotNullVisibilityConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

}

public class ContentNullVisibilityConverter
    : BaseValueConverter<ContentNullVisibilityConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Collapsed;

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class BooleanIsTrueVisibilityConverter
    : BaseValueConverter<BooleanIsTrueVisibilityConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            throw new NullReferenceException();
        }

        return (bool)value && value != null ? Visibility.Visible : Visibility.Collapsed;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class BooleanIsNotTrueVisibilityConverter
    : BaseValueConverter<BooleanIsNotTrueVisibilityConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? Visibility.Collapsed : Visibility.Visible;

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class HasItemsVisibilityConverter
    : BaseValueConverter<HasItemsVisibilityConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (int)value > 0 ? Visibility.Visible : Visibility.Collapsed;

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
