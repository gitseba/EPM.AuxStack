using System.Globalization;

namespace LogProcessor.Wpf.Converters;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class TimeGapConverter : BaseValueConverter<TimeGapConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value.ToString() == string.Empty)
            return "Time Gaps:";

        return $"Gap > {value} s";
    }


    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}