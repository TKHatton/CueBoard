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
(None yet — code hasn't been built against hardware)

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
