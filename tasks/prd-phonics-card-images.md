# PRD: Phonics Card Images

## Introduction

Phonics flashcards currently show only the grapheme (letter/digraph), example word, and a keyword hint. Adding an illustration of the example word gives children a visual association that reinforces the sound-to-meaning connection. Tapping a card will flip between the grapheme view and the image view, creating an interactive learning moment.

## Goals

- Add a built-in illustration for phonics cards with concrete, illustratable example words (skip abstract words like "it", "on", "up", "got", "self", "best", "these", "could", etc.)
- Introduce a tap-to-flip interaction: grapheme+word on one side, image+word on the other
- Ship all images as static assets with the app (no user upload needed)
- Maintain the existing "Got it!" button and navigation interactions

## User Stories

### US-065: Add ImagePath field to GraphemeCard model
**Description:** As a developer, I need the data model to support an image path per phonics card so illustrations can be wired up.

**Acceptance Criteria:**
- [ ] Add `ImagePath` property (string, nullable) to `GraphemeCard` in `Models/PhonicsModels.cs`
- [ ] Typecheck passes

### US-066: Add tap-to-flip interaction on PhonicsCard page
**Description:** As a user, I want to tap a phonics card to flip between the grapheme view and an image of the example word so I can build visual associations.

**Acceptance Criteria:**
- [ ] Tapping the card area (grapheme + word section) flips to show the image + word
- [ ] Tapping again flips back to the grapheme view
- [ ] Flip uses a gentle CSS transition (~0.4s, matching learning cards style)
- [ ] The example word remains visible on both sides
- [ ] If no image is set (`ImagePath` is null), tapping does nothing / card does not flip
- [ ] The "Got it!" button, previous/next arrows, and back button are NOT part of the flippable area
- [ ] Flip state resets when navigating to a different card
- [ ] Uses phonics colour theme (calm-phonics #7BA3C4)
- [ ] Typecheck passes
- [ ] Verify in browser using dev-browser skill

### US-067: Add Phase 2 phonics images and wire up paths
**Description:** As a developer, I need to add illustration PNGs for all Phase 2 cards and set their ImagePath in PhonicsDataService.

**Acceptance Criteria:**
- [ ] PNG files added to `wwwroot/images/phonics/` for Phase 2 cards with concrete example words, named by card ID (e.g., `p2-w1-s.png`)
- [ ] Each image clearly represents the example word (sun, ant, tap, pin, etc.)
- [ ] Cards with abstract/hard-to-illustrate words (e.g., "it", "on", "up", "got", "yes") are skipped — no ImagePath set, card does not flip
- [ ] All illustrated Phase 2 cards in `PhonicsDataService.cs` have `ImagePath` set to `images/phonics/{id}.png`
- [ ] Images render correctly when tapping a Phase 2 card to flip
- [ ] Typecheck passes
- [ ] Verify in browser using dev-browser skill

### US-068: Add Phase 3 phonics images and wire up paths
**Description:** As a developer, I need to add illustration PNGs for all Phase 3 cards and set their ImagePath in PhonicsDataService.

**Acceptance Criteria:**
- [ ] PNG files added to `wwwroot/images/phonics/` for Phase 3 cards with concrete example words, named by card ID
- [ ] Each image clearly represents the example word (rain, feet, night, boat, moon, book, car, fork, nurse, cow, coin, dear, fair, letter)
- [ ] Cards with abstract words are skipped — no ImagePath set
- [ ] All illustrated Phase 3 cards in `PhonicsDataService.cs` have `ImagePath` set
- [ ] Images render correctly when tapping a Phase 3 card to flip
- [ ] Typecheck passes
- [ ] Verify in browser using dev-browser skill

### US-069: Add Phase 4 phonics images and wire up paths
**Description:** As a developer, I need to add illustration PNGs for all Phase 4 cards and set their ImagePath in PhonicsDataService.

**Acceptance Criteria:**
- [ ] PNG files added to `wwwroot/images/phonics/` for Phase 4 cards with concrete example words, named by card ID
- [ ] Each image clearly represents the example word (crab, flag, frog, lamp, tent, sand, etc.)
- [ ] Cards with abstract words (e.g., "bring", "glad", "grip", "plan", "press", "self", "best", "kept", "task", "next") are skipped — no ImagePath set
- [ ] All illustrated Phase 4 cards in `PhonicsDataService.cs` have `ImagePath` set
- [ ] Images render correctly when tapping a Phase 4 card to flip
- [ ] Typecheck passes
- [ ] Verify in browser using dev-browser skill

### US-070: Add Phase 5 phonics images and wire up paths
**Description:** As a developer, I need to add illustration PNGs for all Phase 5 cards and set their ImagePath in PhonicsDataService.

**Acceptance Criteria:**
- [ ] PNG files added to `wwwroot/images/phonics/` for Phase 5 cards with concrete example words, named by card ID
- [ ] Each image clearly represents the example word (play, cloud, pie, toy, bird, cake, snow, etc.)
- [ ] Cards with abstract words (e.g., "each", "when", "new", "these", "could", "they", "me", "go") are skipped — no ImagePath set
- [ ] All illustrated Phase 5 cards in `PhonicsDataService.cs` have `ImagePath` set
- [ ] Images render correctly when tapping a Phase 5 card to flip
- [ ] Typecheck passes
- [ ] Verify in browser using dev-browser skill

### US-071: E2E test for phonics card flip interaction
**Description:** As a developer, I need E2E tests to verify the tap-to-flip interaction works correctly.

**Acceptance Criteria:**
- [ ] Test: tapping a phonics card flips to show the image
- [ ] Test: tapping again flips back to show the grapheme
- [ ] Test: navigating to next card resets flip state
- [ ] Test: "Got it!" button still works after flipping
- [ ] All tests pass
- [ ] Typecheck passes

## Functional Requirements

- FR-1: Add nullable `ImagePath` property to `GraphemeCard` model
- FR-2: When a user taps the phonics card body, toggle between grapheme view and image view
- FR-3: The image view shows the illustration filling the card area with the example word below
- FR-4: The grapheme view shows the existing layout (large grapheme, highlighted word, keyword hint)
- FR-5: Flip transition uses a gentle animation (~0.4s ease)
- FR-6: Flip state resets when route parameters change (navigating between cards)
- FR-7: Cards without an `ImagePath` do not flip (no visual change on tap)
- FR-8: All cards with concrete, illustratable example words have illustrations shipped as PNG files in `wwwroot/images/phonics/`; cards with abstract words have no ImagePath and do not flip
- FR-9: Image filenames match the card ID pattern (e.g., `p2-w1-s.png`, `p3-w2-oo_long.png`)

## Non-Goals

- No user-uploaded phonics images
- No animation beyond a simple flip/fade transition
- No audio playback of the sound
- No changes to the "Got it!" / progress tracking system
- No changes to the sound list page or phase selection page

## Design Considerations

- Flip interaction should feel similar to the learning cards tap-to-reveal pattern
- Image should fill the card area while maintaining aspect ratio (`object-fit: contain`)
- Use the existing phonics colour theme (calm-phonics #7BA3C4) for any new UI elements
- Images should be clear and recognisable at the card size (~200-300px)
- Keep images under 50KB each where possible to avoid bloating the app

## Technical Considerations

- Images stored as static assets in `wwwroot/images/phonics/` — served directly by Blazor WASM
- Image path rendering uses the same pattern as dinosaur cards in `LearningCardGrid.razor` (relative path in `<img src>`)
- The `PhonicsCard.razor` page uses `OnParametersSetAsync` for route changes — flip state must reset there
- Phase 4 has abstract concepts (consonant clusters like "bl", "cr") — images depict the example word (e.g., "black" → black colour/object, "crab" → crab illustration)
- Image naming uses card IDs which may contain underscores for duplicates (e.g., `p3-w2-oo_long.png`, `p5-w3-ch_chef.png`)

## Image Manifest

All 118 images needed, named `{card-id}.png`:

### Phase 2 (35 images)
| File | Example Word |
|------|-------------|
| p2-w1-s.png | sun |
| p2-w1-a.png | ant |
| p2-w1-t.png | tap |
| p2-w1-p.png | pin |
| p2-w2-i.png | it |
| p2-w2-n.png | net |
| p2-w2-m.png | mat |
| p2-w2-d.png | dog |
| p2-w3-g.png | got |
| p2-w3-o.png | on |
| p2-w3-c.png | cat |
| p2-w3-k.png | kid |
| p2-w4-ck.png | duck |
| p2-w4-e.png | egg |
| p2-w4-u.png | up |
| p2-w4-r.png | red |
| p2-w5-h.png | hat |
| p2-w5-b.png | bat |
| p2-w5-f.png | fan |
| p2-w5-ff.png | puff |
| p2-w5-l.png | leg |
| p2-w5-ll.png | bell |
| p2-w5-ss.png | hiss |
| p2-w7-v.png | van |
| p2-w7-w.png | win |
| p2-w7-x.png | fox |
| p2-w7-y.png | yes |
| p2-w8-z.png | zip |
| p2-w8-zz.png | buzz |
| p2-w8-qu.png | queen |
| p2-w8-ch.png | chip |
| p2-w9-sh.png | shop |
| p2-w9-th.png | thin |
| p2-w9-ng.png | ring |
| p2-w9-nk.png | pink |

### Phase 3 (14 images)
| File | Example Word |
|------|-------------|
| p3-w1-ai.png | rain |
| p3-w1-ee.png | feet |
| p3-w1-igh.png | night |
| p3-w1-oa.png | boat |
| p3-w2-oo_long.png | moon |
| p3-w2-oo_short.png | book |
| p3-w2-ar.png | car |
| p3-w2-or.png | fork |
| p3-w3-ur.png | nurse |
| p3-w3-ow.png | cow |
| p3-w3-oi.png | coin |
| p3-w3-ear.png | dear |
| p3-w4-air.png | fair |
| p3-w4-er.png | letter |

### Phase 4 (34 images)
| File | Example Word |
|------|-------------|
| p4-w1-bl.png | black |
| p4-w1-br.png | bring |
| p4-w1-cl.png | clap |
| p4-w1-cr.png | crab |
| p4-w1-dr.png | drop |
| p4-w1-fl.png | flag |
| p4-w1-fr.png | frog |
| p4-w1-gl.png | glad |
| p4-w1-gr.png | grip |
| p4-w1-pl.png | plan |
| p4-w1-pr.png | press |
| p4-w1-sc.png | scam |
| p4-w1-sk.png | skip |
| p4-w1-sl.png | slip |
| p4-w1-sm.png | smell |
| p4-w1-sn.png | snap |
| p4-w1-sp.png | spot |
| p4-w1-st.png | stop |
| p4-w1-sw.png | swim |
| p4-w1-tr.png | trip |
| p4-w1-tw.png | twin |
| p4-w2-ft.png | left |
| p4-w2-lf.png | self |
| p4-w2-lp.png | help |
| p4-w2-lt.png | felt |
| p4-w2-mp.png | lamp |
| p4-w2-nd.png | sand |
| p4-w2-nk.png | pink |
| p4-w2-nt.png | tent |
| p4-w2-pt.png | kept |
| p4-w2-sk.png | task |
| p4-w2-st.png | best |
| p4-w2-xt.png | next |

### Phase 5 (35 images)
| File | Example Word |
|------|-------------|
| p5-w1-ay.png | play |
| p5-w1-ou.png | cloud |
| p5-w1-ie.png | pie |
| p5-w1-ea.png | each |
| p5-w1-oy.png | toy |
| p5-w1-ir.png | bird |
| p5-w1-ue.png | blue |
| p5-w1-aw.png | saw |
| p5-w1-wh.png | when |
| p5-w1-ph.png | phone |
| p5-w1-ew.png | new |
| p5-w1-oe.png | toe |
| p5-w1-au.png | August |
| p5-w2-a_e.png | cake |
| p5-w2-e_e.png | these |
| p5-w2-i_e.png | time |
| p5-w2-o_e.png | home |
| p5-w2-u_e.png | tube |
| p5-w3-ow_snow.png | snow |
| p5-w3-ow_cow.png | cow |
| p5-w3-ie_pie.png | pie |
| p5-w3-ie_field.png | field |
| p5-w3-ea_eat.png | eat |
| p5-w3-ea_bread.png | bread |
| p5-w3-ou_cloud.png | cloud |
| p5-w3-ou_shoulder.png | shoulder |
| p5-w3-ou_could.png | could |
| p5-w3-ey_key.png | key |
| p5-w3-ey_they.png | they |
| p5-w3-a_acorn.png | acorn |
| p5-w3-e_me.png | me |
| p5-w3-i_tiger.png | tiger |
| p5-w3-o_go.png | go |
| p5-w3-u_unicorn.png | unicorn |
| p5-w3-y_happy.png | happy |
| p5-w3-y_fly.png | fly |
| p5-w3-ch_school.png | school |
| p5-w3-ch_chef.png | chef |
| p5-w4-wr.png | write |
| p5-w4-kn.png | knee |
| p5-w4-gn.png | gnome |
| p5-w4-mb.png | lamb |

## Success Metrics

- All 118 phonics cards have an illustration that clearly represents the example word
- Tap-to-flip interaction feels responsive and smooth
- No regression in existing phonics E2E tests
- No noticeable performance impact from loading images

## Open Questions

- Image sourcing: will images be AI-generated, hand-drawn, or sourced from CC-licensed libraries (like the dinosaur DBCLS images)?
- In future, abstract words could be swapped to more concrete alternatives (e.g., "it" → "igloo", "up" → "umbrella") to enable illustrations for all cards
