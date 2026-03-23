# CueBoard — Build Roadmap

> Work in this order. Each phase builds on the last.
> Don't skip ahead — a working Phase 1 beats a broken Phase 3.

---

## Phase 0: Foundation (CURRENT)
**Goal:** Get the development environment running.

- [x] Project structure created (CueBoard.Core + CueBoard.Demo)
- [x] Core models defined (MeetingPlatform, MeetingAction, FlagType, MeetingFlag, KeyCombo)
- [x] Platform detection (process-based + override)
- [x] Control mapping engine (Zoom fully mapped)
- [x] Keyboard service (simulation mode)
- [x] Action handler (routes actions through detection → mapping → keyboard)
- [x] Flagging service (in-memory flag storage)
- [x] Screenshot service (stub)
- [x] Export service (Markdown generation)
- [x] Session state manager
- [x] Hardware simulator (console-based button simulation)
- [x] Demo runner (scripted + interactive modes)
- [x] Project documentation (CLAUDE.md, SPEC.md, etc.)
- [ ] Install .NET 8 SDK
- [ ] Build and run the project successfully
- [ ] Open Logitech MX Creative Console box
- [ ] Research Logitech Actions SDK (how plugins work)

## Phase 1: SDK Integration
**Goal:** Get a CueBoard plugin recognized by the Logitech Actions app.

- [ ] Study SDK docs (from box/developer portal)
- [ ] Create proper plugin manifest/registration
- [ ] Define 3 pages with button and dial declarations
- [ ] Get a "hello world" button press to trigger console output
- [ ] Confirm dial rotation events are received
- [ ] Wire up button display feedback (text on buttons)

## Phase 2: Page 1 — Live Controls
**Goal:** All MUST buttons work end-to-end with Zoom.

- [ ] Mute toggle with ON/OFF feedback on button display
- [ ] Camera toggle with feedback
- [ ] Recording toggle with red indicator
- [ ] Screen Share toggle
- [ ] Chat panel toggle
- [ ] End Meeting for All
- [ ] Dial: Reaction cycling + send
- [ ] Raise Hand (NICE)
- [ ] Gallery/Speaker View (NICE)

## Phase 3: Page 3 — Meeting Intelligence
**Goal:** The "wow" feature works perfectly for the demo.

> Building this before Page 2 because it's the closer —
> the last thing judges see should be the strongest.

- [ ] Flag Moment button with dial-selected type
- [ ] Flag counter on button display
- [ ] Clear Last Flag (undo)
- [ ] Export Summary to Markdown
- [ ] Verify exported file looks clean and impressive
- [ ] Assign to person/department via dial (NICE)
- [ ] Add Note (NICE)
- [ ] Highlight (NICE)
- [ ] Preview Summary on console display (NICE)

## Phase 4: Page 2 — Operator Mode
**Goal:** Host power tools that make judges say "I want that."

- [ ] Mute All
- [ ] Timer: Dial sets duration, press starts/pauses
- [ ] Timer: Visual countdown on console display
- [ ] Clear Timer (reset)
- [ ] Spotlight Speaker (simulated)
- [ ] Lock Meeting (simulated with feedback)
- [ ] Admit from Waiting Room (simulated)
- [ ] Toggle Captions (NICE)
- [ ] Remove Participant with 2-step confirm (NICE)
- [ ] Host Transfer (NICE)

## Phase 5: Polish & Demo Prep
**Goal:** Everything looks and feels professional.

- [ ] Button icons/graphics designed for all buttons
- [ ] Consistent feedback states across all pages
- [ ] Demo script rehearsed end-to-end
- [ ] Emergency fallback (simulation mode) tested
- [ ] Export file formatted and impressive
- [ ] Screen recording of full demo flow
- [ ] Pitch talking points memorized

---

## Decision: Build Order Rationale

**Why Page 3 before Page 2?**
Page 3 (Meeting Intelligence) is the "winning feature" — the thing that makes CueBoard different from just being a keyboard shortcut pad. In the demo, it's the closer. If time runs short, a polished Page 1 + Page 3 is a stronger entry than Page 1 + Page 2.

Page 2 (Operator Mode) is impressive but many of its features are simulated anyway. The timer is the only truly interactive piece, and it can be added late.

**Why not build everything in parallel?**
Because a demo with 3 half-working pages loses to a demo with 2 perfect pages. Ship quality, not quantity.
