using System.Text.Json;
using Microsoft.JSInterop;

namespace NowThenNext.Services;

public class PhonicsProgressService : IPhonicsProgressService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IPhonicsDataService _phonicsData;
    private const string StorageKey = "phonics-progress";

    public PhonicsProgressService(IJSRuntime jsRuntime, IPhonicsDataService phonicsData)
    {
        _jsRuntime = jsRuntime;
        _phonicsData = phonicsData;
    }

    public async Task MarkCompletedAsync(string graphemeId)
    {
        var completed = await GetCompletedSetAsync();
        completed.Add(graphemeId);
        await SaveCompletedSetAsync(completed);
    }

    public async Task<bool> IsCompletedAsync(string graphemeId)
    {
        var completed = await GetCompletedSetAsync();
        return completed.Contains(graphemeId);
    }

    public async Task<string?> GetNextUnlockedGraphemeIdAsync(int phaseId)
    {
        var phase = _phonicsData.GetPhase(phaseId);
        if (phase == null) return null;

        var completed = await GetCompletedSetAsync();
        var allCards = phase.Weeks
            .SelectMany(w => w.Cards)
            .OrderBy(c => c.OrderIndex)
            .ToList();

        // First grapheme is always unlocked; find the first non-completed one
        foreach (var card in allCards)
        {
            if (!completed.Contains(card.Id))
                return card.Id;
        }

        // All completed
        return null;
    }

    public async Task<(int Completed, int Total)> GetPhaseProgressAsync(int phaseId)
    {
        var phase = _phonicsData.GetPhase(phaseId);
        if (phase == null) return (0, 0);

        var completed = await GetCompletedSetAsync();
        var allCards = phase.Weeks.SelectMany(w => w.Cards).ToList();
        var completedCount = allCards.Count(c => completed.Contains(c.Id));

        return (completedCount, allCards.Count);
    }

    public async Task ResetPhaseAsync(int phaseId)
    {
        var phase = _phonicsData.GetPhase(phaseId);
        if (phase == null) return;

        var phaseCardIds = phase.Weeks
            .SelectMany(w => w.Cards)
            .Select(c => c.Id)
            .ToHashSet();

        var completed = await GetCompletedSetAsync();
        completed.ExceptWith(phaseCardIds);
        await SaveCompletedSetAsync(completed);
    }

    public async Task ResetAllAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }

    private async Task<HashSet<string>> GetCompletedSetAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (string.IsNullOrEmpty(json))
                return new HashSet<string>();

            return JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
        }
        catch
        {
            return new HashSet<string>();
        }
    }

    private async Task SaveCompletedSetAsync(HashSet<string> completed)
    {
        var json = JsonSerializer.Serialize(completed);
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch (JSException ex) when (ex.Message.Contains("QuotaExceededError") ||
                                      ex.Message.Contains("quota", StringComparison.OrdinalIgnoreCase))
        {
            throw new StorageQuotaExceededException("Storage quota exceeded. Please delete some data to free up space.", ex);
        }
        catch (Exception)
        {
            // Prevent unhandled exceptions from breaking the UI
        }
    }
}
