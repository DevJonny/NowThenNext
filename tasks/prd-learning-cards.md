# PRD: Learning Cards

## Introduction

Add a Learning Cards section to NowThenNext - a visual flashcard experience for teaching shapes, colours, animals, dinosaurs, and user-created categories. Each card shows an image on the front; tapping reveals the word alongside the image. This complements the existing phonics feature but is separate and non-sequential - all cards are freely explorable with no unlocking or progression gates.

The section ships with four built-in categories populated with bundled SVG illustrations, and supports user-created categories with uploaded images.

## Goals

- Provide a calm, visual-first flashcard experience for learning vocabulary across multiple categories
- Ship with four built-in categories (Shapes, Colours, Animals, Dinosaurs) with bundled illustrations
- Allow users to create custom categories and cards with uploaded images
- Keep the interaction simple: tap to reveal the word, tap again to hide it
- Maintain consistency with the existing app's SEN-friendly design principles

## User Stories

### US-049: Add Learning Cards section to home page
**Description:** As a user, I want to see a Learning Cards option on the home page so that I can access the new flashcard feature.

**Acceptance Criteria:**
- [ ] New "Learning Cards" menu group on home page, visually distinct from Phonics and other sections
- [ ] Uses the `calm-cards` theme colour (soft coral `#D4937A`)
- [ ] Touch target meets 48px minimum
- [ ] Links to `/learning` route
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-050: Learning Cards category listing page
**Description:** As a user, I want to see all available card categories so that I can choose which topic to explore.

**Acceptance Criteria:**
- [ ] Page at `/learning` displays all categories as large tiles
- [ ] Built-in categories show in fixed order: Shapes, Colours, Animals, Dinosaurs
- [ ] Custom categories appear after built-in categories, ordered by creation date
- [ ] Each tile shows the category name and a representative icon/image
- [ ] Back navigation to home page
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-051: Card grid within a category
**Description:** As a user, I want to see all cards in a category displayed as a grid so that I can browse and pick cards to learn.

**Acceptance Criteria:**
- [ ] Page at `/learning/{categoryId}` displays cards in a responsive grid
- [ ] Each card shows only the image (no text visible) on its front face
- [ ] Cards are large enough for easy tapping (minimum 120px)
- [ ] Category name displayed as page heading
- [ ] Back navigation to category listing (`/learning`)
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-052: Card flip to reveal word
**Description:** As a user, I want to tap a card to reveal the word so that I can learn the name of the image.

**Acceptance Criteria:**
- [ ] Tapping an unrevealed card shows the word below/beside the image
- [ ] Image remains visible when word is revealed
- [ ] Word displayed in large, clear Nunito font (minimum 24px)
- [ ] Tapping a revealed card hides the word again (toggle behaviour)
- [ ] Gentle, calming flip/fade animation (consistent with app's animation style)
- [ ] Only one card can be revealed at a time - revealing a new card auto-hides the previously revealed one
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-053: Built-in Shapes category
**Description:** As a developer, I need to create the Shapes data and SVG illustrations so that the category is populated on first use.

**Acceptance Criteria:**
- [ ] Shapes category contains at minimum: circle, square, triangle, rectangle, star, heart, diamond, oval
- [ ] Each shape rendered as a clean, simple SVG with calm fill colours
- [ ] SVGs are inline or bundled as static assets (not external dependencies)
- [ ] Shapes display correctly at card size
- [ ] Typecheck/lint passes

---

### US-054: Built-in Colours category
**Description:** As a developer, I need to create the Colours data and visual swatches so that the category is populated on first use.

**Acceptance Criteria:**
- [ ] Colours category contains at minimum: red, blue, green, yellow, orange, purple, pink, brown, black, white
- [ ] Each colour displayed as a filled rounded rectangle or circle swatch
- [ ] Colour swatches are visually distinct and use actual representative colours (not the calm palette - these are the teaching colours)
- [ ] Typecheck/lint passes

---

### US-055: Built-in Animals category
**Description:** As a developer, I need to create the Animals data and illustrations so that the category is populated on first use.

**Acceptance Criteria:**
- [ ] Animals category contains at minimum: cat, dog, fish, bird, horse, cow, pig, sheep, rabbit, elephant, lion, tiger
- [ ] Each animal has a simple, friendly SVG illustration
- [ ] Illustrations are clear enough for children to identify the animal without the label
- [ ] Typecheck/lint passes

---

### US-056: Built-in Dinosaurs category
**Description:** As a developer, I need to create the Dinosaurs data and illustrations so that the category is populated on first use.

**Acceptance Criteria:**
- [ ] Dinosaurs category contains at minimum: T-Rex, Triceratops, Stegosaurus, Brachiosaurus, Velociraptor, Pteranodon, Ankylosaurus, Diplodocus
- [ ] Each dinosaur has a simple, friendly SVG illustration
- [ ] Illustrations are clear enough for children to identify the dinosaur shape without the label
- [ ] Typecheck/lint passes

---

### US-057: Learning Cards data service
**Description:** As a developer, I need a data service to manage built-in and custom card categories and cards so that the feature has a clean data layer.

**Acceptance Criteria:**
- [ ] `ILearningCardsDataService` interface with methods to get categories, get cards by category, add/edit/delete custom categories and cards
- [ ] Built-in categories and cards defined as static data (similar to `PhonicsDataService`)
- [ ] Custom categories and cards persisted to localStorage (key: `learning-cards`)
- [ ] Built-in content is read-only (cannot be edited or deleted by user)
- [ ] Service registered as scoped in DI
- [ ] Typecheck/lint passes

---

### US-058: Create custom category
**Description:** As a user, I want to create my own card category so that I can teach vocabulary specific to my child's needs.

**Acceptance Criteria:**
- [ ] "Add Category" button/tile visible on the category listing page
- [ ] Tapping opens a form to enter category name and choose an emoji icon
- [ ] Category name is required (validation with friendly message)
- [ ] Emoji picker offers a small curated set of child-friendly emoji (e.g. animals, objects, nature, food) - not a full system picker
- [ ] Selected emoji displays on the category tile alongside the name
- [ ] Default emoji assigned if none selected
- [ ] New category appears on the listing page after creation
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-059: Add custom card to a category
**Description:** As a user, I want to add my own cards to a custom category by uploading an image and typing a word.

**Acceptance Criteria:**
- [ ] "Add Card" button visible on custom category card grid pages
- [ ] Upload an image (reuse existing image upload/compression pipeline)
- [ ] Enter the word/label for the card (required field)
- [ ] Card appears in the grid after saving
- [ ] Image compressed using existing JS interop (`compressImage`)
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-060: Delete custom card
**Description:** As a user, I want to delete a custom card I no longer need.

**Acceptance Criteria:**
- [ ] Delete option available on custom cards (not on built-in cards)
- [ ] Confirmation dialog before deletion (using existing modal pattern)
- [ ] Card removed from grid after deletion
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-061: Delete custom category
**Description:** As a user, I want to delete a custom category I no longer need.

**Acceptance Criteria:**
- [ ] Delete option available on custom categories (not on built-in categories)
- [ ] Confirmation dialog warns that all cards in the category will also be deleted
- [ ] Category and all its cards removed after confirmation
- [ ] Typecheck/lint passes
- [ ] Verify in browser using dev-browser skill

---

### US-062: Include custom cards in backup/restore
**Description:** As a user, I want my custom learning cards included when I back up or restore my data so that I don't lose them.

**Acceptance Criteria:**
- [ ] Custom categories and cards included in backup JSON export
- [ ] Restore correctly imports custom categories and cards
- [ ] Built-in categories are not duplicated on restore
- [ ] Typecheck/lint passes

---

### US-063: E2E tests for Learning Cards
**Description:** As a developer, I need E2E tests covering the learning cards feature to ensure quality.

**Acceptance Criteria:**
- [ ] Test: category listing page shows 4 built-in categories
- [ ] Test: navigating to a category shows card grid
- [ ] Test: tapping a card reveals the word
- [ ] Test: tapping a revealed card hides the word
- [ ] Test: creating a custom category
- [ ] Test: adding a custom card with image upload
- [ ] Test: deleting a custom card
- [ ] Test: deleting a custom category
- [ ] All tests pass
- [ ] Typecheck/lint passes

## Functional Requirements

- FR-1: The app must display a "Learning Cards" entry on the home page linking to `/learning`
- FR-2: The `/learning` page must list all card categories (built-in first, then custom, ordered by creation date)
- FR-3: The `/learning/{categoryId}` page must display all cards in a responsive grid
- FR-4: Each card must show only its image by default (no text)
- FR-5: Tapping a card must reveal the word alongside the image; tapping again must hide the word
- FR-6: The card reveal must include a gentle animation (fade or flip, duration consistent with app style)
- FR-7: Built-in categories (Shapes, Colours, Animals, Dinosaurs) must be populated with SVG illustrations and cannot be edited or deleted
- FR-8: Users must be able to create custom categories with a name and an emoji icon
- FR-9: Users must be able to add cards to custom categories by uploading an image and entering a word
- FR-10: Users must be able to delete custom cards and custom categories (with confirmation)
- FR-11: Custom categories and cards must persist in localStorage under the key `learning-cards`
- FR-12: Custom cards must be included in backup/restore functionality
- FR-13: All interactions must meet the 48px minimum touch target requirement
- FR-14: The feature must use soft coral `#D4937A` as its theme colour (`calm-cards`, `calm-cards-dark: #B87A62`, `calm-cards-light: #E0B0A0`)
- FR-15: Revealing a card must auto-hide any previously revealed card (only one revealed at a time)
- FR-16: All built-in SVG illustrations must be hand-crafted (no external icon libraries or emoji fallbacks)
- FR-17: Custom categories must support an emoji icon selected from a curated picker

## Non-Goals

- No audio pronunciation or text-to-speech
- No progress tracking, scoring, or gamification
- No sequential unlocking or phased learning
- No spaced repetition or quiz mode
- No drag-and-drop card reordering
- No sharing of custom categories between devices (beyond backup/restore)
- No editing of built-in card content

## Design Considerations

- **Theme colour:** Soft coral `#D4937A` for Learning Cards, with dark variant `#B87A62` and light variant `#E0B0A0`. Distinct from Phonics blue and the primary teal
- **Card layout:** Cards should be square or near-square tiles in a responsive grid (2 columns mobile, 3-4 columns tablet/desktop)
- **Flip animation:** A gentle CSS transform (rotateY or fade) lasting ~0.4s keeps the interaction calming
- **SVG illustrations:** All built-in illustrations are hand-crafted SVGs for consistency. Shapes and colours are straightforward geometric SVGs. Animals and dinosaurs use simple, friendly, child-appropriate filled illustrations with minimal detail - designed to be recognisable at small sizes (~120px)
- **Revealed state:** Word appears below the image within the same card tile, card may expand slightly to accommodate text
- **Consistency:** Reuse existing page layout patterns (heading, back button, responsive padding `px-6 py-8` / `sm:px-8 sm:py-10`)

## Technical Considerations

- **Data service pattern:** Follow `PhonicsDataService` singleton pattern for built-in data; custom data stored in localStorage similar to `LocalStorageImageService`
- **SVG storage:** Built-in SVGs can be inline in the data service as string constants or stored as static files in `wwwroot/images/learning/`
- **Image compression:** Custom card uploads must use the existing `compressImage` JS interop
- **localStorage key:** `learning-cards` for custom categories/cards (separate from `images` and `phonics-progress`)
- **Routing:** `/learning` for category list, `/learning/{categoryId}` for card grid. No separate card detail page needed since the flip happens in-place
- **Backup integration:** Extend `Settings.razor` backup/restore to include `learning-cards` localStorage key
- **Models:** New `LearningCategory` and `LearningCard` models in the Models directory

### Suggested Data Model

```csharp
public class LearningCategory
{
    public string Id { get; set; }           // e.g. "shapes", "colours", or GUID for custom
    public string Name { get; set; }         // e.g. "Shapes", "Colours"
    public string Emoji { get; set; }        // e.g. "🎨", "🦁" - displayed on category tile
    public bool IsBuiltIn { get; set; }      // true for the 4 default categories
    public DateTime CreatedAt { get; set; }  // for ordering custom categories
}

public class LearningCard
{
    public string Id { get; set; }           // unique identifier
    public string CategoryId { get; set; }   // parent category
    public string ImageData { get; set; }    // SVG string (built-in) or base64 (custom)
    public string Word { get; set; }         // the vocabulary word
    public bool IsBuiltIn { get; set; }      // true for default cards
}
```

## Success Metrics

- All 4 built-in categories render with clear, identifiable illustrations
- Card flip interaction is smooth and intuitive (single tap to reveal/hide)
- Users can create a custom category and add cards in under 30 seconds
- Feature integrates seamlessly with existing backup/restore
- All E2E tests pass

## Resolved Decisions

- **Theme colour:** Soft coral `#D4937A` (dark: `#B87A62`, light: `#E0B0A0`)
- **Card reveal behaviour:** Auto-hide previous - only one card revealed at a time for focus
- **Illustrations:** Hand-crafted SVGs for all built-in content (no external dependencies)
- **Custom category icons:** Users pick an emoji from a curated set for their category tile

## Open Questions

- None - all questions resolved.
