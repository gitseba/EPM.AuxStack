namespace LogProcessor.Strategy.Models;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class ContentModel
{
    /// <summary>
    /// Previous Row line where the result is significant
    /// </summary>
    public string PreviousLine { get; set; }

    /// <summary>
    /// Row line of the file where the result is significant
    /// </summary>
    public string Line { get; set; }

    /// <summary>
    /// # Number of the semnificative line 
    /// </summary>
    public string LineNumber { get; set; }

    /// <summary>
    /// Difference between line timestamps 
    /// </summary>
    public string Difference { get; set; }
}
