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
    public async Task FoodChoicesButton_NavigatesToFoodChoicesPage()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click the Food Choices button
            var foodButton = page.Locator("a:has-text('Food Choices')");
            await foodButton.ClickAsync();

            // Wait for navigation
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/food-choices", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - verify we're on the food choices page
            Assert.EndsWith("/food-choices", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task MyPicturesButton_NavigatesToMyPicturesPage()
    {
        // Arrange
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click the My Pictures button
            var picturesButton = page.Locator("a:has-text('My Pictures')");
            await picturesButton.ClickAsync();

            // Wait for navigation
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/my-pictures", new PageWaitForURLOptions { Timeout = 10000 });

            // Assert - verify we're on the my pictures page
            Assert.EndsWith("/my-pictures", page.Url);
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
}
