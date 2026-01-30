using Microsoft.Playwright;
using NowThenNext.Tests.E2E.Fixtures;
using NowThenNext.Tests.E2E.Helpers;

namespace NowThenNext.Tests.E2E.Tests;

/// <summary>
/// Smoke tests for the home page functionality.
/// </summary>
[Collection("BlazorApp")]
public class HomePageTests
{
    private readonly BlazorAppFixture _fixture;

    public HomePageTests(BlazorAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task HomePage_LoadsSuccessfully()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Act
            await page.GotoAsync(_fixture.BaseUrl);

            // Wait for Blazor WASM to fully initialize - look for the app content
            await page.WaitForSelectorAsync("h1", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Clear localStorage for test isolation
            await page.ClearLocalStorageAsync();

            // Assert - verify the app title is displayed
            var title = await page.TextContentAsync("h1");
            Assert.Contains("NowThenNext", title, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task HomePage_DisplaysAllFourMainButtons()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            // Act
            await page.GotoAsync(_fixture.BaseUrl);

            // Wait for Blazor WASM to fully initialize - look for navigation buttons
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Assert - verify all four buttons are visible by their text content
            var planButton = page.Locator("a:has-text('Plan the Day')");
            var foodButton = page.Locator("a:has-text('Food Choices')");
            var picturesButton = page.Locator("a:has-text('My Pictures')");
            var favoritesButton = page.Locator("a:has-text('Favorites')");

            await Assertions.Expect(planButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(foodButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(picturesButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(favoritesButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }
}
