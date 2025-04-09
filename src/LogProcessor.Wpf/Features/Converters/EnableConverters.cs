using System.Collections;
using System.Globalization;

namespace LogProcessor.Wpf.Converters;

/// <summary>
/// Purpose: Converters used to enabling/disabling controls
/// Created by: tseb
/// </summary>
public class IsNotNullOrEmptyEnableConverter : BaseValueConverter<IsNotNullOrEmptyEnableConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            string str => !string.IsNullOrEmpty(str),
            ICollection collection => collection.Count > 0,
            IEnumerable enumerable => enumerable.GetEnumerator().MoveNext(),
            _ => value != null
        };


    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class InverseEnableConverter : BaseValueConverter<InverseEnableConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     => value switch
     {
         string str => string.IsNullOrEmpty(str),  // Invert: true when empty, false when not empty
         ICollection collection => collection.Count == 0,  // Invert: true when count is 0, false otherwise
         IEnumerable enumerable => !enumerable.GetEnumerator().MoveNext(),  // Invert: true when no elements, false otherwise
         bool boolean => !boolean,  // Invert: true when no elements, false otherwise
         _ => value == null  // Invert: true when null, false when not null
     };



    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class InverseEnableMultiConverter : BaseMultiValueConverter<InverseEnableMultiConverter>
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values != null && values.All(val => val is not bool boolean))
        {
            return null;
        }

        if ((bool)values[0] == true && (bool)values[1] == false)
        {
            return false;
        }

        if ((bool)values[0] == false && (bool)values[1] == true)
        {
            return false;
        }

        return true;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}