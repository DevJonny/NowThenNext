using Microsoft.Playwright;
using NowThenNext.Tests.E2E.Fixtures;
using NowThenNext.Tests.E2E.Helpers;

namespace NowThenNext.Tests.E2E.Tests;

[Collection("BlazorApp")]
public class PhonicsTests
{
    private readonly BlazorAppFixture _fixture;

    public PhonicsTests(BlazorAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PhonicsButton_VisibleOnHomePage_NavigatesToPhonics()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Assert - Phonics button is visible
            var phonicsButton = page.Locator("a[href='phonics']:has-text('Phonics')");
            await Assertions.Expect(phonicsButton).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            // Act - click Phonics button
            await phonicsButton.ClickAsync();

            // Assert - navigated to /phonics
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/phonics", new PageWaitForURLOptions { Timeout = 10000 });
            Assert.EndsWith("/phonics", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task PhaseSelection_ShowsAllFourPhases()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics");
            await page.WaitForSelectorAsync(".phase-card", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Reload to get clean state after clearing localStorage
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics");
            await page.WaitForSelectorAsync(".phase-card", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - 4 phase cards visible
            var phaseCards = page.Locator(".phase-card");
            await Assertions.Expect(phaseCards).ToHaveCountAsync(4, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - phase names
            await Assertions.Expect(page.Locator(".phase-card:has-text('Phase 2')")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator(".phase-card:has-text('Phase 3')")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator(".phase-card:has-text('Phase 4')")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator(".phase-card:has-text('Phase 5')")).ToBeVisibleAsync();
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task PhaseCard_NavigatesToSoundList()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics");
            await page.WaitForSelectorAsync(".phase-card", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click Phase 2 card
            var phase2Card = page.Locator("a[href='phonics/2']");
            await phase2Card.ClickAsync();

            // Assert - navigated to /phonics/2
            await page.WaitForURLAsync($"{_fixture.BaseUrl}/phonics/2", new PageWaitForURLOptions { Timeout = 10000 });
            Assert.EndsWith("/phonics/2", page.Url);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task PhaseSelection_BackButton_ReturnsToHome()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics");
            await page.WaitForSelectorAsync(".back-button", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();

            // Act - click back button
            var backButton = page.Locator(".back-button");
            await backButton.ClickAsync();

            // Assert - navigated back to home
            await page.WaitForURLAsync(_fixture.BaseUrl, new PageWaitForURLOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    // --- US-047: Sound list and flashcard tests ---

    [Fact]
    public async Task SoundList_ShowsGraphemesGroupedByWeek()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".week-group", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".week-group", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - week groups exist with labels
            var weekGroups = page.Locator(".week-group");
            var count = await weekGroups.CountAsync();
            Assert.True(count >= 7, $"Expected at least 7 week groups for Phase 2, got {count}");

            // Assert - week labels present
            await Assertions.Expect(page.Locator(".week-label:has-text('Week 1')")).ToBeVisibleAsync();
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task SoundList_FirstSoundUnlocked_SubsequentLocked()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - first sound is current (unlocked)
            var currentSound = page.Locator(".sound-current");
            await Assertions.Expect(currentSound).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - locked sounds exist
            var lockedSounds = page.Locator(".sound-locked");
            var lockedCount = await lockedSounds.CountAsync();
            Assert.True(lockedCount > 0, "Expected some locked sounds");
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task Flashcard_ShowsGraphemeExampleWordAndKeyword()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-current", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Act - click the current unlocked sound (first sound = "s")
            await page.Locator(".sound-current").ClickAsync();
            await page.WaitForSelectorAsync(".grapheme-display", new PageWaitForSelectorOptions { Timeout = 10000 });

            // Assert - grapheme displayed
            var grapheme = await page.Locator(".grapheme-display").TextContentAsync();
            Assert.Equal("s", grapheme);

            // Assert - example word displayed
            var exampleWord = await page.Locator(".example-word").TextContentAsync();
            Assert.Contains("sun", exampleWord, StringComparison.OrdinalIgnoreCase);

            // Assert - keyword hint displayed
            var keyword = await page.Locator(".keyword-hint").TextContentAsync();
            Assert.Contains("s is for sun", keyword, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task GotIt_MarksCompleteAndShowsNextSound()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-current", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Navigate to first sound flashcard
            await page.Locator(".sound-current").ClickAsync();
            await page.WaitForSelectorAsync(".got-it-button", new PageWaitForSelectorOptions { Timeout = 10000 });

            // Assert - first sound is "s"
            var grapheme = await page.Locator(".grapheme-display").TextContentAsync();
            Assert.Equal("s", grapheme);

            // Act - click Got it!
            await page.Locator(".got-it-button").ClickAsync();

            // Assert - next sound displayed (should be "a")
            await page.WaitForSelectorAsync(".grapheme-display", new PageWaitForSelectorOptions { Timeout = 10000 });
            var nextGrapheme = await page.Locator(".grapheme-display").TextContentAsync();
            Assert.Equal("a", nextGrapheme);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task CompletedSounds_ShowCheckmarkOnSoundList()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-current", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Complete the first sound
            await page.Locator(".sound-current").ClickAsync();
            await page.WaitForSelectorAsync(".got-it-button", new PageWaitForSelectorOptions { Timeout = 10000 });
            await page.Locator(".got-it-button").ClickAsync();

            // Go back to sound list
            await page.WaitForSelectorAsync(".back-button", new PageWaitForSelectorOptions { Timeout = 10000 });
            await page.Locator(".back-button").ClickAsync();
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 10000 });

            // Assert - completed sound has checkmark
            var completedSounds = page.Locator(".sound-completed");
            await Assertions.Expect(completedSounds).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - checkmark icon exists within completed sound
            var checkIcon = page.Locator(".sound-completed .check-icon");
            await Assertions.Expect(checkIcon).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    [Fact]
    public async Task Progress_PersistsAfterNavigatingAway()
    {
        var page = await _fixture.CreatePageAsync();

        try
        {
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 60000 });
            await page.ClearLocalStorageAsync();
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-current", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Complete the first sound
            await page.Locator(".sound-current").ClickAsync();
            await page.WaitForSelectorAsync(".got-it-button", new PageWaitForSelectorOptions { Timeout = 10000 });
            await page.Locator(".got-it-button").ClickAsync();

            // Navigate away to home
            await page.GotoAsync(_fixture.BaseUrl);
            await page.WaitForSelectorAsync("nav a", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Navigate back to Phase 2 sound list
            await page.GotoAsync($"{_fixture.BaseUrl}/phonics/2");
            await page.WaitForSelectorAsync(".sound-tile", new PageWaitForSelectorOptions { Timeout = 60000 });

            // Assert - first sound is still completed
            var completedSounds = page.Locator(".sound-completed");
            await Assertions.Expect(completedSounds).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });

            // Assert - second sound is now current
            var currentSound = page.Locator(".sound-current");
            await Assertions.Expect(currentSound).ToHaveCountAsync(1, new LocatorAssertionsToHaveCountOptions { Timeout = 10000 });
        }
        finally
        {
            await page.CloseAsync();
        }
    }
}
