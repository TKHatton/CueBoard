# CueBoard — Next Steps (Priority Order)

## 🔴 Must Fix (Blocking Demo)

### 1. Get the Dial Working
The physical dial doesn't respond when rotated. This is the biggest gap — judges will rotate it and expect something to happen.
- Investigate why CueBoard dial actions (Reaction Selector, Timer Duration) don't respond
- May need to assign them properly in Logi Options+ Dial and Roller settings
- Test: rotate dial → reaction type changes on keypad LCD

### 2. Restore Dialpad Defaults
The default Logi profile for the dialpad got overwritten. Need to:
- Reset dialpad to factory defaults in Logi Options+ Settings
- Then carefully add CueBoard actions to corner buttons only
- Keep Undo/Redo or replace with Mute/Camera as planned

### 3. Page 2 Functionality
Host-only buttons (Mute All, Spotlight, Lock, etc.) need testing with 2+ people in a call.
- Schedule a test call with another person
- Verify which shortcuts actually work as host
- Remove or relabel buttons that can't work via shortcuts

## 🟡 Should Do (Makes It Stand Out)

### 4. Action Ring Zoom Profile
Create a Zoom-specific Application Profile for the Action Ring:
- Go to Actions Ring → "+" → Application Profile → select Zoom
- Assign the 8 planned actions
- Test that it auto-switches when Zoom is in focus

### 5. Icon Polish (Final Pass)
Make icons Adobe-style: big, bold, fill the whole button space.
- Color IS the background (full bleed)
- Symbol is large and centered
- No text inside the icon
- Think: Premiere Pro's purple "Pr" but for meeting actions

### 6. Page 3 Screen UI
Meeting Intelligence buttons currently just toggle state. Need visible results:
- Flag Moment → brief toast/notification on screen confirming the flag
- Preview Summary → show a quick overlay of flags collected so far
- Add Note → could open a small input dialog
- Assign → could show a dropdown of participant names

### 7. Fill Page 3 Empty Slots
Two empty spots. Best candidates:
- Launch Zoom (opens Zoom Workplace)
- Launch Google Meet (opens meet.google.com)

## 🟢 Nice to Have (If Time Allows)

### 8. Zoom Plugin Conflict Resolution
If the official Zoom plugin is installed, CueBoard buttons may not fire. Need:
- Instructions for users to disable the Zoom plugin
- Or detect and warn about conflicts

### 9. Demo Video
Record a polished 2-3 minute walkthrough:
- Show the hardware
- Demo Page 1 in a live Zoom call (mute, camera, share, react)
- Demo the timer overlay
- Demo flagging and export
- Close with the branded HTML report

### 10. Google Meet Support
All shortcuts are currently Zoom-only. Google Meet uses different shortcuts:
- Ctrl+D = mute in Meet
- Ctrl+E = camera in Meet
- Could detect which app is running and switch shortcut maps

### 11. Presentation Deck / Submission Package
- Screenshots of the keypad with CueBoard running
- Before/after comparison with Zoom's official plugin
- Architecture diagram
- User story / pitch narrative
