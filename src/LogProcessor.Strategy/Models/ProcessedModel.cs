namespace LogProcessor.Strategy.Models;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class ProcessedModel
{
    /// <summary>
    /// Lines that were processed from file
    /// </summary>
    public IList<ContentModel>? ProcessedLines { get; set; } = [];
}
