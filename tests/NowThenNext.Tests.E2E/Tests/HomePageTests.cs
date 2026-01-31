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

            // Assert - verify the app title is displayed (with arrow format: "Now → Then → Next")
            var title = await page.TextContentAsync("h1");
            Assert.Contains("Now", title, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Then", title, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Next", title, StringComparison.OrdinalIgnoreCase);
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
            // Button order per US-006: Places, Food, Plan the Day, Favorites
            var placesButton = page.Locator("a:has-text('Places')");
            var foodButton = page.Locator("a:has-text('Food')");
            var planButton = page.Locator("a:has-text('Plan the Day')");
            var favoritesButton = page.Locator("a:has-text('Favorites')");

            await Assertions.Expect(placesButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(foodButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(planButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(favoritesButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task PlanTheDayButton_NavigatesToPlanPage()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click the Plan the Day button
            var planButton = page.Locator("a:has-text('Plan the Day')");
            await planButton.ClickAsync();

            // Wait for navigation
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/plan", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - verify we're on the plan page
            Assert.EndsWith("/plan", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task FoodButton_NavigatesToFoodPage()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click the Food button (navigates to food library)
            var foodButton = page.Locator("a:has-text('Food')");
            await foodButton.ClickAsync();

            // Wait for navigation
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/food", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - verify we're on the food page
            Assert.EndsWith("/food", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task PlacesButton_NavigatesToPlacesPage()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click the Places button (navigates to places library)
            var placesButton = page.Locator("a:has-text('Places')");
            await placesButton.ClickAsync();

            // Wait for navigation
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/places", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - verify we're on the places page
            Assert.EndsWith("/places", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task FavoritesButton_NavigatesToFavoritesPage()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click the Favorites button
            var favoritesButton = page.Locator("a:has-text('Favorites')");
            await favoritesButton.ClickAsync();

            // Wait for navigation
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/favorites", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - verify we're on the favorites page
            Assert.EndsWith("/favorites", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task SettingsGearIcon_IsVisibleAndClickable()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("h1", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Assert - verify the settings gear icon is visible
            var settingsButton = page.Locator("a.settings-button");
            await Assertions.Expect(settingsButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Act - click the settings button
            await settingsButton.ClickAsync();

            // Wait for navigation to settings page
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/settings", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - verify we navigated to settings
            Assert.EndsWith("/settings", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }
}
