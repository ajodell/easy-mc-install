using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CustomModpackInstaller.ViewModels;
using CustomModpackInstaller.Views;
using CustomModpackInstaller.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace CustomModpackInstaller;

public partial class App : Application
{
    // Collection of services
    public static IServiceProvider Services { get; private set; } = null!;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Collection of services being registered
            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // HTTP Client
        services.AddSingleton<HttpClient>();

        // Manifest
        services.AddSingleton<IManifestService, StubManifestService>();

        // Main View Model
        services.AddTransient<MainViewModel>();
    }
}