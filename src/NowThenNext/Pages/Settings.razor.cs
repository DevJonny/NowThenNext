using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class Settings
{
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private bool IsBackingUp { get; set; }
    private bool IsRestoring { get; set; }
    private bool ShowRestoreChoiceModal { get; set; }
    private List<ImageItem>? PendingRestoreImages { get; set; }
    private string? SuccessMessage { get; set; }
    private string? ErrorMessage { get; set; }

    private void GoBack()
    {
        Navigation.NavigateTo("");
    }

    private async Task BackupData()
    {
        try
        {
            // Clear any previous messages
            SuccessMessage = null;
            ErrorMessage = null;
            IsBackingUp = true;
            StateHasChanged();

            // Get all images from storage
            var images = await ImageStorage.GetAllImagesAsync();

            // Create backup data structure
            var backup = new BackupData
            {
                Version = "1.0",
                CreatedAt = DateTime.UtcNow,
                Images = images
            };

            // Serialize to JSON
            var json = JsonSerializer.Serialize(backup, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // Generate filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd");
            var filename = $"nowthenext-backup-{timestamp}.json";

            // Trigger download via JavaScript
            await JSRuntime.InvokeVoidAsync("downloadJsonFile", filename, json);

            // Show success message
            SuccessMessage = $"Backup downloaded! ({images.Count} images saved)";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Backup failed: {ex.Message}";
        }
        finally
        {
            IsBackingUp = false;
            StateHasChanged();
        }
    }

    private async Task TriggerRestoreFilePicker()
    {
        // Clear any previous messages
        SuccessMessage = null;
        ErrorMessage = null;
        StateHasChanged();

        // Trigger the hidden file input
        await JSRuntime.InvokeVoidAsync("triggerFileInputClick", "restore-file-input");
    }

    private async Task OnRestoreFileSelected(InputFileChangeEventArgs e)
    {
        try
        {
            SuccessMessage = null;
            ErrorMessage = null;

            var file = e.File;
            if (file == null)
            {
                return;
            }

            // Read the file content
            using var stream = file.OpenReadStream(maxAllowedSize: 50 * 1024 * 1024); // 50MB max
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            // Try to parse and validate the backup file
            BackupData? backup;
            try
            {
                backup = JsonSerializer.Deserialize<BackupData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException)
            {
                ErrorMessage = "Invalid backup file: The file is not valid JSON.";
                StateHasChanged();
                return;
            }

            // Validate backup structure
            if (backup == null)
            {
                ErrorMessage = "Invalid backup file: Could not parse backup data.";
                StateHasChanged();
                return;
            }

            if (backup.Images == null)
            {
                ErrorMessage = "Invalid backup file: No images found in backup.";
                StateHasChanged();
                return;
            }

            // Validate each image has required fields
            foreach (var image in backup.Images)
            {
                if (string.IsNullOrEmpty(image.Id) || string.IsNullOrEmpty(image.Base64Data))
                {
                    ErrorMessage = "Invalid backup file: Some images are missing required data.";
                    StateHasChanged();
                    return;
                }
            }

            // Store pending images and show choice modal
            PendingRestoreImages = backup.Images;
            ShowRestoreChoiceModal = true;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to read backup file: {ex.Message}";
            StateHasChanged();
        }
    }

    private void CancelRestore()
    {
        ShowRestoreChoiceModal = false;
        PendingRestoreImages = null;
        StateHasChanged();
    }

    private async Task PerformReplace()
    {
        if (PendingRestoreImages == null)
        {
            return;
        }

        try
        {
            ShowRestoreChoiceModal = false;
            IsRestoring = true;
            StateHasChanged();

            // Clear all existing images
            await ImageStorage.ClearAllImagesAsync();

            // Save all images from backup
            await ImageStorage.SaveImagesAsync(PendingRestoreImages);

            SuccessMessage = $"Restore complete! {PendingRestoreImages.Count} images imported (replaced all data).";
        }
        catch (StorageQuotaExceededException)
        {
            ErrorMessage = "Restore failed: Storage quota exceeded. The backup is too large for available storage.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Restore failed: {ex.Message}";
        }
        finally
        {
            IsRestoring = false;
            PendingRestoreImages = null;
            StateHasChanged();
        }
    }

    private async Task PerformMerge()
    {
        if (PendingRestoreImages == null)
        {
            return;
        }

        try
        {
            ShowRestoreChoiceModal = false;
            IsRestoring = true;
            StateHasChanged();

            // Get existing images
            var existingImages = await ImageStorage.GetAllImagesAsync();
            var existingIds = new HashSet<string>(existingImages.Select(i => i.Id));

            // Find new images (not already in storage)
            var newImages = PendingRestoreImages.Where(i => !existingIds.Contains(i.Id)).ToList();

            // Merge: add new images to existing
            var mergedImages = existingImages.Concat(newImages).ToList();

            // Save merged list
            await ImageStorage.SaveImagesAsync(mergedImages);

            var skippedCount = PendingRestoreImages.Count - newImages.Count;
            if (skippedCount > 0)
            {
                SuccessMessage = $"Restore complete! {newImages.Count} new images added, {skippedCount} duplicates skipped.";
            }
            else
            {
                SuccessMessage = $"Restore complete! {newImages.Count} images added.";
            }
        }
        catch (StorageQuotaExceededException)
        {
            ErrorMessage = "Restore failed: Storage quota exceeded. Not enough space for the merged data.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Restore failed: {ex.Message}";
        }
        finally
        {
            IsRestoring = false;
            PendingRestoreImages = null;
            StateHasChanged();
        }
    }
}
