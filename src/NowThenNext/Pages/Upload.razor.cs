using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NowThenNext.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class Upload
{
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IImageStorageService ImageStorage { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public string? CategoryParam { get; set; }

    // Event callback for when upload is confirmed - to be used by parent components
    [Parameter]
    public EventCallback<(string Base64Data, string Label, ImageCategory Category)> OnUploadConfirmed { get; set; }

    private StorageWarningBanner? storageWarningBanner;
    private InputFile? fileInput;
    private string? PreviewImage;
    private string? OriginalBase64Data;
    private string? OriginalContentType;
    private string ImageLabel = string.Empty;
    private ImageCategory SelectedCategory = ImageCategory.Places;
    private bool IsSaving = false;
    private bool ShowSuccess = false;
    private string? ErrorMessage = null;

    // Target max dimension for resized images (keeps aspect ratio)
    private const int MaxImageDimension = 800;
    // Target quality for JPEG compression (0.0 - 1.0)
    private const double CompressionQuality = 0.7;

    protected override void OnInitialized()
    {
        // Set category from URL parameter if provided
        if (!string.IsNullOrEmpty(CategoryParam))
        {
            if (Enum.TryParse<ImageCategory>(CategoryParam, true, out var category))
            {
                SelectedCategory = category;
            }
        }
    }

    private async Task TriggerFileInput()
    {
        // This will be handled by the InputFile component through JavaScript interop
        // The actual click is handled via the label/onclick
        await Task.CompletedTask;
    }

    private void HandleDragOver()
    {
        // Prevent default to allow drop
    }

    private async Task HandleDrop()
    {
        // Drop handling is limited in Blazor WASM - rely on InputFile
        await Task.CompletedTask;
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;

        // Clear any previous error
        ErrorMessage = null;
        ShowSuccess = false;

        // Validate file type
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
        {
            ErrorMessage = "Please select a JPG, PNG, or WebP image.";
            return;
        }

        // Read file as base64 (limit to 10MB)
        var maxSize = 10 * 1024 * 1024;
        if (file.Size > maxSize)
        {
            ErrorMessage = "Image is too large. Maximum size is 10MB.";
            return;
        }

        using var stream = file.OpenReadStream(maxSize);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        var base64 = Convert.ToBase64String(bytes);

        OriginalBase64Data = base64;
        OriginalContentType = file.ContentType;
        PreviewImage = $"data:{file.ContentType};base64,{base64}";
    }

    private void ClearPreview()
    {
        PreviewImage = null;
        OriginalBase64Data = null;
        OriginalContentType = null;
        ImageLabel = string.Empty;
        ErrorMessage = null;
        ShowSuccess = false;
    }

    private void SelectCategory(ImageCategory category)
    {
        SelectedCategory = category;
    }

    private async Task OnConfirmUpload()
    {
        if (string.IsNullOrEmpty(PreviewImage) || string.IsNullOrEmpty(OriginalBase64Data))
        {
            return;
        }

        IsSaving = true;
        ErrorMessage = null;
        ShowSuccess = false;
        StateHasChanged();

        try
        {
            // Resize and compress the image client-side using canvas
            var compressedBase64 = await CompressImageAsync(OriginalBase64Data, OriginalContentType ?? "image/jpeg");

            // Create the image item
            var imageItem = new ImageItem
            {
                Base64Data = compressedBase64,
                Label = ImageLabel,
                Category = SelectedCategory,
                IsFavorite = false,
                CreatedAt = DateTime.UtcNow
            };

            // Save to storage
            await ImageStorage.SaveImageAsync(imageItem);

            // Invoke the callback if provided (for component use)
            if (OnUploadConfirmed.HasDelegate)
            {
                await OnUploadConfirmed.InvokeAsync((compressedBase64, ImageLabel, SelectedCategory));
            }

            // Show success and reset form
            ShowSuccess = true;
            PreviewImage = null;
            OriginalBase64Data = null;
            OriginalContentType = null;
            ImageLabel = string.Empty;

            // Re-check storage after successful upload to potentially show warning banner
            if (storageWarningBanner != null)
            {
                await storageWarningBanner.ResetAndCheckAsync();
            }
        }
        catch (StorageQuotaExceededException)
        {
            ErrorMessage = "Storage is full! Please delete some images to free up space.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to save image: {ex.Message}";
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Compress and resize image using browser canvas API
    /// </summary>
    private async Task<string> CompressImageAsync(string base64Data, string contentType)
    {
        // Use JavaScript to resize/compress the image via canvas
        var dataUrl = $"data:{contentType};base64,{base64Data}";
        var compressedDataUrl = await JSRuntime.InvokeAsync<string>(
            "compressImage",
            dataUrl,
            MaxImageDimension,
            CompressionQuality
        );

        // Extract base64 from data URL (remove "data:image/jpeg;base64," prefix)
        var commaIndex = compressedDataUrl.IndexOf(',');
        if (commaIndex >= 0)
        {
            return compressedDataUrl.Substring(commaIndex + 1);
        }

        return compressedDataUrl;
    }

    private void GoBack()
    {
        Navigation.NavigateTo("");
    }
}
