using LogProcessor.Application.Abstractions;
using LogProcessor.Application.State;
using LogProcessor.Infrastructure.Dialogs.Services;
using LogProcessor.Infrastructure.Files.Services;
using LogProcessor.Infrastructure.Mediators;
using LogProcessor.Strategy.Metadata.Services;
using LogProcessor.Wpf.ViewModels;
using LogProcessor.Wpf.Views;
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
        // Singletons
        containerRegistry.RegisterSingleton<FilesStateManager>();

        // Services
        containerRegistry.Register<IFileProviderService, FileProviderService>();
        containerRegistry.Register<IWindowsDialogService, WindowsDialogService>();

        containerRegistry.Register<IFilesMediator, FilesMediator>();
        containerRegistry.Register<IFileMetadataService, FileMetadataService>();

        // Models
    }

    protected override Window CreateShell() => Container.Resolve<ShellWindow>();

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();

        // Components
        ViewModelLocationProvider.Register<FilesView, FilesViewModel>();
        ViewModelLocationProvider.Register<MetadataView, MetadataViewModel>();

        // Windows
        ViewModelLocationProvider.Register<ShellWindow, ShellViewModel>();
    }
}
