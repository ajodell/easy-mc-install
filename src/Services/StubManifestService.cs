namespace CustomModpackInstaller.Services;

using CustomModpackInstaller.Models;
using System.Threading;
using System.Threading.Tasks;

public class StubManifestService : IManifestService
{
    /// <summary>
    /// Fetches and parses a ModpackManifest from the given URL.
    /// Returns a failure result if the URL is unreachable or the JSON is malformed.
    /// </summary>
    public async Task<InstallResult<ModpackManifest>> FetchManifestAsync(
        string url, CancellationToken cancellationToken = default)
    {
        // Simulate download delay
        await Task.Delay(1000, cancellationToken);

        // Hardcoded fake data
        var fakeManifest = new ModpackManifest
        {
           Name = "Test Modpack",
            MinecraftVersion = "1.20.1",
            ModLoader = "fabric",
            ModLoaderVersion = "0.15.7",
            DownloadUrl = "https://example.com/modpack.zip",
            Sha256Hash = "abc123",
            RecommendedRamMb = 4096,
            Version = "1.0.0",
            ChangelogUrl = null
        };

        return InstallResult<ModpackManifest>.Success(fakeManifest);
    }
}