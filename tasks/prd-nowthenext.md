# PRD: NowThenNext

## Introduction

NowThenNext is a Blazor WASM application designed to help children with Special Educational Needs (SEN) understand their daily schedule and make food choices through pictures. The app provides a visual, calming interface where carers can upload images of places, activities, and food, then display a simple "Now → Then → Next" sequence to communicate the plan for the day.

The core principle is visual communication with minimal text, large images, and a calm, distraction-free interface.

## Goals

- Provide a simple, visual way to communicate daily schedules to children with SEN
- Enable carers to upload and organize pictures of places, activities, and food
- Display a clear "Now → Then → Next" carousel showing up to 3 items for the day's plan
- Offer a visual food choice interface using uploaded food pictures
- Work offline with browser local storage (no account or internet required)
- Maintain a calm, accessible UI with large images and minimal text

## User Stories

### US-001: Set up Blazor WASM project with Tailwind CSS
**Description:** As a developer, I need the foundational project structure so I can build the application.

**Acceptance Criteria:**
- [ ] Create new Blazor WASM project named "NowThenNext"
- [ ] Install and configure Tailwind CSS
- [ ] Basic layout component with calm color palette (soft blues, greens, neutral tones)
- [ ] App runs successfully in browser
- [ ] Typecheck/build passes

---

### US-002: Create local storage service for images
**Description:** As a developer, I need to store images in browser local storage so data persists without a backend.

**Acceptance Criteria:**
- [ ] Service can save images as base64 strings to localStorage
- [ ] Service can retrieve all stored images by category (places/food)
- [ ] Service can delete individual images
- [ ] Service can toggle favorite status on images
- [ ] Service can retrieve all favorited images across categories
- [ ] Service can report current storage usage as percentage
- [ ] Handle storage quota errors gracefully with user feedback
- [ ] Typecheck/build passes

---

### US-003: Create image upload component
**Description:** As a carer, I want to upload pictures so I can build my library of places, activities, and food.

**Acceptance Criteria:**
- [ ] Large, clearly labeled upload button
- [ ] Accept common image formats (jpg, png, webp)
- [ ] Preview image before confirming upload
- [ ] Add a simple text label to the image (optional, can be left blank)
- [ ] Select category: "Places" or "Food"
- [ ] Image saved to local storage on confirm
- [ ] Typecheck/build passes

---

### US-004: Create Places library view
**Description:** As a carer, I want to see all my uploaded place/activity pictures so I can manage them.

**Acceptance Criteria:**
- [ ] Grid display of all Places images (large tiles, ~200px minimum)
- [ ] Each tile shows image with optional label below
- [ ] Delete button on each tile (with confirmation)
- [ ] Empty state message when no images uploaded
- [ ] Calm, uncluttered layout
- [ ] Typecheck/build passes

---

### US-005: Create Food library view
**Description:** As a carer, I want to see all my uploaded food pictures so I can manage them.

**Acceptance Criteria:**
- [ ] Grid display of all Food images (large tiles, ~200px minimum)
- [ ] Each tile shows image with optional label below
- [ ] Delete button on each tile (with confirmation)
- [ ] Empty state message when no images uploaded
- [ ] Calm, uncluttered layout
- [ ] Typecheck/build passes

---

### US-006: Create navigation between library views
**Description:** As a user, I want to switch between Places and Food libraries easily.

**Acceptance Criteria:**
- [ ] Clear navigation with large, tappable buttons/tabs
- [ ] Visual indicator of current section
- [ ] Navigation accessible from all library views
- [ ] Icons alongside text labels for accessibility
- [ ] Typecheck/build passes

---

### US-007: Build "Now → Then → Next" selection interface
**Description:** As a carer, I want to select up to 3 items from Places to create the day's schedule.

**Acceptance Criteria:**
- [ ] Display all Places images in selectable grid
- [ ] Tap to select (visual highlight on selected items)
- [ ] Show selection order: 1st = Now, 2nd = Then, 3rd = Next
- [ ] Maximum 3 selections enforced
- [ ] Tap selected item again to deselect
- [ ] Clear "Show Schedule" button when at least 1 item selected
- [ ] Typecheck/build passes

---

### US-008: Build "Now → Then → Next" display carousel
**Description:** As a child, I want to see my day's plan as big, clear pictures so I understand what's happening.

**Acceptance Criteria:**
- [ ] Full-screen or near-full-screen display mode
- [ ] Shows selected images in large format with "Now", "Then", "Next" labels
- [ ] Horizontal layout on desktop/tablet, vertical scroll option on mobile
- [ ] Labels use large, clear font (minimum 24px)
- [ ] Subtle gentle pulse or soft glow animation on the "Now" item only
- [ ] Calm background color, no distracting elements
- [ ] Toggle button to switch between static and auto-advance modes
- [ ] Auto-advance cycles through Now/Then/Next every 5-10 seconds when enabled
- [ ] "Back" button to return to selection (positioned unobtrusively)
- [ ] Typecheck/build passes

---

### US-009: Build food choice display
**Description:** As a carer, I want to show selected food options so the child can indicate their preference.

**Acceptance Criteria:**
- [ ] Full-screen or near-full-screen display mode showing selected Food images
- [ ] Display shows 2-6 items in large grid format
- [ ] Each food image is large and clearly tappable
- [ ] Highlight/animate when tapped to confirm selection
- [ ] Show confirmation screen after child taps their choice
- [ ] Calm, focused layout with no distractions
- [ ] "Back" button to return to selection (positioned unobtrusively)
- [ ] Typecheck/build passes

---

### US-010: Create main menu / home screen
**Description:** As a user, I want a simple home screen to choose between managing my library, schedule mode, food choice mode, or accessing favorites.

**Acceptance Criteria:**
- [ ] App title "Now -> Then -> Next" displayed at top with arrows between words
- [ ] Five large buttons in order: "Places", "Food", "Plan the Day", "Food Choices", "Favorites"
- [ ] Settings gear icon in top corner for backup/restore access
- [ ] Buttons use icons alongside text
- [ ] Calm, welcoming design
- [ ] Typecheck/build passes

---

### US-011: Add responsive design for tablet use
**Description:** As a carer, I want the app to work well on tablets since that's the likely primary device.

**Acceptance Criteria:**
- [ ] Touch-friendly tap targets (minimum 48px)
- [ ] Layout adapts to tablet screen sizes (768px-1024px)
- [ ] Images scale appropriately without distortion
- [ ] No horizontal scroll on main views
- [ ] Typecheck/build passes

---

### US-012: Implement calm color theme
**Description:** As a user, I want the app to use calming colors that won't overstimulate.

**Acceptance Criteria:**
- [ ] Define Tailwind color palette: soft blues, muted greens, warm neutrals
- [ ] Avoid bright/harsh colors (no pure red, bright yellow, etc.)
- [ ] Consistent use of theme across all components
- [ ] Good contrast for accessibility (WCAG AA minimum)
- [ ] Typecheck/build passes

---

### US-013: Create Favorites view
**Description:** As a carer, I want a dedicated Favorites section so I can quickly access frequently used images.

**Acceptance Criteria:**
- [ ] Favorites view accessible from home screen
- [ ] Shows all favorited images from both Places and Food categories
- [ ] Clear visual grouping or labels to distinguish Places vs Food items
- [ ] Grid display with large tiles matching library views
- [ ] Empty state message when no favorites added
- [ ] Typecheck/build passes

---

### US-014: Add favorite/unfavorite functionality to images
**Description:** As a carer, I want to mark images as favorites so they appear in my Favorites view.

**Acceptance Criteria:**
- [ ] Heart/star icon on each image tile in library views
- [ ] Tap icon to toggle favorite status
- [ ] Visual indicator when image is favorited (filled vs outline icon)
- [ ] Favorite status persists in local storage
- [ ] Can unfavorite from Favorites view (removes from view)
- [ ] Typecheck/build passes

---

### US-015: Add storage usage warning
**Description:** As a carer, I want to be warned when storage is getting full so I can manage my images.

**Acceptance Criteria:**
- [ ] Monitor localStorage usage after each upload
- [ ] Show gentle warning banner when storage exceeds 80% capacity
- [ ] Warning includes approximate remaining capacity (e.g., "~5 more images")
- [ ] Warning is dismissible but reappears on next upload if still over threshold
- [ ] Show error message if upload fails due to storage quota
- [ ] Typecheck/build passes

---

### US-016: Create settings panel with backup/restore
**Description:** As a carer, I want to access settings from the home screen to backup and restore my data.

**Acceptance Criteria:**
- [ ] Settings gear icon in top corner of home screen
- [ ] Tapping gear opens settings panel/modal
- [ ] Settings panel shows "Backup Data" and "Restore Data" options
- [ ] Clean, simple layout matching app design
- [ ] Close button to return to home
- [ ] Typecheck/build passes

---

### US-017: Implement data backup (export)
**Description:** As a carer, I want to backup my images and settings so I don't lose them if something goes wrong.

**Acceptance Criteria:**
- [ ] "Backup Data" button in settings
- [ ] Exports all images (Places and Food) with labels, favorites, and metadata
- [ ] Downloads as JSON file with timestamp in filename (e.g., nowthenext-backup-2024-01-15.json)
- [ ] Shows success message after download starts
- [ ] Typecheck/build passes

---

### US-018: Implement data restore (import)
**Description:** As a carer, I want to restore my data from a backup file.

**Acceptance Criteria:**
- [ ] "Restore Data" button in settings
- [ ] Opens file picker to select backup JSON file
- [ ] Validates file format before processing
- [ ] Prompts user to choose: "Replace all data" or "Merge with existing"
- [ ] Replace: Clears existing data, imports backup
- [ ] Merge: Adds new images, skips duplicates (by ID)
- [ ] Shows success message with count of images restored
- [ ] Shows error message if file is invalid
- [ ] Typecheck/build passes

---

### US-019: Build Food Choices selection interface
**Description:** As a carer, I want to select multiple items from Food to create a set of food choices to display.

**Acceptance Criteria:**
- [ ] Display all Food images in selectable grid
- [ ] Tap to select (visual highlight on selected items)
- [ ] No maximum limit on selections (practical limit of 2-6 recommended)
- [ ] Tap selected item again to deselect
- [ ] Clear "Show Choices" button when at least 2 items selected
- [ ] Selected count displayed (e.g., "3 items selected")
- [ ] Typecheck/build passes

## Functional Requirements

- FR-1: The app must be a Blazor WebAssembly (WASM) application
- FR-2: The app must use Tailwind CSS for all styling
- FR-3: Images must be stored in browser localStorage as base64 strings
- FR-4: Users must be able to upload images and assign them to "Places" or "Food" categories
- FR-5: Users must be able to add optional text labels to uploaded images
- FR-6: Users must be able to delete images from either library with a confirmation dialog
- FR-7: Deleting an image must remove it from localStorage permanently
- FR-8: The app title must display as "Now -> Then -> Next" with arrow characters between words
- FR-9: The home screen menu order must be: Places, Food, Plan the Day, Food Choices, Favorites
- FR-10: The "Now → Then → Next" mode must allow selecting 1-3 items from Places
- FR-11: The schedule display must show selected items with clear "Now", "Then", "Next" labels
- FR-12: The schedule display must have a toggle to switch between static and auto-advance modes
- FR-13: The "Now" item in schedule display must have a subtle pulse/glow animation
- FR-14: The "Food Choices" mode must allow selecting 2+ items from Food
- FR-15: The food choice display must show selected items in a large, tappable grid
- FR-16: All interactive elements must be large enough for easy touch interaction (48px minimum)
- FR-17: The UI must use calm, muted colors suitable for children with SEN
- FR-18: Users must be able to mark images as favorites
- FR-19: A dedicated Favorites view must show all favorited images from both categories
- FR-20: The app must warn users when localStorage is approaching capacity (>80%)
- FR-21: The app must adapt to both portrait and landscape orientations without locking
- FR-22: Settings must be accessible via gear icon on home screen
- FR-23: Users must be able to export all data as a downloadable JSON backup file
- FR-24: Users must be able to restore data from a backup file
- FR-25: When restoring, users must choose between replacing all data or merging with existing

## Non-Goals

- No user authentication or multiple user profiles
- No automatic cloud sync (manual backup/restore via file export only for MVP)
- No audio/read-aloud functionality (may be added later)
- No sharing features (backup is for personal use only)
- No scheduling/calendar integration
- No edit functionality for images after upload (delete and re-upload instead)
- No drag-and-drop reordering of library items

## Technical Considerations

- **Blazor WASM**: Client-side only, no server component needed
- **Tailwind CSS**: Use CDN or build process integration
- **Local Storage**: ~5-10MB limit in most browsers; images should be compressed/resized on upload to maximize capacity
- **Image Handling**: Consider resizing images client-side before storing to save space
- **Offline Support**: App should work entirely offline once loaded
- **Browser Support**: Target modern evergreen browsers (Chrome, Edge, Safari, Firefox)

## Testing Requirements

- **Test Framework**: xUnit v3 for all test projects
- **E2E Testing**: Use Playwright for browser-based end-to-end tests
- **Test Coverage**: All user-facing features must have E2E tests that run against a real browser
- **Test Location**: Tests in `tests/NowThenNext.Tests.E2E/` project

### E2E Test Scenarios (Playwright)

Each major user flow should have browser tests:

1. **Home Screen Navigation**
   - All five main buttons are visible and clickable
   - Each button navigates to the correct page

2. **Image Upload Flow**
   - Upload area triggers file picker when clicked
   - Image preview displays after file selection
   - Category selection works (Places/Food)
   - Label input accepts text
   - Confirm saves image and shows success message

3. **Library Views**
   - Places library displays uploaded place images
   - Food library displays uploaded food images
   - Tab navigation switches between libraries
   - Delete button removes image (with confirmation)
   - Favorite toggle works and persists

4. **Schedule Flow (Plan the Day)**
   - Can select up to 3 images
   - Selection order shown (Now/Then/Next badges)
   - Cannot select more than 3
   - "Show Schedule" button appears after selection
   - Schedule display shows images with correct labels
   - Auto-advance toggle works

5. **Food Choices Flow**
   - Can select multiple food items from library
   - Selection count displayed
   - "Show Choices" button appears after selecting 2+ items
   - Display shows selected items in large format
   - Tap feedback works on selection
   - Confirmation shown after child selects a food item

6. **Favorites**
   - Favorited items appear in Favorites view
   - Items grouped by category
   - Can unfavorite from Favorites view

7. **Storage Warning**
   - Warning banner appears when storage usage exceeds 80%
   - Banner is dismissible
   - Error shown when storage quota exceeded

8. **Backup/Restore**
   - Settings gear icon visible on home screen
   - Settings panel opens when gear tapped
   - Backup downloads JSON file
   - Restore accepts JSON file upload
   - Restore prompts for replace vs merge choice
   - Data correctly restored after import

## Design Considerations

- **Color Palette**: Soft, calming colors - consider:
  - Background: Warm off-white or very light blue
  - Primary: Soft teal or muted blue
  - Accent: Gentle green
  - Text: Dark gray (not pure black)
- **Typography**: Clear, rounded sans-serif font; minimum 18px body text, 24px+ for labels
- **Imagery**: Images should have subtle rounded corners, soft shadows
- **Spacing**: Generous padding and margins to avoid cluttered appearance
- **Transitions**: Gentle, slow transitions (200-300ms) rather than snappy ones
- **Animations**: Subtle pulse/glow on "Now" item only; no other looping animations
- **Orientation**: Adapt fluidly to both portrait and landscape; no orientation locking
- **Auto-advance**: When enabled, cycle through schedule items every 5-10 seconds with smooth transitions

## Success Metrics

- Carer can upload an image and see it in the library within 3 taps
- Child can view the "Now → Then → Next" schedule in under 2 taps from home
- Food choices can be displayed in under 2 taps from home
- App loads and works offline after first visit
- No jarring colors or animations that could cause distress

## Decisions Log

| Question | Decision |
|----------|----------|
| Favorites section? | Yes - separate Favorites view accessible from home screen |
| Auto-advance display? | Optional toggle - user can switch between static and auto-advance |
| Orientation locking? | No - adapt fluidly to whatever orientation the device is in |
| Animations? | Subtle pulse/glow on "Now" item only |
| Image limit? | No hard limit - warn user when storage is getting full (>80%) |
| Test framework? | xUnit v3 for all tests |
| E2E testing? | Playwright for browser-based tests against real browser |
| Menu item naming? | Renamed "My Pictures" to "Places", moved to first position |
| App title format? | "Now -> Then -> Next" with arrow characters between words |
| Backup method? | JSON file export/import (cloud sync ideal but file-based for MVP) |
| Backup access? | Settings gear icon on home screen |
| Restore behavior? | User chooses between replace all or merge with existing |
| Food Choices selection? | Yes - separate selection interface similar to Plan the Day, with "Food Choices" button on home screen |
