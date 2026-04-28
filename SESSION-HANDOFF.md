# CueBoard Session Handoff — for fresh chat

> Tyler is finishing the CueBoard hackathon submission. Submission is due **3 AM 2026-04-28**. He has limited time, is tired, and needs help with finish-line tasks (recording, editing, uploading).

## What CueBoard is

C# .NET 8 plugin for the **Logitech MX Creative Console**, built for the Logitech Actions SDK hackathon (top 50 of 1300, going for top 6, finals June 10-11 in Lausanne if selected). Turns the device into a physical control surface for **Zoom Workplace** virtual meetings on Windows 11.

**Hardware:** Keypad (9 buttons + small dial × 3 pages) + Dialpad (large center dial + Show Actions Ring button) + Action Ring (radial 8-slot pop-up menu).

**One-line pitch:** "The first physical control surface purpose-built for virtual meetings."

## Current state — code complete, demo not yet recorded

Everything below is **already built, committed, pushed to main, deployed**:

### Page 1 — Live Controls (working)
Mute · Camera · Record · Screen Share · Open Chat · Reaction (clap) · Raise Hand · View Toggle · End Meeting. Plus a cosmetic Reaction Dial that doesn't reliably rotate — DON'T use it in the demo.

### Page 2 — Operator Mode (working, 7 used + 2 empty slots)
Mute All · Fullscreen · Pause Share · Minimize (Win+Down — Zoom drops to floating thumbnail) · Timer · Copy Invite · Clear Timer. Plus a cosmetic Timer Dial that doesn't reliably rotate — DON'T use it in demo.

### Page 3 — Meeting Intelligence (working)
Flag Moment · Engage Setup · Add Note · Clear Last · Highlight · Interactive Poll · Reset Meeting · Load Transcript · Export Summary. Plus the **Flag Type dial — this DOES work and is the showpiece feature.**

### Action Ring — Tyler's setup
Direct one-tap colored flags: Flag Moment · Flag Decision (blue) · Flag Follow-Up (green) · Action Item (purple) · Highlight (gold) — plus a few Page 1 favorites (Record, Open Chat, End Meeting).

### Engage page (hosted)
**URL: `https://tkhatton.github.io/CueBoard/engage.html`** — single HTML in `docs/` folder, GitHub Pages deploys from `main:/docs`. Has setup mode (form for word cloud prompt + poll questions + rating + feedback email) and play mode (selected via URL fragment with base64-encoded config). Setup saves to localStorage; subsequent presses of Interactive Poll button on CueBoard auto-redirect to play mode and copy the share URL to clipboard.

### Transcript pipeline
CueBoard's `TranscriptService` parses both `.vtt` (WebVTT) and Zoom Workplace's `meeting_saved_closed_caption.txt` plain-text format. Auto-detects from `OneDrive\Documents\Zoom`, `Documents\Zoom`, Desktop, Downloads (24h window). Manual override via Load Transcript button. Tyler's saved transcript file lands at `C:\Users\ltken\OneDrive\Documents\Zoom\[date-time meeting name]\meeting_saved_closed_caption.txt`. The Export Summary HTML shows clickable timestamp pills that scroll the transcript panel to the matching line.

### Flag overlay
When any flag fires, a **big corner card pops** (top-right, 360×96, color-coded by type, with flag emoji). Visible on screen share. Auto-fades.

## What was DELETED — DO NOT rebuild

These were attempted across the build and removed because Zoom genuinely doesn't expose the shortcut, OR the feature didn't add value. **If Tyler asks to add them back, push back firmly.**

| Deleted | Why |
|---|---|
| LockMeetingCommand | Alt+L doesn't fire reliably in Tyler's Zoom Workplace 2026 even with Zoom focused. |
| RenameCommand | Same — Alt+N not honored. |
| SpotlightSpeaker, AdmitFromWaitingRoom, RemoveParticipant, HostTransfer, BreakoutRooms, Whiteboard, VirtualBackground, Pin, Polls (Zoom-side), Captions | Zoom has no global shortcut for any of these. |
| MeetingTimerCommand (Page 3) | Was just an icon — never tracked elapsed meeting time. |
| EmailSummaryCommand (Page 3) | Redundant with Export Summary's built-in Email button. |
| PreviewSummaryCommand (Page 3) | Output was on the small keypad LCD — Tyler couldn't see it. |
| Demo-fallback fake transcript | Showed invented speaker names — misleading. Replaced with honest "no transcript loaded" banner. |

## Important quirks Tyler discovered while testing

- **Mute All** (Alt+M) intentionally doesn't mute the host. Host uses Page 1 Mute (Alt+A). This is Zoom's design, not a bug.
- **Pause Share** only works while you're actively screen-sharing.
- **Copy Invite** opens Zoom's invite dialog where you click "Copy invite link" — clunky but it's Zoom's behavior, no shortcut to skip.
- **Minimize** triggers Zoom's floating thumbnail mode (Win+Down → Zoom shows mini-window). Other windows minimize too — Tyler keeps things closed during demo.
- **Page 1 Reaction Dial and Page 2 Timer Dial don't reliably rotate.** Page 3 Flag Type Dial DOES work. Demo only uses the working one.

## Files Tyler will reference

- **`SPEC.md`** — locked specification of all 3 pages, Action Ring, what's NOT on device + why
- **`DEMO-SCRIPT.md`** — 3-minute beat sheet with Fathom recording strategy
- **`KNOWN-ISSUES.md`** — has stale info, ignore unless asked
- **`docs/engage.html`** — the hosted engage page (deployed via GitHub Pages)
- **`src/CueBoardPlugin/src/`** — all C# source
- **Memory files at `C:\Users\ltken\.claude\projects\C--Users-ltken-OneDrive-Documents-GitHub-CueBoard\memory\`** — `cueboard_zoom_dead_ends.md`, `cueboard_build_patterns.md`, `cueboard_hardware_layout.md`

## Recent commits on `main`

- `56a79a6` Replace outdated 3-MINUTE-DEMO-SCRIPT with refined DEMO-SCRIPT
- `3c9efd3` Min/Max button now reads MINIMIZE on device
- `c9ac318` Lock SPEC, write 3-min demo script, switch Engage to GitHub Pages URL
- `ab14f6f` Hackathon polish: drop dead Zoom shortcuts, add flag burst overlay, hosted Engage

Note: Tyler is on worktree branch `claude/compassionate-ride-42a69f` but it pushes directly to `main`. Don't try to switch branches in the worktree.

## Build / deploy

- `dotnet build src/CueBoardPlugin/src/CueBoardPlugin.csproj` builds the plugin AND auto-reloads it into Logi Options+ via a post-build script. Should produce 0 warnings, 0 errors.
- Pushing `docs/engage.html` to main → GitHub Pages rebuilds within 1-2 min.

## What Tyler still has to do (his side, not yours)

1. ⏳ Open `https://tkhatton.github.io/CueBoard/engage.html` in browser, fill in setup form (word cloud prompt, 3 poll questions, rating prompt, feedback email = ltkenney13@gmail.com), click Generate & Copy Share Link. This populates localStorage so Interactive Poll works.
2. ⏳ In Logi Options+: clear caution-triangle slots on Page 2 (right-click → Remove). Confirmed-final layout in SPEC.md.
3. ⏳ Test framing: tilt laptop down so webcam captures face + device on desk.
4. ⏳ Record demo on Fathom (in Zoom call, narrate + press buttons live, screen-share for Export and Engage).
5. ⏳ Trim ends, upload, submit.

## What he might ask you for help with in the new chat

- A small bug or last-minute behavior fix (build is one command — see above)
- Tweaks to the demo script wording
- A different camera angle / framing strategy
- A revision to the engage page (in `docs/engage.html`)
- A "what would I say if a judge asks X?" prep
- Last-minute SPEC.md cleanup before submission

## Tone for Tyler

Tyler is exhausted. Be direct, prioritize ruthlessly, no over-explaining. If he proposes something out of scope this close to deadline, push back. He values: clear answers, short responses, no fluff. He's said "I hate video editing" — recommend zero-edit approaches. He prefers practical metaphors over jargon.

## One-line summary for fast pickup

> CueBoard is code-complete and pushed. Tyler needs help finishing the recording/edit/submit pipeline by 3 AM 4/28/2026. The script is in `DEMO-SCRIPT.md`, the layouts are locked in `SPEC.md`, GitHub Pages serves the engage URL at https://tkhatton.github.io/CueBoard/engage.html. Don't suggest rebuilding deleted features (see "What was DELETED").
