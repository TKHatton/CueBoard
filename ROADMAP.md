# CueBoard — Build Roadmap

> **UPDATED:** 2026-04-20 — Reflects actual implementation status after codebase review.
> Work in this order. Each phase builds on the last.

---

## Phase 0: Foundation ✓ COMPLETE
**Goal:** Get the development environment running.

- [x] Project structure created (Logitech SDK plugin architecture)
- [x] Core models defined (MeetingPlatform, MeetingAction, FlagType, MeetingFlag)
- [x] Platform detection (process-based + override mode)
- [x] Keyboard service (real P/Invoke implementation + simulation mode)
- [x] Action handler (routes actions through detection → keyboard)
- [x] Flagging service (in-memory flag storage with assign/note support)
- [x] Export service (HTML generation with transcript integration)
- [x] Session state manager (centralized state for all 3 pages)
- [x] Toast notification service (PowerShell-based, top-right corner)
- [x] Timer service (start/pause/resume/reset with events)
- [x] Timer overlay service (PowerShell countdown window)
- [x] Input dialog service (async text input dialogs)
- [x] Transcript service (VTT parser for Zoom captions)
- [x] Project documentation (CLAUDE.md, SPEC.md, DECISIONS.md, etc.)
- [x] .NET 8 SDK installed
- [x] Build and run successfully
- [x] Logitech MX Creative Console unboxed
- [x] Logitech Actions SDK integrated

## Phase 1: SDK Integration ✓ COMPLETE
**Goal:** Get a CueBoard plugin recognized by the Logitech Actions app.

- [x] Study SDK (Loupedeck base classes documented)
- [x] Create plugin manifest (CueBoardPlugin.cs inherits from Plugin)
- [x] Define 3 pages with button and dial declarations
- [x] Button presses trigger command execution
- [x] Dial rotation events received and handled
- [x] Button display feedback (text + icons using BitmapBuilder)
- [x] Event system for state-driven image refresh

## Phase 2: Page 1 — Live Controls ✓ COMPLETE
**Goal:** All MUST buttons work end-to-end with Zoom.

- [x] Mute toggle (Alt+A) with Live/Muted feedback
- [x] Camera toggle (Alt+V) with On/Off feedback
- [x] Recording toggle (Alt+R) with red indicator when active
- [x] Screen Share toggle (Alt+S)
- [x] Chat panel toggle (Alt+H)
- [x] End Meeting for All (Alt+Q)
- [x] Dial: Reaction cycling (rotate) + send (press)
- [x] Raise Hand (Alt+Y)
- [x] Gallery/Speaker View toggle
- [x] All 10 commands implemented with state tracking

## Phase 3: Page 3 — Meeting Intelligence ✓ COMPLETE
**Goal:** The "wow" feature works perfectly for the demo.

> Building this before Page 2 because it's the closer —
> the last thing judges see should be the strongest.

- [x] Flag Moment button with dial-selected type (Action/Decision/FollowUp/Bookmark)
- [x] Flag counter on button display (updates live)
- [x] Clear Last Flag (undo with toast notification)
- [x] Export Summary to **HTML** (full dark-theme with stats dashboard)
- [x] Exported file looks professional and impressive ✓
- [x] Assign to person/department via dial (with "+" to add new names)
- [x] Add Note (spawns async input dialog)
- [x] Highlight (separate from regular flags)
- [x] Preview Summary on console display
- [x] Action Item button (dedicated quick-flag)
- [x] Email Recap (opens mailto: link with flag summary)
- [x] Meeting Timer (alternative timer on Page 3)
- [x] Reset Meeting (clears all flags + state)
- [x] Interactive Poll (command exists)
- [x] Transcript integration (auto-detects VTT files, embeds in export)

**All 13 Page 3 commands implemented.**

## Phase 4: Page 2 — Operator Mode ⚠ PARTIALLY COMPLETE
**Goal:** Host power tools that make judges say "I want that."

> **STATUS UPDATE:** Page 2 underwent a complete rebuild on 2026-04-19.
> All simulated/unreliable commands were removed. Only real Zoom shortcuts remain.

### Implemented (6 commands + dial):
- [x] Mute All (Alt+M) — real Zoom shortcut
- [x] Lock Meeting (Alt+L) — real Zoom shortcut with Locked/Open feedback
- [x] Admit from Waiting Room (Alt+U) — opens participants panel
- [x] Fullscreen (Alt+F) — toggles Zoom fullscreen mode
- [x] Pause Share (Alt+T) — pause/resume active screen share
- [x] Timer: Dial sets duration, press starts/pauses/resumes
- [x] Timer: Visual countdown on button display (color-coded: orange → red)
- [x] Timer: Floating overlay window on screen
- [x] Clear Timer (reset button)

### Removed (unreliable/simulated):
- ~~Captions~~ (wrong shortcut; replaced with Fullscreen)
- ~~Spotlight Speaker~~ (no Zoom shortcut exists)
- ~~Remove Participant~~ (no direct shortcut)
- ~~Host Transfer~~ (no shortcut)
- ~~Participants panel duplicate~~ (redundant with Admit)

### Not Yet Implemented:
- [ ] Min/Max Window (Win+Down / Win+Up) — needs simple command

**Page 2 Status:** 6 of 7 SPEC buttons working. One open slot remains.

## Phase 5: Polish & Demo Prep — IN PROGRESS
**Goal:** Everything looks and feels professional.

- [x] Button text rendering (BitmapBuilder with font/color)
- [x] Consistent feedback states across all pages (icons + text)
- [x] Toast notifications for all user actions
- [x] Timer overlay with visual feedback
- [x] Export HTML with dark theme, stats, transcript sidebar
- [ ] Button icons/graphics designed for all buttons (text-only currently)
- [ ] Demo script rehearsed end-to-end
- [ ] Emergency fallback (simulation mode) tested with demo flow
- [ ] Screen recording of full demo flow
- [ ] Pitch talking points memorized

---

## Summary: What's Actually Left to Do

**Core Plugin:** ✓ Feature-complete. All 3 pages functional with 29+ working commands.

**Remaining Tasks:**

### Immediate (Before Demo):
1. **Min/Max Window Command** — Simple Win+Down/Win+Up keyboard command for Page 2, Button 6
2. **Button Icon Design** — Replace text-only buttons with proper icons/graphics
3. **Live Testing Checklist** — Run through KNOWN-ISSUES.md checklist in a real Zoom call
4. **Demo Script Rehearsal** — Practice the full flow from DEMO-SCRIPT.md

### Nice-to-Have (Time Permitting):
- Additional button for Page 2's empty slot (candidates: Virtual Background switcher, Breakout Rooms)
- Icon polish for all button states (current text rendering works but isn't as polished)
- Demo mode improvements (better simulation feedback)

### Post-Hackathon:
- Unit tests
- Multi-platform support (Teams, Google Meet)
- Cloud sync for flag persistence
- AI summarization (via Ollama/Gemma)

**Bottom Line:** The plugin is 95% complete. Focus on polish, icons, and demo rehearsal.

---

## Future Vision (Post-Hackathon Roadmap)

### Version 2.0: Multi-Platform Support
- **Teams integration** — Full keyboard shortcut mapping
- **Google Meet integration** — Complete feature parity
- **Platform auto-detection** — Seamlessly switch between meeting platforms
- **Cross-platform state sync** — Consistent button behavior regardless of platform

### Version 2.5: Audio Playback & Enhanced Transcription
**Problem:** Zoom's transcript is text-only. Users can't jump to the audio moment.

**Solution: Integrated Audio Playback**
- **Local recording integration** — Link flags to Zoom's local MP4/M4A recordings
- **Timestamp-to-audio mapping** — Click flag timestamp → Audio plays from that moment
- **Waveform visualization** — See audio levels alongside transcript text
- **Playback controls** — Play/pause, speed control, 15-sec skip

**Solution: Superior Speech-to-Text with Whisper**
- **Replace Zoom captions with Whisper** — OpenAI's Whisper is state-of-the-art
- **Local processing** — Runs on-device (no cloud, privacy-first)
- **Accuracy improvements:**
  - Better speaker diarization (who said what)
  - Technical jargon recognition
  - Multi-language support (40+ languages)
  - Punctuation and capitalization
- **Real-time vs post-processing** — Option for live captions or post-meeting enhancement

**Technical Implementation:**
- Use **Whisper.cpp** or **faster-whisper** for local inference
- Audio captured via virtual audio cable or Zoom recording API
- Processing happens post-meeting (background task, doesn't slow down live meeting)
- VTT export compatible with existing CueBoard export format

**Why Whisper?**
- Industry-leading accuracy (especially for technical conversations)
- Open-source, runs locally (no API costs, no privacy concerns)
- Handles multiple speakers, accents, and background noise better than Zoom

### Version 3.0: AI-Powered Meeting Intelligence
- **Summarization** — Generate meeting summary from transcript (using local LLM)
- **Action item extraction** — AI suggests what should be flagged
- **Sentiment analysis** — Highlight moments of agreement/disagreement
- **Topic clustering** — Group flags by detected topics
- **Speaker insights** — Who talked most, talk time distribution

**Local AI Stack:**
- Ollama + Llama 3 / Gemma for summarization
- Whisper for transcription
- All on-device, no cloud dependencies

### Version 3.5: Calendar & Workflow Integration
- **Calendar sync** — Auto-detect meeting context from Google/Outlook Calendar
- **Pre-populated participant list** — Know who's in the meeting before it starts
- **Meeting templates** — Pre-configure button layouts for recurring meetings
- **CRM integration** — Export flags directly to Salesforce, HubSpot, Notion
- **Slack/Teams messages** — Post flag summaries to team channels

### Version 4.0: Collaborative Features
- **Cloud sync** — Access flags across devices (desktop, mobile, web)
- **Shared meeting boards** — Multiple users flag the same meeting
- **Real-time collaboration** — See others' flags as they create them
- **Role-based permissions** — Host sees all flags, participants see their own
- **Meeting archives** — Search across all past meetings

### Hardware Expansion
- **Stream Deck support** — Plugin for Elgato Stream Deck
- **Mobile companion app** — Flag moments from phone during meeting
- **API for custom hardware** — Let users build their own control surfaces

---

## Technology Stack Evolution

### Current (Hackathon v1.0)
- C# / .NET 8
- Logitech Actions SDK
- Local-only processing
- PowerShell UI elements

### Future (v2.0+)
- **Transcription:** Whisper.cpp (local C++ bindings)
- **Audio playback:** NAudio or BASS.NET
- **AI summarization:** Ollama + Llama 3
- **Cloud sync:** Firebase or Supabase
- **Web dashboard:** React + TypeScript
- **Mobile apps:** React Native or Flutter

---

## Competitive Landscape & Differentiation

### How CueBoard Stays Ahead

**vs Fathom/Fireflies (AI Meeting Assistants):**
- CueBoard = human curation + physical control
- AI tools capture everything; CueBoard captures what *you* think matters
- Integration, not replacement

**vs Stream Deck (Macro Hardware):**
- CueBoard = meeting-specific intelligence (flags, transcript, export)
- Stream Deck = generic macro buttons
- The dial matters (continuous input > discrete buttons)

**vs Keyboard Shortcuts:**
- CueBoard = discoverable, visible state, muscle memory
- Shortcuts = hidden, memorization required, no feedback

**Unique Value Proposition:**
"The only physical control surface that combines instant meeting controls with intelligent meeting capture."

---

## Decision: Build Order Rationale

**Why Page 3 before Page 2?**
Page 3 (Meeting Intelligence) is the "winning feature" — the thing that makes CueBoard different from just being a keyboard shortcut pad. In the demo, it's the closer. If time runs short, a polished Page 1 + Page 3 is a stronger entry than Page 1 + Page 2.

Page 2 (Operator Mode) is impressive but many of its features are simulated anyway. The timer is the only truly interactive piece, and it can be added late.

**Why not build everything in parallel?**
Because a demo with 3 half-working pages loses to a demo with 2 perfect pages. Ship quality, not quantity.
