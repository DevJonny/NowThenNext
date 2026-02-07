using Microsoft.Playwright;
using NowThenNext.Tests.E2E.Fixtures;
using NowThenNext.Tests.E2E.Helpers;

namespace NowThenNext.Tests.E2E.Tests;

/// <summary>
/// E2E tests for the activity choices flow.
/// Tests selection of activity items, visual highlighting, confirmation, and display.
/// </summary>
[Collection("BlazorApp")]
public class ActivityChoicesTests
{
    private readonly BlazorAppFixture _fixture;

    public ActivityChoicesTests(BlazorAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ActivitiesLibrary_DisplaysUploadedActivityImages()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload an activity image
            var uniqueLabel = $"ActivityLib_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, uniqueLabel, "Activities", clearStorageFirst: true);

            // Act - Navigate to Activities library
            await page.GotoAsync($"{_fixture.BaseUrl}/activities");
            await page.WaitForSelectorAsync(".image-grid, .empty-state", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - The uploaded image should be displayed with its label
            var imageLabel = page.Locator($"text={uniqueLabel}");
            await Assertions.Expect(imageLabel).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task CanSelectActivityItemsInSelectionView()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload an activity image first
            var uniqueLabel = $"ActivityTest_{Guid.NewGuid().ToString()[..8]}";
            await UploadTestImageAsync(page, uniqueLabel, "Activities", clearStorageFirst: true);

            // Act - Navigate to Activity Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/activity-choices");
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
    public async Task SelectionCountIsDisplayedWhenItemsSelected()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 activity images
            await UploadTestImageAsync(page, $"ActCount1_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"ActCount2_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: false);

            // Navigate to Activity Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/activity-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - Select first item
            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();

            // Assert - Selection count should show "1"
            var selectionCountNumber = page.Locator(".selection-count-number");
            await Assertions.Expect(selectionCountNumber).ToHaveTextAsync("1");

            // Act - Select second item
            await tiles.Nth(1).ClickAsync();

            // Assert - Selection count should show "2"
            await Assertions.Expect(selectionCountNumber).ToHaveTextAsync("2");
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
            // Upload 2 activity images (need at least 2 for Show Choices)
            await UploadTestImageAsync(page, $"ActShow1_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"ActShow2_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: false);

            // Navigate to Activity Choices page
            await page.GotoAsync($"{_fixture.BaseUrl}/activity-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - Show Choices button should NOT be visible with 0 items selected
            var showChoicesButton = page.Locator(".show-choices-button");
            await Assertions.Expect(showChoicesButton).Not.ToBeVisibleAsync();

            // Act - Select first item
            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();

            // Assert - Button should still not be visible with only 1 item
            await Assertions.Expect(showChoicesButton).Not.ToBeVisibleAsync();

            // Act - Select second item
            await tiles.Nth(1).ClickAsync();

            // Assert - Button should now be visible with 2 items selected
            await Assertions.Expect(showChoicesButton).ToBeVisibleAsync();
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task ActivityDisplayShowsSelectedItemsInLargeFormat()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 activity images
            await UploadTestImageAsync(page, $"ActDisp1_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"ActDisp2_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: false);

            // Navigate to Activity Choices page and select items
            await page.GotoAsync($"{_fixture.BaseUrl}/activity-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Click Show Choices
            await page.ClickAsync(".show-choices-button");

            // Wait for activity display page
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/activity-display"), new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - Activity items should be displayed in large format
            var activityItems = page.Locator(".activity-choice-item");
            await Assertions.Expect(activityItems).ToHaveCountAsync(2);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task TappingActivityItemShowsConfirmationScreen()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 activity images
            await UploadTestImageAsync(page, $"ActConfirm1_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"ActConfirm2_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: false);

            // Navigate to Activity Choices page and select items
            await page.GotoAsync($"{_fixture.BaseUrl}/activity-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Click Show Choices
            await page.ClickAsync(".show-choices-button");

            // Wait for activity display page
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/activity-display"), new PageWaitForURLOptions { Timeout = 10000 });

            // Act - Tap first activity item
            var activityItems = page.Locator(".activity-choice-item");
            await activityItems.First.ClickAsync();

            // Assert - Confirmation screen should appear with checkmark
            var confirmationScreen = page.Locator(".confirmation-screen");
            await Assertions.Expect(confirmationScreen).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });

            var confirmationCheckmark = page.Locator(".confirmation-checkmark");
            await Assertions.Expect(confirmationCheckmark).ToBeVisibleAsync();

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
    public async Task ChooseAgainButtonReturnsToActivityChoices()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Upload 2 activity images
            await UploadTestImageAsync(page, $"ActAgain1_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"ActAgain2_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: false);

            // Navigate to Activity Choices page and select items
            await page.GotoAsync($"{_fixture.BaseUrl}/activity-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Click Show Choices
            await page.ClickAsync(".show-choices-button");
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/activity-display"), new PageWaitForURLOptions { Timeout = 10000 });

            // Tap first activity item to show confirmation
            var activityItems = page.Locator(".activity-choice-item");
            await activityItems.First.ClickAsync();
            await page.WaitForSelectorAsync(".confirmation-screen", new PageWaitForSelectorOptions { Timeout = 5000 });

            // Act - Click Choose Again
            await page.ClickAsync(".choose-again-button");

            // Assert - Should return to activity display grid (not selection page)
            await page.WaitForSelectorAsync(".activity-choices-grid", new PageWaitForSelectorOptions { Timeout = 5000 });

            // Confirmation screen should be gone
            var confirmationScreen = page.Locator(".confirmation-screen");
            await Assertions.Expect(confirmationScreen).Not.ToBeVisibleAsync();

            // Activity items should be visible again
            await Assertions.Expect(activityItems).ToHaveCountAsync(2);
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
            // Upload 2 activity images
            await UploadTestImageAsync(page, $"ActBack1_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: true);
            await UploadTestImageAsync(page, $"ActBack2_{Guid.NewGuid().ToString()[..8]}", "Activities", clearStorageFirst: false);

            // Navigate to Activity Choices page and select items
            await page.GotoAsync($"{_fixture.BaseUrl}/activity-choices");
            await page.WaitForSelectorAsync(".image-grid", new PageWaitForSelectorOptions { Timeout = 60000 });

            var tiles = page.Locator(".selectable-tile");
            await tiles.Nth(0).ClickAsync();
            await tiles.Nth(1).ClickAsync();

            // Click Show Choices to go to display
            await page.ClickAsync(".show-choices-button");
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/activity-display"), new PageWaitForURLOptions { Timeout = 10000 });

            // Act - Click the back button
            var backButton = page.Locator(".back-button");
            await backButton.ClickAsync();

            // Assert - Should navigate back to activity choices
            await page.WaitForURLAsync(new System.Text.RegularExpressions.Regex("/activity-choices"), new PageWaitForURLOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    /// <summary>
    /// Helper method to upload a test image through the upload page.
    /// </summary>
    private async Task UploadTestImageAsync(IPage page, string label, string category, bool clearStorageFirst)
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
