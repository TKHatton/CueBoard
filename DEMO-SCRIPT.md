# CueBoard — 3-Minute Demo Script

> **Hard rule:** every action below has been verified working on Tyler's setup. Nothing aspirational, nothing flaky. If it's in this script, it works.
> **Pre-demo checklist** at the bottom — run through it once before recording.

---

## Cold open (0:00 – 0:15)

**On screen:** Tyler holding the Logitech MX Creative Console, Zoom meeting visible.

**Voiceover:**
> "There are control surfaces for music, for streaming, for video editing. There has never been one for virtual meetings. Until now. This is CueBoard."

*Lift the device toward camera. Hold for 1 beat.*

---

## Act 1 — The basics, fast (0:15 – 0:50)

**Goal:** Show that everyday Zoom controls become physical.

Press in this order, one beat each, no hesitation:
1. **Mute** — button shows MUTED
2. **Mute** again — button shows LIVE
3. **Camera** — off
4. **Camera** — on
5. **Raise Hand**
6. **Reaction** — clap appears in Zoom

**Voiceover under the actions:**
> "Mute. Camera. Hand. React. The things you fumble for in toolbars are now muscle memory."

*Beat. Don't linger.*

---

## Act 2 — The host's tools (0:50 – 1:25)

**Goal:** Show this isn't just a toy — hosts get real power.

Switch to **Page 2**.

1. **Mute All** — toast: "All Muted"
2. **Pause Share** *(if you're sharing — otherwise skip)*
3. **Minimize** — Zoom drops into floating thumbnail mode
4. *(restore Zoom from taskbar)*
5. **Copy Invite** — Zoom invite dialog appears, click Copy invite link
6. **Timer** — countdown overlay appears top-center; numbers turn yellow under 25s

**Voiceover:**
> "If you run meetings, you get a control room. Mute everyone. Pause your share without scrambling. A timer everyone can see during screen share."

---

## Act 3 — The hero moment (1:25 – 2:25)

**Goal:** This is where CueBoard becomes the only product like it. Slow down here.

Switch to **Page 3**.

1. **Rotate the Flag Type dial** — toast cycles: Action Item → Decision → Follow-Up → Bookmark.
   - *Voiceover:* "Now I'm in the meeting. Someone makes a decision."
2. *(Land on Decision)*
3. **Press Flag Moment** — **big BLUE flag overlay flashes top-right**
4. **Rotate dial to Action Item.** Press Flag Moment. **Purple flag overlay.**
5. **Press Add Note** — dark dialog appears, cursor already in field. Type "Sarah owns Q3 report by Friday." Press Enter.
6. **Rotate dial to Follow-Up.** Press Flag Moment. **Green flag.**
7. **Press Highlight.** **Gold flag.** *Beat.*

**Voiceover during 4-7:**
> "Every flag is timestamped. Every type has a color. Every moment is captured without typing during the conversation."

8. **Press Export Summary.**

A branded HTML page opens in browser. Show:
- The stat boxes at top (counts by type)
- Click a flag's purple timestamp pill — transcript panel slides open and highlights the actual moment from the live captions

**Voiceover:**
> "Every flag links back to the exact second of the transcript. The summary is shareable, printable, emailable — instantly."

*Pause for 1 beat on the export page. Let it land.*

---

## Act 4 — The crowd (2:25 – 2:55)

**Goal:** Show that CueBoard isn't just for the host — it pulls the room in.

1. **Press Interactive Poll.** Browser opens to the live engage page. The share URL is already in your clipboard.
2. **Switch to Zoom chat. Ctrl+V. Send.**
3. *(Cut to phone or second screen showing an attendee opening the link, voting on a poll, adding a word to the cloud.)*
4. *(Cut back to your screen — the live page updates with their vote.)*

**Voiceover:**
> "One press. Engagement link in the chat. Anyone with the link can vote, leave a word in the cloud, rate the session. Everyone in the room. From any device."

---

## Close (2:55 – 3:00)

**On screen:** Hold the device. Steady shot.

**Voiceover:**
> "CueBoard. The first physical control surface purpose-built for virtual meetings."

*Logitech logo + CueBoard wordmark. End.*

---

## Pre-demo checklist (run before each take)

**Before opening Zoom:**
- [ ] Logi Options+ is open and CueBoard plugin shows no caution symbols
- [ ] Dialpad batteries fresh (test the Show Actions Ring button)
- [ ] Timer set to default (~5 min) — no need to rotate dial during demo

**In Zoom Workplace, before joining the demo meeting:**
- [ ] Open the Transcript panel (top-right toolbar) so a `.txt` is saved on click
- [ ] Disable other shortcut-stealing apps (close Slack, Discord, Teams)
- [ ] Set Zoom window as the focused window before pressing Page 2 buttons
- [ ] Have a second device (phone) ready to receive the engage URL

**Engage page (do once before recording day):**
- [ ] Open `https://tkhatton.github.io/CueBoard/engage.html` directly in your browser
- [ ] Fill in: word cloud prompt, 2-3 poll questions, rating prompt, feedback email
- [ ] Click **Generate & Copy Share Link** — this saves the config to localStorage
- [ ] During the demo, **Interactive Poll** auto-reads localStorage and copies the share URL

**Right before pressing record:**
- [ ] At end of meeting, click **Save transcript** in Zoom's Transcript panel — file lands in `OneDrive\Documents\Zoom\[date]\meeting_saved_closed_caption.txt`
- [ ] If the demo doesn't include a real meeting first, press **Load Transcript** on CueBoard and pick a saved `.txt` from a previous meeting so flag-link demo works

**Failsafe:** if a flag overlay doesn't fire mid-take, keep going — a missed overlay is recoverable. The export at the end is the moment that has to land.
