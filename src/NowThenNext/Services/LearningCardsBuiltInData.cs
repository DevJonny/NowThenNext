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

    /// <summary>
    /// Returns the built-in Colours category with all colour cards.
    /// </summary>
    public static LearningCategory GetColoursCategory() => new()
    {
        Id = "colours",
        Name = "Colours",
        Emoji = "\ud83c\udfa8",
        IsBuiltIn = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };

    /// <summary>
    /// Returns all cards for the built-in Colours category.
    /// Each card displays a rounded rectangle swatch in the actual teaching colour.
    /// </summary>
    public static List<LearningCard> GetColoursCards() =>
    [
        new LearningCard
        {
            Id = "colours-red",
            CategoryId = "colours",
            Word = "Red",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#E74C3C"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-blue",
            CategoryId = "colours",
            Word = "Blue",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#3498DB"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-green",
            CategoryId = "colours",
            Word = "Green",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#2ECC71"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-yellow",
            CategoryId = "colours",
            Word = "Yellow",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#F1C40F"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-orange",
            CategoryId = "colours",
            Word = "Orange",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#E67E22"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-purple",
            CategoryId = "colours",
            Word = "Purple",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#9B59B6"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-pink",
            CategoryId = "colours",
            Word = "Pink",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#E91E9C"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-brown",
            CategoryId = "colours",
            Word = "Brown",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#8B4513"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-black",
            CategoryId = "colours",
            Word = "Black",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#2C3E50"/></svg>"""
        },
        new LearningCard
        {
            Id = "colours-white",
            CategoryId = "colours",
            Word = "White",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><rect x="10" y="10" width="80" height="80" rx="12" fill="#FFFFFF" stroke="#CCCCCC" stroke-width="2"/></svg>"""
        }
    ];

    /// <summary>
    /// Returns the built-in Animals category with all animal cards.
    /// </summary>
    public static LearningCategory GetAnimalsCategory() => new()
    {
        Id = "animals",
        Name = "Animals",
        Emoji = "\ud83d\udc3e",
        IsBuiltIn = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };

    /// <summary>
    /// Returns all cards for the built-in Animals category.
    /// Each card has a simple, friendly SVG illustration recognisable at ~120px.
    /// </summary>
    public static List<LearningCard> GetAnimalsCards() =>
    [
        new LearningCard
        {
            Id = "animals-cat",
            CategoryId = "animals",
            Word = "Cat",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="58" rx="28" ry="30" fill="#E8A87C"/><ellipse cx="50" cy="40" rx="22" ry="20" fill="#E8A87C"/><polygon points="32,28 28,8 40,22" fill="#E8A87C"/><polygon points="68,28 72,8 60,22" fill="#E8A87C"/><polygon points="32,28 28,8 40,22" fill="#D4937A" opacity="0.5"/><polygon points="68,28 72,8 60,22" fill="#D4937A" opacity="0.5"/><circle cx="42" cy="37" r="3" fill="#2C3E50"/><circle cx="58" cy="37" r="3" fill="#2C3E50"/><ellipse cx="50" cy="43" rx="3" ry="2" fill="#D4937A"/><path d="M47 46 Q50 50 53 46" stroke="#2C3E50" stroke-width="1.5" fill="none"/><path d="M22 58 Q8 55 5 50" stroke="#E8A87C" stroke-width="5" stroke-linecap="round" fill="none"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-dog",
            CategoryId = "animals",
            Word = "Dog",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="60" rx="30" ry="28" fill="#C4956A"/><ellipse cx="50" cy="38" rx="24" ry="20" fill="#C4956A"/><ellipse cx="28" cy="28" rx="10" ry="14" fill="#A67B5B" transform="rotate(-15 28 28)"/><ellipse cx="72" cy="28" rx="10" ry="14" fill="#A67B5B" transform="rotate(15 72 28)"/><circle cx="42" cy="35" r="3.5" fill="#2C3E50"/><circle cx="58" cy="35" r="3.5" fill="#2C3E50"/><ellipse cx="50" cy="43" rx="5" ry="4" fill="#2C3E50"/><path d="M45 48 Q50 52 55 48" stroke="#2C3E50" stroke-width="1.5" fill="none"/><ellipse cx="50" cy="50" rx="8" ry="5" fill="#D4B896" opacity="0.6"/><path d="M80 60 Q90 62 88 72" stroke="#C4956A" stroke-width="6" stroke-linecap="round" fill="none"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-fish",
            CategoryId = "animals",
            Word = "Fish",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="48" cy="50" rx="32" ry="20" fill="#5B9A9A"/><polygon points="80,50 98,35 98,65" fill="#5B9A9A"/><circle cx="35" cy="46" r="4" fill="white"/><circle cx="36" cy="45" r="2" fill="#2C3E50"/><path d="M48 38 Q55 32 62 38" stroke="#4A8A8A" stroke-width="2" fill="none"/><path d="M48 44 Q55 38 62 44" stroke="#4A8A8A" stroke-width="2" fill="none"/><path d="M40 55 Q50 60 60 55" stroke="#4A8A8A" stroke-width="1.5" fill="none"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-bird",
            CategoryId = "animals",
            Word = "Bird",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="50" rx="25" ry="22" fill="#7BA3C4"/><circle cx="50" cy="32" r="16" fill="#7BA3C4"/><circle cx="44" cy="30" r="2.5" fill="#2C3E50"/><polygon points="60,32 75,28 60,36" fill="#D4A06A"/><path d="M25 48 Q10 30 15 55" stroke="#6890B0" stroke-width="3" fill="#6890B0"/><path d="M75 48 Q90 30 85 55" stroke="#6890B0" stroke-width="3" fill="#6890B0"/><line x1="42" y1="72" x2="42" y2="88" stroke="#D4A06A" stroke-width="3" stroke-linecap="round"/><line x1="58" y1="72" x2="58" y2="88" stroke="#D4A06A" stroke-width="3" stroke-linecap="round"/><ellipse cx="50" cy="55" rx="12" ry="10" fill="#9BBDD6"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-horse",
            CategoryId = "animals",
            Word = "Horse",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="55" rx="30" ry="22" fill="#A67B5B"/><ellipse cx="28" cy="32" rx="12" ry="16" fill="#A67B5B" transform="rotate(-10 28 32)"/><polygon points="22,20 18,6 26,16" fill="#A67B5B"/><polygon points="34,20 38,6 30,16" fill="#A67B5B"/><circle cx="22" cy="30" r="2.5" fill="#2C3E50"/><ellipse cx="22" cy="40" rx="6" ry="4" fill="#8B6544"/><path d="M22 14 Q28 8 32 14" stroke="#5C3D1E" stroke-width="3" fill="none" stroke-linecap="round"/><line x1="35" y1="72" x2="35" y2="92" stroke="#A67B5B" stroke-width="5" stroke-linecap="round"/><line x1="48" y1="74" x2="48" y2="92" stroke="#A67B5B" stroke-width="5" stroke-linecap="round"/><line x1="55" y1="74" x2="55" y2="92" stroke="#A67B5B" stroke-width="5" stroke-linecap="round"/><line x1="65" y1="72" x2="65" y2="92" stroke="#A67B5B" stroke-width="5" stroke-linecap="round"/><path d="M78 50 Q92 45 90 60 Q88 70 82 65" stroke="#5C3D1E" stroke-width="3" fill="none" stroke-linecap="round"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-cow",
            CategoryId = "animals",
            Word = "Cow",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="55" rx="32" ry="24" fill="white" stroke="#CCCCCC" stroke-width="1"/><ellipse cx="40" cy="50" rx="10" ry="8" fill="#2C3E50"/><ellipse cx="65" cy="55" rx="8" ry="10" fill="#2C3E50"/><ellipse cx="50" cy="32" rx="20" ry="16" fill="white" stroke="#CCCCCC" stroke-width="1"/><circle cx="43" cy="29" r="3" fill="#2C3E50"/><circle cx="57" cy="29" r="3" fill="#2C3E50"/><ellipse cx="50" cy="39" rx="10" ry="6" fill="#E0B0A0"/><ellipse cx="46" cy="38" rx="2" ry="1.5" fill="#2C3E50"/><ellipse cx="54" cy="38" rx="2" ry="1.5" fill="#2C3E50"/><path d="M30 22 Q24 12 20 18" stroke="#A67B5B" stroke-width="3" fill="none" stroke-linecap="round"/><path d="M70 22 Q76 12 80 18" stroke="#A67B5B" stroke-width="3" fill="none" stroke-linecap="round"/><line x1="35" y1="76" x2="35" y2="92" stroke="#AAAAAA" stroke-width="5" stroke-linecap="round"/><line x1="50" y1="76" x2="50" y2="92" stroke="#AAAAAA" stroke-width="5" stroke-linecap="round"/><line x1="58" y1="76" x2="58" y2="92" stroke="#AAAAAA" stroke-width="5" stroke-linecap="round"/><line x1="68" y1="76" x2="68" y2="92" stroke="#AAAAAA" stroke-width="5" stroke-linecap="round"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-pig",
            CategoryId = "animals",
            Word = "Pig",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="52" rx="32" ry="26" fill="#F0B4B4"/><circle cx="50" cy="35" r="20" fill="#F0B4B4"/><circle cx="42" cy="31" r="3" fill="#2C3E50"/><circle cx="58" cy="31" r="3" fill="#2C3E50"/><ellipse cx="50" cy="40" rx="10" ry="7" fill="#E8A0A0"/><circle cx="46" cy="40" r="2.5" fill="#D4937A"/><circle cx="54" cy="40" r="2.5" fill="#D4937A"/><ellipse cx="33" cy="22" rx="8" ry="10" fill="#F0B4B4" transform="rotate(-15 33 22)"/><ellipse cx="33" cy="22" rx="6" ry="8" fill="#E8A0A0" transform="rotate(-15 33 22)"/><ellipse cx="67" cy="22" rx="8" ry="10" fill="#F0B4B4" transform="rotate(15 67 22)"/><ellipse cx="67" cy="22" rx="6" ry="8" fill="#E8A0A0" transform="rotate(15 67 22)"/><line x1="36" y1="74" x2="36" y2="90" stroke="#F0B4B4" stroke-width="6" stroke-linecap="round"/><line x1="50" y1="76" x2="50" y2="90" stroke="#F0B4B4" stroke-width="6" stroke-linecap="round"/><line x1="64" y1="74" x2="64" y2="90" stroke="#F0B4B4" stroke-width="6" stroke-linecap="round"/><path d="M80 48 Q86 44 84 52 Q82 56 80 52" stroke="#F0B4B4" stroke-width="3" fill="#F0B4B4"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-sheep",
            CategoryId = "animals",
            Word = "Sheep",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><circle cx="35" cy="50" r="12" fill="#F5F0E8"/><circle cx="50" cy="45" r="14" fill="#F5F0E8"/><circle cx="65" cy="50" r="12" fill="#F5F0E8"/><circle cx="40" cy="58" r="13" fill="#F5F0E8"/><circle cx="60" cy="58" r="13" fill="#F5F0E8"/><circle cx="50" cy="55" r="14" fill="#F5F0E8"/><circle cx="50" cy="30" r="14" fill="#2C3E50"/><circle cx="43" cy="28" r="2.5" fill="white"/><circle cx="57" cy="28" r="2.5" fill="white"/><ellipse cx="50" cy="34" rx="4" ry="2.5" fill="#E0B0A0"/><ellipse cx="36" cy="26" rx="5" ry="7" fill="#2C3E50" transform="rotate(-20 36 26)"/><ellipse cx="64" cy="26" rx="5" ry="7" fill="#2C3E50" transform="rotate(20 64 26)"/><line x1="38" y1="68" x2="38" y2="88" stroke="#2C3E50" stroke-width="5" stroke-linecap="round"/><line x1="50" y1="70" x2="50" y2="88" stroke="#2C3E50" stroke-width="5" stroke-linecap="round"/><line x1="62" y1="68" x2="62" y2="88" stroke="#2C3E50" stroke-width="5" stroke-linecap="round"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-rabbit",
            CategoryId = "animals",
            Word = "Rabbit",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="60" rx="24" ry="26" fill="#E8DDD0"/><circle cx="50" cy="38" r="18" fill="#E8DDD0"/><ellipse cx="40" cy="12" rx="6" ry="18" fill="#E8DDD0"/><ellipse cx="40" cy="12" rx="4" ry="14" fill="#E0B0A0"/><ellipse cx="60" cy="12" rx="6" ry="18" fill="#E8DDD0"/><ellipse cx="60" cy="12" rx="4" ry="14" fill="#E0B0A0"/><circle cx="43" cy="35" r="3" fill="#2C3E50"/><circle cx="57" cy="35" r="3" fill="#2C3E50"/><ellipse cx="50" cy="42" rx="3" ry="2" fill="#E0B0A0"/><line x1="50" y1="44" x2="50" y2="47" stroke="#2C3E50" stroke-width="1"/><line x1="32" y1="40" x2="18" y2="37" stroke="#2C3E50" stroke-width="1"/><line x1="32" y1="42" x2="18" y2="44" stroke="#2C3E50" stroke-width="1"/><line x1="68" y1="40" x2="82" y2="37" stroke="#2C3E50" stroke-width="1"/><line x1="68" y1="42" x2="82" y2="44" stroke="#2C3E50" stroke-width="1"/><circle cx="70" cy="75" r="8" fill="#E8DDD0"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-elephant",
            CategoryId = "animals",
            Word = "Elephant",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="50" rx="34" ry="28" fill="#9BBDD6"/><circle cx="38" cy="30" r="18" fill="#9BBDD6"/><path d="M26 38 Q20 50 22 65 Q24 70 28 68 Q30 60 32 50" fill="#9BBDD6" stroke="#7BA3C4" stroke-width="1"/><circle cx="33" cy="27" r="3" fill="#2C3E50"/><ellipse cx="20" cy="30" rx="10" ry="14" fill="#9BBDD6"/><ellipse cx="20" cy="30" rx="7" ry="10" fill="#7BA3C4" opacity="0.5"/><line x1="36" y1="75" x2="36" y2="92" stroke="#9BBDD6" stroke-width="7" stroke-linecap="round"/><line x1="50" y1="76" x2="50" y2="92" stroke="#9BBDD6" stroke-width="7" stroke-linecap="round"/><line x1="58" y1="76" x2="58" y2="92" stroke="#9BBDD6" stroke-width="7" stroke-linecap="round"/><line x1="68" y1="75" x2="68" y2="92" stroke="#9BBDD6" stroke-width="7" stroke-linecap="round"/><path d="M82 45 Q88 48 86 55" stroke="#9BBDD6" stroke-width="4" fill="none" stroke-linecap="round"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-lion",
            CategoryId = "animals",
            Word = "Lion",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><circle cx="50" cy="42" r="28" fill="#D4A06A"/><circle cx="50" cy="42" r="20" fill="#E8C88A"/><circle cx="43" cy="38" r="3" fill="#2C3E50"/><circle cx="57" cy="38" r="3" fill="#2C3E50"/><ellipse cx="50" cy="46" rx="4" ry="3" fill="#2C3E50"/><path d="M46 50 Q50 54 54 50" stroke="#2C3E50" stroke-width="1.5" fill="none"/><ellipse cx="50" cy="72" rx="22" ry="18" fill="#E8C88A"/><line x1="38" y1="86" x2="38" y2="96" stroke="#E8C88A" stroke-width="6" stroke-linecap="round"/><line x1="50" y1="88" x2="50" y2="96" stroke="#E8C88A" stroke-width="6" stroke-linecap="round"/><line x1="62" y1="86" x2="62" y2="96" stroke="#E8C88A" stroke-width="6" stroke-linecap="round"/><path d="M80 68 Q90 65 88 75" stroke="#D4A06A" stroke-width="4" fill="none" stroke-linecap="round"/></svg>"""
        },
        new LearningCard
        {
            Id = "animals-tiger",
            CategoryId = "animals",
            Word = "Tiger",
            IsBuiltIn = true,
            ImageData = """<svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"><ellipse cx="50" cy="58" rx="28" ry="26" fill="#E8A84C"/><circle cx="50" cy="35" r="22" fill="#E8A84C"/><ellipse cx="33" cy="18" rx="8" ry="10" fill="#E8A84C"/><ellipse cx="33" cy="18" rx="5" ry="7" fill="#F5D6A0"/><ellipse cx="67" cy="18" rx="8" ry="10" fill="#E8A84C"/><ellipse cx="67" cy="18" rx="5" ry="7" fill="#F5D6A0"/><circle cx="42" cy="32" r="3" fill="#2C3E50"/><circle cx="58" cy="32" r="3" fill="#2C3E50"/><ellipse cx="50" cy="40" rx="4" ry="3" fill="#E0B0A0"/><path d="M47 44 Q50 47 53 44" stroke="#2C3E50" stroke-width="1.5" fill="none"/><ellipse cx="50" cy="36" rx="14" ry="8" fill="#F5D6A0" opacity="0.6"/><line x1="44" y1="24" x2="40" y2="18" stroke="#2C3E50" stroke-width="2.5" stroke-linecap="round"/><line x1="50" y1="22" x2="50" y2="16" stroke="#2C3E50" stroke-width="2.5" stroke-linecap="round"/><line x1="56" y1="24" x2="60" y2="18" stroke="#2C3E50" stroke-width="2.5" stroke-linecap="round"/><line x1="38" y1="52" x2="28" y2="48" stroke="#2C3E50" stroke-width="2" stroke-linecap="round"/><line x1="38" y1="58" x2="28" y2="58" stroke="#2C3E50" stroke-width="2" stroke-linecap="round"/><line x1="62" y1="52" x2="72" y2="48" stroke="#2C3E50" stroke-width="2" stroke-linecap="round"/><line x1="62" y1="58" x2="72" y2="58" stroke="#2C3E50" stroke-width="2" stroke-linecap="round"/><line x1="36" y1="82" x2="36" y2="94" stroke="#E8A84C" stroke-width="6" stroke-linecap="round"/><line x1="50" y1="82" x2="50" y2="94" stroke="#E8A84C" stroke-width="6" stroke-linecap="round"/><line x1="64" y1="82" x2="64" y2="94" stroke="#E8A84C" stroke-width="6" stroke-linecap="round"/><path d="M78 56 Q88 52 86 62 Q84 68 80 64" stroke="#E8A84C" stroke-width="4" fill="#E8A84C"/></svg>"""
        }
    ];

    /// <summary>
    /// Returns the built-in Dinosaurs category with all dinosaur cards.
    /// </summary>
    public static LearningCategory GetDinosaursCategory() => new()
    {
        Id = "dinosaurs",
        Name = "Dinosaurs",
        Emoji = "\ud83e\udd95",
        IsBuiltIn = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };

    /// <summary>
    /// Returns all cards for the built-in Dinosaurs category.
    /// Each card uses a DBCLS/Togopic illustration (CC BY 4.0) from Wikimedia Commons.
    /// </summary>
    public static List<LearningCard> GetDinosaursCards() =>
    [
        new LearningCard
        {
            Id = "dinosaurs-trex",
            CategoryId = "dinosaurs",
            Word = "T-Rex",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/trex.png"
        },
        new LearningCard
        {
            Id = "dinosaurs-triceratops",
            CategoryId = "dinosaurs",
            Word = "Triceratops",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/triceratops.png"
        },
        new LearningCard
        {
            Id = "dinosaurs-stegosaurus",
            CategoryId = "dinosaurs",
            Word = "Stegosaurus",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/stegosaurus.png"
        },
        new LearningCard
        {
            Id = "dinosaurs-brachiosaurus",
            CategoryId = "dinosaurs",
            Word = "Brachiosaurus",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/brachiosaurus.png"
        },
        new LearningCard
        {
            Id = "dinosaurs-allosaurus",
            CategoryId = "dinosaurs",
            Word = "Allosaurus",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/allosaurus.png"
        },
        new LearningCard
        {
            Id = "dinosaurs-pteranodon",
            CategoryId = "dinosaurs",
            Word = "Pteranodon",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/pteranodon.png"
        },
        new LearningCard
        {
            Id = "dinosaurs-ankylosaurus",
            CategoryId = "dinosaurs",
            Word = "Ankylosaurus",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/ankylosaurus.png"
        },
        new LearningCard
        {
            Id = "dinosaurs-diplodocus",
            CategoryId = "dinosaurs",
            Word = "Diplodocus",
            IsBuiltIn = true,
            ImageData = "images/dinosaurs/diplodocus.png"
        }
    ];
}
