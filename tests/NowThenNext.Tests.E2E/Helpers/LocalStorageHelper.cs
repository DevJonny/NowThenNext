using Microsoft.Playwright;

namespace NowThenNext.Tests.E2E.Helpers;

/// <summary>
/// Helper methods for managing localStorage in E2E tests.
/// </summary>
public static class LocalStorageHelper
{
    /// <summary>
    /// Clears all localStorage data for the current page.
    /// Should be called at the start of each test for isolation.
    /// </summary>
    public static async Task ClearLocalStorageAsync(this IPage page)
    {
        await page.EvaluateAsync("() => localStorage.clear()");
    }

    /// <summary>
    /// Clears the specific NowThenNext images storage key.
    /// </summary>
    public static async Task ClearImagesStorageAsync(this IPage page)
    {
        await page.EvaluateAsync("() => localStorage.removeItem('nowthenext_images')");
    }

    /// <summary>
    /// Gets a value from localStorage.
    /// </summary>
    public static async Task<string?> GetLocalStorageItemAsync(this IPage page, string key)
    {
        return await page.EvaluateAsync<string?>($"() => localStorage.getItem('{key}')");
    }

    /// <summary>
    /// Sets a value in localStorage.
    /// </summary>
    public static async Task SetLocalStorageItemAsync(this IPage page, string key, string value)
    {
        await page.EvaluateAsync($"([k, v]) => localStorage.setItem(k, v)", new[] { key, value });
    }
}
