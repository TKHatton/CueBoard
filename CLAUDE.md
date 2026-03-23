# CueBoard — Project Instructions

## What Is This?
CueBoard is a Logitech Actions SDK plugin that turns the Logitech MX Creative Console into a physical control surface for virtual meetings. It was built for the Logitech hackathon (Tyler made top 50 out of 1,300 entrants — goal is top 6).

## The One-Line Pitch
"The first physical control surface purpose-built for virtual meetings."

## Tech Stack
- **Language:** C# (.NET 8)
- **SDK:** Logitech Actions SDK
- **Target Platform:** Windows (Tyler runs Windows 11)
- **Meeting Platform:** Zoom (fully implemented). Teams and Google Meet are scaffolded only.
- **No cloud backend. No AI features. Everything local.**

## Project Structure
```
CueBoard/
├── CLAUDE.md            ← You are here. Master instructions.
├── SPEC.md              ← Current button/dial layout (source of truth)
├── DECISIONS.md         ← Why things are the way they are
├── DEMO-SCRIPT.md       ← Step-by-step demo choreography
├── PITCH.md             ← Hackathon narrative and talking points
├── KNOWN-ISSUES.md      ← Bugs, blockers, open questions
├── ZOOM-SHORTCUTS.md    ← Complete Zoom keyboard shortcut reference
├── ROADMAP.md           ← Build order and milestones
└── src/                 ← Source code (C# solution)
    ├── CueBoard.Core/   ← Core logic (models, services, mappings)
    └── CueBoard.Demo/   ← Console-based demo runner
```

## Hardware
- Logitech MX Creative Console (physical device, sent by Logitech)
- 9 programmable buttons per page (multiple pages)
- 1 dial per page (rotate + press)
- Buttons can show text, icons, and state feedback on their display

## Core Concepts
1. **Pages** — The console has multiple pages of buttons. CueBoard uses 3 pages:
   - Page 1: Live Controls (everyday meeting controls)
   - Page 2: Operator Mode (host power tools)
   - Page 3: Meeting Intelligence (flagging and export)

2. **Dial** — The dial is a major differentiator. Every page MUST have meaningful dial behavior. It's not an afterthought — it's a selling point.

3. **Feedback** — Buttons must show their current state (Muted/Live, Recording indicator, flag counters). This is what makes it feel real vs. a toy.

4. **Platform Detection** — Auto-detect if Zoom is running. Support override for demos without Zoom open.

## Coding Rules
- Clean, readable C#. No enterprise overengineering.
- No unnecessary abstraction layers. Keep it hackathon-friendly.
- Only Zoom needs full implementation. Teams/Meet are stubs.
- Prefer simple dictionary-based mappings over complex configuration systems.
- Simulation mode for all hardware interactions (keyboard, screenshots, etc.)
- Every new feature must work in the demo flow. If it can't be demoed, it doesn't ship.

## What NOT to Do
- Don't add cloud features, AI, or external API calls
- Don't build a full production error-handling system
- Don't add interfaces/DI containers unless the SDK requires them
- Don't over-abstract — three similar lines of code is better than a premature abstraction
- Don't build features for Teams or Google Meet beyond empty stubs
- Don't create documentation files beyond what's already defined above

## Tyler's Preferences
- Explain things using practical metaphors (like explaining to a senior citizen)
- Don't summarize what was just done unless asked
- Keep responses concise and direct
- Tyler has 3 monitors — screen sharing complexity is a real pain point for him
- Tyler uses Fathom for meeting notes — Page 3 should complement, not compete with AI tools
