using LogProcessor.Application.Abstractions;
using LogProcessor.Shared.Models;

namespace LogProcessor.Wpf.ViewModels;

/// <summary>
/// Purpose: Represent Ui inform actions regarding one file item.
/// Created by: tseb
/// </summary>
public class FileViewModel : BaseViewModel, ICheckable
{
    /// <summary>
    /// Represents file details
    /// </summary>
    public FileModel? FileModel { get; set; }

    /// <summary>
    /// Inform the Ui if the file should be enabled
    /// </summary>
    public new bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Inform the Ui if the file is selected 
    /// </summary>
    public bool IsChecked { get; set; } = false;

    /// <summary>
    /// Inform the Ui if the file is selected 
    /// </summary>
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// Inform the Ui if the file is focused 
    /// </summary>
    public bool IsFocused { get; set; } = false;

    /// <summary>
    /// Represent file index in collection
    /// </summary>
    public int Index { get; set; }
}
