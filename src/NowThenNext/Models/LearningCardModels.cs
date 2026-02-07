namespace NowThenNext.Models;

/// <summary>
/// Represents a category of learning cards (e.g. Shapes, Colours, Animals, Dinosaurs, or user-created)
/// </summary>
public class LearningCategory
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public bool IsBuiltIn { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents a single learning flashcard with an image and a word
/// </summary>
public class LearningCard
{
    public string Id { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string ImageData { get; set; } = string.Empty;
    public string Word { get; set; } = string.Empty;
    public bool IsBuiltIn { get; set; }
}
