# CueBoard Session Log

## Session 2 — March 22, 2026 (Evening)

### Hardware Setup
- Unboxed Logitech MX Creative Console (dialpad + keypad)
- Dialpad connected via Bluetooth — working
- Keypad required the included USB-C to USB-C cable specifically (other cables didn't work)
- Keypad lights up and displays LCD buttons when connected
- Installed Logi Options+ and created account
- Downloaded Zoom Workplace desktop app (needed for global keyboard shortcuts)

### Plugin Deployment
- CueBoard plugin built and deployed to Logi Options+ plugin directory
- Plugin appears in Logi Options+ under "CueBoard - Meeting Control Surface Actions"
- Three action groups visible: Live Controls, Operator Mode, Meeting Intelligence
- All 27 button actions available for assignment to keypad
- CueBoard actions also appear on dialpad for assignment to corner buttons

### What's Working (Tested in Live Zoom Call)
- ✅ Mute/Unmute — sends Alt+A, works perfectly
- ✅ Camera On/Off — sends Alt+V, works
- ✅ Record — sends Alt+R, works
- ✅ Screen Share — sends Alt+S, works
- ✅ Open Chat — sends Alt+H, very convenient
- ✅ Raise Hand — sends Alt+Y, toggle works (press once up, press again down)
- ✅ Reaction (Clap) — sends Alt+T then navigates, clapping shows on screen
- ✅ View Toggle — switches between speaker/gallery
- ✅ Timer — countdown appears on keypad LCD AND floating overlay on screen
- ✅ Timer overlay — borderless, draggable, has close X button, numbers visible
- ✅ Button state toggles — icons change (e.g., MUTED vs MIC ON) on keypad LCD
- ✅ Reactions working again after initial issue
- ✅ Keypad stays active during Zoom calls (fixed app-specific profile conflict)

### What's Not Working Yet
- ❌ Dial rotation — doesn't respond when turning the big dial for timer/reactions
- ❌ Page 2 buttons — host-only features (Mute All, Spotlight, etc.) untested/not firing
- ❌ Page 3 buttons — Meeting Intelligence buttons toggle state but no screen UI for notes/assign
- ❌ Action Ring integration — Zoom Application Profile not yet created
- ❌ Dialpad default profile — got overwritten, need to restore Logi defaults

### Features Built This Session
1. **Branded HTML Export** — Export Summary now generates beautiful dark-theme HTML report
   - Auto-opens in browser
   - Color-coded sections (Action Items, Decisions, Follow-Ups, Bookmarks)
   - Stat cards showing totals
   - CueBoard branding throughout
   - Saves to Documents\CueBoard\

2. **Timer Overlay** — Floating on-screen countdown
   - Borderless dark design
   - Draggable window
   - Close X button
   - Shows minutes:seconds countdown
   - Always-on-top so visible during screen share

3. **Icon Generator v2** — Redesigned icons
   - Bigger symbols filling full 80x80 space
   - No text inside icons (Logi Options+ adds labels below)
   - Color-coded by function
   - Still needs more polish (user wants Adobe-style large fill icons)

4. **Universal Plugin Mode** — Changed from Zoom-only to universal
   - Plugin now works on all keypad pages regardless of focused app
   - Prevents keypad going blank when switching to Zoom

### Key Design Decisions Made
- Mute (personal) moves to dialpad corner button — always accessible
- Mute All (host) stays on keypad Page 2
- Meeting Intelligence moves to Page 3 (last page)
- End Meeting stays on keypad, NOT Action Ring (too many steps via ring)
- Action Ring = quick-access toolbar for frequently needed cross-page actions
- Zoom Logitech plugin NOT installed — would conflict with CueBoard
- Icons need to be bigger, bolder, more like Adobe's app icons

### Dialpad Layout Plan
| Position | Assignment |
|----------|-----------|
| Top-left | Mute / Unmute |
| Top-right | Camera On/Off |
| Bottom-left | Back (page navigation) |
| Bottom-right | Show Actions Ring (keep default) |
| Big dial | Reaction Selector (rotate to pick, press to send) |
| Roller | Volume (keep default) |

### Action Ring Plan (8 slots)
| Slot | Action |
|------|--------|
| 1 | Screen Share |
| 2 | Record |
| 3 | Open Chat |
| 4 | Participants |
| 5 | Reactions / Emoji |
| 6 | Flag Moment |
| 7 | Timer |
| 8 | (open — was End Meeting, removed per feedback) |

### Competitor Analysis
- Discovered Logitech has an official Zoom plugin in the marketplace
- Their plugin: basic controls (mute, video, chat, record, share, reactions, hand, view, cloud recording, blur background, annotations)
- All blue icons, clean but undifferentiated
- CueBoard advantages: Timer, Meeting Intelligence (flagging/export), Operator Mode, color-coded icons, multi-page workflow
- Plan to also compare with MS Teams plugin later

### User Feedback Captured
- Timer must show on screen (not just keypad) — DONE
- Icons too small, need to fill entire button like Adobe icons — IN PROGRESS
- Dial needs to be actively useful, not decorative
- Action Ring should be quick-access, not duplicate keypad
- Page 3 needs actual screen UI for notes/assign (currently just button toggles)
- Export should be branded and beautiful — DONE (HTML)
- Want quick-launch buttons for Zoom and Google Meet
- Dyslexic user needs large visual icons, not tiny text
- "Close to excellent" — core works, needs polish and dial integration

---

## Session 1 — Initial Build (Prior Session)
- Scaffolded entire CueBoard plugin project
- Created all 27 button action classes across 3 pages
- Created 3 dial adjustment classes
- Built services: KeyboardService, TimerService, FlagService, ExportService, SessionState
- Generated initial icon set with IconGenerator tool
- Created project docs: SPEC, DECISIONS, RESEARCH, ROADMAP, PITCH, DEMO-SCRIPT
- First successful build and deployment to Logi Options+
