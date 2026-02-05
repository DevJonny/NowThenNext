using NowThenNext.Models;

namespace NowThenNext.Services;

public class PhonicsDataService : IPhonicsDataService
{
    private readonly List<PhonicsPhase> _phases;
    private readonly Dictionary<string, GraphemeCard> _cardLookup;

    public PhonicsDataService()
    {
        _phases = BuildAllPhases();
        _cardLookup = _phases
            .SelectMany(p => p.Weeks)
            .SelectMany(w => w.Cards)
            .ToDictionary(c => c.Id);
    }

    public List<PhonicsPhase> GetAllPhases() => _phases;

    public PhonicsPhase? GetPhase(int phaseId) =>
        _phases.FirstOrDefault(p => p.Id == phaseId);

    public GraphemeCard? GetGraphemeCard(string cardId) =>
        _cardLookup.GetValueOrDefault(cardId);

    private static List<PhonicsPhase> BuildAllPhases() =>
    [
        BuildPhase2(),
        BuildPhase3(),
    ];

    private static PhonicsPhase BuildPhase2()
    {
        var phase = new PhonicsPhase
        {
            Id = 2,
            Name = "Phase 2",
            Description = "Single letters & first digraphs",
            Weeks = new()
        };

        var orderIndex = 0;

        // Week 1: s, a, t, p
        phase.Weeks.Add(BuildWeek(2, 1, ref orderIndex,
            ("s", "sun", "s is for sun"),
            ("a", "ant", "a is for ant"),
            ("t", "tap", "t is for tap"),
            ("p", "pin", "p is for pin")
        ));

        // Week 2: i, n, m, d
        phase.Weeks.Add(BuildWeek(2, 2, ref orderIndex,
            ("i", "it", "i is for it"),
            ("n", "net", "n is for net"),
            ("m", "mat", "m is for mat"),
            ("d", "dog", "d is for dog")
        ));

        // Week 3: g, o, c, k
        phase.Weeks.Add(BuildWeek(2, 3, ref orderIndex,
            ("g", "got", "g is for got"),
            ("o", "on", "o is for on"),
            ("c", "cat", "c is for cat"),
            ("k", "kid", "k is for kid")
        ));

        // Week 4: ck, e, u, r
        phase.Weeks.Add(BuildWeek(2, 4, ref orderIndex,
            ("ck", "duck", "ck is for duck"),
            ("e", "egg", "e is for egg"),
            ("u", "up", "u is for up"),
            ("r", "red", "r is for red")
        ));

        // Week 5: h, b, f, ff, l, ll, ss
        phase.Weeks.Add(BuildWeek(2, 5, ref orderIndex,
            ("h", "hat", "h is for hat"),
            ("b", "bat", "b is for bat"),
            ("f", "fan", "f is for fan"),
            ("ff", "puff", "ff is for puff"),
            ("l", "leg", "l is for leg"),
            ("ll", "bell", "ll is for bell"),
            ("ss", "hiss", "ss is for hiss")
        ));

        // Week 6: Assessment/review - skipped

        // Week 7: v, w, x, y
        phase.Weeks.Add(BuildWeek(2, 7, ref orderIndex,
            ("v", "van", "v is for van"),
            ("w", "win", "w is for win"),
            ("x", "fox", "x is for fox"),
            ("y", "yes", "y is for yes")
        ));

        // Week 8: z, zz, qu, ch
        phase.Weeks.Add(BuildWeek(2, 8, ref orderIndex,
            ("z", "zip", "z is for zip"),
            ("zz", "buzz", "zz is for buzz"),
            ("qu", "queen", "qu is for queen"),
            ("ch", "chip", "ch is for chip")
        ));

        // Week 9: sh, th, ng, nk
        phase.Weeks.Add(BuildWeek(2, 9, ref orderIndex,
            ("sh", "shop", "sh is for shop"),
            ("th", "thin", "th is for thin"),
            ("ng", "ring", "ng is for ring"),
            ("nk", "pink", "nk is for pink")
        ));

        return phase;
    }

    private static PhonicsPhase BuildPhase3()
    {
        var phase = new PhonicsPhase
        {
            Id = 3,
            Name = "Phase 3",
            Description = "Vowel digraphs & trigraphs",
            Weeks = new()
        };

        var orderIndex = 0;

        // Week 1: ai, ee, igh, oa
        phase.Weeks.Add(BuildWeek(3, 1, ref orderIndex,
            ("ai", "rain", "ai is for rain"),
            ("ee", "feet", "ee is for feet"),
            ("igh", "night", "igh is for night"),
            ("oa", "boat", "oa is for boat")
        ));

        // Week 2: oo (long), oo (short), ar, or
        phase.Weeks.Add(new PhonicsWeek
        {
            WeekNumber = 2,
            Cards = new List<GraphemeCard>
            {
                new()
                {
                    Id = "p3-w2-oo_long",
                    Grapheme = "oo",
                    ExampleWord = "moon",
                    KeywordHint = "oo is for moon",
                    PhaseId = 3,
                    WeekNumber = 2,
                    OrderIndex = orderIndex++
                },
                new()
                {
                    Id = "p3-w2-oo_short",
                    Grapheme = "oo",
                    ExampleWord = "book",
                    KeywordHint = "oo is for book",
                    PhaseId = 3,
                    WeekNumber = 2,
                    OrderIndex = orderIndex++
                },
                new()
                {
                    Id = "p3-w2-ar",
                    Grapheme = "ar",
                    ExampleWord = "car",
                    KeywordHint = "ar is for car",
                    PhaseId = 3,
                    WeekNumber = 2,
                    OrderIndex = orderIndex++
                },
                new()
                {
                    Id = "p3-w2-or",
                    Grapheme = "or",
                    ExampleWord = "fork",
                    KeywordHint = "or is for fork",
                    PhaseId = 3,
                    WeekNumber = 2,
                    OrderIndex = orderIndex++
                }
            }
        });

        // Week 3: ur, ow, oi, ear
        phase.Weeks.Add(BuildWeek(3, 3, ref orderIndex,
            ("ur", "nurse", "ur is for nurse"),
            ("ow", "cow", "ow is for cow"),
            ("oi", "coin", "oi is for coin"),
            ("ear", "dear", "ear is for dear")
        ));

        // Week 4: air, er
        phase.Weeks.Add(BuildWeek(3, 4, ref orderIndex,
            ("air", "fair", "air is for fair"),
            ("er", "letter", "er is for letter")
        ));

        return phase;
    }

    private static PhonicsWeek BuildWeek(int phaseId, int weekNumber, ref int orderIndex,
        params (string grapheme, string exampleWord, string keywordHint)[] cards)
    {
        var week = new PhonicsWeek
        {
            WeekNumber = weekNumber,
            Cards = new()
        };

        foreach (var (grapheme, exampleWord, keywordHint) in cards)
        {
            week.Cards.Add(new GraphemeCard
            {
                Id = $"p{phaseId}-w{weekNumber}-{grapheme}",
                Grapheme = grapheme,
                ExampleWord = exampleWord,
                KeywordHint = keywordHint,
                PhaseId = phaseId,
                WeekNumber = weekNumber,
                OrderIndex = orderIndex++
            });
        }

        return week;
    }
}
