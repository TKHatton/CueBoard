using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

const int SIZE = 80;
const int PADDING = 12;
const float SYMBOL_SCALE = 1.5f;

string outputDir = args.Length > 0
    ? args[0]
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src", "CueBoardPlugin", "src", "Resources", "Icons"));

Directory.CreateDirectory(outputDir);
Console.WriteLine($"Generating icons to: {outputDir}");

// Colors
Color BgDark = ColorTranslator.FromHtml("#1a1a1a");
Color Red = ColorTranslator.FromHtml("#E63946");
Color Green = ColorTranslator.FromHtml("#2ECC71");
Color Blue = ColorTranslator.FromHtml("#3498DB");
Color Purple = ColorTranslator.FromHtml("#8B5CF6");
Color Orange = ColorTranslator.FromHtml("#F39C12");
Color Yellow = ColorTranslator.FromHtml("#F1C40F");
Color Gray = ColorTranslator.FromHtml("#555555");
Color DarkBlue = ColorTranslator.FromHtml("#1a3a5c");
Color DarkPurple = ColorTranslator.FromHtml("#2d1f5e");
Color DarkRed = ColorTranslator.FromHtml("#6b2028");
Color DarkGreen = ColorTranslator.FromHtml("#1a5c3a");
Color DarkOrange = ColorTranslator.FromHtml("#5c3a1a");
Color DarkYellow = ColorTranslator.FromHtml("#5c4a0e");
Color Teal = ColorTranslator.FromHtml("#1a8a7a");
Color Coral = ColorTranslator.FromHtml("#c0392b");
Color Indigo = ColorTranslator.FromHtml("#4a3580");
Color Slate = ColorTranslator.FromHtml("#2c3e50");

int generated = 0;

// ===== Helper Methods =====

Bitmap CreateIcon(Color bgColor, string label, Action<Graphics, RectangleF> drawSymbol)
{
    var bmp = new Bitmap(SIZE, SIZE, PixelFormat.Format32bppArgb);
    using var g = Graphics.FromImage(bmp);
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

    // Background with rounded corners
    using var bgBrush = new SolidBrush(bgColor);
    FillRoundedRect(g, bgBrush, new Rectangle(0, 0, SIZE, SIZE), 8);

    // Symbol area - full icon with padding, centered
    var symbolRect = new RectangleF(PADDING, PADDING, SIZE - PADDING * 2, SIZE - PADDING * 2);
    drawSymbol(g, symbolRect);

    return bmp;
}

void Save(Bitmap bmp, string filename)
{
    string path = Path.Combine(outputDir, filename);
    bmp.Save(path, ImageFormat.Png);
    bmp.Dispose();
    generated++;
    Console.WriteLine($"  Created: {filename}");
}

void FillRoundedRect(Graphics g, Brush brush, Rectangle rect, int radius)
{
    using var path = new GraphicsPath();
    int d = radius * 2;
    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
    path.CloseFigure();
    g.FillPath(brush, path);
}

void DrawMicrophone(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f);
    using var brush = new SolidBrush(color);
    float w = 10 * scale, h = 16 * scale;
    // Mic body (rounded rect)
    g.FillEllipse(brush, cx - w / 2, cy - h / 2, w, h * 0.7f);
    g.FillRectangle(brush, cx - w / 2, cy - h / 4, w, h / 2);
    g.FillEllipse(brush, cx - w / 2, cy + h / 4 - h * 0.35f, w, h * 0.7f);
    // Stand arc
    g.DrawArc(pen, cx - w * 0.8f, cy - h * 0.1f, w * 1.6f, h * 0.9f, 0, 180);
    // Stand line
    g.DrawLine(pen, cx, cy + h / 2 + 2 * scale, cx, cy + h / 2 + 6 * scale);
    g.DrawLine(pen, cx - 4 * scale, cy + h / 2 + 6 * scale, cx + 4 * scale, cy + h / 2 + 6 * scale);
}

void DrawX(Graphics g, float cx, float cy, float size, Color color)
{
    using var pen = new Pen(color, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(pen, cx - size, cy - size, cx + size, cy + size);
    g.DrawLine(pen, cx + size, cy - size, cx - size, cy + size);
}

void DrawCamera(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f);
    using var brush = new SolidBrush(color);
    float bw = 20 * scale, bh = 14 * scale;
    // Camera body
    var bodyRect = new RectangleF(cx - bw / 2 - 3 * scale, cy - bh / 2, bw, bh);
    using var bodyPath = RoundedRectPath(bodyRect, 3);
    g.FillPath(brush, bodyPath);
    // Lens triangle
    var pts = new PointF[]
    {
        new(cx + bw / 2 - 3 * scale, cy - 5 * scale),
        new(cx + bw / 2 + 6 * scale, cy - 8 * scale),
        new(cx + bw / 2 + 6 * scale, cy + 8 * scale),
        new(cx + bw / 2 - 3 * scale, cy + 5 * scale),
    };
    g.FillPolygon(brush, pts);
}

void DrawScreen(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f);
    float w = 24 * scale, h = 16 * scale;
    var rect = new RectangleF(cx - w / 2, cy - h / 2 - 2 * scale, w, h);
    using var path = RoundedRectPath(rect, 2);
    g.DrawPath(pen, path);
    // Stand
    g.DrawLine(pen, cx, cy + h / 2 - 2 * scale, cx, cy + h / 2 + 3 * scale);
    g.DrawLine(pen, cx - 6 * scale, cy + h / 2 + 3 * scale, cx + 6 * scale, cy + h / 2 + 3 * scale);
}

void DrawChatBubble(Graphics g, float cx, float cy, float scale, Color color)
{
    using var brush = new SolidBrush(color);
    float w = 26 * scale, h = 18 * scale;
    var rect = new RectangleF(cx - w / 2, cy - h / 2 - 2 * scale, w, h);
    using var path = RoundedRectPath(rect, 6);
    // Tail
    path.AddLine(cx - 4 * scale, cy + h / 2 - 2 * scale, cx - 8 * scale, cy + h / 2 + 5 * scale);
    path.AddLine(cx - 8 * scale, cy + h / 2 + 5 * scale, cx + 2 * scale, cy + h / 2 - 2 * scale);
    g.FillPath(brush, path);
    // Dots
    using var dotBrush = new SolidBrush(BgDark);
    float dotR = 2f;
    g.FillEllipse(dotBrush, cx - 8 * scale - dotR, cy - 2 * scale - dotR, dotR * 2, dotR * 2);
    g.FillEllipse(dotBrush, cx - dotR, cy - 2 * scale - dotR, dotR * 2, dotR * 2);
    g.FillEllipse(dotBrush, cx + 8 * scale - dotR, cy - 2 * scale - dotR, dotR * 2, dotR * 2);
}

void DrawSmiley(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f);
    float r = 14 * scale;
    g.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
    // Eyes
    using var brush = new SolidBrush(color);
    float er = 2 * scale;
    g.FillEllipse(brush, cx - 5 * scale - er, cy - 4 * scale - er, er * 2, er * 2);
    g.FillEllipse(brush, cx + 5 * scale - er, cy - 4 * scale - er, er * 2, er * 2);
    // Smile
    g.DrawArc(pen, cx - 7 * scale, cy - 3 * scale, 14 * scale, 12 * scale, 10, 160);
}

void DrawHand(Graphics g, float cx, float cy, float scale, Color color)
{
    using var brush = new SolidBrush(color);
    using var pen = new Pen(color, 2f);
    // Palm
    float pw = 14 * scale, ph = 16 * scale;
    g.FillEllipse(brush, cx - pw / 2, cy, pw, ph);
    // Fingers (5 rounded lines going up)
    using var fingerPen = new Pen(color, 3.5f * scale) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    float[] offsets = { -5.5f, -2.5f, 0.5f, 3.5f };
    float[] heights = { 14, 18, 17, 13 };
    for (int i = 0; i < 4; i++)
    {
        float fx = cx + offsets[i] * scale;
        g.DrawLine(fingerPen, fx, cy + 4 * scale, fx, cy - heights[i] * scale);
    }
    // Thumb
    g.DrawLine(fingerPen, cx - 7 * scale, cy + 6 * scale, cx - 11 * scale, cy - 2 * scale);
}

void DrawGrid4(Graphics g, float cx, float cy, float scale, Color color)
{
    using var brush = new SolidBrush(color);
    float s = 10 * scale, gap = 3 * scale;
    float left = cx - s - gap / 2, top = cy - s - gap / 2;
    g.FillRectangle(brush, left, top, s, s);
    g.FillRectangle(brush, left + s + gap, top, s, s);
    g.FillRectangle(brush, left, top + s + gap, s, s);
    g.FillRectangle(brush, left + s + gap, top + s + gap, s, s);
}

void DrawSingleSquare(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f);
    float s = 20 * scale;
    g.DrawRectangle(pen, cx - s / 2, cy - s / 2, s, s);
    // Person inside
    using var brush = new SolidBrush(color);
    g.FillEllipse(brush, cx - 3, cy - 6, 6, 6);
    g.FillEllipse(brush, cx - 6, cy + 1, 12, 8);
}

void DrawPhone(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    // Phone handset shape using bezier-ish lines
    float w = 10 * scale;
    g.DrawArc(pen, cx - w, cy - w * 0.6f, w * 2, w * 1.8f, 200, 140);
    // Earpiece and mouthpiece (thick ends)
    using var brush = new SolidBrush(color);
    g.FillEllipse(brush, cx - w - 2, cy + 2, 6, 6);
    g.FillEllipse(brush, cx + w - 4, cy + 2, 6, 6);
}

void DrawPadlock(Graphics g, float cx, float cy, float scale, Color color, bool locked)
{
    using var pen = new Pen(color, 2.5f);
    using var brush = new SolidBrush(color);
    float bw = 18 * scale, bh = 14 * scale;
    // Body
    var bodyRect = new RectangleF(cx - bw / 2, cy - 1 * scale, bw, bh);
    using var bodyPath = RoundedRectPath(bodyRect, 3);
    g.FillPath(brush, bodyPath);
    // Shackle
    float sw = 12 * scale, sh = 10 * scale;
    if (locked)
    {
        g.DrawArc(pen, cx - sw / 2, cy - sh - 1 * scale, sw, sh * 1.5f, 180, 180);
    }
    else
    {
        // Open shackle - shifted right and up
        g.DrawArc(pen, cx - sw / 2 + 2 * scale, cy - sh - 4 * scale, sw, sh * 1.5f, 180, 180);
    }
    // Keyhole
    using var holeBrush = new SolidBrush(BgDark);
    g.FillEllipse(holeBrush, cx - 2.5f, cy + 3 * scale, 5, 5);
    g.FillRectangle(holeBrush, cx - 1.5f, cy + 6 * scale, 3, 4);
}

void DrawPerson(Graphics g, float cx, float cy, float scale, Color color)
{
    using var brush = new SolidBrush(color);
    // Head
    g.FillEllipse(brush, cx - 5 * scale, cy - 12 * scale, 10 * scale, 10 * scale);
    // Body
    g.FillEllipse(brush, cx - 8 * scale, cy + 1 * scale, 16 * scale, 14 * scale);
}

void DrawClock(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f);
    float r = 13 * scale;
    g.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
    // Hands
    using var handPen = new Pen(color, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(handPen, cx, cy, cx, cy - 8 * scale); // 12 o'clock
    g.DrawLine(handPen, cx, cy, cx + 6 * scale, cy + 3 * scale); // ~4 o'clock
}

void DrawFlag(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    using var brush = new SolidBrush(color);
    // Pole
    g.DrawLine(pen, cx - 8 * scale, cy - 14 * scale, cx - 8 * scale, cy + 14 * scale);
    // Flag (triangle/rectangle)
    var pts = new PointF[]
    {
        new(cx - 8 * scale, cy - 14 * scale),
        new(cx + 10 * scale, cy - 7 * scale),
        new(cx - 8 * scale, cy - 0 * scale),
    };
    g.FillPolygon(brush, pts);
}

void DrawDocument(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f);
    float w = 18 * scale, h = 24 * scale;
    var rect = new RectangleF(cx - w / 2, cy - h / 2, w, h);
    using var path = RoundedRectPath(rect, 2);
    g.DrawPath(pen, path);
    // Lines
    float lx = cx - w / 2 + 4 * scale;
    float lw = w - 8 * scale;
    for (int i = 0; i < 3; i++)
    {
        float ly = cy - h / 2 + 7 * scale + i * 5 * scale;
        g.DrawLine(pen, lx, ly, lx + lw * (i == 2 ? 0.6f : 1f), ly);
    }
}

void DrawStar(Graphics g, float cx, float cy, float scale, Color color, bool filled)
{
    var pts = new PointF[10];
    float outerR = 13 * scale, innerR = 6 * scale;
    for (int i = 0; i < 10; i++)
    {
        float angle = (float)(Math.PI / 2 + i * Math.PI / 5);
        float r = i % 2 == 0 ? outerR : innerR;
        pts[i] = new PointF(cx + (float)Math.Cos(angle) * r, cy - (float)Math.Sin(angle) * r);
    }
    if (filled)
    {
        using var brush = new SolidBrush(color);
        g.FillPolygon(brush, pts);
    }
    else
    {
        using var pen = new Pen(color, 2f);
        g.DrawPolygon(pen, pts);
    }
}

void DrawUndoArrow(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    // Curved arrow
    g.DrawArc(pen, cx - 10 * scale, cy - 8 * scale, 20 * scale, 16 * scale, 180, 200);
    // Arrowhead
    float ax = cx - 10 * scale, ay = cy;
    g.DrawLine(pen, ax, ay, ax + 5 * scale, ay - 5 * scale);
    g.DrawLine(pen, ax, ay, ax + 6 * scale, ay + 2 * scale);
}

void DrawChart(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    using var brush = new SolidBrush(color);
    // Bars
    float bw = 4 * scale;
    float[] heights = { 10, 16, 12, 20 };
    float startX = cx - 10 * scale;
    for (int i = 0; i < 4; i++)
    {
        float bx = startX + i * 6 * scale;
        float bh = heights[i] * scale;
        g.FillRectangle(brush, bx, cy + 10 * scale - bh, bw, bh);
    }
    // Axis
    g.DrawLine(pen, cx - 12 * scale, cy + 10 * scale, cx + 14 * scale, cy + 10 * scale);
}

void DrawDownload(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    // Arrow down
    g.DrawLine(pen, cx, cy - 12 * scale, cx, cy + 4 * scale);
    g.DrawLine(pen, cx - 6 * scale, cy - 2 * scale, cx, cy + 4 * scale);
    g.DrawLine(pen, cx + 6 * scale, cy - 2 * scale, cx, cy + 4 * scale);
    // Tray
    g.DrawLine(pen, cx - 10 * scale, cy + 4 * scale, cx - 10 * scale, cy + 10 * scale);
    g.DrawLine(pen, cx - 10 * scale, cy + 10 * scale, cx + 10 * scale, cy + 10 * scale);
    g.DrawLine(pen, cx + 10 * scale, cy + 10 * scale, cx + 10 * scale, cy + 4 * scale);
}

void DrawCheckmark(Graphics g, float cx, float cy, float scale, Color color)
{
    using var pen = new Pen(color, 3.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(pen, cx - 10 * scale, cy, cx - 3 * scale, cy + 8 * scale);
    g.DrawLine(pen, cx - 3 * scale, cy + 8 * scale, cx + 10 * scale, cy - 8 * scale);
}

GraphicsPath RoundedRectPath(RectangleF rect, float radius)
{
    var path = new GraphicsPath();
    float d = radius * 2;
    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
    path.CloseFigure();
    return path;
}


// =============================================
// PAGE 1 - LIVE CONTROLS
// =============================================
Console.WriteLine("\n--- Page 1: Live Controls ---");

// mute-on.png - Red, mic with X
Save(CreateIcon(Red, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawMicrophone(g, cx, cy, SYMBOL_SCALE, Color.White);
    DrawX(g, cx + 12, cy - 12, 7, Color.White);
}), "mute-on.png");

// mute-off.png - Green, mic
Save(CreateIcon(Green, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawMicrophone(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "mute-off.png");

// camera-on.png - Green, camera
Save(CreateIcon(Green, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawCamera(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "camera-on.png");

// camera-off.png - DarkGreen, camera with X
Save(CreateIcon(DarkGreen, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawCamera(g, cx, cy, SYMBOL_SCALE, Color.White);
    DrawX(g, cx + 15, cy - 12, 6, Color.White);
}), "camera-off.png");

// record-on.png - Red, filled circle
Save(CreateIcon(Red, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    using var brush = new SolidBrush(Color.White);
    g.FillEllipse(brush, cx - 18, cy - 18, 36, 36);
}), "record-on.png");

// record-off.png - DarkRed, circle outline
Save(CreateIcon(DarkRed, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    using var pen = new Pen(Color.White, 3f);
    g.DrawEllipse(pen, cx - 18, cy - 18, 36, 36);
}), "record-off.png");

// share-on.png - Blue, screen icon with arrow
Save(CreateIcon(Blue, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawScreen(g, cx, cy, SYMBOL_SCALE, Color.White);
    // Arrow up in screen
    using var pen = new Pen(Color.White, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(pen, cx, cy + 3, cx, cy - 9);
    g.DrawLine(pen, cx - 6, cy - 4, cx, cy - 9);
    g.DrawLine(pen, cx + 6, cy - 4, cx, cy - 9);
}), "share-on.png");

// share-off.png - Slate, screen
Save(CreateIcon(Slate, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawScreen(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "share-off.png");

// chat.png - Blue, chat bubble
Save(CreateIcon(Blue, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawChatBubble(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "chat.png");

// reaction.png - Dark purple, smiley
Save(CreateIcon(DarkPurple, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawSmiley(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "reaction.png");

// hand-up.png - Yellow, raised hand
Save(CreateIcon(Yellow, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2 + 4;
    DrawHand(g, cx, cy, SYMBOL_SCALE, Color.FromArgb(40, 40, 40));
}), "hand-up.png");

// hand-down.png - DarkYellow, hand
Save(CreateIcon(DarkYellow, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2 + 4;
    DrawHand(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "hand-down.png");

// view-gallery.png - DarkBlue, 4 squares
Save(CreateIcon(DarkBlue, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawGrid4(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "view-gallery.png");

// view-speaker.png - DarkBlue, single square
Save(CreateIcon(DarkBlue, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawSingleSquare(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "view-speaker.png");

// end-meeting.png - Red, phone with X
Save(CreateIcon(Red, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawPhone(g, cx, cy, SYMBOL_SCALE, Color.White);
    DrawX(g, cx, cy - 15, 7, Color.White);
}), "end-meeting.png");


// =============================================
// PAGE 2 - OPERATOR MODE
// =============================================
Console.WriteLine("\n--- Page 2: Operator Mode ---");

// mute-all.png - Orange, multiple mics
Save(CreateIcon(Orange, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawMicrophone(g, cx - 14, cy, 1.05f, Color.White);
    DrawMicrophone(g, cx + 14, cy, 1.05f, Color.White);
    DrawX(g, cx + 22, cy - 14, 5, Color.White);
}), "mute-all.png");

// spotlight-on.png - Yellow, star
Save(CreateIcon(Yellow, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawStar(g, cx, cy, SYMBOL_SCALE, Color.FromArgb(40, 40, 40), true);
}), "spotlight-on.png");

// spotlight-off.png - DarkYellow, star outline
Save(CreateIcon(DarkYellow, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawStar(g, cx, cy, SYMBOL_SCALE, Color.White, false);
}), "spotlight-off.png");

// lock-on.png - Red, locked padlock
Save(CreateIcon(Red, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2 + 2;
    DrawPadlock(g, cx, cy, SYMBOL_SCALE, Color.White, true);
}), "lock-on.png");

// lock-off.png - Green, open padlock
Save(CreateIcon(Green, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2 + 2;
    DrawPadlock(g, cx, cy, SYMBOL_SCALE, Color.White, false);
}), "lock-off.png");

// admit.png - Green, person with +
Save(CreateIcon(Green, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawPerson(g, cx - 6, cy, SYMBOL_SCALE, Color.White);
    using var pen = new Pen(Color.White, 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(pen, cx + 16, cy - 8, cx + 16, cy + 6);
    g.DrawLine(pen, cx + 9, cy - 1, cx + 23, cy - 1);
}), "admit.png");

// remove.png - Coral, person with X
Save(CreateIcon(Coral, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawPerson(g, cx - 6, cy, SYMBOL_SCALE, Color.White);
    DrawX(g, cx + 18, cy - 3, 6, Color.White);
}), "remove.png");

// host-transfer.png - Teal, two people with arrow
Save(CreateIcon(Teal, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawPerson(g, cx - 16, cy, 1.1f, Color.White);
    DrawPerson(g, cx + 16, cy, 1.1f, Color.White);
    // Arrow
    using var pen = new Pen(Color.White, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(pen, cx - 6, cy - 8, cx + 6, cy - 8);
    g.DrawLine(pen, cx + 2, cy - 12, cx + 6, cy - 8);
    g.DrawLine(pen, cx + 2, cy - 4, cx + 6, cy - 8);
}), "host-transfer.png");

// captions-on.png - Blue, "CC" text bold
Save(CreateIcon(Blue, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    using var font = new Font("Segoe UI", 26f, FontStyle.Bold);
    using var brush = new SolidBrush(Color.White);
    using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
    g.DrawString("CC", font, brush, cx, cy, sf);
}), "captions-on.png");

// captions-off.png - Slate, "CC"
Save(CreateIcon(Slate, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    using var font = new Font("Segoe UI", 26f, FontStyle.Bold);
    using var brush = new SolidBrush(Color.White);
    using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
    g.DrawString("CC", font, brush, cx, cy, sf);
}), "captions-off.png");

// timer.png - Orange, clock
Save(CreateIcon(Orange, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawClock(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "timer.png");

// timer-clear.png - DarkOrange, clock with X
Save(CreateIcon(DarkOrange, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawClock(g, cx, cy, SYMBOL_SCALE, Color.White);
    DrawX(g, cx + 14, cy - 14, 5, Color.White);
}), "timer-clear.png");


// =============================================
// PAGE 3 - MEETING INTELLIGENCE
// =============================================
Console.WriteLine("\n--- Page 3: Meeting Intelligence ---");

// flag.png - Purple, flag
Save(CreateIcon(Purple, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawFlag(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "flag.png");

// assign.png - Teal, person with tag
Save(CreateIcon(Teal, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawPerson(g, cx, cy, SYMBOL_SCALE, Color.White);
    // Tag icon
    using var pen = new Pen(Color.White, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(pen, cx + 14, cy - 14, cx + 22, cy - 6);
    g.DrawLine(pen, cx + 14, cy - 14, cx + 19, cy - 14);
    g.DrawLine(pen, cx + 14, cy - 14, cx + 14, cy - 9);
}), "assign.png");

// note.png - Indigo, document with pencil
Save(CreateIcon(Indigo, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawDocument(g, cx - 3, cy, SYMBOL_SCALE, Color.White);
    // Pencil
    using var pen = new Pen(Color.White, 2.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
    g.DrawLine(pen, cx + 12, cy - 9, cx + 20, cy - 18);
    g.DrawLine(pen, cx + 12, cy - 9, cx + 9, cy - 3);
}), "note.png");

// highlight.png - DarkYellow, star
Save(CreateIcon(DarkYellow, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawStar(g, cx, cy, SYMBOL_SCALE, Color.White, true);
}), "highlight.png");

// clear-last.png - DarkRed, undo arrow
Save(CreateIcon(DarkRed, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawUndoArrow(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "clear-last.png");

// preview.png - Purple, chart
Save(CreateIcon(Purple, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawChart(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "preview.png");

// export.png - Green-ish, download
Color GreenExport = ColorTranslator.FromHtml("#27ae60");
Save(CreateIcon(GreenExport, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawDownload(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "export.png");

// export-done.png - Green, checkmark
Save(CreateIcon(Green, "", (g, r) =>
{
    float cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
    DrawCheckmark(g, cx, cy, SYMBOL_SCALE, Color.White);
}), "export-done.png");


Console.WriteLine($"\nDone! Generated {generated} icons to: {outputDir}");
