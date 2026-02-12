using Microsoft.Playwright;

namespace NowThenNext.Tests.E2E.Helpers;

/// <summary>
/// Helper methods for managing IndexedDB storage in E2E tests.
/// Method names are kept for backwards compatibility with existing tests.
/// </summary>
public static class LocalStorageHelper
{
    // Map from old localStorage keys to IndexedDB store names
    private static string MapKeyToStoreName(string key) => key switch
    {
        "nowthenext_images" => "images",
        "phonics-progress" => "phonics-progress",
        "learning-cards" => "learning-cards",
        _ => key
    };

    /// <summary>
    /// Clears all IndexedDB and localStorage data for the current page.
    /// Should be called at the start of each test for isolation.
    /// </summary>
    public static async Task ClearLocalStorageAsync(this IPage page)
    {
        await page.EvaluateAsync(@"async () => {
            localStorage.clear();
            if (window.indexedDb && window.indexedDb.clearAll) {
                await window.indexedDb.clearAll();
            }
        }");
    }

    /// <summary>
    /// Clears the images store in IndexedDB.
    /// </summary>
    public static async Task ClearImagesStorageAsync(this IPage page)
    {
        await page.EvaluateAsync(@"async () => {
            if (window.indexedDb && window.indexedDb.removeItem) {
                await window.indexedDb.removeItem('images');
            }
        }");
    }

    /// <summary>
    /// Gets a value from IndexedDB (mapped from old localStorage key).
    /// </summary>
    public static async Task<string?> GetLocalStorageItemAsync(this IPage page, string key)
    {
        var storeName = MapKeyToStoreName(key);
        return await page.EvaluateAsync<string?>(@"async (storeName) => {
            if (window.indexedDb && window.indexedDb.getItem) {
                return await window.indexedDb.getItem(storeName);
            }
            return null;
        }", storeName);
    }

    /// <summary>
    /// Sets a value in IndexedDB (mapped from old localStorage key).
    /// </summary>
    public static async Task SetLocalStorageItemAsync(this IPage page, string key, string value)
    {
        var storeName = MapKeyToStoreName(key);
        await page.EvaluateAsync(@"async ([storeName, value]) => {
            if (window.indexedDb && window.indexedDb.setItem) {
                await window.indexedDb.setItem(storeName, value);
            }
        }", new[] { storeName, value });
    }
}
