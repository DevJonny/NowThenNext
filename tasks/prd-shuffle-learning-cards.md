# PRD: Shuffle Learning Cards

## Introduction

Learning cards currently display in a fixed order every time a category is opened. This makes the experience predictable, which can reduce engagement for children. By shuffling the card order on every visit, children are encouraged to recognise words and images in varying contexts rather than memorising positions.

## Goals

- Display learning cards in a random order each time a category page is opened
- Apply to all categories equally (built-in and custom)
- Keep the "Add Card" tile anchored at the end for custom categories
- No user-facing controls — shuffling happens silently

## User Stories

### US-001: Shuffle cards on page load
**Description:** As a user, I want learning cards to appear in a random order each time I open a category so that children engage with the content rather than memorising card positions.

**Acceptance Criteria:**
- [ ] Cards are displayed in a randomised order when navigating to a category
- [ ] Opening the same category twice in a row produces a different order (with high probability)
- [ ] The "Add Card" tile in custom categories always appears last, after all shuffled cards
- [ ] Shuffling applies to all categories (built-in and custom)
- [ ] Tapping a card to reveal its word still works correctly after shuffle
- [ ] Deleting a card from a custom category still works correctly after shuffle
- [ ] Adding a new card refreshes and re-shuffles the list
- [ ] Typecheck/build passes
- [ ] Verify in browser using dev-browser skill

## Functional Requirements

- FR-1: When `OnParametersSetAsync` loads the cards list, shuffle it into a random order before rendering
- FR-2: Use Fisher-Yates (or equivalent) shuffle via `System.Random` for uniform distribution
- FR-3: The shuffle must not affect the underlying data — only the display order on the current page instance
- FR-4: When cards are reloaded after adding a new card (`SaveNewCard`), re-shuffle the updated list

## Non-Goals

- No manual re-shuffle button or UI indicator
- No persisted shuffle state across visits
- No option to disable shuffling
- No changes to other pages (learning category list, home, etc.)

## Technical Considerations

- The shuffle should happen in `LearningCardGrid.razor.cs` in `OnParametersSetAsync` after fetching cards, and again in `SaveNewCard` after refreshing the cards list
- A simple extension method or inline shuffle using `Random.Shared` keeps it minimal
- The "Add Card" tile is rendered separately in the Razor template (not part of the `Cards` list), so it is unaffected by shuffling

## Success Metrics

- Cards appear in a different order on each visit (verifiable by opening a category multiple times)
- No regression in existing E2E tests
- No performance impact (shuffle is O(n) on a small list)

## Open Questions

- None — scope is straightforward.
