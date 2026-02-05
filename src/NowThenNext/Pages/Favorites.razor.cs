using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class Favorites
{
    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<ImageItem> PlacesFavorites = new();
    private List<ImageItem> FoodFavorites = new();
    private List<ImageItem> ActivitiesFavorites = new();
    private bool IsLoading = true;
    private bool ShowDeleteConfirmation = false;
    private string? ImageToDeleteId = null;

    protected override async Task OnInitializedAsync()
    {
        await LoadFavoritesAsync();
    }

    private async Task LoadFavoritesAsync()
    {
        IsLoading = true;
        StateHasChanged();

        try
        {
            var allFavorites = await ImageStorage.GetFavoriteImagesAsync();
            PlacesFavorites = allFavorites.Where(i => i.Category == ImageCategory.Places).ToList();
            FoodFavorites = allFavorites.Where(i => i.Category == ImageCategory.Food).ToList();
            ActivitiesFavorites = allFavorites.Where(i => i.Category == ImageCategory.Activities).ToList();
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task HandleFavoriteToggle(string imageId)
    {
        await ImageStorage.ToggleFavoriteAsync(imageId);

        // Remove from the appropriate list since we're toggling off favorites
        PlacesFavorites.RemoveAll(i => i.Id == imageId);
        FoodFavorites.RemoveAll(i => i.Id == imageId);
        ActivitiesFavorites.RemoveAll(i => i.Id == imageId);
        StateHasChanged();
    }

    private void HandleDeleteRequest(string imageId)
    {
        ImageToDeleteId = imageId;
        ShowDeleteConfirmation = true;
        StateHasChanged();
    }

    private void CancelDelete()
    {
        ShowDeleteConfirmation = false;
        ImageToDeleteId = null;
        StateHasChanged();
    }

    private async Task ConfirmDelete()
    {
        if (!string.IsNullOrEmpty(ImageToDeleteId))
        {
            await ImageStorage.DeleteImageAsync(ImageToDeleteId);
            PlacesFavorites.RemoveAll(i => i.Id == ImageToDeleteId);
            FoodFavorites.RemoveAll(i => i.Id == ImageToDeleteId);
            ActivitiesFavorites.RemoveAll(i => i.Id == ImageToDeleteId);
        }

        ShowDeleteConfirmation = false;
        ImageToDeleteId = null;
        StateHasChanged();
    }

    private void GoHome()
    {
        Navigation.NavigateTo("");
    }
}
