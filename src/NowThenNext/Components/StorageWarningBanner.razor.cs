using Microsoft.AspNetCore.Components;
using NowThenNext.Services;

namespace NowThenNext.Components;

public partial class StorageWarningBanner
{
    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    /// <summary>
    /// Threshold percentage (0-100) at which to show the warning banner
    /// </summary>
    [Parameter]
    public double WarningThreshold { get; set; } = 80.0;

    /// <summary>
    /// Event callback when banner is dismissed
    /// </summary>
    [Parameter]
    public EventCallback OnDismissed { get; set; }

    private bool ShowBanner = false;
    private bool IsDismissed = false;
    private int EstimatedRemainingImages = 0;

    protected override async Task OnInitializedAsync()
    {
        await CheckStorageAsync();
    }

    /// <summary>
    /// Check storage and update banner visibility.
    /// Call this after uploads to re-check storage status.
    /// </summary>
    public async Task CheckStorageAsync()
    {
        try
        {
            var storageInfo = await ImageStorage.GetStorageInfoAsync();
            ShowBanner = storageInfo.UsagePercentage >= WarningThreshold;
            EstimatedRemainingImages = storageInfo.EstimatedRemainingImages;

            // Update banner visibility based on current storage usage.
            // Note: we intentionally do not modify IsDismissed here; callers can
            // use ResetAndCheckAsync if they want to clear the dismissed state.
            StateHasChanged();
        }
        catch
        {
            // Silently fail - don't show banner if we can't get storage info
            ShowBanner = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Reset the dismissed state so banner can appear again.
    /// Call this after an upload to potentially show the banner again.
    /// </summary>
    public async Task ResetAndCheckAsync()
    {
        IsDismissed = false;
        await CheckStorageAsync();
    }

    private async Task Dismiss()
    {
        IsDismissed = true;
        StateHasChanged();

        if (OnDismissed.HasDelegate)
        {
            await OnDismissed.InvokeAsync();
        }
    }
}
