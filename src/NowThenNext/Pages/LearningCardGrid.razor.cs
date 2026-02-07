using Microsoft.AspNetCore.Components;
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

    private LearningCategory? Category;
    private List<LearningCard> Cards = new();
    private string? RevealedCardId;

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
}
