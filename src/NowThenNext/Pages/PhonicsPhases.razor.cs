using Microsoft.AspNetCore.Components;
using NowThenNext.Models;
using NowThenNext.Services;

namespace NowThenNext.Pages;

public partial class PhonicsPhases
{
    [Inject]
    private IPhonicsDataService PhonicsData { get; set; } = default!;

    [Inject]
    private IPhonicsProgressService PhonicsProgress { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private List<PhonicsPhase> Phases = new();
    private readonly Dictionary<int, (int Completed, int Total)> PhaseProgress = new();
    private bool ShowResetConfirmation;

    protected override async Task OnInitializedAsync()
    {
        await LoadProgress();
    }

    private async Task LoadProgress()
    {
        Phases = PhonicsData.GetAllPhases();

        foreach (var phase in Phases)
        {
            PhaseProgress[phase.Id] = await PhonicsProgress.GetPhaseProgressAsync(phase.Id);
        }
    }

    private void ShowResetAllModal()
    {
        ShowResetConfirmation = true;
    }

    private void CancelReset()
    {
        ShowResetConfirmation = false;
    }

    private async Task ConfirmReset()
    {
        await PhonicsProgress.ResetAllAsync();
        ShowResetConfirmation = false;
        await LoadProgress();
    }

    private void GoBack()
    {
        Navigation.NavigateTo("");
    }
}
