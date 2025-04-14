using LogProcessor.Application.EventModels;
using LogProcessor.Shared.Events;

namespace LogProcessor.Application.State;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class FilesStateManager
{
    public Dictionary<FilesStateEnum, List<Action<object>>> StateManager { get; } = [];

    public FilesStateManager()
    {
        RegisterSubscriptionEvents();
    }

    /// <summary>
    /// Subscribe to events coming from Mediator
    /// </summary>
    private void RegisterSubscriptionEvents()
    {
        // Event subscriptions from Mediator
        EventGlobalInstance.Instance.Event<RootPathSelectedEvent>()
              .Subscribe(async (ev) => SelectedRootPath = ev.Path);

        EventGlobalInstance.Instance.Event<FileSelectedEvent>()
               .Subscribe(async (ev) => SelectedFilePath = ev.FilePath);

        EventGlobalInstance.Instance.Event<FileMetadataUpdateEvent>()
               .Subscribe(async(ev) => Metadata = ev.Metadata);

        EventGlobalInstance.Instance.Event<FilesClearEvent>()
             .Subscribe(async(ev) => Reset());
    }

    /// <summary>
    /// Generic method to update state and notify subscribers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Action identifier key</param>
    /// <param name="property">Property that holds the value state</param>
    /// <param name="value">Value</param>
    private void UpdateState<T>(FilesStateEnum key, ref T property, T value)
    {
        property = value;
        if (StateManager.TryGetValue(key, out var actions))
        {
            foreach (var action in actions)
            {
                action?.Invoke(value);
            }
        }
    }

    /// <summary>
    /// Subscribe to a state
    /// </summary>
    /// <param name="stateOption" <see cref="FilesStateEnum"/>></param>
    /// <param name="action"> Action/Method that will be triggered on state updated</param>
    public void StateSubscription(FilesStateEnum stateOption, Action<object> action)
    {
        if (!StateManager.ContainsKey(stateOption))
            StateManager[stateOption] = [];

        StateManager[stateOption].Add(action);
    }

    /// <summary>
    /// Reset properties to default state
    /// </summary>
    public void Reset()
    {
        _selectedRootPath = string.Empty;
        SelectedFilePath = string.Empty;
        Metadata = null;
        IsClear = true;
    }

    /// <summary>
    /// Reset properties to default state
    /// </summary>
    public void ClearSelectedReset(string[] filePaths)
    {
        // ToDo: In future maybe filePaths will come in handy.
        // For moment it's not wrong or useful to know which files were marked as clear
        SelectedFilePath = string.Empty;
        Metadata = null;
    }

    /// <summary>
    /// Represent selection of a source path where files exist
    /// </summary>
    private string? _selectedRootPath;
    public string? SelectedRootPath
    {
        get => _selectedRootPath;
        set => UpdateState(FilesStateEnum.RootPathSelected, ref _selectedRootPath, value);
    }

    /// <summary>
    /// Represent selection of a file
    /// </summary>
    private string? _selectedFilePath;
    public string? SelectedFilePath
    {
        get => _selectedFilePath;
        set => UpdateState(FilesStateEnum.FilePathSelected, ref _selectedFilePath, value);
    }

    /// <summary>
    /// Represent metadata content of a file
    /// </summary>
    private Dictionary<string, string> _metadata;
    public Dictionary<string, string>? Metadata
    {
        get => _metadata;
        set => UpdateState(FilesStateEnum.FileMetadata, ref _metadata, value);
    }

    /// <summary>
    /// Represent metadata content of a file
    /// </summary>
    private bool _isClear;
    public bool IsClear
    {
        get => _isClear;
        set => UpdateState(FilesStateEnum.Clear, ref _isClear, value);
    }
}
