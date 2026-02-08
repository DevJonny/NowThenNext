using System.Text.Json;
using Microsoft.Playwright;
using NowThenNext.Tests.E2E.Fixtures;
using NowThenNext.Tests.E2E.Helpers;

namespace NowThenNext.Tests.E2E.Tests;

/// <summary>
/// E2E tests for backup and restore functionality.
/// Tests the complete backup/restore flow from settings panel.
/// </summary>
[Collection("BlazorApp")]
public class BackupRestoreTests
{
    private readonly BlazorAppFixture _fixture;

    public BackupRestoreTests(BlazorAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SettingsGearIcon_VisibleOnHomeScreen()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("h1", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Assert - Settings gear icon should be visible
            var settingsButton = page.Locator("a.settings-button");
            await Assertions.Expect(settingsButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task SettingsPanel_OpensWhenGearTapped()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("h1", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - Click the settings gear
            var settingsButton = page.Locator("a.settings-button");
            await settingsButton.ClickAsync();

            // Wait for navigation to settings page
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/settings", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - Settings panel content is visible
            var heading = page.Locator("h1:has-text('Settings')");
            await Assertions.Expect(heading).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Assert - Both backup and restore buttons are visible
            var backupButton = page.Locator("button:has-text('Backup Data')");
            var restoreButton = page.Locator("button:has-text('Restore Data')");

            await Assertions.Expect(backupButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
            await Assertions.Expect(restoreButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task BackupButton_TriggersFileDownload()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // First upload an image so there's something to backup
            var uniqueLabel = $"BackupTest_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, uniqueLabel, "Places", clearStorageFirst: true);

            // Navigate to settings
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("button:has-text('Backup Data')", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - Set up download listener and click backup button
            var downloadTask = page.WaitForDownloadAsync();
            var backupButton = page.Locator("button:has-text('Backup Data')");
            await backupButton.ClickAsync();

            // Assert - Download should be triggered
            var download = await downloadTask;
            Assert.NotNull(download);

            // Assert - Filename should match expected pattern
            Assert.StartsWith("nowthenext-backup-", download.SuggestedFilename);
            Assert.EndsWith(".json", download.SuggestedFilename);

            // Assert - Success message should appear
            await page.WaitForSelectorAsync(".success-message", new PageWaitForSelectorOptions { Timeout = 10000 });
            var successMessage = page.Locator(".success-message");
            await Assertions.Expect(successMessage).ToContainTextAsync("Backup downloaded");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task RestoreButton_HasFileInputElement()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("button:has-text('Restore Data')", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act & Assert - Verify the hidden restore file input exists and accepts JSON files
            var restoreInput = page.Locator("#restore-file-input");
            await Assertions.Expect(restoreInput).ToHaveCountAsync(1);
            await Assertions.Expect(restoreInput).ToHaveAttributeAsync("accept", new System.Text.RegularExpressions.Regex("\\.json"));
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task RestoreFile_ShowsReplaceOrMergeChoiceDialog()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("button:has-text('Restore Data')", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Create a valid backup file
            var backupFilePath = await CreateTestBackupFileAsync(2);

            // Act - Select the backup file via the restore input
            await page.Locator("#restore-file-input").SetInputFilesAsync(backupFilePath);

            // Assert - Modal with replace/merge choice should appear
            await page.WaitForSelectorAsync(".modal-overlay", new PageWaitForSelectorOptions { Timeout = 10000 });
            var modal = page.Locator(".modal-overlay");
            await Assertions.Expect(modal).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });

            // Assert - Modal should show image count from backup
            var modalContent = page.Locator(".modal-content");
            await Assertions.Expect(modalContent).ToContainTextAsync("2 images");

            // Assert - Both options should be present
            var replaceButton = page.Locator("button:has-text('Replace All Data')");
            var mergeButton = page.Locator("button:has-text('Merge with Existing')");

            await Assertions.Expect(replaceButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
            await Assertions.Expect(mergeButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task RestoreReplaceMode_CorrectlyRestoresData()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // First upload an image that will be replaced
            var existingLabel = $"ExistingImage_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, existingLabel, "Places", clearStorageFirst: true);

            // Navigate to settings
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("button:has-text('Restore Data')", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Create a backup file with different images
            var restoredLabel1 = $"RestoredImage1_{Guid.NewGuid().ToString()[..8]}";
            var restoredLabel2 = $"RestoredImage2_{Guid.NewGuid().ToString()[..8]}";
            var backupFilePath = await CreateTestBackupFileAsync(2, new[] { restoredLabel1, restoredLabel2 });

            // Select the backup file via the restore input
            await page.Locator("#restore-file-input").SetInputFilesAsync(backupFilePath);

            // Wait for modal and click Replace
            await page.WaitForSelectorAsync(".modal-overlay", new PageWaitForSelectorOptions { Timeout = 10000 });
            var replaceButton = page.Locator("button:has-text('Replace All Data')");
            await replaceButton.ClickAsync();

            // Wait for success message
            await page.WaitForSelectorAsync(".success-message", new PageWaitForSelectorOptions { Timeout = 10000 });
            var successMessage = page.Locator(".success-message");
            await Assertions.Expect(successMessage).ToContainTextAsync("2 images imported");

            // Navigate to Places library to verify images were restored
            await page.GotoAsync($"{_fixture.BaseUrl}/places");
            await page.WaitForSelectorAsync(".image-grid, .empty-state", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - Restored images should be visible
            var restoredImage1 = page.Locator($"text={restoredLabel1}");
            var restoredImage2 = page.Locator($"text={restoredLabel2}");
            await Assertions.Expect(restoredImage1).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(restoredImage2).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Assert - Original image should NOT be visible (was replaced)
            var existingImage = page.Locator($"text={existingLabel}");
            await Assertions.Expect(existingImage).Not.ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task BackupData_IncludesLearningCardsPayload()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("h1", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Seed custom learning cards data into localStorage
            var learningCardsData = JsonSerializer.Serialize(new
            {
                Categories = new[]
                {
                    new { Id = "test-cat-1", Name = "TestCategory", Emoji = "⭐", IsBuiltIn = false, CreatedAt = DateTime.UtcNow.ToString("o") }
                },
                Cards = new[]
                {
                    new { Id = "test-card-1", CategoryId = "test-cat-1", ImageData = "<svg></svg>", Word = "TestWord", IsBuiltIn = false }
                }
            });
            await page.SetLocalStorageItemAsync("learning-cards", learningCardsData);

            // Navigate to settings and trigger backup
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("button:has-text('Backup Data')", new PageWaitForSelectorOptions { Timeout = 60000 });

            var downloadTask = page.WaitForDownloadAsync();
            await page.Locator("button:has-text('Backup Data')").ClickAsync();
            var download = await downloadTask;

            // Read downloaded backup file
            var downloadPath = await download.PathAsync();
            var backupJson = await File.ReadAllTextAsync(downloadPath);
            using var doc = JsonDocument.Parse(backupJson);
            var root = doc.RootElement;

            // Assert - LearningCardsData is present in backup
            Assert.True(root.TryGetProperty("LearningCardsData", out var lcData), "Backup should contain LearningCardsData");

            // Assert - Contains our test category
            Assert.True(lcData.TryGetProperty("Categories", out var cats));
            var catArray = cats.EnumerateArray().ToList();
            Assert.Contains(catArray, c => c.GetProperty("Name").GetString() == "TestCategory");

            // Assert - Contains our test card
            Assert.True(lcData.TryGetProperty("Cards", out var cards));
            var cardArray = cards.EnumerateArray().ToList();
            Assert.Contains(cardArray, c => c.GetProperty("Word").GetString() == "TestWord");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task RestoreReplace_CorrectlyRestoresLearningCardsData()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("h1", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Navigate to settings
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("button:has-text('Restore Data')", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Create a backup file that includes learning cards data
            var categoryId = Guid.NewGuid().ToString();
            var backupFilePath = await CreateTestBackupFileWithLearningCardsAsync(
                imageCount: 1,
                categoryName: "RestoredCat",
                categoryId: categoryId,
                cardWord: "RestoredWord"
            );

            // Act - restore via replace
            await page.Locator("#restore-file-input").SetInputFilesAsync(backupFilePath);
            await page.WaitForSelectorAsync(".modal-overlay", new PageWaitForSelectorOptions { Timeout = 10000 });
            await page.Locator("button:has-text('Replace All Data')").ClickAsync();
            await page.WaitForSelectorAsync(".success-message", new PageWaitForSelectorOptions { Timeout = 10000 });

            // Assert - learning cards data was written to localStorage
            var lcJson = await page.GetLocalStorageItemAsync("learning-cards");
            Assert.NotNull(lcJson);

            using var doc = JsonDocument.Parse(lcJson);
            var root = doc.RootElement;

            // Assert - restored category exists
            Assert.True(root.TryGetProperty("Categories", out var cats));
            var catArray = cats.EnumerateArray().ToList();
            Assert.Contains(catArray, c => c.GetProperty("Name").GetString() == "RestoredCat");

            // Assert - restored card exists
            Assert.True(root.TryGetProperty("Cards", out var cards));
            var cardArray = cards.EnumerateArray().ToList();
            Assert.Contains(cardArray, c => c.GetProperty("Word").GetString() == "RestoredWord");

            // Verify by navigating to learning cards - custom category should appear
            await page.GotoAsync($"{_fixture.BaseUrl}/learning");
            await page.WaitForSelectorAsync(".category-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await Assertions.Expect(page.Locator(".category-name:has-text('RestoredCat')")).ToBeVisibleAsync(
                new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task RestoreMergeMode_CorrectlyMergesData()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // First upload an image that should remain after merge
            var existingLabel = $"ExistingMerge_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, existingLabel, "Places", clearStorageFirst: true);

            // Navigate to settings
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("button:has-text('Restore Data')", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Create a backup file with different images
            var mergedLabel1 = $"MergedImage1_{Guid.NewGuid().ToString()[..8]}";
            var mergedLabel2 = $"MergedImage2_{Guid.NewGuid().ToString()[..8]}";
            var backupFilePath = await CreateTestBackupFileAsync(2, new[] { mergedLabel1, mergedLabel2 });

            // Select the backup file via the restore input
            await page.Locator("#restore-file-input").SetInputFilesAsync(backupFilePath);

            // Wait for modal and click Merge
            await page.WaitForSelectorAsync(".modal-overlay", new PageWaitForSelectorOptions { Timeout = 10000 });
            var mergeButton = page.Locator("button:has-text('Merge with Existing')");
            await mergeButton.ClickAsync();

            // Wait for success message
            await page.WaitForSelectorAsync(".success-message", new PageWaitForSelectorOptions { Timeout = 10000 });
            var successMessage = page.Locator(".success-message");
            // When there are no duplicates, message says "X images added" (not "X new images added")
            await Assertions.Expect(successMessage).ToContainTextAsync("2 images added");

            // Navigate to Places library to verify all images are present
            await page.GotoAsync($"{_fixture.BaseUrl}/places");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - All images should be visible (existing + merged)
            var existingImage = page.Locator($"text={existingLabel}");
            var mergedImage1 = page.Locator($"text={mergedLabel1}");
            var mergedImage2 = page.Locator($"text={mergedLabel2}");

            await Assertions.Expect(existingImage).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(mergedImage1).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(mergedImage2).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    /// <summary>
    /// Helper method to upload a test image with the specified label and category.
    /// </summary>
    private async Task UploadTestImageAsync(IPage page, string label, string category, bool clearStorageFirst = false)
    {
        await page.GotoAsync($"{_fixture.BaseUrl}/upload");
        await page.WaitForSelectorAsync(".upload-area", new PageWaitForSelectorOptions { Timeout = 60000 });

        if (clearStorageFirst)
        {
            await page.ClearLocalStorageAsync();
        }

        // Upload a test image
        var testImagePath = await CreateTestImageAsync();
        await page.Locator("#file-upload").SetInputFilesAsync(testImagePath);

        // Wait for preview
        await page.WaitForSelectorAsync(".preview-image", new PageWaitForSelectorOptions { Timeout = 10000 });

        // Fill label
        var labelInput = page.Locator("#image-label");
        await labelInput.FillAsync(label);

        // Select category
        var categoryButton = page.Locator($"button:has-text('{category}')");
        await categoryButton.ClickAsync();

        // Confirm upload
        var confirmButton = page.Locator(".confirm-button");
        await confirmButton.ClickAsync();

        // Wait for success message
        await page.WaitForSelectorAsync(".success-message", new PageWaitForSelectorOptions { Timeout = 15000 });
    }

    /// <summary>
    /// Creates a simple test image file for upload testing.
    /// </summary>
    private static async Task<string> CreateTestImageAsync()
    {
        byte[] pngBytes =
        [
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
            0x00, 0x00, 0x00, 0x0D,
            0x49, 0x48, 0x44, 0x52,
            0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x01,
            0x08, 0x02,
            0x00, 0x00, 0x00,
            0x90, 0x77, 0x53, 0xDE,
            0x00, 0x00, 0x00, 0x0C,
            0x49, 0x44, 0x41, 0x54,
            0x08, 0xD7, 0x63, 0xF8, 0xCF, 0xC0, 0x00, 0x00,
            0x01, 0xA8, 0x00, 0x85,
            0xA5, 0xEF, 0x73, 0x4F,
            0x00, 0x00, 0x00, 0x00,
            0x49, 0x45, 0x4E, 0x44,
            0xAE, 0x42, 0x60, 0x82
        ];

        var tempPath = Path.Combine(Path.GetTempPath(), $"test_image_{Guid.NewGuid()}.png");
        await File.WriteAllBytesAsync(tempPath, pngBytes);
        return tempPath;
    }

    /// <summary>
    /// Creates a valid backup JSON file with test images.
    /// </summary>
    private static async Task<string> CreateTestBackupFileAsync(int imageCount, string[]? labels = null)
    {
        // Create a minimal base64 PNG for the test images
        byte[] pngBytes =
        [
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
            0x00, 0x00, 0x00, 0x0D,
            0x49, 0x48, 0x44, 0x52,
            0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x01,
            0x08, 0x02,
            0x00, 0x00, 0x00,
            0x90, 0x77, 0x53, 0xDE,
            0x00, 0x00, 0x00, 0x0C,
            0x49, 0x44, 0x41, 0x54,
            0x08, 0xD7, 0x63, 0xF8, 0xCF, 0xC0, 0x00, 0x00,
            0x01, 0xA8, 0x00, 0x85,
            0xA5, 0xEF, 0x73, 0x4F,
            0x00, 0x00, 0x00, 0x00,
            0x49, 0x45, 0x4E, 0x44,
            0xAE, 0x42, 0x60, 0x82
        ];
        var base64Image = $"data:image/png;base64,{Convert.ToBase64String(pngBytes)}";

        var images = new List<object>();
        for (int i = 0; i < imageCount; i++)
        {
            var label = labels != null && i < labels.Length ? labels[i] : $"BackupImage_{i}";
            images.Add(new
            {
                Id = Guid.NewGuid().ToString(),
                Base64Data = base64Image,
                Label = label,
                Category = 0, // Places
                IsFavorite = false,
                CreatedAt = DateTime.UtcNow.ToString("o")
            });
        }

        var backup = new
        {
            Version = "1.0",
            CreatedAt = DateTime.UtcNow.ToString("o"),
            Images = images
        };

        var json = JsonSerializer.Serialize(backup, new JsonSerializerOptions { WriteIndented = true });
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_backup_{Guid.NewGuid()}.json");
        await File.WriteAllTextAsync(tempPath, json);
        return tempPath;
    }

    /// <summary>
    /// Creates a valid backup JSON file that includes learning cards data.
    /// </summary>
    private static async Task<string> CreateTestBackupFileWithLearningCardsAsync(
        int imageCount,
        string categoryName,
        string categoryId,
        string cardWord)
    {
        byte[] pngBytes =
        [
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
            0x00, 0x00, 0x00, 0x0D,
            0x49, 0x48, 0x44, 0x52,
            0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x01,
            0x08, 0x02,
            0x00, 0x00, 0x00,
            0x90, 0x77, 0x53, 0xDE,
            0x00, 0x00, 0x00, 0x0C,
            0x49, 0x44, 0x41, 0x54,
            0x08, 0xD7, 0x63, 0xF8, 0xCF, 0xC0, 0x00, 0x00,
            0x01, 0xA8, 0x00, 0x85,
            0xA5, 0xEF, 0x73, 0x4F,
            0x00, 0x00, 0x00, 0x00,
            0x49, 0x45, 0x4E, 0x44,
            0xAE, 0x42, 0x60, 0x82
        ];
        var base64Image = $"data:image/png;base64,{Convert.ToBase64String(pngBytes)}";

        var images = new List<object>();
        for (int i = 0; i < imageCount; i++)
        {
            images.Add(new
            {
                Id = Guid.NewGuid().ToString(),
                Base64Data = base64Image,
                Label = $"LCBackupImage_{i}",
                Category = 0,
                IsFavorite = false,
                CreatedAt = DateTime.UtcNow.ToString("o")
            });
        }

        var backup = new
        {
            Version = "1.0",
            CreatedAt = DateTime.UtcNow.ToString("o"),
            Images = images,
            LearningCardsData = new
            {
                Categories = new[]
                {
                    new { Id = categoryId, Name = categoryName, Emoji = "🚗", IsBuiltIn = false, CreatedAt = DateTime.UtcNow.ToString("o") }
                },
                Cards = new[]
                {
                    new { Id = Guid.NewGuid().ToString(), CategoryId = categoryId, ImageData = "<svg></svg>", Word = cardWord, IsBuiltIn = false }
                }
            }
        };

        var json = JsonSerializer.Serialize(backup, new JsonSerializerOptions { WriteIndented = true });
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_backup_lc_{Guid.NewGuid()}.json");
        await File.WriteAllTextAsync(tempPath, json);
        return tempPath;
    }
}
