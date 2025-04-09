using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace LogProcessor.Wpf.Converters;

/// <summary>
/// Purpose: Base value converter that allows direct XAML usage.
/// Usage => DefaultText="{Binding Model.DefaultText, Converter={converters:DefaultButtonContentConverter}}"
/// Binding is being done directly in the markup, no need for reference by key in .Resources
/// 
/// Benefits of This Approach:
/// Extension flexibility: You can easily extend this base class to create more complex converters without rewriting the logic for instance management.
/// Reusable converters: The base class allows you to define converters once and reuse them throughout the application, reducing code duplication.
/// Clean and modular code: The separation of concerns(View, ViewModels, Converter) keeps the code clean and easier to maintain.
/// 
/// Created by: tseb
/// <typeparam name="T">The type of this value converter</typeparam>
/// </summary>
public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
   where T : class, new()
{
    /// <summary>
    /// A single static instance of this value converter
    /// </summary>
    private static T mConverter = null;

    /// <summary>
    /// Provides a static instance of the value converter. 
    /// Part of MarkupExtension and is called when the XAML parser needs to retrieve the value converter.
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return mConverter ?? (mConverter = new T());
    }

    /// <summary>
    /// The method to convert one type to another
    /// </summary>
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    /// <summary>
    /// The method to convert a value back to it's source type
    /// </summary>
    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
}
