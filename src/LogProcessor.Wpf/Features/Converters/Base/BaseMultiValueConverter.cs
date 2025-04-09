using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace LogProcessor.Wpf.Converters;

/// <summary>
/// Purpose: Base value converter that allows direct XAML usage.
/// Basically, it's no longer necessary to use static resource from xaml to access converters that inherit from this base class.
/// Usage => DefaultText="{Binding Model.DefaultText, Converter={converters:DefaultButtonContentConverter}}"
/// Binding is being done directly in the markup, no need for reference by key in .Resources
/// Created by: tseb
/// <typeparam name="T">The type of this value converter</typeparam>
/// </summary>
public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter
   where T : class, new()
{
    /// <summary>
    /// A single static instance of this value converter
    /// </summary>
    private static T mConverter = null;

    /// <summary>
    /// Provides a static instance of the value converter 
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return mConverter ?? (mConverter = new T());
    }

    /// <summary>
    /// The method to convert one type to another
    /// </summary>
    public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

    /// <summary>
    /// The method to convert a value back to it's source type
    /// </summary>
    public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
}
