using System.Globalization;

namespace LogProcessor.Wpf.Converters;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class LogLevelsConverter : BaseValueConverter<LogLevelsConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || value.ToString() == string.Empty)
            return "Log Levels:";
       
        return value;
    }


    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}