# CueBoard Session Log

## Session 4 — April 10, 2026 (Afternoon — Day 1 Critical Fixes)

### Context
- Hackathon deadline: April 22 (12 days out). Top 6 selected, winners go to Switzerland.
- Tyler currently in top 50 of 1,300 entries. Next round selects top 12.
- Tyler reported 4 problems: plugin deactivation in Zoom Workspace, dial not responding,
  reports/preview not working, button flickering.
- 5-day code-complete plan created and approved.

### What Got Fixed
- **DELETED CueBoardApplication.cs** — This was the root cause of plugin turning off.
  The file returned "Zoom" from GetProcessName(), telling the SDK to tie plugin activation
  to the Zoom process. Combined with HasNoApplication=true in the plugin class, this
  created a contradiction. When Zoom Workspace changed window focus, the SDK deactivated
  the plugin. Removing this file means the plugin is always active regardless of which
  app has focus.

- **Fixed button flickering (refresh storm)** — TimerTick was calling NotifyRefreshAllImages()
  every second, updating ALL 32 buttons + 3 dials. Now there are two separate events:
  - TimerDisplayChanged: fires every second, only StartTimerCommand and TimerDial subscribe
  - RefreshAllImages: fires on meaningful state changes (timer expired, etc.)
  New EnableTimerTickUpdates() method in base classes for opt-in per-second updates.

- **Enabled encoder in manifest** — pluginCapabilities was commented out in
  LoupedeckPackage.yaml. Added `pluginCapabilities: Encoder` to declare dial support.
  This may fix the dial not responding.

- **Added diagnostic logging** — CueBoardAdjustment logs its construction (name, group,
  hasReset) so we can confirm the SDK discovers all 3 dial handlers. Plugin logs
  Load/Unload lifecycle with timestamps.

- **Fixed ExportService** — Process.Start with UseShellExecute can fail silently from
  the Logi Plugin Service context. Added cmd.exe /c start fallback with explicit error
  logging so the HTML file always opens (or at minimum, the file path is logged).

### Build Status
- 0 warnings, 0 errors
- Auto-deployed via .link file
- Plugin reload command sent to Logi Plugin Service
- Committed and pushed to main on GitHub

### What Tyler Needs to Test
1. Does plugin stay active when switching between Zoom Workspace tabs and other apps?
2. In Logi Options+ dialpad settings, do CueBoard dial actions (Reaction Selector,
   Timer Duration, Flag Type) appear in the assignment dropdown for the big dial?
3. If they appear, assign one and test rotation.
4. Press Flag Moment a few times, then press Export. Does the HTML report open in browser?
5. Start a timer. Does the timer button update its countdown without all other buttons flickering?

### Next Session Priorities (Day 2)
1. Build PreviewOverlayService — PowerShell WinForms dark-theme window showing flag summary
2. Wire PreviewSummaryCommand to toggle the overlay on/off
3. Implement AddNoteCommand with PowerShell input dialog
4. Implement AssignCommand with preset names dialog
5. Fill Page 3 empty slots: "New Meeting" (reset) + "Share to Chat" (clipboard)
6. Iterate on dial fix based on Tyler's Day 1 test results

---

## Session 3 — March 23, 2026 (Evening — Continued Testing & Refinement)

### What Got Fixed / Added
- ✅ Timer toggle — now press once to start, press again to pause (not reset)
- ✅ Min/Max Window command added — sends Win+Up/Down for dialpad bottom-left
- ✅ Branded HTML export upgraded — dark theme, color-coded, auto-opens in browser
- ✅ Reaction simplified — back to one-press = clap (removed dial selector complexity)
- ✅ Dialpad defaults restored after accidentally overwriting them
- ✅ Keypad labeled correctly as CueBoard across all 3 pages
- ✅ Clear Timer replaced with Participants button on Page 2

### What's Still Not Working
- ❌ Dial rotation — still not responding, biggest remaining blocker
- ❌ Admit button — Zoom has no global shortcut for waiting room admission
- ❌ Page 3 screen UI — pressing Assign/Add Note needs on-screen input fields
- ❌ Page 3 empty slots — 4 buttons still unassigned on Page 3
- ❌ Icons still need Adobe-style redesign (large, bold, fill entire button)
- ❌ Logi Options+ froze at end of session — needs restart

### Layout Changes Made by Tyler
- Moved back/page-nav button to bottom-left of keypad
- Replaced Clear Timer with Participants on Page 2
- Reaction set to direct clap (single press)
- Mute and Camera On/Off on dialpad corner buttons
- Min/Max Window planned for dialpad bottom-left

### Key Insights from This Session
- The dial is the single most important thing to fix — timer duration and reaction selection depend on it
- Reaction dial selector was overengineered — nobody wants a 3-step process for a clap
- Timer should start on press, pause on press — duration set by dial before pressing
- Min/Max Zoom window is genuinely useful during hosting
- Zoom has NO global shortcut for admitting from waiting room — need alternative approach
- User is dyslexic — big visual icons matter more than text labels
- Logi's official Zoom plugin is the benchmark to beat

### Future Ideas Captured (Post-Hackathon)
- AI Launcher page — quick-open Claude, ChatGPT, Gemini, OpenAI with project shortcuts
- Per-app pages for each AI tool (projects, gems, incognito chat)
- Personalizable reaction button (choose your default reaction)
- Whiteboard/AI modes page for newer Zoom features
- Note taker auto-admission feature

### Next Session Priorities (Ordered)
1. **Fix the dial** — get rotation working for timer duration and reactions
2. **Test export** — flag some moments, press Export, verify HTML report
3. **Fill Page 3 empty slots** — decide what goes in 4 remaining buttons
4. **Icon redesign** — Adobe-style large bold icons filling entire button
5. **Action Ring Zoom profile** — set up without breaking defaults
6. **Demo prep** — record a clean demo video showing the full workflow

---

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
