using System.Runtime.InteropServices;

namespace LogProcessor.Infrastructure.Services;

/// <summary>
/// Purpose: https://stackoverflow.com/questions/14600987/code-to-open-windows-explorer-or-focus-if-exists-with-file-selected
/// Created by: tseb
/// </summary>
public static class WindowsFileManager
{
    public static void OpenFolderAndSelectFile(string filePath)
    {
        if (filePath == null)
            throw new ArgumentNullException(nameof(filePath));

        IntPtr pidl = ILCreateFromPathW(filePath);
        SHOpenFolderAndSelectItems(pidl, 0, IntPtr.Zero, 0);
        ILFree(pidl);
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr ILCreateFromPathW(string pszPath);

    [DllImport("shell32.dll")]
    private static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, int cild, IntPtr apidl, int dwFlags);

    [DllImport("shell32.dll")]
    private static extern void ILFree(IntPtr pidl);
}
