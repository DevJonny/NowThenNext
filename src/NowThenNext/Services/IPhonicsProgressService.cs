namespace NowThenNext.Services;

public interface IPhonicsProgressService
{
    Task MarkCompletedAsync(string graphemeId);
    Task<bool> IsCompletedAsync(string graphemeId);
    Task<string?> GetNextUnlockedGraphemeIdAsync(int phaseId);
    Task<(int Completed, int Total)> GetPhaseProgressAsync(int phaseId);
    Task ResetPhaseAsync(int phaseId);
    Task ResetAllAsync();
}
