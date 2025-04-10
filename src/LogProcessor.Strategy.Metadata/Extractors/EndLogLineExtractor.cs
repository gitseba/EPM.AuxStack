using LogProcessor.Strategy.Enums;
using LogProcessor.Strategy.Utils;
using System.Text;
using System.Text.RegularExpressions;

namespace LogProcessor.Strategy.Metadata.Extractors;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class EndLogLineExtractor
{
    public (LogMetadataEnum, string) Extract(string filePath)
    {
        string pattern = @"\b\d{2}:\d{2}:\d{2}\.\d{3}\b"; // Regex for HH:MM:SS.mmm format

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            fs.Seek(0, SeekOrigin.End);

            long position = fs.Length - 1;
            var lineBytes = new List<byte>();

            while (position >= 0)
            {
                fs.Seek(position, SeekOrigin.Begin);
                int b = fs.ReadByte();

                if (b == '\n' || position == 0)
                {
                    if (position == 0) lineBytes.Insert(0, (byte)b); // capture first byte if needed

                    string line = Encoding.UTF8.GetString(lineBytes.ToArray()).TrimEnd('\r', '\n');
                    if (Regex.IsMatch(line, pattern))
                    {
                        var result = TimeUtils.ExtractTime(line);
                        return (LogMetadataEnum.LogEnd, result);
                    }

                    lineBytes.Clear();
                }
                else
                {
                    lineBytes.Insert(0, (byte)b);
                }

                position--;
            }
        }

        return (LogMetadataEnum.LogEnd, string.Empty);
    }
}
