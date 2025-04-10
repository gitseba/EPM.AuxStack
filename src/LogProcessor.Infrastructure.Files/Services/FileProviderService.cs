using System.Runtime.CompilerServices;

namespace LogProcessor.Infrastructure.Files.Services;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class FileProviderService : IFileProviderService, IFileService
{
    /// <summary>
    /// Provide File enumeration options during search
    /// </summary>
    /// <param name="maxRecursionDepth">How deep in folders should enumerate</param>
    private static EnumerationOptions GetDefaultEnumerationOptions(int maxRecursionDepth)
    {
        return new EnumerationOptions
        {
            AttributesToSkip = FileAttributes.Compressed | FileAttributes.Hidden
                               | FileAttributes.System | FileAttributes.Encrypted
                               | FileAttributes.Offline | FileAttributes.Temporary,
            RecurseSubdirectories = true,
            ReturnSpecialDirectories = false,
            MaxRecursionDepth = maxRecursionDepth,
            IgnoreInaccessible = true
        };
    }

    public IEnumerable<FileInfo> GetFiles(string path, string pattern, int maxRecursionDepth = 3)
    {
        return new DirectoryInfo(path)
            .EnumerateFiles(pattern, GetDefaultEnumerationOptions(maxRecursionDepth));
    }

    public IEnumerable<FileInfo> GetUniqueFiles(
           string path,
           string pattern,
           int maxRecursionDepth = 3)
    {
        var uniqueFiles = new HashSet<string>(); // Store file paths to ensure uniqueness

        foreach (var file in GetFiles(path, pattern, maxRecursionDepth))
        {
            if (uniqueFiles.Add(file.FullName)) 
            {
                yield return file;
            }
        }

        uniqueFiles.Clear();
        uniqueFiles = null;
    }

    /// <summary>
    /// Get all the items from provided path in async enumerable method
    /// </summary>
    /// <param name="path">Where to search for files</param>
    /// <param name="patterns">Pattern to search for one type of file or multiple. [E.g multiple files pattern => "*.pdf|*.jpeg"]</param>
    /// <param name="cancellationToken">Token that will stop Async Enumeration process</param>
    public async IAsyncEnumerable<FileInfo> GetFilesAsync(
            string path, string pattern,
            int maxRecursionDepth = 3,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var files = new DirectoryInfo(path)
            .EnumerateFiles(pattern, GetDefaultEnumerationOptions(maxRecursionDepth));

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return file;
            await Task.Yield(); // Keeps execution responsive
        }
    }

    /// <summary>
    /// Provide unique files asynchronously
    /// </summary>
    /// <param name="path">Where to search for files</param>
    /// <param name="patterns">Pattern to search for one type of file or multiple. [E.g multiple files pattern => "*.pdf|*.jpeg"]</param>
    /// <param name="maxRecursionDepth">How deep in folders should the search go</param>
    /// <param name="cancellationToken">Token that will stop Async Enumeration process</param>
    /// <returns></returns>
    public async IAsyncEnumerable<FileInfo> GetUniqueFilesAsync(
            string path,
            string pattern,
            int maxRecursionDepth = 3,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var uniqueFiles = new HashSet<string>(); // Store file paths to ensure uniqueness

        var results = GetFilesAsync(path, pattern, maxRecursionDepth, cancellationToken);

        await foreach (var file in results)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (uniqueFiles.Add(file.Name))  
            {
                yield return file;
            }
        }
    }
}
