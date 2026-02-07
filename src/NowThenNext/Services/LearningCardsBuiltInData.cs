using NowThenNext.Models;

namespace NowThenNext.Services;

/// <summary>
/// Provides built-in learning card categories and cards with hand-crafted SVG illustrations.
/// </summary>
public static class LearningCardsBuiltInData
{
    /// <summary>
    /// Returns the built-in Shapes category with all shape cards.
    /// </summary>
    public static LearningCategory GetShapesCategory() => new()
    {
        Id = "shapes",
        Name = "Shapes",
        Emoji = "\ud83d\udd37",
        IsBuiltIn = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };

    /// <summary>
    /// Returns all cards for the built-in Shapes category.
    /// </summary>
    public static List<LearningCard> GetShapesCards() =>
    [
        new LearningCard
        {
            Id = "shapes-circle",
            CategoryId = "shapes",
            Word = "Circle",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><circle cx="50" cy="50" r="42" fill="#5B9A9A"/></svg>"""
        },
        new LearningCard
        {
            Id = "shapes-square",
            CategoryId = "shapes",
            Word = "Square",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="4" fill="#7BA893"/></svg>"""
        },
        new LearningCard
        {
            Id = "shapes-triangle",
            CategoryId = "shapes",
            Word = "Triangle",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><polygon points="50,8 92,88 8,88" fill="#D4937A"/></svg>"""
        },
        new LearningCard
        {
            Id = "shapes-rectangle",
            CategoryId = "shapes",
            Word = "Rectangle",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="5" y="22" width="90" height="56" rx="4" fill="#7BA3C4"/></svg>"""
        },
        new LearningCard
        {
            Id = "shapes-star",
            CategoryId = "shapes",
            Word = "Star",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><polygon points="50,5 61,38 97,38 68,59 79,93 50,72 21,93 32,59 3,38 39,38" fill="#D4A06A"/></svg>"""
        },
        new LearningCard
        {
            Id = "shapes-heart",
            CategoryId = "shapes",
            Word = "Heart",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><path d="M50 88 C25 65 5 50 5 32 5 18 16 8 30 8 38 8 45 12 50 20 55 12 62 8 70 8 84 8 95 18 95 32 95 50 75 65 50 88Z" fill="#E0B0A0"/></svg>"""
        },
        new LearningCard
        {
            Id = "shapes-diamond",
            CategoryId = "shapes",
            Word = "Diamond",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><polygon points="50,5 92,50 50,95 8,50" fill="#9BBDD6"/></svg>"""
        },
        new LearningCard
        {
            Id = "shapes-oval",
            CategoryId = "shapes",
            Word = "Oval",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="50" rx="45" ry="30" fill="#B87A62"/></svg>"""
        }
    ];
}
