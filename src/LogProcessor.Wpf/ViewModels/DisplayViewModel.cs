using LogProcessor.Application.State;
using LogProcessor.DisplayCapability.Abstractions;
using LogProcessor.Strategy.Models;
using System.Windows.Documents;

namespace LogProcessor.Wpf.ViewModels;

/// <summary>
/// Purpose: Only to handle messages for Ui.
///          It should't handle any business logic, ONLY receiving inputs
/// Created by: tseb
/// </summary>
public class DisplayViewModel : BaseViewModel
{
    private readonly StrategyStateManager _strategyStateManager;
    private readonly FilesStateManager _filesStateManager;
    private readonly IDisplayCapabilityService _displayCapabilityService;

    #region Binding properties
    /// <summary>
    /// Main object that serves the content to the Ui
    /// </summary>
    public FlowDocument? FlowDocument { get; set; } = new FlowDocument();
    #endregion

    public DisplayViewModel(
        StrategyStateManager strategyStateManager,
        FilesStateManager filesStateManager,
        IDisplayCapabilityService displayCapabilityService)
    {
        _strategyStateManager = strategyStateManager;
        _filesStateManager = filesStateManager;
        _displayCapabilityService = displayCapabilityService;
    }

    /// <summary>
    /// Initial load after constructor
    /// </summary>
    public void InitialLoad()
    {
        _strategyStateManager
            .StateSubscription(StrategyStateEnum.IsProcessing, (isProcessing) => IsLoading = (bool)isProcessing);

        _strategyStateManager.StateSubscription(StrategyStateEnum.ProcessedResults, async (results) =>
        {
            FlowDocument?.Blocks.Clear();
            var flowDocResult = await _displayCapabilityService.DisplayAsync(((ProcessedModel)results));

            var blocksToAdd = await Task.Run(() => flowDocResult?.Blocks.ToList());
            int batchSize = 100;

            for (int i = 0; i < blocksToAdd?.Count; i += batchSize)
            {
                var batch = blocksToAdd.Skip(i).Take(batchSize);
                foreach (var block in batch)
                    FlowDocument?.Blocks.Add(block);

                await Task.Delay(1); // Let UI breathe
            }

        });

        _filesStateManager.StateSubscription(FilesStateEnum.Clear, (isClear) =>
        {
            FlowDocument?.Blocks.Clear();
        });
    }
}
