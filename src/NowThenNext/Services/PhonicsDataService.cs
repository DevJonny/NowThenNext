using NowThenNext.Models;

namespace NowThenNext.Services;

public class PhonicsDataService : IPhonicsDataService
{
    private readonly List<PhonicsPhase> _phases;
    private readonly Dictionary<string, GraphemeCard> _cardLookup;

    public PhonicsDataService()
    {
        _phases = BuildAllPhases();
        AssignImagePaths(_phases);
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
        BuildPhase4(),
        BuildPhase5(),
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

    private static PhonicsPhase BuildPhase4()
    {
        var phase = new PhonicsPhase
        {
            Id = 4,
            Name = "Phase 4",
            Description = "Consonant clusters",
            Weeks = new()
        };

        var orderIndex = 0;

        // Week 1: Initial consonant clusters
        phase.Weeks.Add(BuildWeek(4, 1, ref orderIndex,
            ("bl", "black", "bl is for black"),
            ("br", "bring", "br is for bring"),
            ("cl", "clap", "cl is for clap"),
            ("cr", "crab", "cr is for crab"),
            ("dr", "drop", "dr is for drop"),
            ("fl", "flag", "fl is for flag"),
            ("fr", "frog", "fr is for frog"),
            ("gl", "glad", "gl is for glad"),
            ("gr", "grip", "gr is for grip"),
            ("pl", "plan", "pl is for plan"),
            ("pr", "press", "pr is for press"),
            ("sc", "scam", "sc is for scam"),
            ("sk", "skip", "sk is for skip"),
            ("sl", "slip", "sl is for slip"),
            ("sm", "smell", "sm is for smell"),
            ("sn", "snap", "sn is for snap"),
            ("sp", "spot", "sp is for spot"),
            ("st", "stop", "st is for stop"),
            ("sw", "swim", "sw is for swim"),
            ("tr", "trip", "tr is for trip"),
            ("tw", "twin", "tw is for twin")
        ));

        // Week 2: Final consonant clusters
        phase.Weeks.Add(BuildWeek(4, 2, ref orderIndex,
            ("ft", "left", "ft is for left"),
            ("lf", "self", "lf is for self"),
            ("lp", "help", "lp is for help"),
            ("lt", "felt", "lt is for felt"),
            ("mp", "lamp", "mp is for lamp"),
            ("nd", "sand", "nd is for sand"),
            ("nk", "pink", "nk is for pink"),
            ("nt", "tent", "nt is for tent"),
            ("pt", "kept", "pt is for kept"),
            ("sk", "task", "sk is for task"),
            ("st", "best", "st is for best"),
            ("xt", "next", "xt is for next")
        ));

        return phase;
    }

    private static PhonicsPhase BuildPhase5()
    {
        var phase = new PhonicsPhase
        {
            Id = 5,
            Name = "Phase 5",
            Description = "Alternative spellings & split digraphs",
            Weeks = new()
        };

        var orderIndex = 0;

        // Week 1: New spellings
        phase.Weeks.Add(BuildWeek(5, 1, ref orderIndex,
            ("ay", "play", "ay is for play"),
            ("ou", "cloud", "ou is for cloud"),
            ("ie", "pie", "ie is for pie"),
            ("ea", "each", "ea is for each"),
            ("oy", "toy", "oy is for toy"),
            ("ir", "bird", "ir is for bird"),
            ("ue", "blue", "ue is for blue"),
            ("aw", "saw", "aw is for saw"),
            ("wh", "when", "wh is for when"),
            ("ph", "phone", "ph is for phone"),
            ("ew", "new", "ew is for new"),
            ("oe", "toe", "oe is for toe"),
            ("au", "August", "au is for August")
        ));

        // Week 2: Split digraphs
        phase.Weeks.Add(BuildWeek(5, 2, ref orderIndex,
            ("a_e", "cake", "a_e is for cake"),
            ("e_e", "these", "e_e is for these"),
            ("i_e", "time", "i_e is for time"),
            ("o_e", "home", "o_e is for home"),
            ("u_e", "tube", "u_e is for tube")
        ));

        // Week 3: Alternative pronunciations (manual cards due to duplicate graphemes)
        phase.Weeks.Add(new PhonicsWeek
        {
            WeekNumber = 3,
            Cards = new List<GraphemeCard>
            {
                new() { Id = "p5-w3-ow_snow", Grapheme = "ow", ExampleWord = "snow", KeywordHint = "ow can say snow", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ow_cow", Grapheme = "ow", ExampleWord = "cow", KeywordHint = "ow can say cow", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ie_pie", Grapheme = "ie", ExampleWord = "pie", KeywordHint = "ie can say pie", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ie_field", Grapheme = "ie", ExampleWord = "field", KeywordHint = "ie can say field", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ea_eat", Grapheme = "ea", ExampleWord = "eat", KeywordHint = "ea can say eat", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ea_bread", Grapheme = "ea", ExampleWord = "bread", KeywordHint = "ea can say bread", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ou_cloud", Grapheme = "ou", ExampleWord = "cloud", KeywordHint = "ou can say cloud", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ou_shoulder", Grapheme = "ou", ExampleWord = "shoulder", KeywordHint = "ou can say shoulder", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ou_could", Grapheme = "ou", ExampleWord = "could", KeywordHint = "ou can say could", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ey_key", Grapheme = "ey", ExampleWord = "key", KeywordHint = "ey can say key", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ey_they", Grapheme = "ey", ExampleWord = "they", KeywordHint = "ey can say they", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-a_acorn", Grapheme = "a", ExampleWord = "acorn", KeywordHint = "a can say acorn", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-e_me", Grapheme = "e", ExampleWord = "me", KeywordHint = "e can say me", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-i_tiger", Grapheme = "i", ExampleWord = "tiger", KeywordHint = "i can say tiger", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-o_go", Grapheme = "o", ExampleWord = "go", KeywordHint = "o can say go", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-u_unicorn", Grapheme = "u", ExampleWord = "unicorn", KeywordHint = "u can say unicorn", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-y_happy", Grapheme = "y", ExampleWord = "happy", KeywordHint = "y can say happy", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-y_fly", Grapheme = "y", ExampleWord = "fly", KeywordHint = "y can say fly", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ch_school", Grapheme = "ch", ExampleWord = "school", KeywordHint = "ch can say school", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
                new() { Id = "p5-w3-ch_chef", Grapheme = "ch", ExampleWord = "chef", KeywordHint = "ch can say chef", PhaseId = 5, WeekNumber = 3, OrderIndex = orderIndex++ },
            }
        });

        // Week 4: Silent letters
        phase.Weeks.Add(BuildWeek(5, 4, ref orderIndex,
            ("wr", "write", "wr is for write"),
            ("kn", "knee", "kn is for knee"),
            ("gn", "gnome", "gn is for gnome"),
            ("mb", "lamb", "mb is for lamb")
        ));

        return phase;
    }

    private static readonly HashSet<string> CardsWithImages = new()
    {
        // Phase 2
        "p2-w1-s", "p2-w1-a", "p2-w1-t", "p2-w1-p",
        "p2-w2-n", "p2-w2-d",
        "p2-w3-c", "p2-w3-k",
        "p2-w4-ck", "p2-w4-e", "p2-w4-r",
        "p2-w5-f", "p2-w5-ff", "p2-w5-h", "p2-w5-b", "p2-w5-ss", "p2-w5-l", "p2-w5-ll",
        "p2-w7-w", "p2-w7-v", "p2-w7-x", "p2-w7-y",
        "p2-w8-zz", "p2-w8-ch", "p2-w8-qu",
        "p2-w9-nk", "p2-w9-ng", "p2-w9-sh",
        // Phase 3
        "p3-w1-ee", "p3-w1-igh", "p3-w1-ai", "p3-w1-oa",
        "p3-w2-oo_long", "p3-w2-oo_short", "p3-w2-ar", "p3-w2-or",
        "p3-w3-ow", "p3-w3-oi", "p3-w3-ur", "p3-w3-ear",
        "p3-w4-er", "p3-w4-air",
        // Phase 4
        "p4-w1-bl", "p4-w1-cl", "p4-w1-cr", "p4-w1-dr", "p4-w1-fl", "p4-w1-fr",
        "p4-w1-sk", "p4-w1-sm", "p4-w1-sp", "p4-w1-st", "p4-w1-sw", "p4-w1-tr",
        "p4-w2-mp", "p4-w2-nd", "p4-w2-nt",
        // Phase 5
        "p5-w1-ay", "p5-w1-ou", "p5-w1-ie", "p5-w1-oy", "p5-w1-ir",
        "p5-w1-ue", "p5-w1-ph", "p5-w1-oe", "p5-w1-aw",
        "p5-w2-a_e", "p5-w2-i_e", "p5-w2-o_e", "p5-w2-u_e",
        "p5-w3-ow_snow", "p5-w3-ow_cow", "p5-w3-ie_pie", "p5-w3-ie_field",
        "p5-w3-ea_bread", "p5-w3-ea_eat",
        "p5-w3-ou_cloud", "p5-w3-ey_key", "p5-w3-i_tiger", "p5-w3-u_unicorn",
        "p5-w3-y_fly", "p5-w3-y_happy", "p5-w3-ch_school", "p5-w3-ch_chef",
        "p5-w3-a_acorn",
        "p5-w4-wr", "p5-w4-kn", "p5-w4-gn", "p5-w4-mb",
    };

    private static void AssignImagePaths(List<PhonicsPhase> phases)
    {
        foreach (var card in phases
                     .SelectMany(p => p.Weeks)
                     .SelectMany(w => w.Cards)
                     .Where(card => CardsWithImages.Contains(card.Id)))
        {
            card.ImagePath = $"images/phonics/{card.Id}.png";
        }
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
