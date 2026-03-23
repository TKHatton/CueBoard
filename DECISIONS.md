# CueBoard — Design Decisions

> Every decision here was made for a reason. Before changing something,
> read WHY it was decided this way. If you still disagree, update the
> decision AND the reasoning.

---

## Dial Is a First-Class Feature
**Decision:** Every page must have meaningful dial behavior.
**Why:** Logitech is hosting the hackathon. The dial is their hardware differentiator. If we don't use it well, we're ignoring what they're most proud of. Judges will notice. The dial must feel essential, not tacked on.

## Only Zoom — No Exceptions
**Decision:** Only Zoom gets full implementation. Teams and Google Meet are empty stubs.
**Why:** Trying to support 3 platforms in a hackathon guarantees 3 mediocre implementations. One polished Zoom integration beats three broken ones. Tyler primarily uses Zoom.

## Reactions Use Dial, Not Separate Buttons
**Decision:** One Reaction button on Page 1. Dial cycles through reaction types (thumbs up, clap, heart, laugh, cheer). Press dial to send.
**Why:** Reactions are popular but don't deserve 5 separate buttons. The dial approach gives access to all reactions while only using 1 button slot. Also demonstrates dial value to judges.

## "Add Participants" Means Admit from Waiting Room
**Decision:** This is on Page 2 (Operator Mode), not Page 1.
**Why:** Tyler clarified he meant admitting latecomers from the waiting room, not sending invite links. This is a host action, so it belongs on the host page. It's something hosts do dozens of times per meeting.

## Host Transfer Stays In
**Decision:** Keep Host Transfer as a button (Page 2).
**Why:** Tyler experiences this regularly. Common scenario: host leaves early and needs to hand off to a specific deputy. Also solves the "I can't share my screen because I'm not host" problem. Not universally common but very painful when needed.

## Remove Participant Needs Two Steps
**Decision:** Removing a participant requires dial-to-select THEN button-to-confirm.
**Why:** Tyler was nervous about accidentally kicking the wrong person. A single-press kick is too dangerous. The two-step process (dial to find the person, then confirm) feels safe while still being faster than the Zoom menu.

## Page 3: One Flag Button + Dial Type Selection
**Decision:** Replace four similar flag buttons (Action Item, Decision, Follow-Up, Bookmark) with one "Flag Moment" button + dial selects the type.
**Why:** Tyler correctly identified that Action Item, Decision, Follow-Up, and Bookmark all feel the same when you're pressing them live. In the heat of a meeting, you just want to say "THAT was important." The category can be selected before or after via the dial. This also frees up button slots for more useful features.

## Page 3 Complements AI Tools, Doesn't Replace Them
**Decision:** Position CueBoard's flagging as the "director's cut" vs. Fathom/Fireflies' "raw footage."
**Why:** Tyler uses Fathom. AI meeting tools capture everything and summarize algorithmically. CueBoard captures what the HUMAN thinks matters. They work together — Fathom for the full transcript, CueBoard for your curated highlights. Physical buttons make flagging instant and non-disruptive (no mouse, no screen switching, no breaking eye contact).

## Timer Is CueBoard's Own, Not Zoom's
**Decision:** The countdown timer on Page 2 is internal to CueBoard.
**Why:** It appears as a visual countdown on the console's screen. Dial sets duration, press starts/pauses, button resets. Could auto-send a chat message when it hits zero. This is a facilitator tool — workshop leaders constantly need timers.

## Virtual Backgrounds — On the Wishlist
**Decision:** Not currently assigned a button, but noted as a strong candidate for an open slot.
**Why:** Tyler uses different backgrounds for different organizations. Quick switching would be practical. If a button slot opens up, this is a top contender.

## Screen Share Is Complex
**Decision:** Noted as a pain point but keeping the simple toggle for now.
**Why:** Real screen sharing involves choosing whole screen vs. window vs. tab, then which monitor (Tyler has 3). A single button can start/stop sharing, but the selection problem is hard to solve on a 9-button console. Could use the dial to cycle through share targets in a future version.

## No Cloud, No AI, No Backend
**Decision:** Everything runs locally.
**Why:** This is a hackathon prototype. Adding network dependencies creates failure points. Local-only means the demo works on stage even if WiFi dies. Keep it simple, keep it reliable.

## Simulation Mode by Default
**Decision:** All hardware interactions (keyboard shortcuts, screenshots) run in simulation mode.
**Why:** Development and demos can happen without Zoom actually running. The simulator logs what WOULD happen. When connected to real hardware, just flip a flag.
