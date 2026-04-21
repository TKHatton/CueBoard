# CueBoard — Button & Dial Specification

> **Status:** DRAFT — Still being refined. Page 3 is actively being rethought.
> **Last Updated:** 2026-03-16

## Overview
- 3 pages, 9 buttons each, 1 dial per page
- Priority tiers: **MUST** (demo-critical), **NICE** (if time allows), **SIMULATED** (fake it convincingly)
- Only Zoom is fully implemented

---

## PAGE 1: Live Controls
**Purpose:** Fast, obvious meeting control. No thinking required.
**Who it's for:** Everyone in a meeting.

| # | Button | Action | Zoom Shortcut | Priority |
|---|--------|--------|---------------|----------|
| 1 | Mute / Unmute | Toggle mute, show ON/OFF state | Alt+A | MUST |
| 2 | Camera On/Off | Toggle video, visual indicator | Alt+V | MUST |
| 3 | Start/Stop Recording | Toggle recording, red indicator when active | Alt+R | MUST |
| 4 | Screen Share | Start/stop share | Alt+S | MUST |
| 5 | Open Chat | Open chat panel | Alt+H | MUST |
| 6 | Reaction | Send reaction (dial selects which one) | — | NICE |
| 7 | Raise Hand | Raise/lower hand | Alt+Y | NICE |
| 8 | Gallery/Speaker View | Toggle between view modes | — | NICE |
| 9 | End Meeting for All | End meeting as host | Alt+Q | MUST |

**Dial:**
- Rotate → Cycle through reactions (thumbs up, clap, heart, laugh, cheer)
- Press → Send the currently selected reaction
- Alternate use: Scroll through gallery pages when in gallery view

**Feedback:**
- Mute button: Shows "Muted" or "Live"
- Camera button: Shows "Off" or "On"
- Recording: Red indicator when active

---

## PAGE 2: Operator Mode
**Purpose:** Host power tools. Authority and control.
**Who it's for:** Meeting hosts, facilitators, workshop leaders.

| # | Button | Action | Zoom Shortcut | Priority |
|---|--------|--------|---------------|----------|
| 1 | Mute All | Mute all participants | Alt+M | MUST |
| 2 | Lock Meeting | Lock/unlock meeting | Alt+L | MUST |
| 3 | Admit All | Admit all from waiting room (keyboard automation) | Alt+U, Tab, Enter | MUST |
| 4 | Fullscreen | Toggle Zoom fullscreen mode | Alt+F | MUST |
| 5 | Pause Share | Pause/resume screen sharing | Alt+T | MUST |
| 6 | Min/Max Window | Minimize or maximize Zoom window | Win+Down/Up | NICE |
| 7 | Start Timer | Start countdown timer (dial sets duration) | — (internal) | MUST |
| 8 | Copy Invite | Copy meeting invite link to clipboard | Alt+I | MUST |

**Dial:**
- Rotate → Adjust timer duration (minutes)
- Press → Start/pause timer (resumes from paused correctly)

**Feedback:**
- Timer: Shows countdown on button display + optional overlay
- Lock Meeting: Shows "Locked" or "Open"
- Fullscreen: Shows current state
- Pause Share: Shows "PAUSE SHARE" or "RESUME SHARE"

---

## PAGE 3: Meeting Intelligence
**Purpose:** Capture important moments in real-time. The "director's cut" of the meeting.
**Who it's for:** Note-takers, project managers, team leads.

> **STATUS: BEING RETHOUGHT.** The original design had too many similar buttons.
> Key insight: AI tools (Fathom, Fireflies) capture everything. CueBoard captures
> what YOU think matters. It's the human filter, not an AI replacement.

### Current Thinking (v2)

| # | Button | Action | Priority |
|---|--------|--------|----------|
| 1 | Flag Moment | Timestamp a key moment. Dial selects type BEFORE or AFTER pressing. | MUST |
| 2 | Assign | After flagging, attach to a person or department via dial | NICE |
| 3 | Add Note | Attach a text stub to the last flag | NICE |
| 4 | Highlight | Lighter than a flag — just marks timeline as "pay attention here" | NICE |
| 5 | Clear Last Flag | Undo — pops most recent flag | MUST |
| 6 | (Open) | TBD — candidates: Agenda Timer, Next Agenda Item, Share Summary to Chat | — |
| 7 | (Open) | TBD | — |
| 8 | Preview Summary | Shows flag count and breakdown on console screen | NICE |
| 9 | Export Summary | Generate Markdown summary file — BIG demo moment | MUST |

**Dial:**
- Default: Rotate to select flag type (Action Item / Decision / Follow-Up / Bookmark)
- After pressing Assign: Rotate through team members or departments
- Press: Confirm selection

**Feedback:**
- Flag counter on button (e.g., "Flags: 5")
- Export button: Shows "Saved!" briefly after export

### Original Design (v1 — for reference)
| # | Button | Action | Priority |
|---|--------|--------|----------|
| 1 | Action Item | Log with timestamp + counter | MUST |
| 2 | Decision | Log with timestamp + counter | MUST |
| 3 | Follow-Up | Log with timestamp | MUST |
| 4 | Bookmark | General marker with timestamp | MUST |
| 5 | Screenshot | Capture screen | NICE |
| 6 | Add Note | Quick note | NICE |
| 7 | View Flags | Cycle through flags | SIMULATED |
| 8 | Clear Last Flag | Undo | MUST |
| 9 | Export Summary | Generate Markdown | MUST |

---

## Dial Summary (All Pages)

| Page | Rotate | Press |
|------|--------|-------|
| 1 — Live Controls | Cycle reactions | Send selected reaction |
| 2 — Operator Mode | Adjust timer / cycle participants | Start-pause timer / confirm action |
| 3 — Meeting Intelligence | Select flag type / scroll team | Confirm selection |

---

## Minimum Viable Demo (If Time Gets Tight)
Only build these if everything else falls apart:
- **Page 1:** Mute, Camera, Record, Share
- **Page 2:** Mute All, Timer
- **Page 3:** Flag Moment, Clear Last, Export
