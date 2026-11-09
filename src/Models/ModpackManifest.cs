using System.Text.Json.Serialization;

namespace CustomModpackInstaller.Models;

/// <summary>
/// Direct mapping of the GitHub-hosted JSON manifest.
/// </summary>
public record ModpackManifest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("mc_version")]
    public string MinecraftVersion { get; set; } = string.Empty;

    [JsonPropertyName("modloader")]
    public string ModLoader { get; set; } = string.Empty;

    [JsonPropertyName("modloader_version")]
    public string ModLoaderVersion { get; set; } = string.Empty;
    
    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; } = string.Empty;

    [JsonPropertyName("sha256")]
    public string Sha256Hash { get; set; } = string.Empty;

    [JsonPropertyName("recommended_ram_mb")]
    public int RecommendedRamMb { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    // Optional field
    [JsonPropertyName("changelog_url")]
    public string? ChangelogUrl { get; set; } = string.Empty;
}