using Microsoft.WindowsAPICodePack.Dialogs;

namespace LogProcessor.Infrastructure.Dialogs.Services;

/// <summary>
/// Purpose: Windows dialog service
/// Created by: tseb
/// </summary>
public class WindowsDialogService : IWindowsDialogService
{
    public required CommonOpenFileDialog Dialog { get; set; }

    public string OpenDialog(bool isFolderPicker = true)
    {
        var getDrives = DriveInfo.GetDrives();
        Dialog = new()
        {
            InitialDirectory = getDrives[getDrives.Length - 1].RootDirectory.FullName,
            IsFolderPicker = isFolderPicker
        };

        var result = Dialog.ShowDialog() == CommonFileDialogResult.Ok ? Dialog.FileName : string.Empty;
        return result;
    }
}
