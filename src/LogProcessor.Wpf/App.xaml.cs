using LogProcessor.Wpf.ViewModels;
using System.Windows;

namespace LogProcessor.Wpf;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Services
        // SAMPLE: containerRegistry.Register<IService, Service>();

        // Models
        // SAMPLE: containerRegistry.Register<TModel>();
    }

    protected override Window CreateShell() => Container.Resolve<ShellWindow>();


    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();

        // Windows
        ViewModelLocationProvider.Register<ShellWindow, ShellViewModel>();

        // Components
        // SAMPLE: ViewModelLocationProvider.Register<PanelView, PanelViewModel>();
    }
}
