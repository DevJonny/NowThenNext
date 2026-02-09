using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class PhonicsCard
{
    [Inject]
    private IPhonicsDataService PhonicsData { get; set; } = default!;

    [Inject]
    private IPhonicsProgressService PhonicsProgress { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Parameter]
    public int PhaseId { get; set; }

    [Parameter]
    public string GraphemeId { get; set; } = string.Empty;

    private GraphemeCard? Card;
    private List<GraphemeCard> AllCards = new();
    private int CurrentIndex;
    private string? NextUnlockedId;
    private bool IsCurrentUnlocked;
    private bool CanGoPrevious;
    private bool CanGoNext;
    private bool ShowPhaseComplete;
    private bool ShowImage;

    protected override async Task OnParametersSetAsync()
    {
        ShowPhaseComplete = false;
        ShowImage = false;
        await LoadCardState();
    }

    private async Task LoadCardState()
    {
        Card = PhonicsData.GetGraphemeCard(GraphemeId);
        if (Card == null) return;

        var phase = PhonicsData.GetPhase(PhaseId);
        if (phase == null) return;

        AllCards = phase.Weeks.SelectMany(w => w.Cards).OrderBy(c => c.OrderIndex).ToList();
        CurrentIndex = AllCards.FindIndex(c => c.Id == GraphemeId);

        // If the requested grapheme is not part of this phase, disable navigation
        if (CurrentIndex < 0)
        {
            IsCurrentUnlocked = false;
            CanGoPrevious = false;
            CanGoNext = false;
            return;
        }

        NextUnlockedId = await PhonicsProgress.GetNextUnlockedGraphemeIdAsync(PhaseId);

        IsCurrentUnlocked = GraphemeId == NextUnlockedId;
        CanGoPrevious = CurrentIndex > 0;

        // Can go next only if the next card is completed or is the current unlocked
        if (CurrentIndex < AllCards.Count - 1)
        {
            var nextId = AllCards[CurrentIndex + 1].Id;
            var nextCompleted = await PhonicsProgress.IsCompletedAsync(nextId);
            CanGoNext = nextCompleted || nextId == NextUnlockedId;
        }
        else
        {
            CanGoNext = false;
        }
    }

    private async Task HandleGotIt()
    {
        await PhonicsProgress.MarkCompletedAsync(GraphemeId);

        // Check if there's a next unlocked grapheme
        var nextId = await PhonicsProgress.GetNextUnlockedGraphemeIdAsync(PhaseId);

        if (nextId == null)
        {
            // Phase complete!
            ShowPhaseComplete = true;
        }
        else
        {
            // Navigate to next card
            Navigation.NavigateTo($"phonics/{PhaseId}/card/{nextId}");
        }
    }

    private void GoPrevious()
    {
        if (CanGoPrevious)
        {
            var prevCard = AllCards[CurrentIndex - 1];
            Navigation.NavigateTo($"phonics/{PhaseId}/card/{prevCard.Id}");
        }
    }

    private void GoNext()
    {
        if (CanGoNext)
        {
            var nextCard = AllCards[CurrentIndex + 1];
            Navigation.NavigateTo($"phonics/{PhaseId}/card/{nextCard.Id}");
        }
    }

    private string HighlightGrapheme(string word, string grapheme)
    {
        var index = word.IndexOf(grapheme, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
            return System.Net.WebUtility.HtmlEncode(word);

        var before = System.Net.WebUtility.HtmlEncode(word[..index]);
        var match = System.Net.WebUtility.HtmlEncode(word[index..(index + grapheme.Length)]);
        var after = System.Net.WebUtility.HtmlEncode(word[(index + grapheme.Length)..]);

        return $"{before}<span class=\"grapheme-highlight\">{match}</span>{after}";
    }

    private void ToggleImage()
    {
        if (!string.IsNullOrEmpty(Card?.ImagePath))
        {
            ShowImage = !ShowImage;
        }
    }

    private bool HasImage => !string.IsNullOrEmpty(Card?.ImagePath);

    private void GoBack()
    {
        Navigation.NavigateTo($"phonics/{PhaseId}");
    }
}
