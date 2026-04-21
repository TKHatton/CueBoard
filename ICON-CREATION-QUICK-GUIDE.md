# Quick Icon Creation Guide - Get PNG Icons in 5 Minutes

## What You Need
Two icons to match your existing style:
1. **Fullscreen icon** (expanding arrows / corner brackets)
2. **Reset icon** (circular refresh arrow)

---

## Fastest Method: Download Pre-Made Icons

### Step 1: Get Fullscreen Icon
1. Go to **https://lucide.dev**
2. Search: `maximize-2` or `expand`
3. Click the icon
4. Click **"Copy SVG"** button
5. Go to **https://www.svgtopng.com**
6. Paste the SVG code
7. Set width/height to **160** (2x for retina displays)
8. Click "Convert"
9. Download → Rename file to `fullscreen.png`

### Step 2: Get Reset Icon
1. Go back to **https://lucide.dev**
2. Search: `rotate-ccw` or `refresh-ccw`
3. Click the icon
4. Click **"Copy SVG"** button
5. Go to **https://www.svgtopng.com**
6. Paste the SVG code
7. Set width/height to **160**
8. Click "Convert"
9. Download → Rename file to `reset.png`

### Step 3: Color Them Purple (Your Brand Color)

#### Option A: Using Photopea (Free, In-Browser Photoshop)
1. Go to **https://www.photopea.com**
2. Click **File → Open** → Select `fullscreen.png`
3. In the Layers panel, unlock the layer (click the lock icon)
4. Select **Magic Wand Tool (W)** → Click the icon shape
5. **Edit → Fill** → Color: `#8B5CF6` → Click OK
6. **File → Export As → PNG**
7. Save
8. Repeat for `reset.png`

#### Option B: Using PowerShell (Automated, but requires ImageMagick)
```powershell
# Install ImageMagick (if not already installed)
winget install ImageMagick.ImageMagick

# Colorize icons to purple
magick fullscreen.png -colorspace gray -fill "#8B5CF6" -colorize 100 fullscreen-purple.png
magick reset.png -colorspace gray -fill "#8B5CF6" -colorize 100 reset-purple.png
```

#### Option C: Ask AI (Fastest if you have access)
1. Go to **https://www.remove.bg** or similar
2. Upload the PNG
3. Download transparent version
4. Use Photopea or Paint.NET to recolor purple

### Step 4: Copy Icons to Project

**Windows Command Prompt:**
```cmd
copy fullscreen.png "C:\Users\ltken\OneDrive\Documents\GitHub\CueBoard\src\CueBoardPlugin\src\Resources\Icons\"
copy reset.png "C:\Users\ltken\OneDrive\Documents\GitHub\CueBoard\src\CueBoardPlugin\src\Resources\Icons\"
```

**PowerShell:**
```powershell
Copy-Item fullscreen.png "C:\Users\ltken\OneDrive\Documents\GitHub\CueBoard\src\CueBoardPlugin\src\Resources\Icons\"
Copy-Item reset.png "C:\Users\ltken\OneDrive\Documents\GitHub\CueBoard\src\CueBoardPlugin\src\Resources\Icons\"
```

### Step 5: Update Code to Use PNG Icons

Once the PNG files are in place, update the commands:

**FullscreenCommand.cs** (line 25-42):
```csharp
protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
{
    return this._isFullscreen
        ? this.DrawIcon(imageSize, "fullscreen.png")  // Or "fullscreen-exit.png" if you make two versions
        : this.DrawIcon(imageSize, "fullscreen.png");
}
```

**ResetMeetingCommand.cs** (line 53-71):
```csharp
protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
{
    if (this._confirmPending)
    {
        // Keep red background + warning for confirmation
        var builder = new BitmapBuilder(imageSize);
        builder.Clear(new BitmapColor(231, 76, 60));
        builder.DrawText("⚠", BitmapColor.White, 48);
        return builder.ToImage();
    }

    return this.DrawIcon(imageSize, "reset.png");
}
```

### Step 6: Rebuild & Test
```bash
cd "C:\Users\ltken\OneDrive\Documents\GitHub\CueBoard\src\CueBoardPlugin"
dotnet build
```

Restart the Logitech Options+ app → Your new icons should appear!

---

## Alternative: Even Faster - Use Icon Finder

Go to **https://www.iconfinder.com** (has free icons):
1. Search "fullscreen outline purple"
2. Filter: **Free** + **PNG**
3. Download at 128px or larger
4. Search "refresh outline purple" or "reset outline purple"
5. Download
6. Copy to Icons folder

---

## Icon Style Guide (To Match Existing Icons)

When selecting or creating icons, match these specs:

**Style:**
- Outline/stroke icons (not filled)
- Simple, minimal shapes
- Clean lines, no gradients

**Color:**
- Purple stroke: `#8B5CF6` (RGB: 139, 92, 246)
- White for active/important states
- Gray for disabled/inactive: `#B4B4B4`

**Size:**
- 80x80px minimum
- 160x160px recommended (2x for retina)
- Transparent background

**Stroke Width:**
- 4-6px thickness
- Consistent across all icons

---

## Example Icons to Copy

If you want to match the exact style of your existing icons, here are similar ones:

**Your Timer Icon Style:**
- Circle with clock hands
- Source: Lucide's `clock` icon

**Your Lock Icon Style:**
- Padlock with circular outline
- Source: Lucide's `lock` icon

**For Fullscreen:**
- Four corner brackets pointing outward
- Similar to: Lucide's `maximize-2`, Heroicons' `arrows-pointing-out`

**For Reset:**
- Circular arrow (counter-clockwise)
- Similar to: Lucide's `rotate-ccw`, Heroicons' `arrow-path`

---

## If You're Short on Time (Current Solution Works!)

The Unicode symbols I just added (⤢ for fullscreen, ↻ for reset) are:
- Size 48 (much more icon-like than text)
- Proper brand purple color
- Clean and readable
- **Good enough for the hackathon demo**

PNG icons are always nicer, but judges won't penalize you for clean Unicode symbols. Focus on demo rehearsal if you're time-crunched.

---

## Total Time Investment

- **Unicode symbols (current):** Already done ✓ (0 minutes)
- **Download pre-made PNGs:** 3-5 minutes
- **Create custom icons in Figma:** 10-15 minutes
- **Perfect icon matching:** 30+ minutes

**Recommendation:** Stick with Unicode for now, add PNGs post-hackathon if you want perfection.
