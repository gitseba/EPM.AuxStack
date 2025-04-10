namespace LogProcessor.Application.Abstractions;

/// <summary>
/// Purpose: Provide external functinoality 
/// Created by: tseb
/// </summary>
public interface IFilesMediator : IStateManagerSubscribe
{
    string OpenDialog();
    void OpenLocation(string path);
    Task SelectedAsync(string filePath, CancellationToken ctsToken = default);
    IAsyncEnumerable<FileInfo> SearchFilesAsync(
        string path, string extension, int maxReccursionDepth, CancellationToken ctsToken = default);
}
