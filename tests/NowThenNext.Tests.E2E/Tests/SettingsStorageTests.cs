using System.Text.Json;
using Microsoft.Playwright;
using NowThenNext.Tests.E2E.Fixtures;
using NowThenNext.Tests.E2E.Helpers;

namespace NowThenNext.Tests.E2E.Tests;

/// <summary>
/// E2E tests for the storage breakdown section on the Settings page.
/// </summary>
[Collection("BlazorApp")]
public class SettingsStorageTests
{
    private readonly BlazorAppFixture _fixture;

    public SettingsStorageTests(BlazorAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task StorageUsageSection_IsVisibleOnSettingsPage()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("h1:has-text('Settings')", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get fresh state after clearing
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("h1:has-text('Settings')", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - Storage Usage heading is visible
            var heading = page.Locator("h2:has-text('Storage Usage')");
            await Assertions.Expect(heading).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task StorageUsageSection_HasProgressBar()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("h1:has-text('Settings')", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get fresh state
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("h2:has-text('Storage Usage')", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - Progress bar element exists
            var progressBar = page.Locator("[data-testid='storage-progress-bar']");
            await Assertions.Expect(progressBar).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task StorageBreakdownList_ShowsUploadedImagesWithLabelAndSize()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Navigate first so we have a page context for IndexedDB
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("h1:has-text('Settings')", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Seed test image data directly into IndexedDB
            var testImages = new[]
            {
                new
                {
                    Id = Guid.NewGuid().ToString(),
                    Base64Data = Convert.ToBase64String(new byte[500]),
                    Label = "Large Test Image",
                    Category = 0, // Places
                    IsFavorite = false,
                    CreatedAt = DateTime.UtcNow.ToString("o")
                },
                new
                {
                    Id = Guid.NewGuid().ToString(),
                    Base64Data = Convert.ToBase64String(new byte[100]),
                    Label = "Small Test Image",
                    Category = 1, // Food
                    IsFavorite = false,
                    CreatedAt = DateTime.UtcNow.ToString("o")
                }
            };

            var imagesJson = JsonSerializer.Serialize(testImages);
            await page.SetLocalStorageItemAsync("images", imagesJson);

            // Reload settings page to pick up the seeded data
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("h2:has-text('Storage Usage')", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Wait for the item list to render
            var itemList = page.Locator("[data-testid='storage-item-list']");
            await Assertions.Expect(itemList).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Assert - Both images appear in the list
            var items = page.Locator("[data-testid='storage-item']");
            var count = await items.CountAsync();
            Assert.True(count >= 2, $"Expected at least 2 storage items, got {count}");

            // Assert - Labels are visible
            await Assertions.Expect(page.Locator("text=Large Test Image")).ToBeVisibleAsync(
                new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
            await Assertions.Expect(page.Locator("text=Small Test Image")).ToBeVisibleAsync(
                new LocatorAssertionsToBeVisibleOptions { Timeout = 5000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task StorageBreakdownList_IsSortedBySizeDescending()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Navigate first
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("h1:has-text('Settings')", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Seed images with different sizes - "Big" has much more data than "Tiny"
            var testImages = new[]
            {
                new
                {
                    Id = Guid.NewGuid().ToString(),
                    Base64Data = Convert.ToBase64String(new byte[50]),
                    Label = "Tiny Item",
                    Category = 1, // Food
                    IsFavorite = false,
                    CreatedAt = DateTime.UtcNow.ToString("o")
                },
                new
                {
                    Id = Guid.NewGuid().ToString(),
                    Base64Data = Convert.ToBase64String(new byte[5000]),
                    Label = "Big Item",
                    Category = 0, // Places
                    IsFavorite = false,
                    CreatedAt = DateTime.UtcNow.ToString("o")
                }
            };

            var imagesJson = JsonSerializer.Serialize(testImages);
            await page.SetLocalStorageItemAsync("images", imagesJson);

            // Reload settings page
            await page.GotoAsync($"{_fixture.BaseUrl}/settings");
            await page.WaitForSelectorAsync("[data-testid='storage-item-list']", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Get the labels from the item list in order
            var labels = page.Locator("[data-testid='storage-item'] .storage-item-label");
            var firstLabel = await labels.Nth(0).TextContentAsync();
            var secondLabel = await labels.Nth(1).TextContentAsync();

            // Assert - Largest item should be first
            Assert.Equal("Big Item", firstLabel);
            Assert.Equal("Tiny Item", secondLabel);
        }
        finally
        {
            await page.CloseAsync();
        }
    }
}
