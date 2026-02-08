using System.Text.Json;

namespace NowThenNext.Models;

/// <summary>
/// Represents a backup of all NowThenNext data for export/import
/// </summary>
public class BackupData
{
    /// <summary>
    /// Version of the backup format for future compatibility
    /// </summary>
    public string Version { get; set; } = "1.0";

    /// <summary>
    /// Timestamp when the backup was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// All images (Places, Food, and Activities) with their full data
    /// </summary>
    public List<ImageItem> Images { get; set; } = new();

    /// <summary>
    /// Custom learning cards data (categories and cards created by the user).
    /// Stored as a raw JSON element to preserve the localStorage format.
    /// Null when no custom learning cards exist or when restoring from an older backup.
    /// </summary>
    public JsonElement? LearningCardsData { get; set; }
}
