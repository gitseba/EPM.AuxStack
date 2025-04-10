namespace LogProcessor.Infrastructure.Dialogs.Services;

/// <summary>
/// Purpose: Abstraction for Dialog interaction
/// Created by: tseb
/// </summary>
public interface IWindowsDialogService
{
    string OpenDialog(bool isFolderPicker = true);
}
