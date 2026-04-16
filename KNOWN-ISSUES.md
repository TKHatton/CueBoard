# CueBoard — Known Issues & Open Questions

> Track everything that's broken, missing, or undecided here.
> Check this at the start of every session.

---

## Blockers (Must Fix Before Demo)

### .NET 8 SDK Not Installed
- **Status:** Tyler is clearing disk space (done — 31 GB free now)
- **Next step:** Install from https://dot.net/download
- **Impact:** Can't build or run anything until this is done

### Logitech Actions SDK Not Yet Explored
- **Status:** Tyler has the hardware (unopened box)
- **Next step:** Tyler will photograph SDK docs/packaging
- **Need to learn:**
  - How plugins register themselves (manifest/config)
  - How pages, buttons, and dials are defined
  - How button display/feedback works (text, icons, colors)
  - Project template structure
  - How to deploy/test a plugin on the device

---

## Open Design Questions

### Page 3 Layout (v2) — Not Finalized
- Moved from 4 flag buttons to 1 flag button + dial selection
- Two open button slots (6 and 7) — candidates:
  - Agenda Timer / Next Agenda Item
  - Share Summary to Chat
  - Something else?
- Need to decide before building

### Page 3 "Assign" Feature — Scope Unknown
- Idea: After flagging, attach flag to a person or department
- How does the team list get populated? Manual config? Zoom participant list?
- This might be too complex for the hackathon — could be SIMULATED

### Screen Share Complexity
- Tyler has 3 monitors, real sharing involves multiple selection steps
- Current implementation is a simple toggle — may not be impressive enough
- Could the dial help select share target? Needs SDK research.

### Virtual Backgrounds
- Tyler wants quick switching between preset backgrounds
- No button slot currently assigned
- Could replace an open slot on Page 1 or 2 if one frees up

---

## Known Bugs

### Fixed 2026-04-15
- **Captions (Page 2)** wasn't sending Alt+F2 — only flipping internal state.
  Fixed in `Actions/Page2/CaptionsCommand.cs` so the keystroke actually fires.

### Open / Suspected
- **Admit and Participants are duplicates** — both send Alt+U (open the
  participants panel). Functional, just redundant. Consider repurposing
  Admit, or accept it as "this is how you admit people from waiting room
  in real Zoom anyway" since Zoom has no dedicated admit shortcut.
- **TimerDial press from paused state** restarts as if fresh duration
  rather than resuming remaining seconds (StartTimerCommand handles this
  correctly — TimerDial does not). Low priority since most people will
  press the Timer button, not the dial.

---

## Live Test Checklist (Run on next Zoom call with another person)

### Page 1 — Live Controls
- [ ] Mute / Unmute (Alt+A) — verify state flips and icon updates
- [ ] Camera On/Off (Alt+V)
- [ ] Recording (Alt+R) — needs host or recording permission
- [ ] Screen Share (Alt+S) — note: opens the share picker dialog
- [ ] Chat (Alt+H) — opens chat panel
- [ ] Raise Hand (Alt+Y)
- [ ] Reaction dial: rotate cycles emoji, press sends selected one
- [ ] Gallery/Speaker view toggle
- [ ] End Meeting (Alt+Q) — saved for last; Zoom will prompt to confirm

### Page 2 — Operator Mode (host required for most)
- [ ] **Mute All** (Alt+M) — host only. Confirm participants get muted.
- [ ] **Captions** (Alt+F2) — JUST FIXED. Captions must be enabled in
      Zoom Settings > Accessibility first, otherwise nothing happens.
- [ ] **Breakout Rooms** (Alt+B) — host only. Should open the breakout panel.
- [ ] **Participants** (Alt+U) — opens/closes panel
- [ ] **Admit** — same shortcut as Participants right now (duplicate).
      In real Zoom you click Admit inside that panel.
- [ ] **Min/Max Window** — Win+Down / Win+Up. Test outside Zoom too.
- [ ] **Timer button** — press starts countdown, floating overlay appears
      on screen, press again pauses, press again resumes.
- [ ] **Timer dial** — rotate adjusts minutes (verify display), press
      starts/pauses. Heads-up on resume-from-paused issue above.
- [ ] **Clear Timer** — resets to zero, overlay should close.
- [ ] **Spotlight / Lock / Remove / Host Transfer** — these are
      SIMULATED. They flip an icon but don't actually change Zoom.
      Decide: leave as cosmetic, or remove from layout to avoid the
      "I pressed it but nothing happened" question from a judge.

### Page 3 — Meeting Intelligence
- [ ] Flag Moment + dial selects type — add a few of each
- [ ] **NEW: Action Item** — single tap creates ActionItem flag AND
      pops the typing dialog so you can describe it in one motion
- [ ] Add Note (attaches to most recent flag)
- [ ] Assign (dial cycles people, "+" lets you add a new name)
- [ ] Highlight
- [ ] Meeting Timer / Reset Meeting / Preview Summary
- [ ] **NEW: Email Recap** — should open your default mail client
      (Outlook, Gmail-as-default, etc.) with a draft containing all
      flags grouped by type. CONFIRM: what client opens by default?
- [ ] Export Summary — full HTML opens in browser

### Things only you can answer after testing
- Does the dial rotation feel responsive on Page 1 (reactions) and
  Page 3 (flag types) — visible toast on each turn?
- Does the timer overlay actually appear on the right monitor in your
  3-monitor setup, or does it land somewhere weird?
- Are the SIMULATED Page 2 buttons confusing in a real demo? If yes,
  prune them from the layout for the hackathon submission.

---

## Page 3 Layout Recommendation (after these changes)

You now have 12 Page 3 actions in the SDK; the device shows 9. Suggested
9-button placement:

1. Flag Moment
2. **Action Item** (NEW — replaces Clear Last; Reset Meeting already covers undo-all)
3. Highlight
4. Assign
5. Add Note
6. Meeting Timer
7. Preview Summary
8. **Email Recap** (NEW — fills the empty slot)
9. Export Summary

Leave Clear Last, Reset Meeting, and Interactive Poll available in Logi
Options+ but unmapped — you can swap them in if a button on the layout
isn't earning its spot.

---

## Open Question: "Real" Summarization

You asked if there's a way to actually summarize beyond Export Summary.
Short answer: not without AI, and CLAUDE.md says no AI for the hackathon.
The bullet-point grouping in Export Summary IS the summary at this layer.
A real LLM-generated paragraph ("In this 32-minute meeting the team
agreed to X, blocked on Y, and assigned Z to Sarah...") would need
either a cloud API call or a local Ollama+Gemma instance. Logged as a
post-hackathon feature — pairs naturally with the Ollama setup you
already have planned for the new computers.

---

## Things That Are Stubbed / Simulated
- **KeyboardService:** Logs to console instead of sending real keystrokes
- **ScreenshotService:** Returns fake file path, doesn't capture screen
- **All Page 2 "simulated" actions:** Spotlight, Lock Meeting, Admit, Remove, Host Transfer — these print to console but don't interact with Zoom's API
- **Platform Detection:** Works by process name; has manual override for demos

---

## Technical Debt (Fix Later, Not Now)
- No unit tests (hackathon — speed over coverage)
- No DI container (not needed yet)
- Console app only — no Logitech SDK integration yet
- KeyCombo.Parse doesn't validate modifier names
