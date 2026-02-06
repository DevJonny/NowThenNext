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
}
