namespace LogProcessor.Shared.Models;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class FileModel
{
    /// <summary>
    /// // Full path of the file
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Name of file from path
    /// </summary>
    public string Name => System.IO.Path.GetFileName(Path ?? string.Empty);

    /// <summary>
    /// File extension
    /// </summary>
    public string? Extension => Path != null ? System.IO.Path.GetExtension(Path) : string.Empty;

    /// <summary>
    /// Last time when content of file was modified
    /// </summary>
    public DateTime ModifiedAt => Path != null ? File.GetLastWriteTime(Path) : default;

    /// <summary>
    /// Size of the file
    /// </summary>
    public long Size => Path != null ? new FileInfo(Path).Length : default;
}
