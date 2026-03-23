# Zoom Keyboard Shortcuts Reference

> These are the Zoom desktop shortcuts CueBoard maps to.
> Source: Zoom's built-in shortcut settings (Windows).
> Some actions don't have shortcuts and must be simulated.

---

## Meeting Controls (Page 1)

| Action | Shortcut | Notes |
|--------|----------|-------|
| Toggle Mute | Alt+A | Must be in meeting |
| Toggle Video | Alt+V | |
| Toggle Recording | Alt+R | Local recording; cloud recording may differ |
| Start/Stop Screen Share | Alt+S | Opens share picker |
| Toggle Chat Panel | Alt+H | |
| Raise/Lower Hand | Alt+Y | |
| End Meeting | Alt+Q | Shows confirmation dialog |
| Toggle Participants Panel | Alt+U | |
| Toggle Full Screen | Alt+F | |
| Switch Gallery/Speaker View | — | No direct shortcut — must simulate via UI |

## Reactions

| Reaction | Shortcut | Notes |
|----------|----------|-------|
| Thumbs Up | — | No direct shortcut |
| Clap | — | No direct shortcut |
| Heart | — | No direct shortcut |
| Laugh | — | No direct shortcut |
| Cheer | — | No direct shortcut |

> Reactions don't have keyboard shortcuts in Zoom. Options:
> 1. Use Zoom's SDK/API if available
> 2. Simulate via UI automation (find and click the reaction button)
> 3. Use accessibility shortcuts if Zoom exposes them

## Host Controls (Page 2)

| Action | Shortcut | Notes |
|--------|----------|-------|
| Mute All | Alt+M | Host only |
| Toggle Closed Captions | Alt+F2 | May vary by Zoom version |
| Spotlight Speaker | — | No shortcut — must simulate |
| Lock/Unlock Meeting | — | No shortcut — must simulate |
| Admit from Waiting Room | — | No shortcut — must simulate |
| Remove Participant | — | No shortcut — must simulate |
| Host Transfer | — | No shortcut — must simulate |
| Breakout Rooms | — | No shortcut — must simulate |

## Other Useful Shortcuts

| Action | Shortcut | Notes |
|--------|----------|-------|
| Push to Talk | Hold Spacebar | Only when muted |
| Switch to Next Camera | — | No shortcut |
| Pause/Resume Recording | Alt+P | |
| Switch Active Speaker View | — | No shortcut |
| Navigate to Prev/Next Page (Gallery) | Page Up / Page Down | |
| Show/Hide Meeting Controls | Alt | Auto-hide mode only |
| Zoom In (Shared Content) | Ctrl+= | When viewing shared content |
| Zoom Out (Shared Content) | Ctrl+- | When viewing shared content |

---

## Simulated Actions Strategy

For actions without keyboard shortcuts, CueBoard will:
1. **Log the action** to console/state (always works)
2. **Show feedback** on the button display (state change)
3. **NOT actually trigger Zoom** — the demo focuses on CueBoard's behavior

This is acceptable for the hackathon. Judges understand that some host controls
require Zoom's API, which isn't available for keyboard-only plugins.

In a production version, these would use:
- Zoom's REST API or SDK
- UI automation (finding and clicking Zoom's UI elements)
- Accessibility tree interaction
