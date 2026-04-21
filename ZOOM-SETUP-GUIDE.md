# Zoom Setup Guide for CueBoard Demo Testing

> **Purpose:** Configure Zoom in advance so you can test all CueBoard features during demo rehearsal.
> Some features need pre-configuration, some require you to be the host, and some need other participants.

---

## Prerequisites

### You'll Need:
1. **Zoom Desktop Client** (not web browser version)
2. **Zoom account** with host permissions
3. **A second device or test account** for participant testing (phone, tablet, or friend's computer)
4. **Meeting settings configured** (see below)

---

## Part 1: Zoom Account Settings (Do This Once)

Go to https://zoom.us/profile/setting and configure these settings **before** your test meeting:

### Enable Required Features:

1. **Waiting Room** (for testing Admit button)
   - Navigate to: Settings → In Meeting (Advanced) → Waiting Room
   - Toggle **ON**
   - Participants will wait for you to admit them (tests Page 2, Button 3)

2. **Live Transcript / Closed Captions** (for transcript export)
   - Settings → In Meeting (Basic) → Closed Captioning
   - Toggle **ON** both "Enable live transcription service" and "Save Captions"
   - This lets CueBoard find `.vtt` transcript files for export

3. **Breakout Rooms** (for testing, though not currently mapped to a button)
   - Settings → In Meeting (Basic) → Breakout Room
   - Toggle **ON**
   - Check "Allow host to assign participants to breakout rooms when scheduling"

4. **Polling/Quizzes** (for Interactive Poll button on Page 3)
   - Settings → In Meeting (Basic) → Polling
   - Toggle **ON**
   - You'll create polls in the meeting scheduler (see below)

5. **Meeting Lock** (for Page 2, Button 2)
   - Settings → In Meeting (Basic) → Lock Meeting
   - Should be enabled by default
   - Allows you to lock the meeting (no new participants can join)

6. **Screen Sharing** (for Page 1, Button 4)
   - Settings → In Meeting (Basic) → Screen Sharing
   - Set to "Host Only" or "All Participants" (your choice)

---

## Part 2: Schedule a Test Meeting (Pre-Configure Demo Features)

### Create Your Demo Meeting:

1. Go to https://zoom.us/meeting/schedule
2. **Topic:** "CueBoard Demo Test Meeting"
3. **When:** Schedule for your demo rehearsal time
4. **Duration:** 1 hour
5. **Recurring:** No (or "Recurring meeting" if you want to reuse it)
6. **Meeting ID:** Generate Automatically
7. **Passcode:** Optional (but recommended for realism)
8. **Waiting Room:** ✓ Enable (so you can test the Admit button)
9. **Enable join before host:** OFF (so you control when it starts)

### Add Polls for Interactive Poll Testing:

**While still on the meeting scheduler page:**

1. Scroll down to find **"Polls/Quizzes"** section
2. Click **"Create"** or **"Add"**
3. Create 2-3 sample polls:

   **Poll 1: "Demo Readiness Check"**
   - Question: "How excited are you about CueBoard?"
   - Type: Single Choice
   - Options:
     - 🔥 Extremely excited (10/10)
     - 👍 Pretty excited (7-9)
     - 😐 Moderately excited (4-6)
     - 🤔 Needs more convincing (1-3)

   **Poll 2: "Feature Priority"** (Multiple Choice)
   - Question: "Which features would you use most?"
   - Type: Multiple Choice
   - Options:
     - Mute/Camera controls
     - Meeting Intelligence flagging
     - Host power tools (Mute All, Timer)
     - Reactions via dial

   **Poll 3: "Meeting Duration Preference"**
   - Question: "Ideal meeting length?"
   - Type: Single Choice
   - Options:
     - < 30 minutes
     - 30-60 minutes
     - 1-2 hours
     - 2+ hours (I love meetings!)

4. Save each poll
5. Click **"Save"** at the bottom of the meeting scheduler

**Why This Matters:**
When you press the Interactive Poll button during the demo, it will launch one of these pre-configured polls. Judges will see that you can trigger polls from the hardware without navigating Zoom menus.

---

## Part 3: Start Your Test Meeting & Configure On-the-Fly

### When You Launch the Test Meeting:

1. **Start the Meeting**
   - Open Zoom Desktop Client
   - Click "Meetings" → Your scheduled meeting → "Start"
   - CueBoard should auto-detect Zoom is running

2. **Verify Host Controls Are Available**
   - Bottom toolbar should show **"Security"** button (host only)
   - You should see **"Breakout Rooms"** button (host only)
   - **"Manage Participants"** shows host controls

3. **Join from a Second Device** (to test participant features)
   - Use your phone, tablet, or a friend's computer
   - Join using the meeting ID
   - You'll land in the Waiting Room (tests Admit button)

4. **Admit Participant**
   - In Zoom, click "Participants" or use Alt+U (or press CueBoard Page 2, Button 3)
   - You'll see "Tyler's iPhone (Waiting Room)"
   - Click "Admit" to let them in

5. **Enable Live Transcript** (for Export Summary transcript feature)
   - Click **"Captions"** button (or Alt+C)
   - Select **"Enable Auto-Transcription"**
   - Zoom will now generate a `.vtt` transcript file
   - CueBoard's Export Summary will auto-find this file in `Documents/Zoom/`

---

## Part 4: Testing CueBoard Features

### Page 1: Live Controls — Test Checklist

| Button | What to Test | How to Verify |
|--------|--------------|---------------|
| **Mute** | Press button | Zoom shows red muted icon; button shows "MUTED" |
| **Camera** | Press button | Video turns off; button shows "OFF" |
| **Recording** | Press button (host only) | Zoom shows red "Recording" banner; button turns red |
| **Screen Share** | Press button | Share picker opens; select a window/screen |
| **Chat** | Press button | Chat panel slides in from right |
| **Reaction** | **ROTATE dial first**, then press button | Selected reaction appears above your video |
| **Raise Hand** | Press button | Hand icon appears next to your name |
| **Gallery/Speaker** | Press button | View toggles between modes |
| **End Meeting** | Press button | Zoom prompts "End meeting for all?" |

**Reaction Dial Deep Test:**
1. Rotate dial → Toast shows "Reaction: Thumbs Up"
2. Rotate more → Toast shows "Reaction: Heart"
3. Rotate more → Toast shows "Reaction: Laugh"
4. Rotate more → Toast shows "Reaction: Tada"
5. **Now press the button** → Selected reaction sends
6. Try pressing dial instead of button → Same reaction sends

---

### Page 2: Operator Mode — Host-Only Features

**Prerequisites:** You must be the host. Some features need participants in the meeting.

| Button | What to Test | How to Verify |
|--------|--------------|---------------|
| **Mute All** | Have participant join; press button | Participant gets muted; Zoom shows "Host muted you" |
| **Lock Meeting** | Press button | Zoom toolbar shows 🔒 icon; new participants can't join |
| **Admit** | Have someone wait in waiting room; press button | Participants panel opens showing waiting room list |
| **Fullscreen** | Press button | Zoom enters fullscreen (F11-style); press again to exit |
| **Pause Share** | Start sharing; press button | Share freezes but stays visible |
| **Timer Dial** | Rotate dial | Toast shows minutes (e.g., "Timer: 3 min"); button updates |
| **Timer Button** | Press button after setting duration | Floating overlay appears with countdown; button shows time |
| **Timer Pause** | Press timer button while running | Timer pauses; button shows ❚❚ symbol |
| **Timer Resume** | Press timer button while paused | Timer continues from remaining time |
| **Clear Timer** | Press while timer is running | Overlay closes; timer resets to idle state |

**Timer Test Flow:**
1. Rotate dial → Set to 2 minutes
2. Press Timer button → Overlay appears, countdown starts
3. Wait 30 seconds → Verify overlay counts down
4. Press Timer button again → Pauses at ~1:30
5. Press Timer button again → Resumes from 1:30
6. Press Clear Timer → Overlay closes immediately

---

### Page 3: Meeting Intelligence — Flagging & Export

**Prerequisites:** Have a meeting running with captions enabled (for transcript integration).

| Button | What to Test | How to Verify |
|--------|--------------|---------------|
| **Flag Moment Dial** | Rotate dial | Toast shows "Flag: Action Item" → "Decision" → "Follow-Up" → "Bookmark" |
| **Flag Moment Button** | Set dial to "Decision"; press button | Toast shows "Decision flagged"; button counter increments |
| **Action Item** | Press button | Toast shows "Action Item flagged" (quick-flag shortcut) |
| **Assign** | Press Assign; rotate dial; press dial | Toast shows participant names; pressing dial assigns flag to selected person |
| **Add Note** | Flag something; press Add Note | Input dialog appears; type note; press Enter; toast confirms |
| **Highlight** | Press button | Toast shows "Highlight added" (lighter than a flag) |
| **Preview** | Press button | Button display shows "VIEW (3)" where 3 = flag count |
| **Preview (double)** | Press twice quickly (< 2 sec) | Button shows "DEMO LOADED" in purple; demo flags appear |
| **Export Summary** | Press button | Browser opens with HTML summary; flags grouped by type; button shows "SAVED!" |
| **Email Recap** | Press button | Default mail client opens with draft email containing flag summary |

**Full Page 3 Workflow Test:**
1. Start meeting, enable captions
2. Rotate Flag Moment dial → Select "Action Item"
3. Press Flag Moment button → "Action Item flagged"
4. Press Assign button → Rotate dial to "Sarah" → Press dial → Assigned
5. Press Add Note → Type "Follow up on API design" → Enter
6. Press Flag Moment button again → Another flag
7. Press Preview → Button shows "VIEW (2)"
8. Talk for 30 seconds (generates transcript)
9. Press Export Summary → Browser opens with HTML export showing both flags + transcript snippet

---

## Part 5: Testing Interactive Poll (Page 3)

**Prerequisites:** Polls must be created during meeting scheduling (see Part 2).

### How to Test:

1. **Launch the Poll from CueBoard:**
   - Press the **Interactive Poll button** on Page 3
   - CueBoard sends the keyboard shortcut to open the poll panel

2. **Launch the Poll from Zoom (manual fallback):**
   - Click "Polls" button in Zoom toolbar (bottom)
   - Select one of your pre-configured polls
   - Click "Launch Poll"

3. **Participants Vote:**
   - Participants see the poll on their screen
   - They select options and click "Submit"

4. **End the Poll:**
   - Click "End Poll" in Zoom
   - Results appear for everyone
   - You can share results or re-launch

**Demo Script Suggestion:**
> "Here's where CueBoard integrates with Zoom's existing features. I've pre-configured a poll asking which features you'd use most. Watch as I launch it from the hardware without touching my mouse. [Press Interactive Poll button]. Participants can vote now, and when I end the poll, we'll see live results."

---

## Part 6: Breakout Rooms (Currently Unmapped)

Breakout rooms don't have a dedicated CueBoard button yet, but you can pre-assign them:

### Pre-Assign Breakout Rooms:

1. During meeting scheduler, click **"Breakout Room - Pre-assign"**
2. Create rooms:
   - Room 1: "Design Team"
   - Room 2: "Dev Team"
   - Room 3: "Marketing"
3. Assign participants (if you know their email addresses)
4. Save

**In the Live Meeting:**
- Click "Breakout Rooms" button in Zoom toolbar
- Your pre-assigned rooms appear
- Click "Open All Rooms" to send participants to their rooms

**Demo Suggestion:**
If you add a Breakout Room button to CueBoard later (Page 2 has open slots), it could:
- Open the breakout rooms panel (keyboard shortcut: Alt+Shift+B? — verify in Zoom shortcuts)
- Or automatically "Open All Rooms" if rooms are pre-configured

---

## Part 7: Transcript & Export Testing

### How to Get a Transcript File:

1. **Enable Live Transcript in the meeting:**
   - Click "Captions" button → "Enable Auto-Transcription"
   - Zoom generates a `.vtt` file while you talk

2. **Where Zoom Saves Transcripts:**
   - `C:\Users\[YourName]\Documents\Zoom\`
   - Or `Desktop`, `Downloads`, or custom recording folder

3. **Test CueBoard's Auto-Detection:**
   - Talk for 1-2 minutes to generate transcript content
   - Press **Export Summary** button on Page 3
   - CueBoard scans `Documents`, `Desktop`, `Downloads` for the newest `.vtt` file
   - HTML export includes a **Transcript Panel** on the right side with timestamped speaker text

4. **What the Export Looks Like:**
   - Dark-theme HTML page
   - Stats dashboard (flag counts by type)
   - Flags grouped by type (Action Items, Decisions, etc.)
   - Each flag shows timestamp, assignee, note
   - Transcript sidebar with speaker names + text
   - Print/Email/Download buttons at top

---

## Part 8: Common Testing Pitfalls

### "The button does nothing!"
- **Check:** Is Zoom actually running? CueBoard's detection is process-based.
- **Check:** Are you the host? Many Page 2 features require host privileges.
- **Fix:** Use the manual override in settings if needed.

### "Reactions only send Clap"
- **Cause:** You're pressing the button without rotating the dial first.
- **Fix:** Rotate the dial to select a reaction, THEN press button or dial.

### "Timer overlay doesn't appear"
- **Check:** Is PowerShell execution policy blocking scripts?
- **Fix:** Run `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` in PowerShell (admin).

### "Polls don't launch"
- **Cause:** Polls weren't pre-configured in the meeting scheduler.
- **Fix:** Edit the meeting on zoom.us and add polls before starting.

### "Export Summary doesn't include transcript"
- **Cause:** Captions weren't enabled, or the `.vtt` file is in a different folder.
- **Fix:** Enable "Live Transcription" in the meeting, wait 1-2 minutes for content, then export.

### "Admit button doesn't admit participants"
- **Cause:** Alt+U opens the participants panel; you still need to click "Admit" manually.
- **Clarification:** CueBoard opens the panel for you (faster than mouse navigation). It doesn't auto-admit because that would be dangerous (you might not want to admit everyone).

---

## Part 9: Quick Pre-Demo Checklist

**30 Minutes Before Demo:**

- [ ] Zoom scheduled meeting created with polls
- [ ] Waiting room enabled in Zoom settings
- [ ] Live transcription enabled in Zoom settings
- [ ] Second device ready to join as participant (phone, tablet, friend)
- [ ] CueBoard plugin loaded and running
- [ ] Test meeting started; Zoom detected by CueBoard
- [ ] Participant joined and sitting in waiting room
- [ ] Admitted participant using CueBoard button (Alt+U → click Admit)
- [ ] Reactions tested: dial rotation confirmed (toast notifications visible)
- [ ] Timer tested: set duration, start, pause, resume, clear
- [ ] Flags tested: create, assign, add note, export
- [ ] Export HTML opened in browser successfully
- [ ] Interactive poll launched and tested with participant vote

---

## Part 10: Demo Day Emergency Checklist

**If something breaks during the live demo:**

1. **Zoom not detected:**
   - Close and reopen Zoom Desktop Client
   - Restart CueBoard plugin
   - Use simulation mode if detection fails

2. **Button doesn't respond:**
   - Check if you're the host (for Page 2 features)
   - Check if meeting is active
   - Fall back to manual keyboard shortcut (you have ZOOM-SHORTCUTS.md)

3. **Export Summary fails:**
   - Close browser, try again
   - Mention "This normally works; I've exported 50+ times in testing"
   - Show a backup HTML export file you generated earlier

4. **Interactive Poll doesn't launch:**
   - Manually click Polls button in Zoom
   - Explain: "The button triggers the same keyboard shortcut I'm using now"

5. **Reaction dial not cycling:**
   - Remind judges: "Rotate first, then press"
   - Show toast notifications appearing as you rotate

---

## What Judges Need to Understand

When demoing features that aren't standard Zoom shortcuts, explain:

### Interactive Poll:
> "This button launches a poll I pre-configured in Zoom's scheduler. The hardware replaces the need to dig through Zoom's menus. For teams that run workshops or training sessions, this means you can trigger audience engagement tools without breaking eye contact or leaving the conversation."

### Export Summary:
> "This isn't a Zoom feature—it's CueBoard's own meeting intelligence system. The flags I created during this meeting are now exported as a formatted HTML document with timestamps, assignments, and notes. It auto-detects Zoom's transcript file and integrates it, so you get your curated highlights alongside the AI-generated transcript. Think of it as the director's cut vs. the raw footage."

### Timer:
> "This is CueBoard's internal timer, not Zoom's. It appears as a floating overlay on my screen. Workshop facilitators can set time-boxed activities—rotate the dial to set duration, press to start, press again to pause. No external timer app needed."

---

## Summary

**You're ready to demo when:**
- ✅ Zoom account settings configured (waiting room, captions, polls, etc.)
- ✅ Test meeting scheduled with pre-configured polls
- ✅ Second device ready to act as participant
- ✅ All 3 pages tested end-to-end with real Zoom interactions
- ✅ You've rehearsed the full demo script with working features
- ✅ Backup HTML export file saved in case live export fails
- ✅ You understand how to explain CueBoard-specific features (Export, Timer, Polls)

**Most importantly:**
Judges don't care if every feature is perfect. They care that you **demonstrate value**, **show polish where it matters**, and **understand your user's pain points**. Page 1 + Page 3 working flawlessly is better than all 3 pages half-working.

---

Good luck! 🎯
