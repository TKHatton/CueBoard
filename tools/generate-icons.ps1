
# CueBoard Icon Generator
# Generates 80x80 PNG icons with dark gray background and purple accent
# Run: powershell -ExecutionPolicy Bypass -File generate-icons.ps1

Add-Type -AssemblyName System.Drawing

$tempDir = Join-Path $env:TEMP "CueBoardIcons"
$outDir = Join-Path $PSScriptRoot "..\src\CueBoardPlugin\src\Resources\Icons"
if (Test-Path $tempDir) { Remove-Item $tempDir -Recurse -Force }
New-Item -ItemType Directory -Path $tempDir -Force | Out-Null

$size = 80
$bg = [Drawing.Color]::FromArgb(42, 42, 53)        # #2a2a35
$accent = [Drawing.Color]::FromArgb(139, 92, 246)   # #8B5CF6
$white = [Drawing.Color]::White
$red = [Drawing.Color]::FromArgb(231, 76, 60)
$green = [Drawing.Color]::FromArgb(46, 204, 113)
$gold = [Drawing.Color]::FromArgb(245, 158, 11)
$teal = [Drawing.Color]::FromArgb(13, 148, 136)
$blue = [Drawing.Color]::FromArgb(46, 134, 222)
$pink = [Drawing.Color]::FromArgb(220, 80, 120)

function New-Icon {
    param([string]$Name, [scriptblock]$DrawFunc, [Drawing.Color]$Background = $bg)

    $bmp = New-Object Drawing.Bitmap($size, $size)
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = 'AntiAlias'
    $g.TextRenderingHint = 'AntiAlias'

    # Fill background
    $g.Clear($Background)

    # Draw icon
    & $DrawFunc $g

    $path = Join-Path $tempDir "$Name.png"
    $bmp.Save($path, [Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose()
    $bmp.Dispose()
    Write-Host "  Created: $Name.png"
}

function Draw-Line($g, $x1, $y1, $x2, $y2, $color = $accent, $width = 3) {
    $pen = New-Object Drawing.Pen($color, $width)
    $pen.StartCap = 'Round'; $pen.EndCap = 'Round'
    $g.DrawLine($pen, $x1, $y1, $x2, $y2)
    $pen.Dispose()
}

function Draw-Rect($g, $x, $y, $w, $h, $color = $accent, $width = 3) {
    $pen = New-Object Drawing.Pen($color, $width)
    $pen.LineJoin = 'Round'
    $g.DrawRectangle($pen, $x, $y, $w, $h)
    $pen.Dispose()
}

function Fill-Rect($g, $x, $y, $w, $h, $color = $accent) {
    $brush = New-Object Drawing.SolidBrush($color)
    $g.FillRectangle($brush, $x, $y, $w, $h)
    $brush.Dispose()
}

function Draw-Circle($g, $cx, $cy, $r, $color = $accent, $width = 3) {
    $pen = New-Object Drawing.Pen($color, $width)
    $g.DrawEllipse($pen, ($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
    $pen.Dispose()
}

function Fill-Circle($g, $cx, $cy, $r, $color = $accent) {
    $brush = New-Object Drawing.SolidBrush($color)
    $g.FillEllipse($brush, ($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
    $brush.Dispose()
}

function Draw-Text($g, $text, $x, $y, $fontSize = 12, $color = $accent) {
    $font = New-Object Drawing.Font('Segoe UI', $fontSize, [Drawing.FontStyle]::Bold)
    $brush = New-Object Drawing.SolidBrush($color)
    $sf = New-Object Drawing.StringFormat
    $sf.Alignment = 'Center'; $sf.LineAlignment = 'Center'
    $g.DrawString($text, $font, $brush, [Drawing.RectangleF]::new($x, $y, 80-$x*2, 80-$y*2), $sf)
    $font.Dispose(); $brush.Dispose(); $sf.Dispose()
}

function Draw-Slash($g, $color = $red, $width = 3) {
    Draw-Line $g 58 18 22 58 $color $width
}

Write-Host "Generating CueBoard icons..."
Write-Host ""

# ===== PAGE 1: Live Controls =====
Write-Host "Page 1: Live Controls"

# Mute OFF (rounded mic capsule, active)
New-Icon "mute-off" {
    param($g)
    # Mic capsule (rounded pill shape)
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 30, 14, 20, 16, 180, 180)  # top round
    $g.DrawLine($pen, 30, 22, 30, 40)             # left side
    $g.DrawLine($pen, 50, 22, 50, 40)             # right side
    $g.DrawArc($pen, 30, 32, 20, 16, 0, 180)      # bottom round
    # Base arc
    $g.DrawArc($pen, 22, 30, 36, 26, 0, 180)
    $pen.Dispose()
    # Stand
    Draw-Line $g 40 56 40 64 $accent 3
    Draw-Line $g 30 64 50 64 $accent 3
}

# Mute ON (rounded mic with slash)
New-Icon "mute-on" {
    param($g)
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 30, 14, 20, 16, 180, 180)
    $g.DrawLine($pen, 30, 22, 30, 40)
    $g.DrawLine($pen, 50, 22, 50, 40)
    $g.DrawArc($pen, 30, 32, 20, 16, 0, 180)
    $g.DrawArc($pen, 22, 30, 36, 26, 0, 180)
    $pen.Dispose()
    Draw-Line $g 40 56 40 64 $accent 3
    Draw-Line $g 30 64 50 64 $accent 3
    Draw-Slash $g
}

# Camera ON
New-Icon "camera-on" {
    param($g)
    Draw-Rect $g 14 26 34 28 $accent 3
    # Lens triangle
    Draw-Line $g 52 30 66 22 $accent 3
    Draw-Line $g 66 22 66 58 $accent 3
    Draw-Line $g 66 58 52 50 $accent 3
}

# Camera OFF
New-Icon "camera-off" {
    param($g)
    Draw-Rect $g 14 26 34 28 $accent 3
    Draw-Line $g 52 30 66 22 $accent 3
    Draw-Line $g 66 22 66 58 $accent 3
    Draw-Line $g 66 58 52 50 $accent 3
    Draw-Slash $g
}

# Record ON (filled red circle - actively recording)
New-Icon "record-on" {
    param($g)
    Fill-Circle $g 40 40 20 $red
}

# Record OFF (red circle outline - ready to record)
New-Icon "record-off" {
    param($g)
    Draw-Circle $g 40 40 20 $red 3
}

# Share ON
New-Icon "share-on" {
    param($g)
    Draw-Rect $g 16 20 48 36 $accent 3
    # Arrow up from screen
    Draw-Line $g 40 14 40 44 $green 3
    Draw-Line $g 32 22 40 14 $green 3
    Draw-Line $g 48 22 40 14 $green 3
}

# Share OFF
New-Icon "share-off" {
    param($g)
    Draw-Rect $g 16 20 48 36 $accent 3
    Draw-Line $g 32 62 48 62 $accent 3
}

# Chat
New-Icon "chat" {
    param($g)
    # Speech bubble
    $pen = New-Object Drawing.Pen($accent, 3); $pen.LineJoin = 'Round'
    $points = @(
        [Drawing.Point]::new(16, 16),
        [Drawing.Point]::new(64, 16),
        [Drawing.Point]::new(64, 48),
        [Drawing.Point]::new(36, 48),
        [Drawing.Point]::new(24, 62),
        [Drawing.Point]::new(24, 48),
        [Drawing.Point]::new(16, 48),
        [Drawing.Point]::new(16, 16)
    )
    $g.DrawPolygon($pen, $points)
    $pen.Dispose()
    # Dots
    Fill-Circle $g 30 32 3 $accent
    Fill-Circle $g 40 32 3 $accent
    Fill-Circle $g 50 32 3 $accent
}

# Reaction
New-Icon "reaction" {
    param($g)
    Draw-Circle $g 40 40 22 $accent 3
    # Eyes
    Fill-Circle $g 32 35 3 $accent
    Fill-Circle $g 48 35 3 $accent
    # Smile arc
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 28, 34, 24, 16, 0, 180)
    $pen.Dispose()
}

# Hand raised (solid filled palm — like the icon pack examples)
New-Icon "hand-up" {
    param($g)
    $brush = New-Object Drawing.SolidBrush($accent)
    # Build a hand shape using filled regions
    # Palm (wide rounded rect)
    $g.FillRectangle($brush, 24, 32, 34, 22)
    $g.FillEllipse($brush, 24, 46, 34, 18)   # rounded bottom
    # Fingers (thick rounded rectangles, close together)
    $g.FillRectangle($brush, 25, 10, 8, 26)   # index
    $g.FillEllipse($brush, 25, 7, 8, 8)       # index tip
    $g.FillRectangle($brush, 34, 8, 8, 28)    # middle (tallest)
    $g.FillEllipse($brush, 34, 5, 8, 8)       # middle tip
    $g.FillRectangle($brush, 43, 10, 8, 26)   # ring
    $g.FillEllipse($brush, 43, 7, 8, 8)       # ring tip
    $g.FillRectangle($brush, 52, 16, 7, 20)   # pinky (shorter)
    $g.FillEllipse($brush, 52, 13, 7, 8)      # pinky tip
    # Thumb (angled oval on left side)
    $saved = $g.Transform.Clone()
    $g.TranslateTransform(16, 34)
    $g.RotateTransform(-30)
    $g.FillEllipse($brush, -5, -10, 10, 22)
    $g.Transform = $saved
    $brush.Dispose()
}

# Hand down (same hand, with red slash)
New-Icon "hand-down" {
    param($g)
    $brush = New-Object Drawing.SolidBrush($accent)
    $g.FillRectangle($brush, 24, 32, 34, 22)
    $g.FillEllipse($brush, 24, 46, 34, 18)
    $g.FillRectangle($brush, 25, 10, 8, 26)
    $g.FillEllipse($brush, 25, 7, 8, 8)
    $g.FillRectangle($brush, 34, 8, 8, 28)
    $g.FillEllipse($brush, 34, 5, 8, 8)
    $g.FillRectangle($brush, 43, 10, 8, 26)
    $g.FillEllipse($brush, 43, 7, 8, 8)
    $g.FillRectangle($brush, 52, 16, 7, 20)
    $g.FillEllipse($brush, 52, 13, 7, 8)
    $saved = $g.Transform.Clone()
    $g.TranslateTransform(16, 34)
    $g.RotateTransform(-30)
    $g.FillEllipse($brush, -5, -10, 10, 22)
    $g.Transform = $saved
    $brush.Dispose()
    Draw-Slash $g
}

# View Gallery
New-Icon "view-gallery" {
    param($g)
    # 4 rectangles in a grid
    Draw-Rect $g 14 14 22 22 $accent 3
    Draw-Rect $g 44 14 22 22 $accent 3
    Draw-Rect $g 14 44 22 22 $accent 3
    Draw-Rect $g 44 44 22 22 $accent 3
}

# View Speaker
New-Icon "view-speaker" {
    param($g)
    # One big rectangle
    Draw-Rect $g 14 14 52 52 $accent 3
    # Person silhouette
    Draw-Circle $g 40 30 8 $accent 2
    $pen = New-Object Drawing.Pen($accent, 2)
    $g.DrawArc($pen, 28, 40, 24, 20, 180, 180)
    $pen.Dispose()
}

# End Meeting (BYE text)
New-Icon "end-meeting" {
    param($g)
    Draw-Text $g "BYE" 0 0 22 $red
}

# ===== PAGE 2: Operator Mode =====
Write-Host "Page 2: Operator Mode"

# Mute All (two rounded mics with slash)
New-Icon "mute-all" {
    param($g)
    $pen = New-Object Drawing.Pen($accent, 2); $pen.StartCap = 'Round'; $pen.EndCap = 'Round'
    # Mic 1 (left, rounded capsule)
    $g.DrawArc($pen, 16, 14, 14, 10, 180, 180)
    $g.DrawLine($pen, 16, 19, 16, 34)
    $g.DrawLine($pen, 30, 19, 30, 34)
    $g.DrawArc($pen, 16, 28, 14, 10, 0, 180)
    $g.DrawArc($pen, 12, 26, 22, 16, 0, 180)
    $g.DrawLine($pen, 23, 42, 23, 50)
    # Mic 2 (right, rounded capsule)
    $g.DrawArc($pen, 44, 14, 14, 10, 180, 180)
    $g.DrawLine($pen, 44, 19, 44, 34)
    $g.DrawLine($pen, 58, 19, 58, 34)
    $g.DrawArc($pen, 44, 28, 14, 10, 0, 180)
    $g.DrawArc($pen, 40, 26, 22, 16, 0, 180)
    $g.DrawLine($pen, 51, 42, 51, 50)
    $pen.Dispose()
    Draw-Slash $g
}

# Spotlight ON
New-Icon "spotlight-on" {
    param($g)
    # Spotlight beam
    Draw-Line $g 40 16 24 56 $gold 3
    Draw-Line $g 40 16 56 56 $gold 3
    Draw-Line $g 24 56 56 56 $gold 3
    Fill-Circle $g 40 16 5 $gold
}

# Spotlight OFF
New-Icon "spotlight-off" {
    param($g)
    Draw-Line $g 40 16 24 56 $accent 3
    Draw-Line $g 40 16 56 56 $accent 3
    Draw-Line $g 24 56 56 56 $accent 3
    Fill-Circle $g 40 16 5 $accent
}

# Lock ON
New-Icon "lock-on" {
    param($g)
    # Lock body
    Fill-Rect $g 24 38 32 24 $accent
    # Lock shackle
    $pen = New-Object Drawing.Pen($accent, 4)
    $g.DrawArc($pen, 28, 18, 24, 24, 180, 180)
    $pen.Dispose()
    # Keyhole
    Fill-Circle $g 40 48 4 $bg
}

# Lock OFF
New-Icon "lock-off" {
    param($g)
    Draw-Rect $g 24 38 32 24 $accent 3
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 28 , 18, 24, 24, 180, 180)
    $pen.Dispose()
}

# Admit (person with +)
New-Icon "admit" {
    param($g)
    Draw-Circle $g 34 28 10 $accent 3
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 18, 44, 32, 22, 180, 180)
    $pen.Dispose()
    # Plus
    Draw-Line $g 58 28 58 44 $green 3
    Draw-Line $g 50 36 66 36 $green 3
}

# Remove (person with X)
New-Icon "remove" {
    param($g)
    Draw-Circle $g 34 28 10 $accent 3
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 18, 44, 32, 22, 180, 180)
    $pen.Dispose()
    # X
    Draw-Line $g 52 28 64 40 $red 3
    Draw-Line $g 64 28 52 40 $red 3
}

# Host Transfer (arrows)
New-Icon "host-transfer" {
    param($g)
    # Two people with arrow between
    Draw-Circle $g 24 28 8 $accent 2
    Draw-Circle $g 56 28 8 $accent 2
    # Arrow right
    Draw-Line $g 28 52 52 52 $accent 3
    Draw-Line $g 46 46 52 52 $accent 3
    Draw-Line $g 46 58 52 52 $accent 3
}

# Captions ON
New-Icon "captions-on" {
    param($g)
    Draw-Rect $g 14 20 52 40 $accent 3
    Draw-Text $g "CC" 0 0 18 $accent
}

# Captions OFF
New-Icon "captions-off" {
    param($g)
    Draw-Rect $g 14 20 52 40 $accent 3
    Draw-Text $g "CC" 0 0 18 $accent
    Draw-Slash $g
}

# Timer (simple clock with two hands)
New-Icon "timer" {
    param($g)
    # Clock face circle
    Draw-Circle $g 40 40 22 $accent 3
    # Hour hand (short, pointing to 10)
    Draw-Line $g 40 40 32 28 $accent 3
    # Minute hand (long, pointing to 2)
    Draw-Line $g 40 40 52 26 $accent 2
    # Center dot
    Fill-Circle $g 40 40 3 $accent
    # 12 o'clock tick
    Draw-Line $g 40 18 40 22 $accent 2
    # 3 o'clock tick
    Draw-Line $g 58 40 62 40 $accent 2
    # 6 o'clock tick
    Draw-Line $g 40 58 40 62 $accent 2
    # 9 o'clock tick
    Draw-Line $g 18 40 22 40 $accent 2
}

# Timer Clear (clock with slash)
New-Icon "timer-clear" {
    param($g)
    Draw-Circle $g 40 40 22 $accent 2
    Draw-Line $g 40 40 32 28 $accent 2
    Draw-Line $g 40 40 52 26 $accent 2
    Fill-Circle $g 40 40 3 $accent
    Draw-Slash $g
}

# Participants (two people side by side)
New-Icon "participants" {
    param($g)
    # Person 1 (left)
    Draw-Circle $g 28 24 9 $accent 3
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 14, 40, 28, 22, 180, 180)
    $pen.Dispose()
    # Person 2 (right, slightly overlapping)
    Draw-Circle $g 52 24 9 $accent 3
    $pen2 = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen2, 38, 40, 28, 22, 180, 180)
    $pen2.Dispose()
}

# Min/Max (arrows)
New-Icon "minmax" {
    param($g)
    # Expand arrows (two diagonal arrows pointing outward)
    # Top-left arrow
    Draw-Line $g 16 16 32 32 $accent 3
    Draw-Line $g 16 16 28 16 $accent 3
    Draw-Line $g 16 16 16 28 $accent 3
    # Bottom-right arrow
    Draw-Line $g 64 64 48 48 $accent 3
    Draw-Line $g 64 64 52 64 $accent 3
    Draw-Line $g 64 64 64 52 $accent 3
}

# ===== PAGE 3: Meeting Intelligence =====
Write-Host "Page 3: Meeting Intelligence"

# Flag (pole with pennant banner)
New-Icon "flag" {
    param($g)
    # Pole
    Draw-Line $g 24 10 24 68 $accent 3
    # Banner (rectangular flag with slight wave)
    $pen = New-Object Drawing.Pen($accent, 3); $pen.LineJoin = 'Round'; $pen.StartCap = 'Round'; $pen.EndCap = 'Round'
    $g.DrawLine($pen, 24, 12, 62, 12)  # top edge
    $g.DrawLine($pen, 62, 12, 62, 38)  # right edge
    $g.DrawLine($pen, 62, 38, 24, 38)  # bottom edge
    $pen.Dispose()
    # Wave line across the flag
    $penWave = New-Object Drawing.Pen($accent, 2)
    $g.DrawArc($penWave, 30, 18, 24, 14, 180, 180)
    $penWave.Dispose()
    # Base
    Draw-Line $g 18 68 30 68 $accent 2
}

# Assign (person with arrow)
New-Icon "assign" {
    param($g)
    Draw-Circle $g 32 28 10 $accent 3
    $pen = New-Object Drawing.Pen($accent, 3)
    $g.DrawArc($pen, 16, 44, 32, 22, 180, 180)
    $pen.Dispose()
    # Arrow pointing right
    Draw-Line $g 52 36 66 36 $accent 3
    Draw-Line $g 60 30 66 36 $accent 3
    Draw-Line $g 60 42 66 36 $accent 3
}

# Note (pencil)
New-Icon "note" {
    param($g)
    # Notepad
    Draw-Rect $g 18 14 44 52 $accent 3
    # Lines on notepad
    Draw-Line $g 26 28 54 28 $accent 2
    Draw-Line $g 26 38 54 38 $accent 2
    Draw-Line $g 26 48 44 48 $accent 2
}

# Highlight (star)
New-Icon "highlight" {
    param($g)
    $brush = New-Object Drawing.SolidBrush($gold)
    $cx = 40; $cy = 40; $outerR = 22; $innerR = 10
    $points = New-Object 'Drawing.PointF[]' 10
    for ($i = 0; $i -lt 10; $i++) {
        $angle = [Math]::PI * 2 * $i / 10 - [Math]::PI / 2
        $r = if ($i % 2 -eq 0) { $outerR } else { $innerR }
        $points[$i] = [Drawing.PointF]::new($cx + $r * [Math]::Cos($angle), $cy + $r * [Math]::Sin($angle))
    }
    $g.FillPolygon($brush, $points)
    $brush.Dispose()
}

# Clear Last (undo arrow)
New-Icon "clear-last" {
    param($g)
    $pen = New-Object Drawing.Pen($accent, 3); $pen.StartCap = 'Round'; $pen.EndCap = 'Round'
    $g.DrawArc($pen, 18, 22, 44, 36, 220, 260)
    $pen.Dispose()
    # Arrow head
    Draw-Line $g 20 30 28 22 $accent 3
    Draw-Line $g 20 30 30 34 $accent 3
}

# Preview (eye icon - like password show/hide)
New-Icon "preview" {
    param($g)
    # Eye shape - two arcs forming an almond
    $pen = New-Object Drawing.Pen($accent, 3); $pen.StartCap = 'Round'; $pen.EndCap = 'Round'
    $g.DrawArc($pen, 10, 20, 60, 44, 200, 140)   # top lid
    $g.DrawArc($pen, 10, 16, 60, 44, 20, 140)     # bottom lid
    $pen.Dispose()
    # Iris (circle)
    Draw-Circle $g 40 40 10 $accent 3
    # Pupil (filled)
    Fill-Circle $g 40 40 5 $accent
}

# Export (download arrow)
New-Icon "export" {
    param($g)
    # Arrow pointing down
    Draw-Line $g 40 14 40 48 $accent 3
    Draw-Line $g 28 38 40 50 $accent 3
    Draw-Line $g 52 38 40 50 $accent 3
    # Base line
    Draw-Line $g 18 60 62 60 $accent 3
    Draw-Line $g 18 60 18 52 $accent 3
    Draw-Line $g 62 60 62 52 $accent 3
}

# Export Done (checkmark)
New-Icon "export-done" {
    param($g)
    Draw-Line $g 22 40 36 56 $green 4
    Draw-Line $g 36 56 60 24 $green 4
}

# Poll (bar chart)
New-Icon "poll" {
    param($g)
    Fill-Rect $g 16 46 12 18 $accent
    Fill-Rect $g 34 30 12 34 $accent
    Fill-Rect $g 52 18 12 46 $accent
}

Write-Host ""
Write-Host "Copying to final location..."
Copy-Item "$tempDir\*.png" $outDir -Force
Write-Host "Done! All icons in: $outDir"
Remove-Item $tempDir -Recurse -Force
