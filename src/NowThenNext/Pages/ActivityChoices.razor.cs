using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class ActivityChoices
{
    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<ImageItem> ActivityImages = new();
    private List<ImageItem> SelectedImages = new();
    private bool IsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadActivitiesAsync();
    }

    private async Task LoadActivitiesAsync()
    {
        IsLoading = true;
        StateHasChanged();

        try
        {
            ActivityImages = await ImageStorage.GetImagesByCategoryAsync(ImageCategory.Activities);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private void ToggleSelection(ImageItem image)
    {
        var existingIndex = SelectedImages.FindIndex(i => i.Id == image.Id);

        if (existingIndex >= 0)
        {
            // Deselect
            SelectedImages.RemoveAt(existingIndex);
        }
        else
        {
            // Select (no maximum limit)
            SelectedImages.Add(image);
        }

        StateHasChanged();
    }

    private void ClearSelection()
    {
        SelectedImages.Clear();
        StateHasChanged();
    }

    private void ShowChoices()
    {
        // Pass selected image IDs as comma-separated query string
        var imageIds = string.Join(",", SelectedImages.Select(i => i.Id));
        Navigation.NavigateTo($"activity-display?ids={imageIds}");
    }

    private string GetImageSrc(ImageItem image)
    {
        if (image.Base64Data.StartsWith("data:"))
        {
            return image.Base64Data;
        }
        return $"data:image/jpeg;base64,{image.Base64Data}";
    }

    private void GoHome()
    {
        Navigation.NavigateTo("");
    }
}
