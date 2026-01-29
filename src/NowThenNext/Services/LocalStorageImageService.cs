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
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
