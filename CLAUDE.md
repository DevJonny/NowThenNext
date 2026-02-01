# NowThenNext - Claude Code Context

## Project Overview
Visual schedule, food choice, and activity choice app for children with SEN (Special Educational Needs). Displays pictures in Now/Then/Next format to communicate daily plans and offers visual choice boards for food and activities.

## Tech Stack
- **Framework**: Blazor WebAssembly (WASM) - .NET 10 RC (preview)
- **Language**: C# / .NET
- **Styling**: Tailwind CSS CDN with custom SEN-friendly theme
- **Storage**: Browser localStorage (base64 encoded images, ~5MB quota)
- **Testing**: Playwright E2E tests with xUnit v3
- **Font**: Nunito (Google Fonts) - rounded, friendly typography

## Key Design Principles
1. **Calm & Clear**: Soft colors (teal primary #5B9A9A, muted green accent #7BA893, off-white bg #F9F7F3), generous spacing, minimum 48px touch targets
2. **No Harsh Colors**: Avoid pure red/bright yellow - use calm amber #D4A06A for warnings
3. **Accessibility**: WCAG AA contrast (~8.5:1 ratio), large fonts (18px base, 24px+ labels), rounded sans-serif
4. **Simplicity**: Minimal UI, no distractions, straightforward navigation
5. **Visual-First**: Large images, clear icons, visual feedback on interactions
6. **Slow Animations**: Calming, gentle transitions (3s pulse, 7s auto-advance)

## Project Structure
```
src/NowThenNext/           # Main Blazor WASM application
├── Pages/                 # Razor pages for each view
├── Components/            # Reusable Blazor components
├── Models/                # Data models (ImageItem, etc.)
├── Services/              # Business logic (ImageStorageService)
└── wwwroot/               # Static assets

tests/NowThenNext.Tests.E2E/  # E2E tests with Playwright
```

## Core Features
1. **Image Libraries**: Places, Food, Activities
2. **Schedule Builder**: Select up to 3 places for Now/Then/Next display
3. **Choice Boards**: Food Choices and Activity Choices (2-6 items)
4. **Favorites**: Cross-category favorites view
5. **Backup/Restore**: Export/import data as JSON

## Current Status
- Branch: `ralph/nowthenext-mvp`
- Completed: US-001 through US-032 (32/37 user stories)
- **In Progress**: US-033 through US-037 (Activities category feature)

## Next Steps (Pending)
- [ ] US-033: Add Activities & Activity Choices buttons to home screen
- [ ] US-034: Create Activities library view
- [ ] US-035: Build Activity Choices selection interface
- [ ] US-036: Build activity choice display with confirmation
- [ ] US-037: E2E tests for activity choices flow

## Important Files
- [`prd.json`](fleet-file://3j4f3alj5183smoqcslr/Users/jonnyolliff-lee/code/NowThenNext/prd.json?type=file&root=%252F) - Full PRD with all 37 user stories
- [`progress.txt`](fleet-file://3j4f3alj5183smoqcslr/Users/jonnyolliff-lee/code/NowThenNext/progress.txt?type=file&root=%252F) - Detailed implementation history and learnings
- [`src/NowThenNext/Pages/Home.razor`](fleet-file://3j4f3alj5183smoqcslr/Users/jonnyolliff-lee/code/NowThenNext/src/NowThenNext/Pages/Home.razor?type=file&root=%252F) - Main menu/navigation
- [`src/NowThenNext/Services/LocalStorageImageService.cs`](fleet-file://3j4f3alj5183smoqcslr/Users/jonnyolliff-lee/code/NowThenNext/src/NowThenNext/Services/LocalStorageImageService.cs?type=file&root=%252F) - Data persistence layer
- [`src/NowThenNext/wwwroot/index.html`](fleet-file://3j4f3alj5183smoqcslr/Users/jonnyolliff-lee/code/NowThenNext/src/NowThenNext/wwwroot/index.html?type=file&root=%252F) - Tailwind config & JS interop functions
- [`tests/NowThenNext.Tests.E2E/`](fleet-file://3j4f3alj5183smoqcslr/Users/jonnyolliff-lee/code/NowThenNext/tests/NowThenNext.Tests.E2E?type=file&root=%252F) - E2E test suite (44 tests)

## Development Commands
```bash
# Run application
dotnet run --project src/NowThenNext

# Run E2E tests
dotnet test tests/NowThenNext.Tests.E2E

# Build project
dotnet build
```

## Design Patterns
- **Category-based architecture**: All images categorized as Places, Food, or Activities
- **localStorage JSON storage**: Images stored as base64 with metadata
- **Reusable components**: ImageTile component shared across all libraries
- **Parallel workflows**: Food Choices and Activity Choices follow same pattern as templates

## Routing Map
| Route | Page | Description |
|-------|------|-------------|
| `/` | Home.razor | Main menu: Places, Food, Activities, Plan the Day, Food Choices, Activity Choices, Favorites |
| `/places` | PlacesLibrary.razor | Places image library |
| `/food` | FoodLibrary.razor | Food image library |
| `/activities` | ActivitiesLibrary.razor | Activities image library (to be implemented) |
| `/upload` | Upload.razor | Image upload (optional category param) |
| `/upload/{category}` | Upload.razor | Image upload with pre-selected category |
| `/plan` | PlanDay.razor | Schedule selection (Now/Then/Next) |
| `/schedule` | ScheduleDisplay.razor | Schedule display (`?ids=id1,id2,id3`) |
| `/food-choices` | FoodChoices.razor | Food choice selection |
| `/food-display` | FoodDisplay.razor | Food display (`?ids=id1,id2,...`) |
| `/activity-choices` | ActivityChoices.razor | Activity choice selection (to be implemented) |
| `/activity-display` | ActivityDisplay.razor | Activity display (to be implemented) |
| `/favorites` | Favorites.razor | Favorited images from all categories |
| `/settings` | Settings.razor | Backup/restore data |

## Data Model

### ImageItem
```csharp
public class ImageItem
{
    public string Id { get; set; }           // GUID
    public string Base64Data { get; set; }   // Base64 image data
    public string Label { get; set; }        // Optional label
    public ImageCategory Category { get; set; } // Places/Food/Activities
    public bool IsFavorite { get; set; }     // Favorited flag
    public DateTime CreatedAt { get; set; }  // Creation timestamp
}
```

### ImageCategory Enum
```csharp
public enum ImageCategory
{
    Places = 0,
    Food = 1,
    Activities = 2  // Added in US-032
}
```

## Common Code Patterns

### Service Injection
```razor
@inject IImageStorageService ImageStorage
@inject IJSRuntime JSRuntime
@inject NavigationManager Navigation
```

### Tailwind + Custom Colors
```razor
<!-- Use calm-* classes defined in Tailwind config (in wwwroot/index.html) -->
<div class="bg-calm-bg text-calm-text">
  <button class="bg-calm-primary hover:bg-calm-primary-dark">
```

### Image Compression (JS Interop)
```csharp
// Compress to max 800px, 0.7 quality
var compressed = await JSRuntime.InvokeAsync<string>(
    "compressImage", dataUrl, 800, 0.7);
```

### Storage Operations
```csharp
// Get by category
var images = await ImageStorage.GetImagesByCategoryAsync(ImageCategory.Places);

// Save image
await ImageStorage.SaveImageAsync(new ImageItem { ... });

// Delete image
await ImageStorage.DeleteImageAsync(imageId);

// Toggle favorite
await ImageStorage.ToggleFavoriteAsync(imageId);

// Check storage usage
var info = await ImageStorage.GetStorageInfoAsync();
// Returns: UsagePercentage, EstimatedRemainingImages, CurrentUsageBytes, EstimatedQuotaBytes
```

### E2E Test Pattern
```csharp
[Collection("BlazorApp")]
public class MyTests(BlazorAppFixture fixture)
{
    [Fact]
    public async Task MyTest()
    {
        var page = fixture.Page;
        await page.GotoAsync(fixture.BaseUrl);
        await page.ClearLocalStorageAsync(); // Clear AFTER navigating
        // ... test logic
    }
}
```

## Important Implementation Details

### Escape Characters in Razor
- Use `@@keyframes` (double @) for CSS animations in `<style>` blocks
- Arrow in title uses Unicode: `→` (displayed as "Now → Then → Next")

### Grid Layouts
- Use `min-width: 0` on grid items to allow proper shrinking in CSS Grid
- Fixed columns (`repeat(3, 1fr)`) often better than `auto-fill` for consistency
- Tablet breakpoints: 768px (md:) and 1024px (lg:)

### Modal Pattern
- Boolean flag for visibility (e.g., `ShowDeleteConfirmation`)
- `@onclick:stopPropagation="true"` on modal content to prevent backdrop clicks

### File Uploads
- Use `InputFile` component with `accept` attribute
- `OpenReadStream(maxSize)` to read files (50MB max for restore)
- Convert to base64 via `MemoryStream` and `Convert.ToBase64String`

### Selection Patterns
- **Schedule (PlanDay)**: Max 3 items, numbered badges (1=Now, 2=Then, 3=Next)
- **Food Choices**: Min 2 items, no max limit, checkmark badges
- **Activity Choices**: Min 2 items, no max limit, checkmark badges (to be implemented)

### E2E Test Timing
- Blazor WASM needs extra init time - use 60s timeout on `WaitForSelectorAsync`
- Use `WaitForURLAsync()` for navigation assertions
- Clear localStorage AFTER navigating to page (requires page context)

## Test Status
- **Total E2E Tests**: 44 passing
- **Test Files**: HomePageTests, UploadTests, LibraryTests, ScheduleTests, FoodChoicesTests, BackupRestoreTests
- **Coverage**: All completed user stories (US-001 to US-032)

## Notes for Development
- Always maintain calm visual design - avoid bright/harsh colors
- Keep touch targets ≥48px for accessibility
- Test in browser for visual verification
- E2E tests required for all user-facing features
- Storage quota warnings at 80% capacity
- Dev server runs on **localhost:5161**
- Use `pkill -f "dotnet"` to kill stray processes before E2E tests
- Responsive padding pattern: `px-6 py-8` mobile, `sm:px-8 sm:py-10` larger screens
