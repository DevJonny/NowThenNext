# PRD: Migrate Razor Pages to Code-Behind Pattern

## Introduction

Migrate the 5 remaining `.razor` files that use inline `@code` blocks to the code-behind pattern (`.razor.cs` files). This brings the codebase into consistency — 13 pages already use code-behind, but 3 phonics pages and 2 components still have inline C# logic. Moving to code-behind improves separation of concerns and keeps `.razor` files focused on markup.

## Goals

- Eliminate all `@code` blocks from `.razor` files
- Create corresponding `.razor.cs` code-behind files for each migrated component
- No behaviour changes — purely structural refactor
- All 73 E2E tests continue to pass after migration

## User Stories

### US-055: Migrate PhonicsCard.razor to code-behind
**Description:** As a developer, I want PhonicsCard.razor to use a code-behind file so that C# logic is separated from markup.

**Acceptance Criteria:**
- [ ] Create `PhonicsCard.razor.cs` as a `partial class PhonicsCard : ComponentBase`
- [ ] Move all fields, parameters, methods, enums, and lifecycle overrides from the `@code` block into the code-behind file
- [ ] Remove the `@code { ... }` block from `PhonicsCard.razor`
- [ ] Move `@using` and `@inject` directives to the code-behind as `using` statements and `[Inject]` properties
- [ ] Keep only `@page` directive and HTML/Razor markup in `PhonicsCard.razor`
- [ ] Build passes
- [ ] All 73 E2E tests pass (`dotnet test tests/NowThenNext.Tests.E2E`)

### US-056: Migrate StorageWarningBanner.razor to code-behind
**Description:** As a developer, I want StorageWarningBanner.razor to use a code-behind file so that C# logic is separated from markup.

**Acceptance Criteria:**
- [ ] Create `StorageWarningBanner.razor.cs` as a `partial class StorageWarningBanner : ComponentBase`
- [ ] Move all fields, parameters, methods, and lifecycle overrides from the `@code` block into the code-behind file
- [ ] Remove the `@code { ... }` block from `StorageWarningBanner.razor`
- [ ] Move `@using` and `@inject` directives to the code-behind as `using` statements and `[Inject]` properties
- [ ] Keep only HTML/Razor markup in `StorageWarningBanner.razor`
- [ ] Build passes
- [ ] All 73 E2E tests pass (`dotnet test tests/NowThenNext.Tests.E2E`)

### US-057: Migrate PhonicsSoundList.razor to code-behind
**Description:** As a developer, I want PhonicsSoundList.razor to use a code-behind file so that C# logic is separated from markup.

**Acceptance Criteria:**
- [ ] Create `PhonicsSoundList.razor.cs` as a `partial class PhonicsSoundList : ComponentBase`
- [ ] Move all fields, parameters, methods, enums, and lifecycle overrides from the `@code` block into the code-behind file
- [ ] Remove the `@code { ... }` block from `PhonicsSoundList.razor`
- [ ] Move `@using` and `@inject` directives to the code-behind as `using` statements and `[Inject]` properties
- [ ] Keep only `@page` directive and HTML/Razor markup in `PhonicsSoundList.razor`
- [ ] Build passes
- [ ] All 73 E2E tests pass (`dotnet test tests/NowThenNext.Tests.E2E`)

### US-058: Migrate PhonicsPhases.razor to code-behind
**Description:** As a developer, I want PhonicsPhases.razor to use a code-behind file so that C# logic is separated from markup.

**Acceptance Criteria:**
- [ ] Create `PhonicsPhases.razor.cs` as a `partial class PhonicsPhases : ComponentBase`
- [ ] Move all fields, methods, and lifecycle overrides from the `@code` block into the code-behind file
- [ ] Remove the `@code { ... }` block from `PhonicsPhases.razor`
- [ ] Move `@using` and `@inject` directives to the code-behind as `using` statements and `[Inject]` properties
- [ ] Keep only `@page` directive and HTML/Razor markup in `PhonicsPhases.razor`
- [ ] Build passes
- [ ] All 73 E2E tests pass (`dotnet test tests/NowThenNext.Tests.E2E`)

### US-059: Migrate ImageTile.razor to code-behind
**Description:** As a developer, I want ImageTile.razor to use a code-behind file so that C# logic is separated from markup.

**Acceptance Criteria:**
- [ ] Create `ImageTile.razor.cs` as a `partial class ImageTile : ComponentBase`
- [ ] Move all fields, parameters, callbacks, and methods from the `@code` block into the code-behind file
- [ ] Remove the `@code { ... }` block from `ImageTile.razor`
- [ ] Move `@using` and `@inject` directives to the code-behind as `using` statements and `[Inject]` properties
- [ ] Keep only HTML/Razor markup in `ImageTile.razor`
- [ ] Build passes
- [ ] All 73 E2E tests pass (`dotnet test tests/NowThenNext.Tests.E2E`)

## Functional Requirements

- FR-1: Each migrated `.razor` file must have a corresponding `.razor.cs` file using `partial class` inheriting from `ComponentBase`
- FR-2: `@inject` directives become `[Inject] public/protected` properties in the code-behind
- FR-3: `@using` directives become standard `using` statements in the code-behind (except those needed for Razor markup rendering)
- FR-4: `[Parameter]` and `[CascadingParameter]` attributes remain on properties in the code-behind
- FR-5: `EventCallback` parameters remain as public properties with `[Parameter]` in the code-behind
- FR-6: `<style>` blocks remain in the `.razor` file (they are markup, not C#)
- FR-7: Private fields referenced in markup must become `protected` in the code-behind so the `.razor` file can access them
- FR-8: No functional changes — all pages and components must behave identically after migration

## Non-Goals

- No refactoring of logic within the migrated code
- No changes to HTML markup or CSS styles
- No migration of files that don't have `@code` blocks (Home.razor, NotFound.razor, MainLayout.razor, NavMenu.razor)
- No new tests — existing E2E tests validate behaviour is preserved

## Technical Considerations

- **Run E2E tests after every story**: Each migration must be verified independently by running `dotnet test tests/NowThenNext.Tests.E2E` before moving to the next story. Do not batch migrations — a failing test must be caught and fixed before proceeding.
- Blazor code-behind files use `partial class` — the class name must match the `.razor` filename exactly
- Fields/properties referenced in `.razor` markup must be `protected` (not `private`) in the code-behind
- The `@page` directive must stay in the `.razor` file — it cannot go in code-behind
- `@using NowThenNext.Models` and similar directives needed for type references in markup should remain in the `.razor` file or be in `_Imports.razor`
- Check `_Imports.razor` for globally available usings before adding redundant `using` statements to code-behind files
- Follow the existing code-behind pattern used by the 13 pages that already have `.razor.cs` files (e.g., `Home.razor` has no code-behind because it has no logic, but `PlanDay.razor` + `PlanDay.razor.cs` is a good reference)

## Success Metrics

- 0 remaining `@code` blocks in any `.razor` file
- All 73 E2E tests pass
- Build passes with 0 errors
- No behaviour changes
