using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class FoodLibrary
{
    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<ImageItem> Images = new();
    private bool IsLoading = true;
    private bool ShowDeleteConfirmation = false;
    private string? ImageToDeleteId = null;

    protected override async Task OnInitializedAsync()
    {
        await LoadImagesAsync();
    }

    private async Task LoadImagesAsync()
    {
        IsLoading = true;
        StateHasChanged();

        try
        {
            Images = await ImageStorage.GetImagesByCategoryAsync(ImageCategory.Food);
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

        // Update local state
        var image = Images.FirstOrDefault(i => i.Id == imageId);
        if (image != null)
        {
            image.IsFavorite = !image.IsFavorite;
            StateHasChanged();
        }
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
            Images.RemoveAll(i => i.Id == ImageToDeleteId);
        }

        ShowDeleteConfirmation = false;
        ImageToDeleteId = null;
        StateHasChanged();
    }

    private void GoBack()
    {
        Navigation.NavigateTo("/");
    }
}
