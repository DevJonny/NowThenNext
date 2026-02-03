using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class ActivityDisplay
{
    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<ImageItem> ActivityItems = new();
    private string? SelectedActivityId = null;
    private bool IsLoading = true;
    private bool ShowConfirmation = false;
    private ImageItem? ConfirmedActivity = null;

    protected override async Task OnInitializedAsync()
    {
        await LoadActivityItemsAsync();
    }

    private async Task LoadActivityItemsAsync()
    {
        IsLoading = true;
        StateHasChanged();

        try
        {
            // Parse image IDs from query string
            var uri = new Uri(Navigation.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var idsParam = query["ids"];

            if (!string.IsNullOrEmpty(idsParam))
            {
                var imageIds = idsParam.Split(',', StringSplitOptions.RemoveEmptyEntries);

                // Load all activity images from storage and filter by IDs
                var allImages = await ImageStorage.GetImagesByCategoryAsync(ImageCategory.Activities);

                // Maintain the order from the query string
                foreach (var id in imageIds)
                {
                    var image = allImages.FirstOrDefault(img => img.Id == id);
                    if (image != null)
                    {
                        ActivityItems.Add(image);
                    }
                }
            }
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private void SelectActivity(string activityId)
    {
        // Select the activity and show confirmation screen
        SelectedActivityId = activityId;
        ConfirmedActivity = ActivityItems.FirstOrDefault(a => a.Id == activityId);
        ShowConfirmation = true;
        StateHasChanged();
    }

    private void ChooseAgain()
    {
        // Reset to selection mode
        ShowConfirmation = false;
        ConfirmedActivity = null;
        SelectedActivityId = null;
        StateHasChanged();
    }

    private string GetLayoutClass()
    {
        return ActivityItems.Count switch
        {
            2 => "two-items",
            3 => "three-items",
            4 => "four-items",
            5 => "five-items",
            6 => "six-items",
            _ when ActivityItems.Count > 6 => "six-items",
            _ => "two-items"
        };
    }

    private string GetImageSrc(ImageItem image)
    {
        if (image.Base64Data.StartsWith("data:"))
        {
            return image.Base64Data;
        }
        return $"data:image/jpeg;base64,{image.Base64Data}";
    }

    private void GoBack()
    {
        if (ShowConfirmation)
        {
            // If on confirmation screen, go back to selection
            ChooseAgain();
        }
        else
        {
            Navigation.NavigateTo("/activity-choices");
        }
    }
}
