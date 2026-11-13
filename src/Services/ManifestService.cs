namespace CustomModpackInstaller.Services;

using CustomModpackInstaller.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class ManifestService : IManifestService
{
    private readonly HttpClient _httpClient;

    // Inject the HttpClient using the constructor
    public ManifestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    /// <summary>
    /// Fetches and parses a ModpackManifest from the given URL.
    /// Returns a failure result if the URL is unreachable or the JSON is malformed.
    /// </summary>
    public async Task<InstallResult<ModpackManifest>> FetchManifestAsync(
        string url, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response;
        // Get the data from the GitHub URL
        try
        {
            response = await _httpClient.GetAsync(url, cancellationToken);
        }
        // Network error
        catch (HttpRequestException ex)
        {
            return InstallResult<ModpackManifest>.Failure($"Manifest HTTP request error. Error: {ex.HttpRequestError}, Message: {ex.InnerException?.Message}");
        }
        // Request cancelled by user or timed out
        catch (TaskCanceledException)
        {
            return InstallResult<ModpackManifest>.Failure("Manifest download was canceled or timed out");
        }

        // Server responded but with an undesirable status code
        if (!response.IsSuccessStatusCode)
        {
            return InstallResult<ModpackManifest>.Failure(
                $"Server returned {(int)response.StatusCode} ({response.ReasonPhrase})");
        }

        // Starts to convert the content to a string while it streams in
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            // Converts the whole string to a ModpackManifest
            var manifest = JsonSerializer.Deserialize<ModpackManifest>(json);

            // If the manifest is empty or null
            if (manifest is null)
            {
                return InstallResult<ModpackManifest>.Failure("Manifest was either empty or null");
            }

            // Set of all required properties
            var requiredProperties = new HashSet<string>(StringComparer.Ordinal)
            {
                "name",
                "mc_version",
                "modloader",
                "modloader_version",
                "download_url",
                "sha256",
                "recommended_ram_mb",
                "version"
            };

            // Set of all allowed properties
            var allowedProperties = new HashSet<string>(requiredProperties, StringComparer.Ordinal)
            {
                "changelog_url"
            };

            // Get a lightweight JSON document
            using var document = JsonDocument.Parse(json);
            // Get the string names of each entry at the root of the document
            var presentProperties = document.RootElement
                .EnumerateObject()
                .Select(property => property.Name)
                .ToHashSet(StringComparer.Ordinal);

            // Get the missing and extra keys
            var missing = requiredProperties.Except(presentProperties).ToArray();
            var unexpected = presentProperties.Except(allowedProperties).ToArray();

            // Accumulate the errors if any are found
            if (missing.Length > 0 || unexpected.Length > 0)
            {
                // List for accumulating errors
                var issues = new List<string>();

                // Add the missing entries
                if (missing.Length > 0)
                {
                    issues.Add($"Missing entries: {string.Join(", ", missing)}");
                }
                // Add the unexpected entries
                if (unexpected.Length > 0)
                {
                    issues.Add($"Unexpected entries: {string.Join(", ", unexpected)}");
                }

                return InstallResult<ModpackManifest>.Failure($"Invalid manifest: {string.Join('\n', issues)}");
            }
            // Return the final manifest if nothing else fails
            return InstallResult<ModpackManifest>.Success(manifest);
        }
        // Any other JSON exceptions
        catch (JsonException ex)
        {
            return InstallResult<ModpackManifest>.Failure($"Invalid manifest format: {ex.Message}");
        }
    }
}