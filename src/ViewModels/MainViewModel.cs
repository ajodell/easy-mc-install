using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomModpackInstaller.Services;

namespace CustomModpackInstaller.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    // The manifest service
    private readonly IManifestService _manifestService;

    // The manifest URL
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FetchManifestCommand))]
    public partial string ManifestUrl { get; set; } = "";

    // Constructor for MainViewModel
    public MainViewModel(IManifestService manifestService)
    {
        _manifestService = manifestService;
    }

    // Method for checking if there's text in the box
    private bool CanGetUrl() => !string.IsNullOrWhiteSpace(ManifestUrl);

    // Command for getting the file from the URL
    [RelayCommand (CanExecute = nameof(CanGetUrl))]
    private async Task FetchManifest()
    {
        var result = await _manifestService.FetchManifestAsync(ManifestUrl);
        Console.WriteLine(result.IsSuccess ? $"Fetched {result.Value?.Name}" : result.ErrorMessage);
    }
}
