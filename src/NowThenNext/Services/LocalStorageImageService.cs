using System.Text.Json;
using Microsoft.JSInterop;
using NowThenNext.Models;

namespace NowThenNext.Services;

/// <summary>
/// Implementation of IImageStorageService using browser IndexedDB via JS interop
/// </summary>
public class LocalStorageImageService : IImageStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StoreName = "images";

    // Fallback quota estimate when navigator.storage.estimate() is unavailable
    private const long FallbackQuotaBytes = 500 * 1024 * 1024; // 500MB

    // Average compressed image size estimate (used for capacity calculations)
    private const long AverageImageSizeBytes = 100 * 1024; // 100KB average

    public LocalStorageImageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveImageAsync(ImageItem image)
    {
        var images = await GetAllImagesAsync();

        // Check if image with this Id already exists (update) or is new (add)
        var existingIndex = images.FindIndex(i => i.Id == image.Id);
        if (existingIndex >= 0)
        {
            images[existingIndex] = image;
        }
        else
        {
            images.Add(image);
        }

        await SaveAllImagesAsync(images);
    }

    public async Task<List<ImageItem>> GetImagesByCategoryAsync(ImageCategory category)
    {
        var images = await GetAllImagesAsync();
        return images.Where(i => i.Category == category).ToList();
    }

    public async Task<List<ImageItem>> GetFavoriteImagesAsync()
    {
        var images = await GetAllImagesAsync();
        return images.Where(i => i.IsFavorite).ToList();
    }

    public async Task DeleteImageAsync(string id)
    {
        var images = await GetAllImagesAsync();
        images.RemoveAll(i => i.Id == id);
        await SaveAllImagesAsync(images);
    }

    public async Task<bool> ToggleFavoriteAsync(string id)
    {
        var images = await GetAllImagesAsync();
        var image = images.FirstOrDefault(i => i.Id == id);

        if (image == null)
        {
            return false;
        }

        image.IsFavorite = !image.IsFavorite;
        await SaveAllImagesAsync(images);
        return image.IsFavorite;
    }

    public async Task<List<ImageItem>> GetAllImagesAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("indexedDb.getItem", StoreName);

            if (string.IsNullOrEmpty(json))
            {
                return new List<ImageItem>();
            }

            return JsonSerializer.Deserialize<List<ImageItem>>(json) ?? new List<ImageItem>();
        }
        catch
        {
            return new List<ImageItem>();
        }
    }

    private async Task SaveAllImagesAsync(List<ImageItem> images)
    {
        var json = JsonSerializer.Serialize(images);
        try
        {
            await _jsRuntime.InvokeVoidAsync("indexedDb.setItem", StoreName, json);
        }
        catch (JSException ex) when (ex.Message.Contains("QuotaExceededError") ||
                                      ex.Message.Contains("quota", StringComparison.OrdinalIgnoreCase))
        {
            throw new StorageQuotaExceededException("Storage quota exceeded. Please delete some images to free up space.", ex);
        }
    }

    public async Task<double> GetStorageUsagePercentageAsync()
    {
        var (usage, quota) = await GetStorageEstimateAsync();
        if (quota <= 0) return 0;
        return Math.Min(100.0, (usage / (double)quota) * 100.0);
    }

    public async Task<int> GetEstimatedRemainingCapacityAsync()
    {
        var (usage, quota) = await GetStorageEstimateAsync();
        var remainingBytes = Math.Max(0, quota - usage);
        return (int)(remainingBytes / AverageImageSizeBytes);
    }

    public async Task<StorageInfo> GetStorageInfoAsync()
    {
        var (usage, quota) = await GetStorageEstimateAsync();
        var usagePercentage = quota > 0 ? Math.Min(100.0, (usage / (double)quota) * 100.0) : 0;
        var remainingBytes = Math.Max(0, quota - usage);
        var estimatedRemaining = (int)(remainingBytes / AverageImageSizeBytes);

        return new StorageInfo(
            UsagePercentage: usagePercentage,
            EstimatedRemainingImages: estimatedRemaining,
            CurrentUsageBytes: usage,
            EstimatedQuotaBytes: quota
        );
    }

    private async Task<(long Usage, long Quota)> GetStorageEstimateAsync()
    {
        try
        {
            var estimate = await _jsRuntime.InvokeAsync<StorageEstimate>("indexedDb.getStorageEstimate");
            var quota = estimate.Quota > 0 ? estimate.Quota : FallbackQuotaBytes;
            return (estimate.Usage, quota);
        }
        catch
        {
            return (0, FallbackQuotaBytes);
        }
    }

    public async Task ClearAllImagesAsync()
    {
        await _jsRuntime.InvokeVoidAsync("indexedDb.removeItem", StoreName);
    }

    public async Task SaveImagesAsync(List<ImageItem> images)
    {
        await SaveAllImagesAsync(images);
    }

    private record StorageEstimate(long Usage, long Quota);
}
