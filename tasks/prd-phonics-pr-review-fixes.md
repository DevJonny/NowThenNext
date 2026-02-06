# PRD: Phonics PR #3 Review Fixes

## Introduction

Address all 6 code review comments from the Copilot review on PR #3 (feat: Phonics flashcards). These are must-fix items before merging: missing E2E test coverage for flashcard navigation, localStorage error handling consistency, and code quality improvements (explicit LINQ filtering, readonly fields).

## Goals

- Close all 6 review comments on PR #3
- Add missing E2E test coverage for flashcard prev/next navigation arrows
- Make localStorage error handling consistent across all services
- Improve code quality with explicit LINQ filtering and readonly field declarations
- All 69+ E2E tests continue to pass after changes

## User Stories

### US-049: E2E tests for flashcard navigation arrows
**Description:** As a developer, I want E2E tests covering the previous/next navigation arrows on the flashcard page so that US-044 has complete test coverage.

**Acceptance Criteria:**
- [ ] Test: Previous arrow navigates to the previous card when clicking on a completed card and going back
- [ ] Test: Next arrow navigates to the next card when the next card is completed/unlocked
- [ ] Test: Previous arrow is disabled (has `nav-disabled` class and `disabled` attribute) on the first card in a phase
- [ ] Test: Next arrow is disabled when the next card is locked (not yet completed or unlocked)
- [ ] Tests added to `PhonicsTests.cs` following existing patterns (page creation via `_fixture.CreatePageAsync()`, try/finally with `page.CloseAsync()`, localStorage cleared after first navigation)
- [ ] All new tests pass
- [ ] All existing 69 E2E tests still pass

### US-050: Extract StorageQuotaExceededException and add quota error handling to PhonicsProgressService
**Description:** As a developer, I want `StorageQuotaExceededException` moved to a shared location and `SaveCompletedSetAsync` in `PhonicsProgressService` to handle localStorage quota exceeded errors so that error handling is consistent with `LocalStorageImageService` and the user experience is not broken by unhandled exceptions.

**Acceptance Criteria:**
- [ ] Extract `StorageQuotaExceededException` from `LocalStorageImageService.cs` into a new shared file `src/NowThenNext/Services/StorageQuotaExceededException.cs`
- [ ] Update `LocalStorageImageService.cs` to remove the inline exception class and reference the shared file instead
- [ ] `SaveCompletedSetAsync` in `PhonicsProgressService.cs` wraps the `localStorage.setItem` call in a try-catch block
- [ ] Catches `JSException` where the message contains "QuotaExceededError" or "quota" (matching the pattern in `LocalStorageImageService.SaveAllImagesAsync` at line 104)
- [ ] Throws `StorageQuotaExceededException` (from the shared file) with a descriptive message on quota exceeded
- [ ] General exceptions are caught to prevent unhandled errors breaking the UI
- [ ] Build passes with 0 errors

### US-051: Use explicit LINQ filtering in GetNextUnlockedGraphemeIdAsync
**Description:** As a developer, I want `GetNextUnlockedGraphemeIdAsync` in `PhonicsProgressService` to use explicit LINQ `.FirstOrDefault()` with a predicate instead of a `foreach` loop with an implicit filter, improving code clarity.

**Acceptance Criteria:**
- [ ] Replace the `foreach` loop (lines 43-47 in `PhonicsProgressService.cs`) with `allCards.FirstOrDefault(card => !completed.Contains(card.Id))`
- [ ] Return `nextCard?.Id` instead of returning inside the loop
- [ ] Remove the `// All completed return null` comment (now implicit in the null-coalescing)
- [ ] Behaviour is unchanged: returns the first non-completed card ID, or null if all completed
- [ ] Build passes with 0 errors

### US-052: Use explicit LINQ filtering in PhonicsSoundList LoadState
**Description:** As a developer, I want the `LoadState` method in `PhonicsSoundList.razor` to use explicit LINQ filtering with `Task.WhenAll` instead of a `foreach` loop with an implicit filter, improving clarity and efficiency.

**Acceptance Criteria:**
- [ ] Replace the `foreach` loop (lines 340-344 in `PhonicsSoundList.razor`) that iterates cards and conditionally adds to `CompletedIds`
- [ ] Use `Task.WhenAll` with `Select` to check completion of all cards in parallel
- [ ] Filter completed results with `.Where()` and project to a `HashSet<string>`
- [ ] Behaviour is unchanged: `CompletedIds` contains exactly the IDs of completed cards
- [ ] Build passes with 0 errors

### US-053: Make PhaseProgress field readonly in PhonicsPhases.razor
**Description:** As a developer, I want the `PhaseProgress` dictionary field in `PhonicsPhases.razor` to be declared `readonly` since it is only mutated via dictionary operations (not reassigned).

**Acceptance Criteria:**
- [ ] Change `private Dictionary<int, (int Completed, int Total)> PhaseProgress = new();` to `private readonly Dictionary<int, (int Completed, int Total)> PhaseProgress = new();` in `PhonicsPhases.razor` (line 279)
- [ ] Build passes with 0 errors

### US-054: Make CompletedIds field readonly in PhonicsSoundList.razor
**Description:** As a developer, I want the `CompletedIds` HashSet field in `PhonicsSoundList.razor` to be declared `readonly` since it is only mutated via set operations (not reassigned).

**Acceptance Criteria:**
- [ ] Change `private HashSet<string> CompletedIds = new();` to `private readonly HashSet<string> CompletedIds = new();` in `PhonicsSoundList.razor` (line 322)
- [ ] Build passes with 0 errors

## Functional Requirements

- FR-1: Add E2E tests for previous arrow navigation on flashcard page
- FR-2: Add E2E tests for next arrow navigation on flashcard page
- FR-3: Add E2E tests verifying arrows are disabled at boundaries (first card, locked next card)
- FR-4: Wrap `PhonicsProgressService.SaveCompletedSetAsync` localStorage write in try-catch for `JSException` with quota exceeded
- FR-5: Rethrow as `StorageQuotaExceededException` (reuse existing exception class from `LocalStorageImageService.cs`)
- FR-6: Replace `foreach` implicit filter in `GetNextUnlockedGraphemeIdAsync` with `FirstOrDefault` predicate
- FR-7: Replace `foreach` implicit filter in `PhonicsSoundList.LoadState` with `Task.WhenAll` + `Where` + `ToHashSet`
- FR-8: Mark `PhaseProgress` field as `readonly` in `PhonicsPhases.razor`
- FR-9: Mark `CompletedIds` field as `readonly` in `PhonicsSoundList.razor`

## Non-Goals

- No new UI features or visual changes
- No changes to phonics data or GPC content
- No refactoring beyond what the review comments specifically request
- No changes to existing passing tests

## Technical Considerations

- `StorageQuotaExceededException` is being extracted from `LocalStorageImageService.cs` into its own shared file `src/NowThenNext/Services/StorageQuotaExceededException.cs` as part of US-050, since two services now need it.
- The `Task.WhenAll` refactor in US-052 changes from sequential `await` calls to parallel — this is safe because `IsCompletedAsync` is a read-only operation against localStorage.
- E2E test setup requires completing a sound first (click current sound, click "Got it!") to create a state where navigation arrows can be meaningfully tested.

## Success Metrics

- All 6 PR review comments resolved
- 0 build errors
- All existing 69 E2E tests pass
- New navigation E2E tests pass (expected 73+ total)
- PR #3 ready to merge

## Open Questions

None — all resolved.
