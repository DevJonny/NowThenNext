using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class PhonicsSoundList
{
    [Inject]
    private IPhonicsDataService PhonicsData { get; set; } = default!;

    [Inject]
    private IPhonicsProgressService PhonicsProgress { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Parameter]
    public int PhaseId { get; set; }

    private PhonicsPhase? Phase;
    private (int Completed, int Total) Progress;
    private readonly HashSet<string> CompletedIds = new();
    private string? NextUnlockedId;
    private bool ShowResetConfirmation;

    protected override async Task OnInitializedAsync()
    {
        await LoadState();
    }

    private async Task LoadState()
    {
        Phase = PhonicsData.GetPhase(PhaseId);
        if (Phase == null) return;

        Progress = await PhonicsProgress.GetPhaseProgressAsync(PhaseId);
        NextUnlockedId = await PhonicsProgress.GetNextUnlockedGraphemeIdAsync(PhaseId);

        var allCards = Phase.Weeks.SelectMany(w => w.Cards).ToList();
        var completionResults = await Task.WhenAll(
            allCards.Select(async card => (card.Id, IsCompleted: await PhonicsProgress.IsCompletedAsync(card.Id))));
        CompletedIds.Clear();
        CompletedIds.UnionWith(completionResults.Where(r => r.IsCompleted).Select(r => r.Id));
    }

    private CardState GetCardState(string cardId)
    {
        if (CompletedIds.Contains(cardId))
            return CardState.Completed;
        if (cardId == NextUnlockedId)
            return CardState.Current;
        return CardState.Locked;
    }

    private void ShowResetModal()
    {
        ShowResetConfirmation = true;
    }

    private void CancelReset()
    {
        ShowResetConfirmation = false;
    }

    private async Task ConfirmReset()
    {
        await PhonicsProgress.ResetPhaseAsync(PhaseId);
        ShowResetConfirmation = false;
        await LoadState();
    }

    private void GoBack()
    {
        Navigation.NavigateTo("phonics");
    }

    private enum CardState
    {
        Completed,
        Current,
        Locked
    }
}
