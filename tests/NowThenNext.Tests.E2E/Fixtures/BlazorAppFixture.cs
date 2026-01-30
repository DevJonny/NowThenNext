using System.Diagnostics;
using Microsoft.Playwright;

namespace NowThenNext.Tests.E2E.Fixtures;

/// <summary>
/// Fixture that starts the Blazor WASM app and provides a Playwright browser for E2E testing.
/// Implements IAsyncLifetime for xUnit v3 async fixture support.
/// </summary>
public class BlazorAppFixture : IAsyncLifetime
{
    private Process? _serverProcess;
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public string BaseUrl { get; } = "http://localhost:5161";

    public IBrowser Browser => _browser ?? throw new InvalidOperationException("Browser not initialized");
    public IPlaywright Playwright => _playwright ?? throw new InvalidOperationException("Playwright not initialized");

    public async ValueTask InitializeAsync()
    {
        // Start the Blazor WASM server
        await StartServerAsync();

        // Initialize Playwright and launch browser
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    private async Task StartServerAsync()
    {
        // Find the project path by searching upward for the src/NowThenNext folder
        var projectPath = FindProjectPath();

        _serverProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run",
                WorkingDirectory = projectPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                EnvironmentVariables =
                {
                    ["ASPNETCORE_ENVIRONMENT"] = "Development"
                }
            }
        };

        var outputBuilder = new System.Text.StringBuilder();
        var errorBuilder = new System.Text.StringBuilder();

        _serverProcess.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null) outputBuilder.AppendLine(e.Data);
        };
        _serverProcess.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is not null) errorBuilder.AppendLine(e.Data);
        };

        _serverProcess.Start();
        _serverProcess.BeginOutputReadLine();
        _serverProcess.BeginErrorReadLine();

        // Wait for the server to be ready by polling the endpoint
        using var client = new HttpClient();
        var maxRetries = 60; // Increase timeout for initial build
        var delay = TimeSpan.FromSeconds(1);

        for (var i = 0; i < maxRetries; i++)
        {
            // Check if process has exited unexpectedly
            if (_serverProcess.HasExited)
            {
                throw new InvalidOperationException(
                    $"Server process exited with code {_serverProcess.ExitCode}.\n" +
                    $"Output: {outputBuilder}\nError: {errorBuilder}");
            }

            try
            {
                var response = await client.GetAsync(BaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch (HttpRequestException)
            {
                // Server not ready yet
            }

            await Task.Delay(delay);
        }

        throw new TimeoutException(
            $"Server did not start within {maxRetries} seconds.\n" +
            $"Output: {outputBuilder}\nError: {errorBuilder}");
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();

        if (_serverProcess is not null && !_serverProcess.HasExited)
        {
            _serverProcess.Kill(entireProcessTree: true);
            _serverProcess.Dispose();
        }
    }

    /// <summary>
    /// Creates a new browser context with a fresh page.
    /// Each test should use this to get isolated browser state.
    /// </summary>
    public async Task<IPage> CreatePageAsync()
    {
        var context = await Browser.NewContextAsync();
        return await context.NewPageAsync();
    }

    private static string FindProjectPath()
    {
        // Start from the current directory and search upward for the repo root
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            var srcPath = Path.Combine(directory.FullName, "src", "NowThenNext");
            if (Directory.Exists(srcPath) && File.Exists(Path.Combine(srcPath, "NowThenNext.csproj")))
            {
                return srcPath;
            }
            directory = directory.Parent;
        }

        // Fallback: try common test execution paths
        var fallbackPaths = new[]
        {
            // Running from test project output directory
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "src", "NowThenNext")),
            // Running from repo root
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "src", "NowThenNext"))
        };

        foreach (var path in fallbackPaths)
        {
            if (Directory.Exists(path) && File.Exists(Path.Combine(path, "NowThenNext.csproj")))
            {
                return path;
            }
        }

        throw new DirectoryNotFoundException(
            $"Could not find NowThenNext project. Current directory: {Directory.GetCurrentDirectory()}");
    }
}
