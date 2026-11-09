namespace CustomModpackInstaller.Services;

using CustomModpackInstaller.Models;
using System.Threading;
using System.Threading.Tasks;

public interface IManifestService
{
    /// <summary>
    /// Fetches and parses a ModpackManifest from the given URL.
    /// Returns a failure result if the URL is unreachable or the JSON is malformed.
    /// </summary>
    Task<InstallResult<ModpackManifest>> FetchManifestAsync(
        string url, CancellationToken cancellationToken = default);
}