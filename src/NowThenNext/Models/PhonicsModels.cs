namespace NowThenNext.Models;

public class PhonicsPhase
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<PhonicsWeek> Weeks { get; set; } = new();
}

public class PhonicsWeek
{
    public int WeekNumber { get; set; }
    public List<GraphemeCard> Cards { get; set; } = new();
}

public class GraphemeCard
{
    public string Id { get; set; } = string.Empty;
    public string Grapheme { get; set; } = string.Empty;
    public string ExampleWord { get; set; } = string.Empty;
    public string KeywordHint { get; set; } = string.Empty;
    public int PhaseId { get; set; }
    public int WeekNumber { get; set; }
    public int OrderIndex { get; set; }
    public string? ImagePath { get; set; }
}
