# CueBoard Branding & Icon Guide

## Brand Colors

### Primary Purple (Used Throughout Plugin)
**RGB:** `139, 92, 246`
**Hex:** `#8B5CF6`
**Usage:** Accent color for text, highlights, active states, brand identity

### Dark Background
**RGB:** `42, 42, 53`
**Hex:** `#2A2A35`
**Usage:** Button backgrounds, overlays, export HTML

### Additional Colors
- **Red (Alerts/Warnings):** RGB `231, 76, 60` / Hex `#E74C3C`
- **Orange (Timer Warning):** RGB `243, 156, 18` / Hex `#F39C12`
- **White (Text):** RGB `255, 255, 255` / Hex `#FFFFFF`
- **Gray (Inactive Text):** RGB `180, 180, 180` / Hex `#B4B4B4`

---

## Landing Page Branding

**Recommendation:** Update your landing page to match the plugin's purple (`#8B5CF6`).

**Why?**
- The purple `#8B5CF6` is used in 50+ places across the codebase (buttons, toasts, overlays, export HTML)
- Changing the codebase to match your landing page would require updating:
  - 9 command files
  - 5 service files (Toast, Export, Timer Overlay, Input Dialog)
  - All button rendering logic
- Much easier to update one landing page than 50+ code references

**Where to use the purple on your landing page:**
- Primary CTA buttons
- Section headers
- Feature highlights
- Logo accent (if applicable)
- Links and hover states

---

## Current Button Status

### Buttons with PNG Icons (All Good ✓)
**Page 1:**
- Mute (mute-on.png, mute-off.png)
- Camera (camera-on.png, camera-off.png)
- Recording (record-on.png, record-off.png)
- Screen Share (share-on.png, share-off.png)
- Chat (chat.png)
- Reaction (reaction.png)
- Raise Hand (hand-up.png, hand-down.png)
- Gallery/Speaker View (view-gallery.png, view-speaker.png)
- End Meeting (end-meeting.png)

**Page 2:**
- Mute All (mute-all.png)
- Lock Meeting (lock-on.png, lock-off.png)
- Admit (admit.png)
- Pause Share (share-on.png, share-off.png)
- Timer (timer.png)
- Clear Timer (timer-clear.png)
- Min/Max Window (minmax.png) ✓ *Just added*

**Page 3:**
- Flag Moment (flag.png)
- Assign (assign.png)
- Add Note (note.png)
- Highlight (highlight.png)
- Clear Last (clear-last.png)
- Preview (preview.png)
- Export (export.png, export-done.png)
- Poll (poll.png)

### Buttons with Text-Only Display (Improved ✓)
1. **Fullscreen (Page 2)** — Shows "FULL SCREEN" in purple / "EXIT FULL" in gray
2. **Reset Meeting (Page 3)** — Shows "RESET MEETING" in purple / "CONFIRM RESET" in red

These have been updated with cleaner text rendering and proper branding colors.

---

## Creating PNG Icons (If You Want to Replace Text Buttons)

If you want to create PNG icons for Fullscreen and Reset Meeting:

### Icon Specifications
- **Size:** 80x80 pixels (will be scaled by the SDK)
- **Format:** PNG with transparency
- **Background:** Transparent
- **Icon Color:** White (`#FFFFFF`) or Purple (`#8B5CF6`)
- **Style:** Simple, bold, clear at small sizes

### Recommended Tools
1. **Figma** (free, web-based, great for quick icons)
2. **Inkscape** (free, desktop, SVG → PNG export)
3. **Canva** (simple drag-and-drop)
4. **Icon libraries:** Heroicons, Feather Icons, Material Icons (all free)

### Icon Ideas

**Fullscreen Icon:**
- Two opposing arrows pointing outward (⤢ style)
- Four corners with arrows expanding
- Reference: Browser fullscreen button icons

**Reset Meeting Icon:**
- Circular arrow (↻ or ⟳)
- Trash can icon
- Restart/refresh symbol
- Reference: Browser reload/reset icons

### Where to Save Icons
Save PNG files to:
```
C:\Users\ltken\OneDrive\Documents\GitHub\CueBoard\src\CueBoardPlugin\src\Resources\Icons\
```

**File names:**
- `fullscreen-on.png` (when in fullscreen)
- `fullscreen-off.png` (when not in fullscreen)
- `reset.png` (default state)

### Update Commands to Use Icons

**Fullscreen:**
```csharp
protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
{
    return this._isFullscreen
        ? this.DrawIcon(imageSize, "fullscreen-on.png")
        : this.DrawIcon(imageSize, "fullscreen-off.png");
}
```

**Reset Meeting:**
```csharp
protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
{
    if (this._confirmPending)
    {
        // Keep red text for confirmation (important visual warning)
        var builder = new BitmapBuilder(imageSize);
        builder.Clear(new BitmapColor(231, 76, 60));
        builder.DrawText("CONFIRM\nRESET", BitmapColor.White, 10);
        return builder.ToImage();
    }

    return this.DrawIcon(imageSize, "reset.png");
}
```

---

## Testing Your Icons

1. Add PNG files to `src/CueBoardPlugin/src/Resources/Icons/`
2. Rebuild the plugin: `dotnet build`
3. Restart Logitech Loupedeck service (or reopen Logi Options+ app)
4. Check the console buttons — icons should appear

**If icons don't appear:**
- Check file names match exactly (case-sensitive)
- Verify icons are in the correct folder
- Check build output to ensure icons were embedded
- Fallback: CueBoardCommand.DrawIcon() will show filename as text if PNG not found

---

## Quick Win: Text Buttons Are Fine for Demo

The current text-based buttons for Fullscreen and Reset Meeting:
- ✓ Use proper branding colors (purple `#8B5CF6`)
- ✓ Have clear, readable text
- ✓ Match the dark theme (`#2A2A35` background)
- ✓ Show state changes (fullscreen on/off, reset confirmation)
- ✓ Look professional on the hardware

**For the hackathon demo, these work perfectly.** Judges won't penalize text buttons if they're clean and functional. Save icon creation for post-hackathon polish if you have time.

---

## Summary

**What's Done:**
- ✓ All buttons have consistent branding (purple accents, dark backgrounds)
- ✓ Min/Max Window button created with existing minmax.png icon
- ✓ Fullscreen button improved with cleaner text rendering
- ✓ Reset Meeting button improved with purple/red color scheme

**Branding Decision:**
- **Update your landing page to use `#8B5CF6` (purple)** — much easier than changing 50+ code references

**Optional (Post-Demo):**
- Create PNG icons for Fullscreen and Reset Meeting if you want visual consistency with other buttons
- Current text-based versions are demo-ready and look professional

**Your call:** Icons are always nice, but the current implementation is polished enough for judges. Focus on demo rehearsal and testing.
