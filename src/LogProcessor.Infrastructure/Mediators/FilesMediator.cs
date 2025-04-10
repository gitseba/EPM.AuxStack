using LogProcessor.Application.Abstractions;
using LogProcessor.Application.EventModels;
using LogProcessor.Application.State;
using LogProcessor.Infrastructure.Dialogs.Services;
using LogProcessor.Infrastructure.Files.Services;
using LogProcessor.Infrastructure.Services;
using LogProcessor.Shared.Events;
using LogProcessor.Strategy.Metadata.Services;
using System.Runtime.CompilerServices;

namespace LogProcessor.Infrastructure.Mediators;

/// <summary>
/// Purpose: Role of Mediator is to handle services and update state manager 
/// Created by: tseb
/// </summary>
public class FilesMediator : IFilesMediator
{
    private readonly FilesStateManager _fileStateManager;
    private readonly IWindowsDialogService _dialogService;
    private readonly IFileProviderService _fileProvider;
    private readonly IFileMetadataService _fileMetadataService;

    public FilesMediator(
        FilesStateManager fileStateManager,
        IWindowsDialogService dialogService,
        IFileProviderService fileProvider,
        IFileMetadataService fileMetadataService)
    {
        _fileStateManager = fileStateManager;
        _dialogService = dialogService ?? throw new ArgumentNullException();
        _fileProvider = fileProvider ?? throw new ArgumentNullException();
        _fileMetadataService = fileMetadataService;
    }

    /// <summary>
    /// Subscribe to any state action
    /// </summary>
    /// <param name="stateOption">Available Option to subscribe to</param>
    /// <param name="action">Actino to be triggered when subscribed state changes</param>
    public void StateManagerSubscription(FilesStateEnum stateOption, Action<object> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        _fileStateManager.StateSubscription(stateOption, action);
    }

    /// <summary>
    /// Opens a Dialog to select a path
    /// </summary>
    public string OpenDialog() =>
        _dialogService.OpenDialog(isFolderPicker: true) ?? string.Empty;

    /// <summary>
    /// Open the location of the file 
    /// </summary>
    public void OpenLocation(string path)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(path);
        WindowsFileManager.OpenFolderAndSelectFile(path);
    }

    /// <summary>
    /// Search for files and notify state manager
    /// </summary>
    public async IAsyncEnumerable<FileInfo> SearchFilesAsync(
        string path, string extension, int maxRecursionDepth,
        [EnumeratorCancellation] CancellationToken ctsToken = default)
    {
        var results = _fileProvider.GetUniqueFilesAsync(path, extension, maxRecursionDepth: maxRecursionDepth, ctsToken);

        await foreach (var file in results)
        {
            yield return file;
        }
    }

    /// <summary>
    /// Selected file 
    /// </summary>
    public async Task SelectedAsync(string filePath,
        [EnumeratorCancellation] CancellationToken ctsToken = default)
    {
        EventGlobalInstance.Instance.Event<FileSelectedEvent>()
               .Publish(new FileSelectedEvent() { FilePath = filePath });

        // Process file metadata
        var metadata = await _fileMetadataService.ProcessAsync(filePath, ctsToken);
        if (metadata.Count != 0)
        {
            EventGlobalInstance.Instance.Event<FileMetadataUpdateEvent>()
               .Publish(new FileMetadataUpdateEvent() { Metadata = metadata.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value) });
        }
    }
}
