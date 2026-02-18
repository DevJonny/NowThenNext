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
    /// Each card uses a Noto Emoji illustration (Apache 2.0) from Google.
    /// </summary>
    public static List<LearningCard> GetAnimalsCards() =>
    [
        new LearningCard
        {
            Id = "animals-cat",
            CategoryId = "animals",
            Word = "Cat",
            IsBuiltIn = true,
            ImageData = "images/animals/cat.png"
        },
        new LearningCard
        {
            Id = "animals-dog",
            CategoryId = "animals",
            Word = "Dog",
            IsBuiltIn = true,
            ImageData = "images/animals/dog.png"
        },
        new LearningCard
        {
            Id = "animals-fish",
            CategoryId = "animals",
            Word = "Fish",
            IsBuiltIn = true,
            ImageData = "images/animals/fish.png"
        },
        new LearningCard
        {
            Id = "animals-bird",
            CategoryId = "animals",
            Word = "Bird",
            IsBuiltIn = true,
            ImageData = "images/animals/bird.png"
        },
        new LearningCard
        {
            Id = "animals-horse",
            CategoryId = "animals",
            Word = "Horse",
            IsBuiltIn = true,
            ImageData = "images/animals/horse.png"
        },
        new LearningCard
        {
            Id = "animals-cow",
            CategoryId = "animals",
            Word = "Cow",
            IsBuiltIn = true,
            ImageData = "images/animals/cow.png"
        },
        new LearningCard
        {
            Id = "animals-pig",
            CategoryId = "animals",
            Word = "Pig",
            IsBuiltIn = true,
            ImageData = "images/animals/pig.png"
        },
        new LearningCard
        {
            Id = "animals-sheep",
            CategoryId = "animals",
            Word = "Sheep",
            IsBuiltIn = true,
            ImageData = "images/animals/sheep.png"
        },
        new LearningCard
        {
            Id = "animals-rabbit",
            CategoryId = "animals",
            Word = "Rabbit",
            IsBuiltIn = true,
            ImageData = "images/animals/rabbit.png"
        },
        new LearningCard
        {
            Id = "animals-elephant",
            CategoryId = "animals",
            Word = "Elephant",
            IsBuiltIn = true,
            ImageData = "images/animals/elephant.png"
        },
        new LearningCard
        {
            Id = "animals-lion",
            CategoryId = "animals",
            Word = "Lion",
            IsBuiltIn = true,
            ImageData = "images/animals/lion.png"
        },
        new LearningCard
        {
            Id = "animals-tiger",
            CategoryId = "animals",
            Word = "Tiger",
            IsBuiltIn = true,
            ImageData = "images/animals/tiger.png"
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
