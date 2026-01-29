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
}
