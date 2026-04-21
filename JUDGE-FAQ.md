# CueBoard Judge FAQ — Understanding the Features

> **Purpose:** Quick reference for explaining CueBoard features to hackathon judges.
> Focus on what makes CueBoard different from "just a programmable keyboard."

---

## The Core Pitch

**CueBoard is the first physical control surface purpose-built for virtual meetings.**

It's not a macro keyboard. It's a meeting co-pilot that combines:
1. **Instant access** to Zoom controls (no mouse, no menus)
2. **Meeting intelligence** (flag important moments in real-time)
3. **Host power tools** (timers, participant management)
4. **Physical feedback** (see your state at a glance)

---

## Q: "Isn't this just keyboard shortcuts?"

**A:** For Page 1 (Live Controls), yes — but the value is **speed, visibility, and muscle memory**.

**The problem:**
- Alt+A to mute? Most people don't know that shortcut.
- Even if you do, you can't tell if you're muted without looking at Zoom.
- Accidentally unmuted while your dog barks = embarrassing.

**What CueBoard solves:**
- **One button = one action.** No modifier keys to remember.
- **Button shows your current state:** "MUTED" or "LIVE" in realtime.
- **Tactile feedback:** You know you hit Mute without checking the screen.
- **Dial for reactions:** Cycle through 5 reactions with one hand, send with a press.

**Analogy:**
It's like the difference between keyboard shortcuts for audio editing vs. a physical mixing board. Both do the same thing, but professionals choose the board because it's faster, more intuitive, and you can see your state at a glance.

---

## Q: "What's the dial do?"

**A:** The dial is context-aware. It does different things on each page.

**Page 1 (Live Controls):**
- Rotate → Cycle through reactions (Clap, Thumbs Up, Heart, Laugh, Tada)
- Press → Send the selected reaction
- Toast notification shows which reaction is selected

**Page 2 (Operator Mode):**
- Rotate → Adjust timer duration (1-60 minutes)
- Press → Start/Pause/Resume timer
- Button display shows duration while idle, countdown while running

**Page 3 (Meeting Intelligence):**
- Rotate → Select flag type (Action Item, Decision, Follow-Up, Bookmark)
- Press → Confirm flag
- When in Assign mode: Rotate → Select participant, Press → Assign flag

**Why this matters:**
The dial isn't tacked on — it's essential. Every page uses it meaningfully. Logitech's hosting this hackathon; showing deep integration with their hardware differentiator (the dial) demonstrates respect for the platform.

---

## Q: "What's Meeting Intelligence? Is that AI?"

**A:** No AI. It's **human curation** during the meeting.

**The problem:**
Tools like Fathom and Fireflies use AI to summarize everything. But AI doesn't know what *you* think matters. It transcribes everything equally.

**What CueBoard does differently:**
- Press "Flag Moment" when something important happens (a decision, an action item, a key insight).
- The button timestamps it, categorizes it (via dial), and optionally assigns it to someone.
- At the end of the meeting, "Export Summary" generates a formatted HTML document with:
  - All your flags grouped by type
  - Timestamps linking to Zoom's AI transcript (if enabled)
  - Assignees and notes
  - Stats dashboard

**The value:**
- **Instant:** Flag moments as they happen, without breaking eye contact or typing notes.
- **Non-disruptive:** Physical button press is silent and doesn't require alt-tabbing.
- **Complements AI:** Fathom gives you the raw footage. CueBoard gives you the director's cut.
- **Actionable:** Export includes assignments, not just summaries.

**Analogy:**
It's like bookmarking important moments in a video while you're watching it, so you can skip straight to the good parts later.

---

## Q: "How does the export work? Does it use an API?"

**A:** No cloud, no API, no external dependencies.

**How it works:**
1. Flags are stored in-memory during the meeting (timestamp + type + assignee + note).
2. When you press "Export Summary," CueBoard:
   - Generates an HTML file (dark theme, professional formatting)
   - Auto-detects Zoom's `.vtt` transcript file (if captions were enabled)
   - Parses the transcript and includes relevant snippets alongside each flag
   - Opens the HTML in your default browser
3. The HTML file is saved locally and can be emailed, printed, or shared.

**What's included in the export:**
- **Stats dashboard:** "3 Action Items, 2 Decisions, 1 Follow-Up"
- **Flags grouped by type:** Color-coded sections with timestamps
- **Transcript integration:** Expandable sidebar showing speaker + text
- **Action buttons:** Print, Email Draft, Download

**Why no cloud?**
- **Privacy:** No meeting data leaves your machine.
- **Reliability:** Works offline. No network = no failure point.
- **Hackathon-friendly:** One less thing to debug on stage.

---

## Q: "What's the Timer for?"

**A:** The timer is CueBoard's own countdown feature for meeting facilitators.

**Use cases:**
- Workshop leader: "You have 5 minutes to discuss in breakout groups"
- Standup meeting: "Each person gets 2 minutes"
- Time-boxed brainstorming: "10 minutes to generate ideas"

**How it works:**
1. Rotate dial → Set duration (1-60 minutes)
2. Press Timer button → Floating overlay appears on screen with countdown
3. Press again → Pause (overlay freezes)
4. Press again → Resume from where you paused
5. Press Clear Timer → Reset everything

**Why it's better than a phone timer:**
- **Visible to everyone:** The overlay is on your shared screen (if you're sharing)
- **Color-coded:** White → Orange (< 30 sec) → Red (< 10 sec)
- **One-handed control:** Set, start, pause, clear without touching a mouse
- **No alt-tabbing:** Timer overlay stays on top, doesn't block your work

**What judges see:**
A floating window with large countdown text, color-changing as time runs out, controlled entirely from the hardware dial and button.

---

## Q: "What's Interactive Poll? Did you integrate with Zoom's API?"

**A:** No API. CueBoard uses keyboard shortcuts to open Zoom's built-in polling feature.

**How it works:**
1. Before the meeting, you pre-configure polls in Zoom's meeting scheduler (web interface).
2. During the meeting, pressing the Interactive Poll button sends the keyboard shortcut to open the poll panel.
3. You select a poll and launch it (or CueBoard can auto-launch the first poll if scripted).
4. Participants vote using Zoom's standard polling interface.

**Why this is valuable:**
- **Speed:** Launch a poll without navigating Zoom's menus.
- **Professionalism:** No fumbling through UI during a live demo or training session.
- **Works with existing features:** You're not rebuilding polling — you're making it accessible.

**Demo script suggestion:**
> "I've pre-configured a poll asking which features you'd use most. Watch as I trigger it from the hardware. [Press button]. Zoom's poll launches instantly. This is especially useful for workshop leaders who run frequent audience engagement activities."

---

## Q: "What's Assign? How does that work?"

**A:** Assign attaches a flag to a person or department.

**Workflow:**
1. Flag a moment (e.g., "Action Item")
2. Press Assign button
3. Rotate dial → See participant names (Sarah, Mike, Jordan, Dev Team, Marketing, Ops)
4. Rotate to "+" → Opens input dialog to add a new name
5. Press dial → Assigns the last flag to the selected person

**Where the names come from:**
- **Default list:** Pre-configured team members/departments (defined in code)
- **Dynamic additions:** Press "+" on the dial to add names on-the-fly via input dialog
- **Could pull from Zoom participant list in v2** (not implemented for hackathon)

**What it shows in the export:**
Each flag displays:
```
🎯 Action Item  |  3:47 PM
Assigned to: Sarah
Note: Follow up on API design decisions
```

**Why this matters:**
Flags without assignments are just notes. Flags with assignments are *action items*. CueBoard makes it fast to assign during the meeting (no typing, no breaking flow).

---

## Q: "What's the Preview button do?"

**A:** Preview updates the button's display to show flag stats.

**Behavior:**
- **Single press:** Button shows "VIEW (5)" where 5 = total flag count
- **Double press (< 2 seconds):** Loads demo data (for testing without a real meeting)

**Why this exists:**
- Quick visual confirmation of how many flags you've created
- Demo data loader helps with testing and stage demos (if live meeting fails)
- Console-based feedback (you see stats on the hardware, not just in the export)

**This is a status button, not an action button.** It doesn't open a window — it just updates its own display.

---

## Q: "Does it work with Teams or Google Meet?"

**A:** Not yet. Zoom only for the hackathon.

**Why Zoom-only?**
- **Focus over breadth:** One polished platform beats three broken ones.
- **Time constraints:** Hackathon deadline prioritizes depth.
- **Personal use case:** Tyler (the builder) primarily uses Zoom.

**Post-hackathon roadmap:**
- Teams support (same architecture, different keyboard shortcuts)
- Google Meet support
- Platform-agnostic features (Timer, Export work regardless of meeting platform)

---

## Q: "How do you know which meeting platform is active?"

**A:** Process detection + manual override.

**How it works:**
1. CueBoard scans running processes for "Zoom.exe" (Windows)
2. If found → Zoom shortcuts enabled
3. If not found → Simulation mode (logs actions but doesn't send keystrokes)
4. Manual override available in settings (for demos without Zoom running)

**Why not scan for window titles?**
- Process detection is more reliable
- Works even if Zoom is minimized or on another monitor
- Doesn't require polling active windows (lighter on resources)

---

## Q: "What happens if someone presses the wrong button?"

**Safety features:**

1. **Dangerous actions require confirmation:**
   - "End Meeting" prompts Zoom's native "End for all?" dialog
   - "Mute All" is a one-way action (unmuting requires participants to unmute themselves)

2. **Undo features:**
   - "Clear Last Flag" removes the most recent flag (Page 3)
   - "Reset Meeting" clears all flags + state (Page 3)

3. **Toast notifications:**
   - Every action shows a confirmation toast (top-right corner, 2-second fade)
   - "Muted," "Flag added," "Timer started," etc.
   - Provides immediate feedback that the action succeeded

4. **Visual state indicators:**
   - Buttons show current state: "MUTED" vs "LIVE", "Recording" indicator, etc.
   - Timer button shows countdown while running
   - Flag count updates live on button display

---

## Q: "Why not use a Stream Deck or Elgato?"

**A:** You could, but CueBoard targets the **Logitech MX Creative Console** for specific reasons:

**Hardware advantages:**
1. **The dial:** Stream Deck doesn't have a dial. The dial enables continuous adjustment (reactions, timer duration, participant selection) that buttons can't replicate.
2. **Professional design:** The MX Creative Console is designed for productivity workflows, not just streaming. It fits on a desk next to a keyboard.
3. **Logitech ecosystem:** The hackathon is hosted by Logitech. Demonstrating deep integration with their hardware shows respect for the platform.

**Software advantages:**
- CueBoard isn't just button macros — it has internal state management, session tracking, and export generation.
- Stream Deck plugins are typically simpler (trigger a shortcut, display an icon). CueBoard tracks meeting state, timer countdowns, flag assignments, etc.

**That said:** A Stream Deck version could exist post-hackathon. The core logic (flagging, export, timer) is platform-agnostic.

---

## Q: "How is this better than just learning keyboard shortcuts?"

**A:** Three reasons:

### 1. Discovery & Accessibility
Most people don't know Alt+A mutes. Even fewer know Alt+R records, or Alt+M mutes all participants. CueBoard makes these features **discoverable** (labeled buttons) and **accessible** (no modifier keys).

### 2. Cognitive Load
During a stressful meeting (presenting to a client, facilitating a workshop, handling a conflict), you don't want to think "What was that shortcut again?" Physical buttons create **muscle memory**. You *know* where Mute is without looking.

### 3. State Visibility
Keyboard shortcuts don't show feedback. CueBoard buttons **display current state**:
- "MUTED" or "LIVE"
- "Recording" with red indicator
- Timer countdown: "2:34"
- Flag count: "Flags: 5"

**This is the control surface advantage.** Like how DJs use hardware controllers even though software has all the same features. Speed + tactile feedback + state visibility = professional workflow.

---

## Q: "What's your target user?"

**Primary:**
- **Meeting-heavy professionals:** Product managers, team leads, consultants, workshop facilitators
- **People who spend 10+ hours/week in virtual meetings**
- **Users who value speed and professionalism** (no fumbling with menus during client calls)

**Secondary:**
- **Content creators** who live-stream meetings or workshops (Timer overlay is useful for time-boxed segments)
- **Executive assistants** managing others' meetings (host power tools like Mute All, Lock Meeting)
- **Training facilitators** running workshops (Interactive Poll, Timer, Meeting Intelligence)

**Not for:**
- Casual users who attend 1-2 meetings per week (not worth the hardware investment)
- Users who don't have the Logitech MX Creative Console

---

## Q: "What's the business model?"

**Not built for the hackathon scope, but here's the thinking:**

### Option 1: Free Plugin (Community Growth)
- CueBoard is free forever
- Drives adoption of Logitech hardware ("I want that console to use CueBoard")
- Monetize through premium templates, training courses, or enterprise features later

### Option 2: Freemium SaaS (Not Cloud-Based Yet)
- Free tier: Page 1 (Live Controls)
- Paid tier ($5/mo): Pages 2 + 3 (Host tools + Meeting Intelligence)
- No cloud backend for hackathon, but could add sync/backup features later

### Option 3: One-Time Purchase
- $19.99 one-time license
- Unlocks all features forever
- No subscription fatigue

**What judges need to know:**
The product isn't designed to make money *yet*. The hackathon goal is to prove **the concept has value**. Monetization comes later if users love it.

---

## Q: "What's the biggest technical challenge you solved?"

**Answer 1: Async Input Dialogs**
PowerShell-based input dialogs that don't block the plugin's main thread. Used for "Add Note" and "Assign → +" (add new participant). The challenge was making a UI element appear *outside* the plugin while maintaining async/await for responsiveness.

**Answer 2: Transcript Integration**
Auto-detecting Zoom's `.vtt` transcript files across multiple possible folders (Documents, Desktop, Downloads), parsing the VTT format (speaker + timestamp + text), and injecting relevant snippets into the HTML export next to each flag.

**Answer 3: Timer Resume Logic**
When the timer is paused, CueBoard needs to remember *remaining seconds* (not original duration). On resume, it calculates elapsed time from `DateTime.UtcNow` to ensure accuracy even if the system clock drifts. The overlay window also needs to be killed and restarted with the new duration.

**Answer 4: State-Driven Image Refresh**
Buttons update their display based on meeting state (muted, recording, etc.). The challenge was preventing "refresh storms" (re-rendering every button every frame). Solution: Event-driven architecture — buttons only refresh when `RefreshAllImages` event fires after meaningful state changes.

---

## Q: "What would you build next if you had more time?"

**Version 2.0: Multi-Platform (Next 3 months)**
- Teams and Google Meet support with full feature parity
- Platform auto-detection (seamlessly switch between meeting apps)
- Pull participant list from Zoom/Teams API for smarter Assign feature

**Version 2.5: Audio Playback & Whisper Integration (6 months)**
This is the big one:
- **Audio playback** — Click flag timestamp → Audio plays from that moment
- **Whisper speech-to-text** — Replace Zoom's captions with OpenAI's Whisper
- **Why Whisper?** State-of-the-art accuracy, runs locally (no cloud), better speaker detection, handles technical jargon
- **Local processing** — Privacy-first, no API costs
- Link flags to Zoom's local recordings (MP4/M4A files)
- Waveform visualization alongside transcript

**Version 3.0: AI-Powered Intelligence (12 months)**
- Meeting summarization (Ollama + Llama 3, local LLM)
- AI suggests what should be flagged (action item detection)
- Sentiment analysis (highlight moments of agreement/disagreement)
- Speaker insights (talk time, participation metrics)

**Version 3.5: Workflow Integration (18 months)**
- Calendar sync (Google/Outlook, pre-populate participants)
- CRM integration (export to Salesforce, HubSpot, Notion)
- Slack/Teams posting (share flags to channels)
- Meeting templates (recurring meeting layouts)

**Version 4.0: Collaboration (24 months)**
- Cloud sync (access flags on mobile, web dashboard)
- Shared meeting boards (multiple users flag the same meeting)
- Real-time collaboration (see others' flags live)
- Meeting archives (search across past meetings)

**Hardware expansion:**
- Stream Deck plugin
- Mobile companion app
- API for custom control surfaces

---

## Summary: Key Points for Judges

**What CueBoard is:**
- Physical control surface for virtual meetings (Zoom)
- Combines instant access (keyboard shortcuts), meeting intelligence (flagging), and host tools (timer, participant management)
- Built for the Logitech MX Creative Console (dial is essential, not an afterthought)

**What makes it different:**
- **Not just macros:** State tracking, session management, HTML export generation
- **Not replacing AI tools:** Complements Fathom/Fireflies with human curation
- **Not cloud-dependent:** Everything local, works offline, no privacy concerns

**What works right now:**
- All Page 1 buttons (Live Controls)
- 6 of 7 Page 2 buttons (Operator Mode) — missing Min/Max Window
- All 13 Page 3 commands (Meeting Intelligence)
- Timer with floating overlay
- Export with transcript integration
- Toast notifications for every action

**What needs polish:**
- Button icons (text-only currently)
- Demo rehearsal
- Live testing in a real Zoom call

**The pitch:**
> "Virtual meetings are now a permanent part of work. We've optimized our keyboards, mice, monitors, and chairs. Why not optimize our *meeting interface*? CueBoard turns meetings from a necessary evil into a controlled, professional workflow."

---

Good luck! 🚀
