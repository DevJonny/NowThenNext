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

    protected override async Task OnInitializedAsync()
    {
        Categories = await LearningCardsData.GetAllCategoriesAsync();
    }

    private void GoBack()
    {
        Navigation.NavigateTo("");
    }
}
