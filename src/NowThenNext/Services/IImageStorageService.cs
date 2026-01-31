using NowThenNext.Models;

namespace NowThenNext.Services;

/// <summary>
/// Service for storing and retrieving images from browser localStorage
/// </summary>
public interface IImageStorageService
{
    /// <summary>
    /// Save an image to localStorage
    /// </summary>
    Task SaveImageAsync(ImageItem image);

    /// <summary>
    /// Retrieve all images by category
    /// </summary>
    Task<List<ImageItem>> GetImagesByCategoryAsync(ImageCategory category);

    /// <summary>
    /// Retrieve all favorited images across all categories
    /// </summary>
    Task<List<ImageItem>> GetFavoriteImagesAsync();

    /// <summary>
    /// Delete an image by its Id
    /// </summary>
    Task DeleteImageAsync(string id);

    /// <summary>
    /// Toggle the favorite status of an image
    /// </summary>
    Task<bool> ToggleFavoriteAsync(string id);

    /// <summary>
    /// Get all images (for internal use)
    /// </summary>
    Task<List<ImageItem>> GetAllImagesAsync();

    /// <summary>
    /// Get current storage usage as a percentage of estimated quota (0-100)
    /// </summary>
    Task<double> GetStorageUsagePercentageAsync();

    /// <summary>
    /// Get estimated remaining image capacity based on average image size
    /// </summary>
    Task<int> GetEstimatedRemainingCapacityAsync();

    /// <summary>
    /// Storage information including usage percentage and remaining capacity
    /// </summary>
    Task<StorageInfo> GetStorageInfoAsync();

    /// <summary>
    /// Clear all images from storage
    /// </summary>
    Task ClearAllImagesAsync();

    /// <summary>
    /// Save multiple images to storage (used for restore operations)
    /// </summary>
    Task SaveImagesAsync(List<ImageItem> images);
}

/// <summary>
/// Contains storage usage information
/// </summary>
public record StorageInfo(
    double UsagePercentage,
    int EstimatedRemainingImages,
    long CurrentUsageBytes,
    long EstimatedQuotaBytes
);
