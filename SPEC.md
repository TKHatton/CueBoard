# CueBoard — Locked Specification

> **Status:** LOCKED for hackathon submission
> **Last Updated:** 2026-04-27
> **Hardware:** Logitech MX Creative Console (Keypad + Dialpad + Action Ring)
> **Meeting Platform:** Zoom Workplace (Windows 11)

---

## Hardware Surfaces Used

CueBoard ships with three input surfaces:

1. **Keypad** — 9 buttons + 1 small dial, organized into 3 pages
2. **Dialpad** — large center dial + Show Actions Ring button (bottom-right)
3. **Action Ring** — radial pop-up of 8 slots, each assigned a CueBoard action

---

## PAGE 1 — Live Controls

**Purpose:** Fast, obvious meeting control. No thinking required.
**For:** Anyone in a meeting.

| # | Button | Action | Zoom Shortcut |
|---|--------|--------|---------------|
| 1 | Mute / Unmute | Toggle Zoom microphone | Alt+A |
| 2 | Camera On/Off | Toggle Zoom video | Alt+V |
| 3 | Record | Start/Stop Zoom recording | Alt+R |
| 4 | Screen Share | Start/stop share | Alt+S |
| 5 | Open Chat | Open chat panel | Alt+H |
| 6 | Reaction | Send a clap reaction | Alt+T |
| 7 | Raise Hand | Raise/lower hand | Alt+Y |
| 8 | View Toggle | Gallery ↔ Speaker | — |
| 9 | End Meeting | End meeting for all | Alt+Q |

**Dial — Reaction Selector:** Present but cosmetic. Rotation is not reliably wired in this version. Demo uses the clap button directly.

---

## PAGE 2 — Operator Mode

**Purpose:** Host power tools.
**For:** Hosts, facilitators, workshop leaders.

| # | Button | Action | Zoom Shortcut |
|---|--------|--------|---------------|
| 1 | Mute All | Mute all participants (host only) | Alt+M |
| 2 | Fullscreen | Toggle Zoom fullscreen | Alt+F |
| 3 | Pause Share | Pause/resume active screen share | Alt+T |
| 4 | Minimize | Minimize active window — Zoom shows its floating thumbnail | Win+Down |
| 5 | Timer | Start / pause countdown timer (with overlay on screen) | — (internal) |
| 6 | Copy Invite | Open Zoom's invite dialog | Alt+I |
| 7 | Clear Timer | Reset the timer overlay | — (internal) |

*Two slots (8, 9) intentionally left empty.*

**Dial — Timer Duration:** Present but cosmetic. Timer uses its default duration (5 minutes); demo presses the Timer button directly.

---

## PAGE 3 — Meeting Intelligence

**Purpose:** Capture the moments that matter and turn them into a shareable summary.
**For:** Note-takers, project managers, team leads.

| # | Button | Action |
|---|--------|--------|
| 1 | Flag Moment | Flag the current moment with the type selected on the Flag Type dial |
| 2 | Engage Setup | Pre-meeting: open the hosted engage page in setup mode |
| 3 | Add Note | PowerShell input dialog — type a note attached to the last flag |
| 4 | Clear Last | Remove the most recent flag |
| 5 | Highlight | Mark a "this matters" moment without typing |
| 6 | Interactive Poll | Open the live engage page (play mode) — share link auto-copied to clipboard |
| 7 | Reset Meeting | Clear all flags + restart timer (3-second double-press confirm) |
| 8 | Load Transcript | File picker for a Zoom .vtt or `meeting_saved_closed_caption.txt` |
| 9 | Export Summary | Branded HTML report — flag breakdown, transcript-linked moments, email/print/download buttons |

**Dial — Flag Type Selector:** ⭐ The showpiece. Rotate cycles through Action Item / Decision / Follow-Up / Bookmark. Press confirms. After the dial selects, every Flag Moment press records that type.

---

## ACTION RING

8 radial slots, opened by pressing **Show Actions Ring** on the Dialpad. Slot order is configurable in Logi Options+. Recommended population:

- Record · Open Chat · Engage Setup · End Meeting
- Flag Moment · Flag Decision · Flag Follow-Up · Action Item

Rationale: action ring duplicates Page 1 favorites for one-press access during demo, and gives every flag color a direct shortcut without needing the dial.

---

## What's intentionally NOT on the device

These were considered, attempted, or built — and removed/excluded for clear reasons:

| Feature | Why it's gone |
|---------|--------------|
| Lock Meeting (Alt+L) | Doesn't fire reliably in Zoom Workplace 2026, even with Zoom focused. |
| Rename (Alt+N) | Same — Alt+N not honored on this Zoom version. |
| Spotlight Speaker | Zoom has no global keyboard shortcut for it. |
| Admit from Waiting Room | No global shortcut for the actual admit (only opens the panel). |
| Host Transfer | No shortcut, would need fragile UI automation. |
| Breakout Rooms | Alt+B unreliable. |
| Whiteboard / Annotation / Virtual Background / Pin / Polls (Zoom-side) | Zoom exposes no global shortcuts for any of these. |
| Meeting Timer (elapsed-time button) | Was decorative — never tracked actual elapsed time. |
| Email Summary (standalone) | Redundant — Export Summary's HTML has a built-in Email button. |
| Preview Summary (count breakdown on keypad LCD) | Output was hard to read on the small device screen. |

---

## Services and How They Work

- **KeyboardService** — Win32 `keybd_event` for keystrokes; finds the Zoom main window for focus-first sends.
- **TimerService** + **TimerOverlayService** — countdown timer with PowerShell topmost overlay (visible during desktop screen share).
- **FlagService** + **SessionState** — in-memory flag log, dial-selected type, demo data loader.
- **InputDialogService** — PowerShell dark-theme input box for notes; auto-focused on open.
- **ToastService** — small corner toast for status, BIG flag-style overlay for flag events (color-coded by type, visible during screen share).
- **TranscriptService** — parses both WebVTT (.vtt) and Zoom Workplace's `meeting_saved_closed_caption.txt` format. Auto-detects from `OneDrive\Documents\Zoom`, `Documents\Zoom`, Desktop, Downloads (24-hour window). Manual override via Load Transcript.
- **ExportService** — generates the branded HTML report with flag-to-transcript anchor links, print/email/download buttons, and an honest "no transcript" banner when none is found.
- **Engage page (GitHub Pages)** — `https://tkhatton.github.io/CueBoard/engage.html` hosts the shareable engagement page (word cloud, polls, ratings, feedback). Setup mode and play mode are the same file, switched by URL fragment.
