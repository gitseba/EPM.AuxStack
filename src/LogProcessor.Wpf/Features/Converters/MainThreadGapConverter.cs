using System.Globalization;

namespace LogProcessor.Wpf.Converters;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class MainThreadGapConverter : BaseValueConverter<MainThreadGapConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value.ToString() == string.Empty)
            return "MainTh [1]:";

        return $"[TID:1] GAP > {value} s";
    }


    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
