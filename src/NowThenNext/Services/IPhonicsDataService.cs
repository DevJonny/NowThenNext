using NowThenNext.Models;

namespace NowThenNext.Services;

public interface IPhonicsDataService
{
    List<PhonicsPhase> GetAllPhases();
    PhonicsPhase? GetPhase(int phaseId);
    GraphemeCard? GetGraphemeCard(string cardId);
}
