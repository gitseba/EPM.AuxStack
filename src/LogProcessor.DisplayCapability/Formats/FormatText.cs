using System.Text.RegularExpressions;
using System.Windows.Media;

namespace LogProcessor.DisplayCapability.Formats;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public static class FormatText
{
    public static Brush ConsoleColorToBrush(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => new SolidColorBrush(Colors.Black),
            ConsoleColor.DarkBlue => new SolidColorBrush(Colors.DarkBlue),
            ConsoleColor.DarkGreen => new SolidColorBrush(Colors.DarkGreen),
            ConsoleColor.DarkCyan => new SolidColorBrush(Colors.DarkCyan),
            ConsoleColor.DarkRed => new SolidColorBrush(Colors.DarkRed),
            ConsoleColor.DarkMagenta => new SolidColorBrush(Colors.DarkMagenta),
            ConsoleColor.DarkYellow => new SolidColorBrush(Colors.DarkGoldenrod),
            ConsoleColor.Gray => new SolidColorBrush(Colors.Gray),
            ConsoleColor.DarkGray => new SolidColorBrush(Colors.DimGray),
            ConsoleColor.Blue => new SolidColorBrush(Colors.Blue),
            ConsoleColor.Green => new SolidColorBrush(Colors.Green),
            ConsoleColor.Cyan => new SolidColorBrush(Colors.Cyan),
            ConsoleColor.Red => new SolidColorBrush(Colors.Red),
            ConsoleColor.Magenta => new SolidColorBrush(Colors.Magenta),
            ConsoleColor.Yellow => new SolidColorBrush(Colors.Yellow),
            ConsoleColor.White => new SolidColorBrush(Colors.White),
            _ => new SolidColorBrush(Colors.White) // Default to White if no match
        };
    }

    /// <summary>
    /// Write text on console using custom colors for specific strings
    /// Format: $"Hello [{ConsoleColor.Green} World]!" 
    /// </summary>
    /// <param name="inputText">String text using description format</param>
    public static string ColorFormatCapability(string inputText)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(inputText);
        
        try
        {
            // Extract text between | marker ( E.g: some text | this will be extracted | some more text) 
            Match match = Regex.Match(inputText, @"\|(.*?)\|");

            if (match.Success)
            {
                string result = match.Groups[1].Value;


                Console.WriteLine(result); // Output
                return result;
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        return string.Empty;
    }
}
