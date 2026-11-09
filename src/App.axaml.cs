using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CustomModpackInstaller.ViewModels;
using CustomModpackInstaller.Views;
using CustomModpackInstaller.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomModpackInstaller;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Collection of services being registered
            var services = new ServiceCollection()
                .AddSingleton<IManifestService, StubManifestService>()
                .AddTransient<MainViewModel>()
                .BuildServiceProvider();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}