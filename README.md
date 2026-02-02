# NowThenNext

A visual schedule and choice board app designed for children with Special Educational Needs (SEN).

**[Try it now at devjonny.github.io/NowThenNext](https://devjonny.github.io/NowThenNext/)**

---

## Purpose

NowThenNext helps carers communicate daily plans and offer choices to children using clear, familiar pictures. The app uses a calm, distraction-free design with soft colors and large touch targets suitable for tablet use.

## Features

### Visual Schedule (Now → Then → Next)
Upload pictures of places and activities, then select up to three to display as a visual schedule showing what's happening now, what comes next, and what follows after that. The current activity is gently highlighted to help the child focus.

### Food Choices
Upload pictures of food items and present 2-6 options for the child to choose from. When they tap their choice, a confirmation screen shows their selection.

### Activity Choices
Similar to food choices - upload activity pictures and let the child select from multiple options.

### Image Libraries
- **Places** - Locations and activities for schedules
- **Food** - Food items for meal choices
- **Activities** - Activities for choice boards
- **Favorites** - Quick access to frequently used images across all categories

### Backup & Restore
Export all images and settings to a JSON file for safekeeping, and restore from backup on any device.

### Install as App (PWA)
Install NowThenNext to your device's home screen for a full-screen, app-like experience. The app works offline after installation and automatically updates when new versions are available.

## Design Principles

- **Calm colors** - Soft teal, muted green, warm off-white (no harsh reds or bright yellows)
- **Large touch targets** - Minimum 48px for easy tapping
- **Minimal distractions** - Clean layouts with generous spacing
- **Accessible** - WCAG AA contrast compliance

## Technology

Built with Blazor WebAssembly and Tailwind CSS. All data is stored locally in the browser using localStorage - no account or internet connection required after initial load.

## License

This project is licensed under the [PolyForm Noncommercial License 1.0.0](LICENSE).

**You are free to:**
- Use the app for personal, educational, or charitable purposes
- Modify and share the code
- Contribute improvements back to the project

**You may not:**
- Use this software for commercial purposes
- Sell or monetize the software or derivatives

This ensures NowThenNext remains a free resource for families and carers of children with SEN.
