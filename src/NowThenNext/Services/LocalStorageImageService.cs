using System.Text.Json;
using Microsoft.JSInterop;
using NowThenNext.Models;

namespace NowThenNext.Services;

/// <summary>
/// Implementation of IImageStorageService using browser localStorage via JS interop
/// </summary>
public class LocalStorageImageService : IImageStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "nowthenext_images";

    // localStorage quota is typically 5MB, but we use a conservative estimate
    private const long EstimatedQuotaBytes = 5 * 1024 * 1024; // 5MB

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
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);

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
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch (JSException ex) when (ex.Message.Contains("QuotaExceededError") ||
                                      ex.Message.Contains("quota", StringComparison.OrdinalIgnoreCase))
        {
            throw new StorageQuotaExceededException("Storage quota exceeded. Please delete some images to free up space.", ex);
        }
    }

    public async Task<double> GetStorageUsagePercentageAsync()
    {
        var currentUsage = await GetCurrentStorageUsageBytesAsync();
        return Math.Min(100.0, (currentUsage / (double)EstimatedQuotaBytes) * 100.0);
    }

    public async Task<int> GetEstimatedRemainingCapacityAsync()
    {
        var currentUsage = await GetCurrentStorageUsageBytesAsync();
        var remainingBytes = Math.Max(0, EstimatedQuotaBytes - currentUsage);
        return (int)(remainingBytes / AverageImageSizeBytes);
    }

    public async Task<StorageInfo> GetStorageInfoAsync()
    {
        var currentUsage = await GetCurrentStorageUsageBytesAsync();
        var usagePercentage = Math.Min(100.0, (currentUsage / (double)EstimatedQuotaBytes) * 100.0);
        var remainingBytes = Math.Max(0, EstimatedQuotaBytes - currentUsage);
        var estimatedRemaining = (int)(remainingBytes / AverageImageSizeBytes);

        return new StorageInfo(
            UsagePercentage: usagePercentage,
            EstimatedRemainingImages: estimatedRemaining,
            CurrentUsageBytes: currentUsage,
            EstimatedQuotaBytes: EstimatedQuotaBytes
        );
    }

    private async Task<long> GetCurrentStorageUsageBytesAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (string.IsNullOrEmpty(json))
            {
                return 0;
            }
            // Calculate byte size of the stored JSON string (UTF-16 in localStorage = 2 bytes per char)
            return json.Length * 2;
        }
        catch
        {
            return 0;
        }
    }
}

/// <summary>
/// Exception thrown when localStorage quota is exceeded
/// </summary>
public class StorageQuotaExceededException : Exception
{
    public StorageQuotaExceededException(string message) : base(message) { }
    public StorageQuotaExceededException(string message, Exception innerException) : base(message, innerException) { }
}
