using LogProcessor.Strategy.Enums;
using LogProcessor.Strategy.Metadata.Extractors;

namespace LogProcessor.Strategy.Metadata.Services;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class FileMetadataService : IFileMetadataService
{
    /// <summary>
    /// Metadata information about the file extracted from content
    /// </summary>
    public Dictionary<LogMetadataEnum, string> Metadata { get; set; } = [];

    public async Task<Dictionary<LogMetadataEnum, string>> ProcessAsync(string filePath,
        CancellationToken ctsToken = default)
    {
        var textLines = new List<string>();

        // Read only the first 500 lines of code. Metadata are in the first lines
        using (var reader = new StreamReader(filePath))
        {
            // This will approach will be more expensive
            //while (!reader.EndOfStream)
            //{
            //    textLines.Add(reader.ReadLine() ?? string.Empty);
            //}

            // Instead: I know that most of metadata can be found in the first 500 lines
            for (int i = 0; i < 500 && !reader.EndOfStream; i++)
            {
                textLines.Add(reader.ReadLine() ?? string.Empty);
            }
        }

        if (textLines is null || textLines.Count == 0)
        {
            return Metadata;
        }

        await Task.Run(async () =>
        {
            try
            {
                if (Metadata.Count != 0)
                {
                    Metadata.Clear();
                }

                Metadata.TryAdd(LogMetadataEnum.FileName, Path.GetFileName(filePath));
                if (filePath.Contains("SerialPort"))
                {
                    return;
                }

                IList<IMetadataExtractor> registered = RegisteredExtractors.Get();
                foreach (var metadataInfo in registered)
                {
                    var extract = metadataInfo.Extract(textLines);
                    Metadata.TryAdd(extract.Item1, extract.Item2);
                }
                EndLogLineExtractor endLogLineExtractor = new();
                var endLogLine = endLogLineExtractor.Extract(filePath);
                Metadata.TryAdd(endLogLine.Item1, endLogLine.Item2);

                TotalLinesExtractor totalLinesExtractor = new();
                var totalLines = await totalLinesExtractor.ExtractAsync(filePath, ctsToken);
                Metadata.TryAdd(totalLines.Item1, totalLines.Item2);
            }
            catch (Exception ex)
            {
                throw;
            }
        }, ctsToken);

        return Metadata;
    }
}
