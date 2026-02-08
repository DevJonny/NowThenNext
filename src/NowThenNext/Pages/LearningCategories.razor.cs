using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class LearningCategories
{
    [Inject]
    private ILearningCardsDataService LearningCardsData { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<LearningCategory> Categories = new();

    // Modal state
    private bool ShowAddCategoryModal { get; set; }
    private string NewCategoryName { get; set; } = string.Empty;
    private string SelectedEmoji { get; set; } = string.Empty;
    private bool ShowNameValidation { get; set; }

    private const string DefaultEmoji = "\U0001F4CB"; // clipboard emoji

    // Delete category modal state
    private bool ShowDeleteCategoryModal { get; set; }
    private string? CategoryToDeleteId;
    private string? CategoryToDeleteName;

    // Curated list of child-friendly emojis (~30)
    private static readonly string[] AvailableEmojis =
    [
        "\U0001F697", // car
        "\U0001F68C", // bus
        "\U0001F682", // locomotive
        "\U0001F6F3", // ship (passenger)
        "\U00002708", // airplane
        "\U0001F680", // rocket
        "\U0001F34E", // apple
        "\U0001F34C", // banana
        "\U0001F353", // strawberry
        "\U0001F955", // carrot
        "\U0001F382", // cake
        "\U0001F366", // ice cream
        "\U0001F332", // tree
        "\U0001F33B", // sunflower
        "\U0001F308", // rainbow
        "\U00002B50", // star
        "\U0001F319", // crescent moon
        "\U00002600", // sun
        "\U0001F3E0", // house
        "\U0001F3EB", // school
        "\U0001F3B5", // musical note
        "\U0001F3A8", // palette
        "\U0001F4DA", // books
        "\U000026BD", // football
        "\U0001F3C0", // basketball
        "\U0001F457", // dress
        "\U0001F45F", // running shoe
        "\U0001F9E9", // puzzle piece
        "\U0001F381", // wrapped gift
        "\U0001F48E", // gem stone
    ];

    protected override async Task OnInitializedAsync()
    {
        Categories = await LearningCardsData.GetAllCategoriesAsync();
    }

    private void GoBack()
    {
        Navigation.NavigateTo("");
    }

    private void OpenAddCategoryModal()
    {
        NewCategoryName = string.Empty;
        SelectedEmoji = string.Empty;
        ShowNameValidation = false;
        ShowAddCategoryModal = true;
    }

    private void CloseAddCategoryModal()
    {
        ShowAddCategoryModal = false;
    }

    private void SelectEmoji(string emoji)
    {
        SelectedEmoji = SelectedEmoji == emoji ? string.Empty : emoji;
    }

    private void RequestDeleteCategory(string categoryId, string categoryName)
    {
        CategoryToDeleteId = categoryId;
        CategoryToDeleteName = categoryName;
        ShowDeleteCategoryModal = true;
    }

    private void CancelDeleteCategory()
    {
        ShowDeleteCategoryModal = false;
        CategoryToDeleteId = null;
        CategoryToDeleteName = null;
    }

    private async Task ConfirmDeleteCategory()
    {
        if (!string.IsNullOrEmpty(CategoryToDeleteId))
        {
            try
            {
                await LearningCardsData.DeleteCustomCategoryAsync(CategoryToDeleteId);
                Categories = await LearningCardsData.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete custom category: {ex.Message}");
            }
        }

        ShowDeleteCategoryModal = false;
        CategoryToDeleteId = null;
        CategoryToDeleteName = null;
        StateHasChanged();
    }

    private async Task SaveNewCategory()
    {
        ShowNameValidation = true;

        if (string.IsNullOrWhiteSpace(NewCategoryName))
            return;

        var category = new LearningCategory
        {
            Id = Guid.NewGuid().ToString(),
            Name = NewCategoryName.Trim(),
            Emoji = string.IsNullOrEmpty(SelectedEmoji) ? DefaultEmoji : SelectedEmoji,
            IsBuiltIn = false,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await LearningCardsData.AddCustomCategoryAsync(category);
            Categories = await LearningCardsData.GetAllCategoriesAsync();
            ShowAddCategoryModal = false;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save custom category: {ex.Message}");
        }
    }
}
