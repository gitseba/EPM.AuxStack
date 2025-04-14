using LogProcessor.Application.Abstractions;
using LogProcessor.Application.State;
using LogProcessor.Shared;
using LogProcessor.Shared.Extensions;
using LogProcessor.Shared.Models;
using LogProcessor.Wpf.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;

namespace LogProcessor.Wpf.ViewModels;

/// <summary>
/// Purpose: Represent ALL files.
///          For a single item/file <see cref="FileViewModel"/>
/// Created by: tseb
/// </summary>
public class FilesViewModel : BaseViewModel
{
    private readonly IFilesMediator _filesMediator;

    #region BindingProperties
    /// <summary>
    /// Root source path (where all the files exist)
    /// </summary>
    public string RootPathSelected { get; set; }

    /// <summary>
    /// Notify Ui if all files are checked
    /// </summary>
    public bool IsCheckAll { get; set; }

    /// <summary>
    /// File collection find on selected directory
    /// </summary>
    public ObservableCollection<FileViewModel> Files { get; set; } = [];

    /// <summary>
    /// Token used to interrupt an busy action
    /// </summary>
    public CancellationTokenSource? Cts { get; set; }
    #endregion

    #region Commands
    public ICommand SearchCommand { get; set; }
    public ICommand ClearCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand CheckAllCommand { get; set; }
    public ICommand OpenLocationCommand { get; set; }
    public ICommand SelectedCommand { get; set; }
    #endregion Commands

    public FilesViewModel(IFilesMediator filesMediator)
    {
        // Services
        _filesMediator = filesMediator ?? throw new ArgumentNullException();

        // Commands
        SearchCommand = new RelayCommand(async () => await OnSearchChainAsync());
        ClearCommand = new RelayCommand(OnClearChecked);
        CancelCommand = new RelayCommand(OnCancel);
        CheckAllCommand = new RelayCommand<bool>((input) => Files.CheckAll(input));
        OpenLocationCommand = new RelayCommand<FileViewModel>(OnOpenLocation);
        SelectedCommand = new RelayCommand<FileViewModel>(async (fileVm) => await OnSelected(fileVm));
    }

    /// <summary>
    /// Initial state
    /// </summary>
    public void InitialLoad()
    {
        // Events
        Files.CollectionChanged += (s, e) => { IsCheckAll = Files.Count == 0; };

        _filesMediator.Subscribe(stateOption: FilesStateEnum.RootPathSelected,
          action: (rootPath) => RootPathSelected = rootPath.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Open Dialog and browse for target directory (that holds files ready to be processed)
    /// </summary>
    private string OpenDialog() => _filesMediator.OpenDialog() ?? string.Empty;

    /// <summary>
    /// Trigger when user double click on file to open location
    /// </summary>
    /// <param name="fileVm"></param>
    private void OnOpenLocation(FileViewModel fileVm)
    {
        if (fileVm is null || fileVm.FileModel is null)
            return;

        _filesMediator.OpenLocation(path: fileVm?.FileModel?.Path);
    }

    /// <summary>
    /// Trigger when another file was selected
    /// </summary>
    /// <param name="fileVm"></param>
    private async Task OnSelected(FileViewModel fileVm)
    {
        if (fileVm?.FileModel is null)
            return;

        using (Cts = new CancellationTokenSource())
        {
            try
            {
                await _filesMediator.SelectedAsync(fileVm.FileModel.Path, Cts.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred: {ex.Message}");
            }
            finally
            {
                Cts?.Cancel();
                Cts = null;
            }
        }
    }

    /// <summary>
    /// Cancel any Ui performing action
    /// </summary>
    private void OnCancel() => Cts?.Cancel();

    /// <summary>
    /// Search for files in the selected directory
    /// </summary>
    /// <param name="path">Directory where to look for files</param>
    /// <param name="extensionPattern">File extension pattern E.g: *.txt</param>
    /// <param name="token">Operation Cancelation Token</param>
    public async Task SearchAsync(string path, string extensionPattern, CancellationToken ctsToken = default)
    {
        try
        {
            var files = _filesMediator.SearchFilesAsync(path, extensionPattern, maxReccursionDepth: 3, ctsToken);
            int index = 1;

            await foreach (var file in files)
            {
                // UI thread safety for ObservableCollection updates
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() => Files.Add(new FileViewModel
                {
                    FileModel = new FileModel { Path = file.FullName },
                    Index = index++
                }), DispatcherPriority.Background, ctsToken);
            }
        }
        catch (TaskCanceledException ex)
        {
            Debug.WriteLine("Search action was canceled." + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception occurred: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Wrapper method for combining multiple operations  
    /// </summary>
    private async Task OnSearchChainAsync()
    {
        string selectedPath = OpenDialog();

        // If the user cancels the dialog or doesn't select a valid path => exit
        if (string.IsNullOrEmpty(selectedPath))
        {
            Debug.WriteLine("No valid path selected. Search action is canceled.");
            return;
        }

        if (Files.Count != 0) OnClearAll();

        // Start search action
        using (Cts = new CancellationTokenSource())
        {
            try
            {
                await RunnableCommand.ExecuteAsync(() => IsEnabled && IsLoading,  // Get the current flag state
                     value => { IsEnabled = value; IsLoading = value; },          // Set the flag state
                     async (action) =>
                     {
                         await SearchAsync(selectedPath, GlobalSettings.FileTargetExtensionPattern, Cts.Token);
                     },
                     Cts.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred: {ex.Message}");
            }
            finally
            {
                Cts?.Cancel();
                Cts = null;
            }
        }
    }

    /// <summary>
    /// Clear all displayed files
    /// </summary>
    private void OnClearAll()
    {
        Files.CheckAll(true);
        Files.RemoveChecked();

        if (Files?.Count == 0)
        {
            _filesMediator.ClearFiles();
        }
    }

    /// <summary>
    /// Clear only the checked files
    /// </summary>
    private void OnClearChecked()
    {
        // Clear was pressed but no item was selected
        if (!Files.Any(x => x.IsChecked)) return;

        var checkedFiles = Files.Where(f => f.IsChecked).Select(f => f.FileModel?.Path).ToArray();

        if (Files.All(f => f.IsChecked)) OnClearAll();

        Files.RemoveChecked();

        _filesMediator.ClearFiles();
    }
}
