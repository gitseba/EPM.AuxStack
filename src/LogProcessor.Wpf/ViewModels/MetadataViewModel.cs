using LogProcessor.Application.Abstractions;
using LogProcessor.Application.EventModels;
using LogProcessor.Application.State;
using LogProcessor.Infrastructure.Mediators;
using System.ComponentModel;
using System.Windows.Data;

namespace LogProcessor.Wpf.ViewModels;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public class MetadataViewModel : BaseViewModel
{
    private readonly IFilesMediator _filesMediator;

    #region Binding properties
    /// <summary>
    /// File Metadata from content
    /// </summary>
    public ICollectionView? FilteredMetadata { get; set; }
    #endregion

    public MetadataViewModel(IFilesMediator fileMediator)
    {
        _filesMediator = fileMediator;
    }

    /// <summary>
    /// Initial state
    /// </summary>
    public void InitialLoad()
    {
        // Events
        _filesMediator.StateManagerSubscription(stateOption: FilesStateEnum.FileMetadata,
            action: (metadata) => UpdateMetadata((Dictionary<string, string>)metadata));
    }

    /// <summary>
    /// Update file metadata after file selection
    /// </summary>
    /// <param name="metadata">Extracted metadata about the file</param>
    private void UpdateMetadata(Dictionary<string, string> metadata)
    {
        if (metadata is null)
        {
            FilteredMetadata = null;
            return;
        }

        FilteredMetadata = CollectionViewSource.GetDefaultView(metadata);
        FilteredMetadata.Filter = item =>
        {
            KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)item;
            return !string.IsNullOrWhiteSpace(kvp.Value);
        };
    }
}
