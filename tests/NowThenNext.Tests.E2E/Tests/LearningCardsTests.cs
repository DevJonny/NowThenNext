using Microsoft.Playwright;
using NowThenNext.Tests.E2E.Fixtures;
using NowThenNext.Tests.E2E.Helpers;

namespace NowThenNext.Tests.E2E.Tests;

[Collection("BlazorApp")]
public class LearningCardsTests
{
    private readonly BlazorAppFixture _fixture;

    public LearningCardsTests(BlazorAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task LearningCardsButton_VisibleOnHomePage_NavigatesToLearning()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Assert - Learning Cards button is visible
            var learningButton = page.Locator("a[href='learning']:has-text('Learning Cards')");
            await Assertions.Expect(learningButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Act - click Learning Cards button
            await learningButton.ClickAsync();

            // Assert - navigated to /learning
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/learning", new PageWaitForURLOptions { Timeout = 10000 });
            Assert.EndsWith("/learning", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task CategoryListing_ShowsFourBuiltInCategories()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/learning");
            await page.WaitForSelectorAsync(".category-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get clean state after clearing localStorage
            await page.GotoAsync($"{_fixture.BaseUrl}/learning");
            await page.WaitForSelectorAsync(".category-tile", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - 4 built-in category tiles + 1 add category tile = 5 total .category-tile elements
            var categoryTiles = page.Locator(".category-tile");
            await Assertions.Expect(categoryTiles).ToHaveCountAsync(5, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - category names are visible
            await Assertions.Expect(page.Locator(".category-name:has-text('Shapes')")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(page.Locator(".category-name:has-text('Colours')")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(page.Locator(".category-name:has-text('Animals')")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
            await Assertions.Expect(page.Locator(".category-name:has-text('Dinosaurs')")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task ShapesCategory_ShowsCorrectNumberOfCards()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/learning/shapes");
            await page.WaitForSelectorAsync(".learning-card", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get clean state
            await page.GotoAsync($"{_fixture.BaseUrl}/learning/shapes");
            await page.WaitForSelectorAsync(".learning-card", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - 8 shape cards
            var cards = page.Locator(".learning-card");
            await Assertions.Expect(cards).ToHaveCountAsync(8, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - page heading shows category name
            var heading = page.Locator("h1");
            await Assertions.Expect(heading).ToContainTextAsync("Shapes", new LocatorAssertionsToContainTextOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task TappingCard_RevealsWord_TappingAgainHidesIt()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/learning/shapes");
            await page.WaitForSelectorAsync(".learning-card", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get clean state
            await page.GotoAsync($"{_fixture.BaseUrl}/learning/shapes");
            await page.WaitForSelectorAsync(".learning-card", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - no cards have the revealed class initially
            var revealedCards = page.Locator(".learning-card.revealed");
            await Assertions.Expect(revealedCards).ToHaveCountAsync(0, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Act - tap the first card
            var firstCard = page.Locator(".card-tap-area").First;
            await firstCard.ClickAsync();

            // Assert - the first card now has the revealed class
            await Assertions.Expect(page.Locator(".learning-card.revealed")).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - the word "Circle" is visible (first shape card)
            var visibleWord = page.Locator(".card-word-visible");
            await Assertions.Expect(visibleWord).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
            await Assertions.Expect(visibleWord).ToContainTextAsync("Circle", new LocatorAssertionsToContainTextOptions { Timeout = 10000 });

            // Act - tap the same card again
            await firstCard.ClickAsync();

            // Assert - the card is no longer revealed
            await Assertions.Expect(page.Locator(".learning-card.revealed")).ToHaveCountAsync(0, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
            await Assertions.Expect(page.Locator(".card-word-visible")).ToHaveCountAsync(0, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task RevealingSecondCard_AutoHidesFirst()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/learning/shapes");
            await page.WaitForSelectorAsync(".learning-card", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get clean state
            await page.GotoAsync($"{_fixture.BaseUrl}/learning/shapes");
            await page.WaitForSelectorAsync(".learning-card", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - tap the first card
            var cardTapAreas = page.Locator(".card-tap-area");
            await cardTapAreas.Nth(0).ClickAsync();

            // Assert - first card revealed, word "Circle" visible
            await Assertions.Expect(page.Locator(".learning-card.revealed")).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
            await Assertions.Expect(page.Locator(".card-word-visible")).ToContainTextAsync("Circle", new LocatorAssertionsToContainTextOptions { Timeout = 10000 });

            // Act - tap the second card
            await cardTapAreas.Nth(1).ClickAsync();

            // Assert - still only one card revealed
            await Assertions.Expect(page.Locator(".learning-card.revealed")).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - the word is now "Square" (second shape), not "Circle"
            var visibleWord = page.Locator(".card-word-visible");
            await Assertions.Expect(visibleWord).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
            await Assertions.Expect(visibleWord).ToContainTextAsync("Square", new LocatorAssertionsToContainTextOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task CreateCustomCategory_AppearsOnListingPage()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/learning");
            await page.WaitForSelectorAsync(".category-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get clean state
            await page.GotoAsync($"{_fixture.BaseUrl}/learning");
            await page.WaitForSelectorAsync(".category-tile", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - starts with 5 tiles (4 built-in + 1 add)
            await Assertions.Expect(page.Locator(".category-tile")).ToHaveCountAsync(5, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Act - click the Add Category tile
            var addTile = page.Locator(".add-category-tile");
            await addTile.ClickAsync();

            // Assert - modal opens
            var modal = page.Locator(".modal-content");
            await Assertions.Expect(modal).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Act - enter category name
            await page.FillAsync("#categoryName", "Vehicles");

            // Act - select an emoji (first one: car)
            var emojiOptions = page.Locator(".emoji-option");
            await emojiOptions.First.ClickAsync();

            // Assert - emoji is selected
            await Assertions.Expect(emojiOptions.First).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("emoji-selected"), new LocatorAssertionsToHaveClassOptions { Timeout = 10000 });

            // Act - click Create button
            await page.Locator(".modal-save-button").ClickAsync();

            // Assert - modal closes
            await Assertions.Expect(modal).ToBeHiddenAsync(new LocatorAssertionsToBeHiddenOptions { Timeout = 10000 });

            // Assert - new category appears (5 + 1 custom = 6 tiles)
            await Assertions.Expect(page.Locator(".category-tile")).ToHaveCountAsync(6, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - the new category name is visible
            await Assertions.Expect(page.Locator(".category-name:has-text('Vehicles')")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task DeleteCustomCategory_RemovesFromListingPage()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/learning");
            await page.WaitForSelectorAsync(".category-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get clean state
            await page.GotoAsync($"{_fixture.BaseUrl}/learning");
            await page.WaitForSelectorAsync(".category-tile", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Step 1 - Create a custom category first
            await page.Locator(".add-category-tile").ClickAsync();
            await page.WaitForSelectorAsync(".modal-content", new PageWaitForSelectorOptions { Timeout = 10000 });
            await page.FillAsync("#categoryName", "Toys");
            await page.Locator(".modal-save-button").ClickAsync();
            await Assertions.Expect(page.Locator(".modal-content")).ToBeHiddenAsync(new LocatorAssertionsToBeHiddenOptions { Timeout = 10000 });

            // Assert - custom category created (6 tiles)
            await Assertions.Expect(page.Locator(".category-tile")).ToHaveCountAsync(6, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
            await Assertions.Expect(page.Locator(".category-name:has-text('Toys')")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Assert - delete button visible only on custom category (not on built-in)
            var deleteButtons = page.Locator(".delete-category-btn");
            await Assertions.Expect(deleteButtons).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Act - click delete button on the custom category
            await deleteButtons.First.ClickAsync();

            // Assert - delete confirmation modal appears
            var deleteModal = page.Locator(".modal-content:has-text('Delete Category')");
            await Assertions.Expect(deleteModal).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Assert - warning message mentions category name
            await Assertions.Expect(page.Locator(".modal-message")).ToContainTextAsync("Toys", new LocatorAssertionsToContainTextOptions { Timeout = 10000 });

            // Act - confirm deletion
            await page.Locator(".modal-delete-button").ClickAsync();

            // Assert - modal closes and category is removed (back to 5 tiles)
            await Assertions.Expect(page.Locator(".category-tile")).ToHaveCountAsync(5, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
            await Assertions.Expect(page.Locator(".category-name:has-text('Toys')")).ToBeHiddenAsync(new LocatorAssertionsToBeHiddenOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }
}
