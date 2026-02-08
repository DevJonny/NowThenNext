using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class LearningCardGrid
{
    [Parameter]
    public string CategoryId { get; set; } = string.Empty;

    [Inject]
    private ILearningCardsDataService LearningCardsData { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private LearningCategory? Category;
    private List<LearningCard> Cards = new();
    private string? RevealedCardId;

    // Add card modal state
    private bool ShowAddCardModal;
    private string NewCardWord = string.Empty;
    private string? NewCardPreviewImage;
    private string? NewCardOriginalBase64;
    private string? NewCardOriginalContentType;
    private bool IsAddingCard;
    private string? AddCardErrorMessage;
    private string? AddCardValidationMessage;

    // Delete card modal state
    private bool ShowDeleteCardConfirmation;
    private string? CardToDeleteId;

    // Image compression settings (matches Upload.razor)
    private const int MaxImageDimension = 800;
    private const double CompressionQuality = 0.7;

    protected override async Task OnParametersSetAsync()
    {
        RevealedCardId = null;

        var allCategories = await LearningCardsData.GetAllCategoriesAsync();
        Category = allCategories.FirstOrDefault(c => c.Id == CategoryId);

        if (Category != null)
        {
            Cards = await LearningCardsData.GetCardsByCategoryAsync(CategoryId);
        }
        else
        {
            Cards = new();
        }
    }

    private void ToggleCard(string cardId)
    {
        if (RevealedCardId == cardId)
        {
            RevealedCardId = null;
        }
        else
        {
            RevealedCardId = cardId;
        }
    }

    private void GoBack()
    {
        Navigation.NavigateTo("learning");
    }

    private bool IsCustomCategory => Category != null && !Category.IsBuiltIn;

    private void RequestDeleteCard(string cardId)
    {
        CardToDeleteId = cardId;
        ShowDeleteCardConfirmation = true;
    }

    private void CancelDeleteCard()
    {
        ShowDeleteCardConfirmation = false;
        CardToDeleteId = null;
    }

    private async Task ConfirmDeleteCard()
    {
        if (!string.IsNullOrEmpty(CardToDeleteId))
        {
            try
            {
                await LearningCardsData.DeleteCustomCardAsync(CardToDeleteId);
                Cards.RemoveAll(c => c.Id == CardToDeleteId);

                // If the deleted card was revealed, clear the revealed state
                if (RevealedCardId == CardToDeleteId)
                {
                    RevealedCardId = null;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete learning card: {ex.Message}");
            }
        }

        ShowDeleteCardConfirmation = false;
        CardToDeleteId = null;
        StateHasChanged();
    }

    private void OpenAddCardModal()
    {
        ShowAddCardModal = true;
        NewCardWord = string.Empty;
        NewCardPreviewImage = null;
        NewCardOriginalBase64 = null;
        NewCardOriginalContentType = null;
        IsAddingCard = false;
        AddCardErrorMessage = null;
        AddCardValidationMessage = null;
    }

    private void CloseAddCardModal()
    {
        ShowAddCardModal = false;
    }

    private async Task HandleCardFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;

        AddCardErrorMessage = null;
        AddCardValidationMessage = null;

        // Validate file type
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
        {
            AddCardErrorMessage = "Please select a JPG, PNG, or WebP image.";
            return;
        }

        // Limit to 10MB
        var maxSize = 10 * 1024 * 1024;
        if (file.Size > maxSize)
        {
            AddCardErrorMessage = "Image is too large. Maximum size is 10MB.";
            return;
        }

        using var stream = file.OpenReadStream(maxSize);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        var base64 = Convert.ToBase64String(bytes);

        NewCardOriginalBase64 = base64;
        NewCardOriginalContentType = file.ContentType;
        NewCardPreviewImage = $"data:{file.ContentType};base64,{base64}";
    }

    private void ClearCardPreview()
    {
        NewCardPreviewImage = null;
        NewCardOriginalBase64 = null;
        NewCardOriginalContentType = null;
    }

    private async Task SaveNewCard()
    {
        AddCardValidationMessage = null;
        AddCardErrorMessage = null;

        // Validate word
        if (string.IsNullOrWhiteSpace(NewCardWord))
        {
            AddCardValidationMessage = "Please enter a word for this card.";
            return;
        }

        // Validate image
        if (string.IsNullOrEmpty(NewCardOriginalBase64))
        {
            AddCardValidationMessage = "Please upload an image for this card.";
            return;
        }

        IsAddingCard = true;
        StateHasChanged();

        try
        {
            // Compress image using existing JS interop
            var dataUrl = $"data:{NewCardOriginalContentType ?? "image/jpeg"};base64,{NewCardOriginalBase64}";
            var compressedDataUrl = await JSRuntime.InvokeAsync<string>(
                "compressImage",
                dataUrl,
                MaxImageDimension,
                CompressionQuality
            );

            // Extract base64 from data URL
            var compressedBase64 = compressedDataUrl;
            var commaIndex = compressedDataUrl.IndexOf(',');
            if (commaIndex >= 0)
            {
                compressedBase64 = compressedDataUrl.Substring(commaIndex + 1);
            }

            var card = new LearningCard
            {
                Id = Guid.NewGuid().ToString(),
                CategoryId = CategoryId,
                ImageData = compressedBase64,
                Word = NewCardWord.Trim(),
                IsBuiltIn = false
            };

            await LearningCardsData.AddCustomCardAsync(card);

            // Refresh cards list
            Cards = await LearningCardsData.GetCardsByCategoryAsync(CategoryId);

            // Close modal
            ShowAddCardModal = false;
        }
        catch (StorageQuotaExceededException)
        {
            AddCardErrorMessage = "Storage is full! Please delete some cards to free up space.";
        }
        catch (Exception ex)
        {
            AddCardErrorMessage = $"Failed to save card: {ex.Message}";
            Console.Error.WriteLine($"Failed to save learning card: {ex.Message}");
        }
        finally
        {
            IsAddingCard = false;
            StateHasChanged();
        }
    }
}
