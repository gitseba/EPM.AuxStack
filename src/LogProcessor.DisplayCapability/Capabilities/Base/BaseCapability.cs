using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;

namespace LogProcessor.DisplayCapability.Capabilities;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class BaseCapability
{
    protected readonly FlowDocument _flowDocument;

    public BaseCapability(FlowDocument flowDocument)
    {
        _flowDocument = flowDocument;
    }

    public virtual void Display(IEnumerable<string> input)
    {

    }

    public FlowDocument DefaultDisplay(string input)
    {
        _flowDocument.Blocks.Add(new Paragraph(new Run(input)));
        return _flowDocument;
    }

    public FlowDocument ColorLineDisplay(string input, Brush brushColor)
    {
        _flowDocument.Blocks.Add(new Paragraph(new Run(input) { Foreground = brushColor }));
        return _flowDocument;
    }

    public FlowDocument ColorLogLevelCapability(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;

        // Regex pattern to find [INF] or any other log level inside brackets
        Match match = Regex.Match(input, @"\[[A-Z]+\]");

        if (match.Success)
        {
            string before = input.Substring(0, match.Index);       // Text before [INF]
            string logLevel = match.Value;                         // Extracted [INF]
            string after = input.Substring(match.Index + logLevel.Length); // Text after [INF]

            Paragraph paragraph = new Paragraph();

            // Normal text
            Run normalText1 = new Run(before);
            paragraph.Inlines.Add(normalText1);

            // Colored [INF]
            Run coloredText = new Run(logLevel);
            if (logLevel.Contains("INF"))
            {
                coloredText.Foreground = Brushes.White; // Change color here
            }
            else if (logLevel.Contains("WRN"))
            {
                coloredText.Foreground = Brushes.Yellow;
            }
            else if (logLevel.Contains("ERR"))
            {
                coloredText.Foreground = Brushes.Red;
            }
            else
            {
                coloredText.Foreground = Brushes.LightBlue;
            }

            paragraph.Inlines.Add(coloredText);

            // Normal text after
            Run normalText2 = new Run(after);
            paragraph.Inlines.Add(normalText2);

            _flowDocument.Blocks.Add(paragraph);
            return _flowDocument;
        }
        else
        {
            // print normally
            return DefaultDisplay(input);
        }
    }

    public void ClearEntriesCapability()
    {
        if (_flowDocument is null || _flowDocument.Blocks.Count == 0)
        {
            return;
        }
        _flowDocument.Blocks.Clear();
    }
}
