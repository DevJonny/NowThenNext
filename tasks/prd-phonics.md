# PRD: Phonics Section (UK Letters and Sounds Phases 2-5)

## Introduction

Add a phonics section to NowThenNext that displays flashcard-style grapheme cards based on the UK Letters and Sounds phonics framework (Phases 2-5). A parent or teacher shows the child each card featuring a large, clear grapheme (letter or letter combination) alongside a keyword and example word. Sounds unlock sequentially within each phase, following the standard teaching order. All four phases (2-5) are freely selectable.

This is a visual-only feature - the parent/teacher says the sound aloud. Interactive features (tap-to-hear audio) are planned for a future iteration.

## Goals

- Provide a calm, visual phonics flashcard experience consistent with the app's SEN-friendly design
- Cover the full Letters and Sounds programme (Phases 2-5) with accurate GPC ordering
- Allow parents/teachers to select any phase to work on
- Unlock sounds sequentially within each phase to follow the teaching order
- Track progress in localStorage so sessions persist across visits

## User Stories

### US-038: Define phonics data model and static GPC data
**Description:** As a developer, I need a data structure for phonics phases, weeks, and grapheme cards so the app can display phonics content.

**Acceptance Criteria:**
- [ ] Create `PhonicsPhase` model with: Id, Name, Description, list of `PhonicsWeek`
- [ ] Create `PhonicsWeek` model with: WeekNumber, list of `GraphemeCard`
- [ ] Create `GraphemeCard` model with: Id, Grapheme (e.g. "sh"), ExampleWord (e.g. "ship"), KeywordHint (e.g. "sh is for ship"), PhaseId, WeekNumber, OrderIndex
- [ ] Create a static `PhonicsDataService` that returns all phases with their GPCs
- [ ] Phase 2 data populated (9 weeks, ~30 GPCs including ff, ll, ss, zz, ch, sh, th, ng, nk, qu)
- [ ] Phase 3 data populated (4-5 weeks, vowel digraphs/trigraphs: ai, ee, igh, oa, oo, ar, or, ur, ow, oi, ear, air, er)
- [ ] Phase 4 data populated (consonant clusters: initial bl, br, cl, cr, dr, fl, fr, gl, gr, pl, pr, sc, sk, sl, sm, sn, sp, st, sw, tr, tw; final ft, lf, lp, lt, mp, nd, nk, nt, pt, sk, st, xt - with example words)
- [ ] Phase 5 data populated (alternative spellings: ay, ou, ie, ea, oy, ir, ue, aw, wh, ph, ew, oe, au; split digraphs: a_e, e_e, i_e, o_e, u_e; alternative pronunciations)
- [ ] Build passes

### US-039: Add phonics progress service
**Description:** As a developer, I need a service to track which graphemes a child has completed so progress persists across sessions.

**Acceptance Criteria:**
- [ ] Create `PhonicsProgressService` that stores progress in localStorage
- [ ] Method to mark a grapheme as completed by its Id
- [ ] Method to get the next unlocked grapheme for a given phase
- [ ] Method to get completion count/percentage for a phase
- [ ] Method to reset progress for a single phase
- [ ] Method to reset all phonics progress
- [ ] First grapheme in each phase is always unlocked
- [ ] Completing a grapheme unlocks the next one in sequence (within the same phase)
- [ ] Register service in `Program.cs`
- [ ] Build passes

### US-040: Add Phonics button to home page
**Description:** As a user, I want a Phonics option on the home menu so I can access phonics flashcards.

**Acceptance Criteria:**
- [ ] New "Phonics" group on the home page (below Activities group)
- [ ] Single "Phonics" button with a book/letter icon
- [ ] Uses a new calm colour that fits the palette (e.g. calm-phonics: soft blue #7BA3C4)
- [ ] Navigates to `/phonics` route
- [ ] Consistent style with existing menu buttons (48px+ touch target, hover effects)
- [ ] Build passes
- [ ] Verify in browser

### US-041: Phase selection screen
**Description:** As a parent/teacher, I want to choose which phonics phase to work on so I can match the child's current learning stage.

**Acceptance Criteria:**
- [ ] New page at `/phonics` route
- [ ] Back button navigates to home
- [ ] Shows 4 phase cards: Phase 2, Phase 3, Phase 4, Phase 5
- [ ] Each card shows: phase name, brief description (e.g. "Single letters & first digraphs"), progress indicator (e.g. "12/30 sounds")
- [ ] Progress bar or fraction showing completion
- [ ] Tapping a phase card navigates to `/phonics/{phaseId}`
- [ ] Calm, consistent visual design (rounded cards, soft colours, generous spacing)
- [ ] 48px+ touch targets
- [ ] Build passes
- [ ] Verify in browser

### US-042: Sound list within a phase
**Description:** As a parent/teacher, I want to see all sounds in a phase organised by week, with clear indication of which are completed, current, and locked.

**Acceptance Criteria:**
- [ ] New page at `/phonics/{phaseId}` route
- [ ] Back button navigates to `/phonics`
- [ ] Header shows phase name and progress (e.g. "Phase 2 - 8/30")
- [ ] Sounds displayed in a grid, grouped by week with week labels
- [ ] Three visual states for each sound tile:
  - **Completed**: checkmark overlay, slightly muted
  - **Current** (next unlocked): highlighted border, prominent - this is the next sound to learn
  - **Locked**: greyed out / faded, shows a lock icon, not tappable
- [ ] Tapping a completed or current sound navigates to `/phonics/{phaseId}/card/{graphemeId}`
- [ ] Tapping a locked sound does nothing (no navigation)
- [ ] Each sound tile shows the grapheme text (e.g. "sh", "ai")
- [ ] 48px+ touch targets
- [ ] Build passes
- [ ] Verify in browser

### US-043: Flashcard display
**Description:** As a parent/teacher, I want to show a large, clear flashcard of a grapheme so the child can focus on the letter(s) and example word.

**Acceptance Criteria:**
- [ ] New page at `/phonics/{phaseId}/card/{graphemeId}` route
- [ ] Back button navigates to sound list (`/phonics/{phaseId}`)
- [ ] Large grapheme displayed prominently in the centre (minimum 72px font, bold)
- [ ] Example word displayed below the grapheme (e.g. "sun" for "s") with the grapheme highlighted/coloured within the word
- [ ] Keyword hint displayed in smaller text (e.g. "s is for sun")
- [ ] Clean, minimal layout - no distractions
- [ ] Calm background colour, high contrast text
- [ ] Build passes
- [ ] Verify in browser

### US-044: Flashcard navigation and completion
**Description:** As a parent/teacher, I want to mark a sound as learned and move to the next one so the child progresses through the phase.

**Acceptance Criteria:**
- [ ] "Got it!" / "Learned" button at bottom of flashcard (prominent, calm-phonics colour)
- [ ] Tapping "Got it!" marks the current grapheme as completed
- [ ] After marking complete, automatically navigates to the next unlocked grapheme's flashcard
- [ ] If the completed grapheme was the last in the phase, show a "Phase Complete!" celebration message and navigate back to the sound list
- [ ] "Previous" and "Next" navigation arrows to browse already-completed sounds (without changing completion state)
- [ ] Cannot navigate forward past the current unlocked sound
- [ ] 48px+ touch targets on all buttons
- [ ] Build passes
- [ ] Verify in browser

### US-045: Reset progress
**Description:** As a parent/teacher, I want to reset phonics progress so the child can start a phase again or start fresh.

**Acceptance Criteria:**
- [ ] Reset button on the phase selection screen (`/phonics`) to reset all progress
- [ ] Reset button on each phase's sound list (`/phonics/{phaseId}`) to reset that phase only
- [ ] Both reset actions show a confirmation modal before proceeding (consistent with existing delete confirmation pattern)
- [ ] After reset, progress returns to the beginning (first sound unlocked)
- [ ] Build passes
- [ ] Verify in browser

### US-046: E2E tests for phonics phase selection
**Description:** As a developer, I need E2E tests for the phonics section to ensure quality.

**Acceptance Criteria:**
- [ ] Test: Phonics button visible on home page and navigates to `/phonics`
- [ ] Test: Phase selection screen shows all 4 phases
- [ ] Test: Tapping a phase navigates to the sound list
- [ ] Test: Back button on phase selection returns to home
- [ ] All tests pass

### US-047: E2E tests for sound list and flashcard display
**Description:** As a developer, I need E2E tests for the sound list and flashcard functionality.

**Acceptance Criteria:**
- [ ] Test: Sound list shows graphemes grouped by week
- [ ] Test: First sound is unlocked, subsequent sounds are locked
- [ ] Test: Tapping unlocked sound shows flashcard
- [ ] Test: Flashcard displays grapheme, example word, and keyword
- [ ] Test: "Got it!" button marks sound as complete and shows next sound
- [ ] Test: Completed sounds show checkmark on sound list
- [ ] Test: Progress persists after navigating away and returning
- [ ] All tests pass

### US-048: E2E tests for progress reset
**Description:** As a developer, I need E2E tests for the reset progress functionality.

**Acceptance Criteria:**
- [x] Test: Reset single phase shows confirmation modal
- [x] Test: Confirming reset clears progress for that phase only
- [x] Test: Reset all shows confirmation modal
- [x] Test: Confirming reset all clears progress for all phases
- [x] Test: Cancelling reset does not clear progress
- [x] All tests pass

## Functional Requirements

- FR-1: The app must include all Letters and Sounds Phases 2-5 GPCs in the correct teaching order
- FR-2: Each grapheme card must display the grapheme, an example keyword word, and a keyword hint
- FR-3: All four phases must be selectable from the phase selection screen at any time
- FR-4: Within each phase, graphemes must unlock one at a time in sequential order
- FR-5: The first grapheme in each phase must always be unlocked
- FR-6: Completing a grapheme must unlock the next grapheme in the same phase
- FR-7: Progress must persist in localStorage across browser sessions
- FR-8: Users must be able to reset progress per-phase or for all phases
- FR-9: Locked graphemes must be visually distinct and not tappable
- FR-10: The flashcard display must be large, clear, and distraction-free
- FR-11: The Phonics section must appear as a top-level menu item on the home page
- FR-12: All interactive elements must meet the 48px minimum touch target requirement
- FR-13: Phase 4 cards must show consonant clusters with example words (no new single GPCs)
- FR-14: Phase 5 cards must include split digraphs displayed as "a_e" format with example words

## Non-Goals (Out of Scope)

- No audio playback (parent/teacher says the sound aloud) - planned for future iteration
- No interactive tap-to-hear phoneme feature - planned for future iteration
- No reading/blending practice (combining sounds into words)
- No writing/tracing practice
- No assessment or quiz mode
- No custom grapheme cards (all content is the static Letters and Sounds programme)
- No images on flashcards (text-only cards for now)
- No integration with the existing image library
- No cloud sync of progress

## Design Considerations

- Use the existing calm visual design system (soft colours, rounded corners, Nunito font)
- Introduce a new `calm-phonics` colour (suggested: soft blue #7BA3C4 / dark #6890B0 / light #9BBDD6) to differentiate from Places (teal), Food (green), and Activities (purple)
- Flashcard grapheme text should be very large (72px+) and bold for visual clarity
- The example word on the flashcard should highlight the target grapheme in the phonics colour
- Locked sounds should use a subtle greyed-out style, not harsh crossed-out styling
- Phase cards on the selection screen should show a gentle progress bar
- The "Got it!" button should feel rewarding but calm (no harsh animations)
- Follow the existing back-button and header patterns from other pages

## Technical Considerations

- Phonics data is static and hardcoded (no user upload needed) - define in a `PhonicsDataService`
- Progress stored in localStorage as JSON (key: `phonics-progress`)
- Follow existing service patterns: inject `IPhonicsDataService` and `IPhonicsProgressService`
- Reuse existing localStorage interop from `LocalStorageImageService` pattern
- Route structure: `/phonics`, `/phonics/{phaseId}`, `/phonics/{phaseId}/card/{graphemeId}`
- GraphemeCard Id format: `p{phase}-w{week}-{grapheme}` (e.g. `p2-w1-s`, `p3-w4-ai`, `p5-sd-a_e`)
- Phase 4 has no new single-letter GPCs; cards represent consonant clusters instead
- Consider a `PhonicsCategory` enum or similar to distinguish from existing `ImageCategory`

## GPC Reference (Letters and Sounds Phases 2-5)

### Phase 2 (9 weeks)
| Week | Graphemes |
|------|-----------|
| 1 | s (sun), a (ant), t (tap), p (pin) |
| 2 | i (it), n (net), m (mat), d (dog) |
| 3 | g (got), o (on), c (cat), k (kid) |
| 4 | ck (duck), e (egg), u (up), r (red) |
| 5 | h (hat), b (bat), f/ff (fan/puff), l/ll (leg/bell), ss (hiss) |
| 6 | Assessment/review |
| 7 | v (van), w (win), x (fox), y (yes) |
| 8 | z/zz (zip/buzz), qu (queen), ch (chip) |
| 9 | sh (shop), th (thin), ng (ring), nk (pink) |

### Phase 3 (5 weeks)
| Week | Graphemes |
|------|-----------|
| 1 | ai (rain), ee (feet), igh (night), oa (boat) |
| 2 | oo-long (moon), oo-short (book), ar (car), or (fork) |
| 3 | ur (nurse), ow (cow), oi (coin), ear (dear) |
| 4 | air (fair), er (letter) |
| 5 | Consolidation/longer words |

### Phase 4 (Consonant Clusters - no new GPCs)
| Group | Clusters |
|-------|----------|
| Initial | bl (black), br (bring), cl (clap), cr (crab), dr (drop), fl (flag), fr (frog), gl (glad), gr (grip), pl (plan), pr (press), sc (scam), sk (skip), sl (slip), sm (smell), sn (snap), sp (spot), st (stop), sw (swim), tr (trip), tw (twin) |
| Final | ft (left), lf (self), lp (help), lt (felt), mp (lamp), nd (sand), nk (pink), nt (tent), pt (kept), sk (task), st (best), xt (next) |

### Phase 5 (Alternative Spellings, Split Digraphs, Alternative Pronunciations)
| Group | Graphemes |
|-------|-----------|
| New spellings | ay (play), ou (cloud), ie (pie), ea (each), oy (toy), ir (bird), ue (blue), aw (saw), wh (when), ph (phone), ew (new), oe (toe), au (August) |
| Split digraphs | a_e (cake), e_e (these), i_e (time), o_e (home), u_e (tube) |
| Alt. pronunciations | ow: snow/cow, ie: pie/field, ea: eat/bread, ou: cloud/shoulder/could, ey: key/they, a: cat/acorn, e: bed/me, i: bin/tiger, o: hot/go, u: up/unicorn, y: yes/happy/fly, ch: chin/school/chef |
| Silent letters | wr (write), kn (knee), gn (gnome), mb (lamb) |

## Success Metrics

- Parent/teacher can navigate to a specific phase and show a flashcard in under 3 taps from home
- All Letters and Sounds Phases 2-5 GPCs are accurately represented
- Progress saves reliably across sessions
- Flashcard text is readable from arm's length (suitable for showing to a child)
- Visual design is consistent with the rest of the app (calm, SEN-friendly)

## Open Questions

- Should Phase 6 (assessment/review) week in Phase 2 be skipped or shown as a milestone card?
- For Phase 5 alternative pronunciations, should each alternative be a separate card or shown together on one card?
- Should there be a "phase complete" animation/reward beyond the celebration message?
- Future: when adding audio (tap-to-hear), should we use pre-recorded phoneme audio or text-to-speech?
- Future: should the interactive mode (US story 1B) be a separate mode toggle or a different section?
