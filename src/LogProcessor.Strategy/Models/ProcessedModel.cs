using LogProcessor.Strategy.Abstractions;

namespace LogProcessor.Strategy.Models;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class ProcessedModel
{
    /// <summary>
    /// Strategy used to process
    /// </summary>
    public IStrategy Strategy { get; set; }

    /// <summary>
    /// Selected Option for processing
    /// </summary>
    public object SelectedOption { get; set; }

    /// <summary>
    /// Lines that were processed from file
    /// </summary>
    public IList<ContentModel>? ProcessedLines { get; set; } = [];
}
