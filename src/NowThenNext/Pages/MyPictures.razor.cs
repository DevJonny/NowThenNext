using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class MyPictures
{
    [Parameter]
    public string? Tab { get; set; }

    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<ImageItem> Images = new();
    private bool IsLoading = true;
    private bool ShowDeleteConfirmation = false;
    private string? ImageToDeleteId = null;

    private string CurrentTab => string.IsNullOrEmpty(Tab) ? "places" : Tab.ToLowerInvariant();
    private bool IsPlacesActive => CurrentTab == "places";
    private bool IsFoodActive => CurrentTab == "food";

    protected override async Task OnInitializedAsync()
    {
        await LoadImagesAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadImagesAsync();
    }

    private async Task LoadImagesAsync()
    {
        IsLoading = true;
        StateHasChanged();

        try
        {
            var category = IsPlacesActive ? ImageCategory.Places : ImageCategory.Food;
            Images = await ImageStorage.GetImagesByCategoryAsync(category);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private void SwitchTab(string tab)
    {
        Navigation.NavigateTo($"my-pictures/{tab}");
    }

    private async Task HandleFavoriteToggle(string imageId)
    {
        await ImageStorage.ToggleFavoriteAsync(imageId);

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

    private void GoHome()
    {
        Navigation.NavigateTo("");
    }
}
