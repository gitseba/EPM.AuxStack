using LogProcessor.Application.Abstractions;
using LogProcessor.Application.State;
using LogProcessor.Strategies;
using LogProcessor.Strategy.Abstractions;
using LogProcessor.Wpf.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace LogProcessor.Wpf.ViewModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class StrategiesViewModel : BaseViewModel
{
    private readonly StrategyStateManager _strategyStateManager;
    private readonly IStrategyMediator _strategyMediator;

    #region Binding Properties
    /// <summary>
    /// Path of the single file selected
    /// </summary>
    public string FilePathSelected { get; set; }

    public IStrategy ActiveStrategy { get; set; }

    /// <summary>
    /// Flag to decide if the strategy view is visible or not
    /// </summary>
    public bool IsVisible => !string.IsNullOrEmpty(FilePathSelected);

    /// <summary>
    /// Inserted text for filtering purpose
    /// </summary>
    public string FilterText { get; set; } = "Client.Presentation.Module.Perfusion";

    public ObservableCollection<string> TimeGapOptions { get; set; }
    public string TimeGapSelectedItem { get; set; }

    public ObservableCollection<string> MainThreadTimeDifferences { get; set; }
    public string SelectedMainThreadTimeItem { get; set; }

    public ObservableCollection<CheckedViewModel> LogLevels { get; set; }
    public CheckedViewModel SelectedLogLevel { get; set; }

    /// <summary>
    /// Token used to interrupt an busy action
    /// </summary>
    public CancellationTokenSource? Cts { get; set; }
    #endregion

    #region Commands
    public ICommand TimeGapChangedCommand { get; set; }
    public ICommand MainThreadGapSelectionChangedCommand { get; set; }
    public ICommand LogLevelsChangedCommand { get; set; }
    public ICommand DropDownClosedCommand { get; set; }
    public ICommand FilterCommand { get; set; }
    #endregion

    public StrategiesViewModel(
        StrategyStateManager strategyStateManager,
        IStrategyMediator strategyMediator)
    {
        _strategyStateManager = strategyStateManager;
        _strategyMediator = strategyMediator;

        TimeGapChangedCommand = new RelayCommand<string>(async (selectedValue) => await OnTimeGapChangedAsync(selectedValue));
        MainThreadGapSelectionChangedCommand = new RelayCommand<string>(async (selectedValue) => await OnMainThreadSelectionChangedAsync(selectedValue));
        LogLevelsChangedCommand = new RelayCommand<CheckedViewModel>((checkedOption) => OnLogLevelsValueChanged(checkedOption));
        DropDownClosedCommand = new RelayCommand(async () => await OnDropDownClosedAsync());
        FilterCommand = new RelayCommand<string>(async (filter) => await OnFilterTextAsync(filter));
        _strategyMediator.Subscribe(FilesStateEnum.FilePathSelected, async (filePath) => await OnFileSelected(filePath));
        _strategyMediator.Subscribe(StrategyStateEnum.ActiveStrategy, (ev) =>
        {
            InitialSelection();
        });
    }

    /// <summary>
    /// Initial state triggered from xaml
    /// </summary>
    public void InitialLoad()
    {
        TimeGapOptions = [string.Empty, "10", "20", "30", "45", "60"];
        MainThreadTimeDifferences = [string.Empty, "1", "2", "3", "4", "5", "10", "30", "60"];
        LogLevels =
        [
            new CheckedViewModel { Content = string.Empty, IsChecked = false},
            new CheckedViewModel { Content = "WRN" , IsChecked = false},
            new CheckedViewModel { Content = "ERR" , IsChecked = false},
            new CheckedViewModel { Content = "FTL" , IsChecked = false}
        ];

        InitialSelection();
    }

    /// <summary>
    /// Default selection
    /// </summary>
    public void InitialSelection()
    {
        TimeGapSelectedItem = TimeGapOptions.First();
        SelectedMainThreadTimeItem = MainThreadTimeDifferences.First();
        SelectedLogLevel = LogLevels.First();
        UnCheckLogLevels(null);
    }

    /// <summary>
    /// Update results when new file is being selected based on active strategy and selected value
    /// </summary>
    /// <param name="filePath">selected file</param>
    /// <returns></returns>
    private async Task OnFileSelected(object filePath)
    {
        FilePathSelected = filePath?.ToString() ?? string.Empty;

        if (string.IsNullOrEmpty(FilePathSelected) || !IsVisible || _strategyStateManager.ActiveStrategy is null)
        {
            InitialLoad();
            return;
        }

        await _strategyMediator.HandleAsync(
            _strategyStateManager.ActiveStrategy,
            _strategyStateManager.ActiveStrategyValue);
    }

    /// <summary>
    /// Time gap value changed command
    /// </summary>
    /// <param name="selectedValue">Values represented by <see cref="TimeGapOptions"/></param>
    private async Task OnTimeGapChangedAsync(string selectedValue)
    {
        if (selectedValue is null)
        {
            return;
        }

        // Start search action
        using (Cts = new CancellationTokenSource())
        {
            try
            {
                ActiveStrategy = new TimeGapStrategy();
                await RunnableCommand.ExecuteAsync(() => IsEnabled && IsLoading,  // Get the current flag state
                     value => { IsEnabled = value; IsLoading = value; },          // Set the flag state
                     async (action) =>
                     {
                         await _strategyMediator
                             .HandleAsync(ActiveStrategy, selectedValue);
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
    /// Main Thread gap value changed command
    /// </summary>
    /// <param name="selectedValue">Values represented by <see cref="MainThreadTimeDifferences"/></param>
    private async Task OnMainThreadSelectionChangedAsync(string selectedValue)
    {
        if (selectedValue is null)
        {
            return;
        }

        ActiveStrategy = new MainThreadGapStrategy();

        await _strategyMediator
            .HandleAsync(ActiveStrategy, selectedValue);
    }

    /// <summary>
    /// Triggered when log levels check is being changed
    /// </summary>
    /// <param name="checkedOption"></param>
    private void OnLogLevelsValueChanged(CheckedViewModel checkedOption)
    {
        UnCheckLogLevels(checkedOption);
    }

    private void UnCheckLogLevels(CheckedViewModel checkedOption)
    {
        // Deselect all options if First option is checked
        if (checkedOption is null || checkedOption.Content == string.Empty)
        {
            foreach (var item in LogLevels)
            {
                item.IsChecked = false;
            }
        }
    }

    /// <summary>
    /// When log levels combobox is being closed, run the strategy
    /// </summary>
    private async Task OnDropDownClosedAsync()
    {
        // Start search action
        using (Cts = new CancellationTokenSource())
        {
            try
            {
                await RunnableCommand.ExecuteAsync(() => IsEnabled && IsLoading,  // Get the current flag state
                     value => { IsEnabled = value; IsLoading = value; },          // Set the flag state
                     async (action) =>
                     {
                         ActiveStrategy = new LogLevelsStrategy();
                         await _strategyMediator
                                .HandleAsync(ActiveStrategy,
                                LogLevels.Where(x => x.IsChecked).Select(x => x.Content).ToArray(),
                                Cts.Token);
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
    /// Filter based on input text
    /// </summary>
    /// <param name="filter"> inserted text </param>
    private async Task OnFilterTextAsync(string filter)
    {
        if (string.IsNullOrEmpty(filter)) return;
        
        Cts?.Cancel();
        // Start search action
        using (Cts = new CancellationTokenSource())
        {
            try
            {
                await RunnableCommand.ExecuteAsync(() => IsEnabled && IsLoading,  // Get the current flag state
                  value => { IsEnabled = value; IsLoading = value; },          // Set the flag state
                  async (action) =>
                  {
                      ActiveStrategy = new FilterStrategy();
                      await _strategyMediator
                       .HandleAsync(ActiveStrategy, filter, Cts.Token)
                       .ConfigureAwait(false);
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
}
