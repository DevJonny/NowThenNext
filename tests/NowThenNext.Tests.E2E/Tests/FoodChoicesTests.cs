using Microsoft.Playwright;
using NowThenNext.Tests.E2E.Fixtures;
using NowThenNext.Tests.E2E.Helpers;

namespace NowThenNext.Tests.E2E.Tests;

/// <summary>
/// E2E tests for the food choices flow.
/// Tests selection of food items, visual highlighting, and food display.
/// </summary>
[Collection("BlazorApp")]
public class FoodChoicesTests
{
    private readonly BlazorAppFixture _fixture;

    public FoodChoicesTests(BlazorAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CanSelectFoodItemsInSelectionView()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload a food image first
            var uniqueLabel = $"FoodTest_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, uniqueLabel, "Food", clearStorageFirst: true);

            // Act - Navigate to Food Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid, .empty-state", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Find and click the selectable tile
            var selectableTile = page.Locator(".selectable-tile").First;
            await selectableTile.ClickAsync();

            // Assert - The tile should have the 'selected' class
            await Assertions.Expect(selectableTile).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("selected"));
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task SelectedItemsAreVisuallyHighlighted()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 food images
            await UploadTestImageAsync(page, $"Food1_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"Food2_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: false);

            // Navigate to Food Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - Select the first food item
            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();

            // Assert - First tile should have the 'selected' class (visual highlight)
            await Assertions.Expect(tiles.Nth(0)).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("selected"));

            // Assert - A checkmark badge should appear on selected items
            var checkmark = page.Locator(".selection-checkmark");
            await Assertions.Expect(checkmark).ToBeVisibleAsync();

            // Assert - Second tile should NOT be selected
            await Assertions.Expect(tiles.Nth(1)).Not.ToHaveClassAsync(new System.Text.RegularExpressions.Regex("selected"));
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task SelectionCountIsDisplayedWhenItemsSelected()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 food images
            await UploadTestImageAsync(page, $"Count1_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"Count2_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: false);

            // Navigate to Food Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - Select one item
            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();

            // Assert - Selection count should show "1" and text about needing 2+
            var selectionCount = page.Locator(".selection-count");
            await Assertions.Expect(selectionCount).ToBeVisibleAsync();
            var countNumber = page.Locator(".selection-count-number");
            await Assertions.Expect(countNumber).ToContainTextAsync("1");

            // Act - Select another item
            await tiles.Nth(1).ClickAsync();

            // Assert - Selection count should show "2"
            await Assertions.Expect(countNumber).ToContainTextAsync("2");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task ShowChoicesButtonAppearsWhenItemsSelected()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 food images (minimum required for choices)
            await UploadTestImageAsync(page, $"FoodA_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"FoodB_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: false);

            // Navigate to Food Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - Show Choices button should NOT be visible initially
            var showChoicesButton = page.Locator(".show-choices-button");
            await Assertions.Expect(showChoicesButton).Not.ToBeVisibleAsync();

            // Act - Select only one item (not enough for choices)
            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();

            // Assert - Show Choices button should still NOT be visible (need at least 2)
            await Assertions.Expect(showChoicesButton).Not.ToBeVisibleAsync();

            // Act - Select a second item
            await tiles.Nth(1).ClickAsync();

            // Assert - Now Show Choices button should be visible
            await Assertions.Expect(showChoicesButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
            await Assertions.Expect(showChoicesButton).ToContainTextAsync("Show Choices");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task FoodDisplayShowsSelectedItemsInLargeFormat()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 food images with known labels
            var label1 = $"Pizza_{Guid.NewGuid().ToString()[..8]}";
            var label2 = $"Pasta_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, label1, "Food", clearStorageFirst: true);
            await UploadTestImageAsync(page, label2, "Food", clearStorageFirst: false);

            // Navigate to Food Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Select both food items
            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Click Show Choices button
            var showChoicesButton = page.Locator(".show-choices-button");
            await showChoicesButton.ClickAsync();

            // Wait for navigation to food display
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/food-display"), new PageWaitForURLOptions { Timeout = 10000 });
            await page.WaitForSelectorAsync(".food-choices-grid, .empty-state", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - Food items should be displayed in large format
            var foodItems = page.Locator(".food-choice-item");
            await Assertions.Expect(foodItems).ToHaveCountAsync(2);

            // Assert - Each food item should have an image
            var foodImages = page.Locator(".food-image");
            await Assertions.Expect(foodImages).ToHaveCountAsync(2);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task TappingFoodItemShowsConfirmationScreen()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 food images with known labels
            var label1 = $"SelectFeedback1_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, label1, "Food", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"SelectFeedback2_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: false);

            // Navigate to Food Choices page and select items
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Navigate to food display
            var showChoicesButton = page.Locator(".show-choices-button");
            await showChoicesButton.ClickAsync();
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/food-display"), new PageWaitForURLOptions { Timeout = 10000 });
            await page.WaitForSelectorAsync(".food-choices-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - Tap the first food item on the display page
            var foodItem = page.Locator(".food-choice-item").First;
            await foodItem.ClickAsync();

            // Assert - Confirmation screen should appear with checkmark
            var confirmationCheckmark = page.Locator(".confirmation-checkmark");
            await Assertions.Expect(confirmationCheckmark).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });

            // Assert - Selected food should be displayed in confirmation
            var confirmedFoodCard = page.Locator(".confirmed-food-card");
            await Assertions.Expect(confirmedFoodCard).ToBeVisibleAsync();

            // Assert - Choose Again button should be visible
            var chooseAgainButton = page.Locator(".choose-again-button");
            await Assertions.Expect(chooseAgainButton).ToBeVisibleAsync();
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task ChooseAgainButtonReturnsToFoodChoices()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 food images
            await UploadTestImageAsync(page, $"ChooseAgain1_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"ChooseAgain2_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: false);

            // Navigate to Food Choices page and select items
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Navigate to food display
            var showChoicesButton = page.Locator(".show-choices-button");
            await showChoicesButton.ClickAsync();
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/food-display"), new PageWaitForURLOptions { Timeout = 10000 });
            await page.WaitForSelectorAsync(".food-choices-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Tap a food item to get to confirmation screen
            var foodItem = page.Locator(".food-choice-item").First;
            await foodItem.ClickAsync();

            // Wait for confirmation screen
            await page.WaitForSelectorAsync(".confirmation-screen", new PageWaitForSelectorOptions { Timeout = 5000 });

            // Act - Click Choose Again button
            var chooseAgainButton = page.Locator(".choose-again-button");
            await chooseAgainButton.ClickAsync();

            // Assert - Should return to the food choices grid (not the food-choices selection page)
            var foodChoicesGrid = page.Locator(".food-choices-grid");
            await Assertions.Expect(foodChoicesGrid).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });

            // Assert - Food items should be visible again for re-selection
            var foodItems = page.Locator(".food-choice-item");
            await Assertions.Expect(foodItems).ToHaveCountAsync(2);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task BackButtonReturnsToSelection()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 food images
            await UploadTestImageAsync(page, $"BackBtn1_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"BackBtn2_{Guid.NewGuid().ToString()[..8]}", "Food", clearStorageFirst: false);

            // Navigate to Food Choices page and select items
            await page.GotoAsync($"{_fixture.BaseUrl}/food-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Navigate to food display
            var showChoicesButton = page.Locator(".show-choices-button");
            await showChoicesButton.ClickAsync();

            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/food-display"), new PageWaitForURLOptions { Timeout = 10000 });
            await page.WaitForSelectorAsync(".food-choices-grid, .empty-state", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - Click the back button
            var backButton = page.Locator(".back-button");
            await backButton.ClickAsync();

            // Assert - Should navigate back to /food-choices
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/food-choices"), new PageWaitForURLOptions { Timeout = 10000 });
            Assert.EndsWith("/food-choices", page.Url);

            // Assert - The Food Choices page content should be visible
            var heading = page.Locator("h1");
            await Assertions.Expect(heading).ToContainTextAsync("Food Choices");
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
}
