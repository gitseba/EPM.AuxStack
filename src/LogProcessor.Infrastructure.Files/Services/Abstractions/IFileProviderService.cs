using System.Runtime.CompilerServices;

namespace LogProcessor.Infrastructure.Files.Services;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public interface IFileProviderService
{
    IEnumerable<FileInfo> GetFiles(string path, string pattern, int maxRecursionDepth = 3);

    IEnumerable<FileInfo> GetUniqueFiles(string path, string pattern, int maxRecursionDepth = 3);

    IAsyncEnumerable<FileInfo> GetFilesAsync(string path, string pattern, int maxRecursionDepth = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default);

    IAsyncEnumerable<FileInfo> GetUniqueFilesAsync(string path, string pattern, int maxRecursionDepth = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default);
}
