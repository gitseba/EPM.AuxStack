using LogProcessor.Strategy.Enums;

namespace LogProcessor.Strategy.Metadata.Extractors;

/// <summary>
/// Purpose: Extract the total number of lines in the file
/// Created by: tseb
/// </summary>
public class TotalLinesExtractor
{
    public async Task<(LogMetadataEnum, string)> ExtractAsync(string filePath, CancellationToken ctsToken)
    {
        int total = 0;

        byte[] buffer = new byte[1024 * 1024]; // 1Mb buffer

        using var stream = File.OpenRead(filePath);
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, ctsToken)) > 0)
        {
            for (int i = 0; i < bytesRead; i++)
            {
                if (buffer[i] == '\n') // hit the last empty in order to count the line
                    total++;
            }
        }

        return (LogMetadataEnum.TotalLinesCount, total.ToString());
    }
}
