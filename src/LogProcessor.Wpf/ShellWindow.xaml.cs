using System.Windows;
using LogProcessor.Wpf.Features;

namespace LogProcessor.Wpf;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public partial class ShellWindow : Window
{
    public ShellWindow()
    {
        InitializeComponent();

        // Resizing shell window, without this class, it's expanding screen edges.
        // This class keeps the shell betwen screen boundaries.
        _ = new WindowResizer(this);
    }
}
