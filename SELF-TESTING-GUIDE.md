# CueBoard Self-Testing Guide — Testing with Just You

> **Goal:** Test all CueBoard features using only your computer + phone.
> **Time needed:** 20-30 minutes

---

## Setup: Join Meeting from Two Devices

### Step 1: Start Meeting on Computer
1. Open Zoom Desktop Client on your computer
2. Click "New Meeting"
3. You're now the host

### Step 2: Join from Phone
1. Copy the meeting ID from your computer
2. Open Zoom app on your phone
3. Join meeting → Enter meeting ID
4. Join Audio → Use "Call via Device Audio"
5. **Important:** Your phone will be in the waiting room if you enabled it

**Now you have:**
- Computer = Host (you + CueBoard)
- Phone = Participant (for testing host features)

---

## Testing Strategy

### What You CAN Test Alone
✓ All of Page 1 (Live Controls)
✓ Most of Page 2 (Operator Mode)
✓ All of Page 3 (Meeting Intelligence)

### What You CAN'T Fully Test
✗ Mute All (phone will mute, but you won't hear the difference)
✗ Remove Participant (would need a 3rd person)
✗ Admit from Waiting Room (requires someone actually waiting)

**But that's okay!** You can test the *buttons work* (send keystrokes, show feedback) even if you can't verify the *Zoom outcome*.

---

## Page 1: Live Controls — Full Test

| Button | How to Test | What to Look For |
|--------|-------------|------------------|
| **Mute** | Press button | Button shows "MUTED", toast appears, Zoom shows muted icon |
| **Camera** | Press button | Button shows "OFF", video feed stops |
| **Recording** | Press button (requires host) | Zoom shows "Recording" banner, button turns red |
| **Screen Share** | Press button | Zoom's share picker opens |
| **Chat** | Press button | Chat panel slides in from right |
| **Reaction** | Rotate dial → Press button | Toast shows reactions cycling; pressing sends selected reaction above your video |
| **Raise Hand** | Press button | Hand icon appears next to your name in participant list |
| **Gallery/Speaker** | Press button | View switches between modes |
| **End Meeting** | Press button | Zoom prompts "End meeting for all?" (DON'T ACTUALLY END) |

**Critical Test: Reactions**
1. Rotate dial → Watch toast: "Reaction: Thumbs Up"
2. Rotate more → "Reaction: Heart"
3. Rotate more → "Reaction: Clap"
4. **Now press button** → Selected reaction appears
5. Try pressing **dial** instead of button → Same reaction sends

**This proves the dial works!**

---

## Page 2: Operator Mode — Partial Test

### Buttons You CAN Test Fully:

| Button | How to Test | What to Look For |
|--------|-------------|------------------|
| **Mute All** | Press button | Phone's Zoom shows "Host muted you"; computer shows toast |
| **Lock Meeting** | Press button | Zoom shows lock icon in toolbar; button shows "LOCKED" |
| **Fullscreen** | Press button | Zoom enters fullscreen mode; press again to exit |
| **Pause Share** | Start sharing, then press button | Shared screen freezes (but stays visible) |
| **Timer** | Rotate dial to 2 min → Press button | Floating overlay appears with countdown |
| **Timer Pause** | While timer running, press button | Timer pauses; button shows ❚❚ symbol |
| **Timer Resume** | While paused, press button | Timer resumes from remaining time |
| **Captions** | Press button | Zoom toolbar shows captions icon active; toast appears |

### Buttons You CAN'T Fully Test Alone:

**Admit from Waiting Room:**
- Press button → Participants panel opens
- You'll see your phone isn't in the waiting room (because you already admitted yourself)
- **What to say in demo:** "This opens the waiting room panel where I can admit guests"

**Min/Max Window:**
- Press button → Active window minimizes
- **Test it on Notepad first** to see if it works

---

## Page 3: Meeting Intelligence — Full Test

| Button | How to Test | What to Look For |
|--------|-------------|------------------|
| **Flag Moment Dial** | Rotate dial | Toast shows: "Flag: Action Item" → "Decision" → "Follow-Up" → "Bookmark" |
| **Flag Moment** | Set dial to "Decision", press button | Toast: "Decision flagged"; button counter shows "1" |
| **Assign** | Press Assign → Rotate dial → Press dial | Toast cycles participants ("Sarah", "Mike", etc.); pressing dial confirms assignment |
| **Add Note** | Flag something, press Add Note | Input dialog appears; type text; press Enter; toast confirms |
| **Highlight** | Press button | Toast: "Highlight added" |
| **Action Item** | Press button | Toast: "Action Item flagged" (quick shortcut) |
| **Preview** | Press button | Button shows "VIEW (2)" where 2 = flag count |
| **Preview (double)** | Press twice quickly (<2 sec) | Button shows "DEMO LOADED" (demo data loaded) |
| **Export Summary** | Press button | Browser opens with HTML export; button shows "SAVED!" |
| **Email Recap** | Press button | Default mail client opens with draft email |

### Critical Test: Full Flagging Workflow

**Do this 5-minute test to prove Page 3 works end-to-end:**

1. **Enable Live Transcript in Zoom** (Required for export)
   - Click "Captions" button in Zoom
   - Select "Enable Auto-Transcription"
   - Talk for 1-2 minutes (Zoom generates a `.vtt` file)

2. **Create Flags:**
   - Rotate dial → Select "Action Item"
   - Press Flag Moment → Toast confirms
   - Press Assign → Rotate to "Sarah" → Press dial
   - Press Add Note → Type "Follow up on API design" → Enter

3. **Create More Flags:**
   - Rotate dial → Select "Decision"
   - Press Flag Moment
   - Press Add Note → Type "Approved new feature"

4. **Export:**
   - Press Export Summary
   - Browser opens with HTML
   - Verify you see:
     - Stats dashboard (2 flags: 1 Action Item, 1 Decision)
     - Flags grouped by type
     - Assignments ("Sarah")
     - Notes ("Follow up on API design")
     - Transcript sidebar with timestamped text

**If this works, Page 3 is demo-ready!**

---

## Testing Checklist (Do This Before Demo)

### Pre-Test Setup (5 min):
- [ ] Start Zoom meeting on computer
- [ ] Join from phone as 2nd participant
- [ ] Enable Live Transcript (Zoom → Captions → Enable Auto-Transcription)
- [ ] Talk for 1-2 minutes (generates transcript content)

### Page 1 Tests (5 min):
- [ ] Mute → See button change + toast
- [ ] Camera → Video stops
- [ ] Reaction dial → Cycle 3 reactions, send one
- [ ] Screen Share → Share picker opens
- [ ] Chat → Panel slides in

### Page 2 Tests (5 min):
- [ ] Mute All → Phone shows "Host muted you"
- [ ] Lock Meeting → Lock icon appears in Zoom
- [ ] Timer → Dial sets 2 min, press starts, overlay appears
- [ ] Pause Timer → Press button, timer pauses
- [ ] Clear Timer → Overlay closes (FIXED NOW!)
- [ ] Captions → Toggle on/off, toast appears

### Page 3 Tests (10 min):
- [ ] Flag dial → Cycle through flag types (toast confirms)
- [ ] Flag Moment → Create 2-3 flags
- [ ] Assign → Assign one flag to "Sarah"
- [ ] Add Note → Attach note to flag
- [ ] Export Summary → HTML opens in browser
- [ ] Verify HTML has flags, assignments, notes, transcript

### Final Checks:
- [ ] All toast notifications appear correctly
- [ ] Button displays update (muted/live, timer countdown, flag count)
- [ ] No error messages in plugin logs
- [ ] Export HTML looks professional

---

## Common Testing Issues & Fixes

### "Mute All doesn't seem to work"
**Fix:** Check that your phone is unmuted first. When you press Mute All, your phone should show "Host muted you" at the bottom of the screen.

### "Export Summary doesn't include transcript"
**Fix:**
1. Enable captions in Zoom: Captions → Enable Auto-Transcription
2. Talk for 1-2 minutes (Zoom needs time to generate `.vtt` file)
3. Check if transcript file exists in `C:\Users\[YourName]\Documents\Zoom\`

### "Timer overlay doesn't appear"
**Fix:**
- PowerShell execution policy might be blocking it
- Run: `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` in PowerShell (admin)

### "Reactions only send Clap"
**Fix:** You need to **rotate the dial first** before pressing the button. Default is Clap (index 0). Rotate → See toast → Press.

### "Captions button does nothing"
**Fix:** Alt+C is the Zoom shortcut. If it doesn't work:
- Check Zoom settings → In Meeting (Basic) → Closed Captioning is enabled
- Make sure Zoom is the active window when you press the button

---

## What to Record for Demo Video

If you're creating a screen recording for submission:

**Recording 1: Page 1 (30 seconds)**
- Show 5 buttons working: Mute, Camera, Reaction (dial), Screen Share, Chat
- Screen capture + webcam in corner showing hands on console

**Recording 2: Page 3 Full Workflow (60 seconds)**
- Flag 3 moments (different types)
- Assign one to a person
- Add a note
- Export Summary (show full HTML)
- Screen capture + webcam

**Recording 3: Timer Demo (20 seconds)**
- Rotate dial to set duration
- Press to start (show overlay)
- Press to pause
- Press to resume
- Clear Timer (show overlay closes)

**Editing tip:** Add text overlays explaining what each button does. Example:
- "Mute toggle — shows state on button"
- "Dial cycles reactions"
- "Export generates HTML with transcript"

---

## Self-Testing Workflow (Full 30-Minute Test)

**Minute 0-5: Setup**
- Start Zoom meeting (computer + phone join)
- Enable captions
- Talk for 2 minutes (generate transcript)
- Open CueBoard plugin

**Minute 5-10: Page 1 Test**
- Test Mute, Camera, Reaction dial, Screen Share, Chat
- Verify toast notifications appear
- Verify button displays update

**Minute 10-15: Page 2 Test**
- Test Mute All (phone gets muted)
- Test Lock Meeting (icon appears)
- Test Timer (full start/pause/resume/clear cycle)
- Test Captions

**Minute 15-25: Page 3 Test (The Big One)**
- Create 3 flags (different types)
- Assign one to "Sarah"
- Add note to another
- Export Summary
- Verify HTML looks good

**Minute 25-30: Repeat Demo Script**
- Run through the full 3-minute demo script
- Time yourself
- Note any rough spots

---

## Questions Only You Can Answer After Testing

- **Does the dial feel responsive?** Do you see toast notifications on each rotation?
- **Do the buttons feel right?** Is the feedback clear?
- **Does the HTML export look impressive?** Would you be proud to show it to judges?
- **Can you do the 3-minute demo script without notes?** Practice until yes.

---

## Bottom Line

**You CAN test 90% of CueBoard features by yourself.** The stuff you can't test (Remove Participant, Admit) is minor. Focus your testing energy on:

1. **Page 1 reactions dial** (judges will love this)
2. **Page 3 full flagging workflow + export** (your winning feature)
3. **Timer start/pause/clear** (shows polish)

If those three things work flawlessly, you're demo-ready.

Good luck! 🎯
