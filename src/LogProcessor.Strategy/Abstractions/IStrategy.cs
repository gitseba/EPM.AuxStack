using LogProcessor.Strategy.Models;

namespace LogProcessor.Strategy.Abstractions;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IStrategy
{
    Task<ProcessedModel?> ProcessAsync(object input, IList<string> textLines, CancellationToken ctsToken = default);
}
