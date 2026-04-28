namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class ToastService
    {
        /// <summary>
        /// Shows a small dark-theme toast notification on the user's screen.
        /// Auto-disappears after the specified duration. Private — only on user's display.
        /// </summary>
        public void ShowToast(String icon, String message, Int32 durationMs = 2500)
        {
            try
            {
                var script = GenerateToastScript(icon, message, durationMs);
                var scriptPath = Path.Combine(Path.GetTempPath(), "CueBoardToast.ps1");
                File.WriteAllText(scriptPath, script);

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-ExecutionPolicy Bypass -WindowStyle Hidden -File \"{scriptPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
            }
            catch (Exception ex)
            {
                PluginLog.Warning($"Toast failed: {ex.Message}");
            }
        }

        // Convenience methods for common toast types
        public void FlagAdded(String typeName) =>
            this.ShowFlagOverlay(typeName, ColorForFlagType(typeName));

        public void Assigned(String personName) =>
            this.ShowToast("→", $"Assigned to {personName}");

        public void NoteAdded(String preview) =>
            this.ShowToast("📝", preview.Length > 40 ? preview.Substring(0, 40) + "..." : preview);

        public void HighlightAdded(Int32 count) =>
            this.ShowFlagOverlay("Highlight", "245,158,11");

        private static String ColorForFlagType(String typeName)
        {
            // RGB strings used directly by the PowerShell script.
            switch (typeName)
            {
                case "Action":
                case "Action Item":
                case "ActionItem": return "139,92,246";
                case "Decision": return "52,152,219";
                case "Follow-Up":
                case "FollowUp": return "46,204,113";
                case "Highlight": return "245,158,11";
                case "Bookmark": return "243,156,18";
                default: return "139,92,246";
            }
        }

        /// <summary>
        /// Bigger, flag-styled corner overlay. Visible to meeting attendees during desktop
        /// screen-share but unobtrusive enough to stay on during conversation.
        /// </summary>
        public void ShowFlagOverlay(String typeName, String rgb)
        {
            try
            {
                var script = GenerateFlagScript(typeName, rgb);
                var scriptPath = Path.Combine(Path.GetTempPath(), "CueBoardFlag.ps1");
                File.WriteAllText(scriptPath, script);

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-ExecutionPolicy Bypass -WindowStyle Hidden -File \"{scriptPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
            }
            catch (Exception ex)
            {
                PluginLog.Warning($"Flag overlay failed: {ex.Message}");
            }
        }

        private static String GenerateFlagScript(String typeName, String rgb)
        {
            var safeName = typeName.Replace("'", "''").Replace("\"", "`\"").ToUpperInvariant();
            var time = DateTime.Now.ToString("h:mm:ss tt");

            return $@"
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$f = New-Object Windows.Forms.Form
$f.Width = 360
$f.Height = 96
$f.FormBorderStyle = 'None'
$f.BackColor = [Drawing.Color]::FromArgb(20, 20, 28)
$f.TopMost = $true
$f.StartPosition = 'Manual'
$f.ShowInTaskbar = $false
$f.Opacity = 0.0

$screen = [Windows.Forms.Screen]::PrimaryScreen.WorkingArea
$f.Location = New-Object Drawing.Point(($screen.Right - 380), 24)

# Rounded corners
$path = New-Object Drawing.Drawing2D.GraphicsPath
$path.AddArc(0, 0, 18, 18, 180, 90)
$path.AddArc(($f.Width - 18), 0, 18, 18, 270, 90)
$path.AddArc(($f.Width - 18), ($f.Height - 18), 18, 18, 0, 90)
$path.AddArc(0, ($f.Height - 18), 18, 18, 90, 90)
$path.CloseFigure()
$f.Region = New-Object Drawing.Region($path)

# Bold color bar on left (8 px wide) — type-coded
$accent = New-Object Windows.Forms.Panel
$accent.BackColor = [Drawing.Color]::FromArgb({rgb})
$accent.Location = New-Object Drawing.Point(0, 0)
$accent.Size = New-Object Drawing.Size(8, 96)
$f.Controls.Add($accent)

# Big flag glyph — uses Segoe UI Emoji
$flag = New-Object Windows.Forms.Label
$flag.Text = [char]0xD83D + [char]0xDEA9
$flag.Font = New-Object Drawing.Font('Segoe UI Emoji', 30)
$flag.Location = New-Object Drawing.Point(20, 18)
$flag.Size = New-Object Drawing.Size(64, 60)
$flag.TextAlign = 'MiddleCenter'
$f.Controls.Add($flag)

# Brand row
$brand = New-Object Windows.Forms.Label
$brand.Text = 'CUEBOARD · FLAGGED'
$brand.Font = New-Object Drawing.Font('Segoe UI', 8, [Drawing.FontStyle]::Bold)
$brand.ForeColor = [Drawing.Color]::FromArgb({rgb})
$brand.Location = New-Object Drawing.Point(92, 14)
$brand.AutoSize = $true
$f.Controls.Add($brand)

# Type name (big)
$type = New-Object Windows.Forms.Label
$type.Text = '{safeName}'
$type.Font = New-Object Drawing.Font('Segoe UI', 18, [Drawing.FontStyle]::Bold)
$type.ForeColor = [Drawing.Color]::White
$type.Location = New-Object Drawing.Point(90, 30)
$type.AutoSize = $true
$f.Controls.Add($type)

# Time
$ts = New-Object Windows.Forms.Label
$ts.Text = '{time}'
$ts.Font = New-Object Drawing.Font('Segoe UI', 8)
$ts.ForeColor = [Drawing.Color]::FromArgb(140, 140, 160)
$ts.Location = New-Object Drawing.Point(92, 66)
$ts.AutoSize = $true
$f.Controls.Add($ts)

# Fade in
$fadeIn = New-Object Windows.Forms.Timer
$fadeIn.Interval = 12
$fadeIn.Add_Tick({{
    if ($f.Opacity -lt 0.95) {{ $f.Opacity += 0.10 }}
    else {{ $f.Opacity = 0.95; $fadeIn.Stop() }}
}})

# Auto-close (hold 2.4s, then fade)
$autoClose = New-Object Windows.Forms.Timer
$autoClose.Interval = 2400
$autoClose.Add_Tick({{
    $autoClose.Stop()
    $fadeOut = New-Object Windows.Forms.Timer
    $fadeOut.Interval = 12
    $fadeOut.Add_Tick({{
        if ($f.Opacity -gt 0.05) {{ $f.Opacity -= 0.07 }}
        else {{ $f.Close() }}
    }})
    $fadeOut.Start()
}})

$f.Add_Shown({{ $fadeIn.Start(); $autoClose.Start() }})
[Windows.Forms.Application]::Run($f)
";
        }

        public void FlagCleared(String typeName) =>
            this.ShowToast("↩", $"{typeName} removed");

        public void MeetingReset() =>
            this.ShowToast("🔄", "Meeting reset");

        public void Exported(String path) =>
            this.ShowToast("✓", "Summary exported");

        private static String GenerateToastScript(String icon, String message, Int32 durationMs)
        {
            var safeMessage = message.Replace("'", "''").Replace("\"", "`\"");
            var safeIcon = icon.Replace("'", "''");

            return $@"
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$f = New-Object Windows.Forms.Form
$f.Width = 340
$f.Height = 52
$f.FormBorderStyle = 'None'
$f.BackColor = [Drawing.Color]::FromArgb(30, 30, 42)
$f.TopMost = $true
$f.StartPosition = 'Manual'
$f.ShowInTaskbar = $false
$f.Opacity = 0.0

# Position top-right
$screen = [Windows.Forms.Screen]::PrimaryScreen.WorkingArea
$f.Location = New-Object Drawing.Point(($screen.Right - 360), 20)

# Rounded appearance via region
$path = New-Object Drawing.Drawing2D.GraphicsPath
$path.AddArc(0, 0, 16, 16, 180, 90)
$path.AddArc(($f.Width - 16), 0, 16, 16, 270, 90)
$path.AddArc(($f.Width - 16), ($f.Height - 16), 16, 16, 0, 90)
$path.AddArc(0, ($f.Height - 16), 16, 16, 90, 90)
$path.CloseFigure()
$f.Region = New-Object Drawing.Region($path)

# Purple accent bar on left
$accent = New-Object Windows.Forms.Panel
$accent.BackColor = [Drawing.Color]::FromArgb(139, 92, 246)
$accent.Location = New-Object Drawing.Point(0, 0)
$accent.Size = New-Object Drawing.Size(4, 52)
$f.Controls.Add($accent)

# Icon
$iconLbl = New-Object Windows.Forms.Label
$iconLbl.Text = '{safeIcon}'
$iconLbl.Font = New-Object Drawing.Font('Segoe UI Emoji', 16)
$iconLbl.Location = New-Object Drawing.Point(14, 8)
$iconLbl.Size = New-Object Drawing.Size(36, 36)
$iconLbl.ForeColor = [Drawing.Color]::White
$f.Controls.Add($iconLbl)

# Brand
$brand = New-Object Windows.Forms.Label
$brand.Text = 'CUEBOARD'
$brand.Font = New-Object Drawing.Font('Segoe UI', 7, [Drawing.FontStyle]::Bold)
$brand.ForeColor = [Drawing.Color]::FromArgb(139, 92, 246)
$brand.Location = New-Object Drawing.Point(52, 6)
$brand.AutoSize = $true
$f.Controls.Add($brand)

# Message
$msg = New-Object Windows.Forms.Label
$msg.Text = '{safeMessage}'
$msg.Font = New-Object Drawing.Font('Segoe UI', 11)
$msg.ForeColor = [Drawing.Color]::White
$msg.Location = New-Object Drawing.Point(52, 22)
$msg.Size = New-Object Drawing.Size(275, 24)
$f.Controls.Add($msg)

# Fade-in timer
$fadeIn = New-Object Windows.Forms.Timer
$fadeIn.Interval = 15
$fadeIn.Add_Tick({{
    if ($f.Opacity -lt 0.95) {{
        $f.Opacity += 0.08
    }} else {{
        $f.Opacity = 0.95
        $fadeIn.Stop()
    }}
}})

# Auto-close timer
$autoClose = New-Object Windows.Forms.Timer
$autoClose.Interval = {durationMs}
$autoClose.Add_Tick({{
    $autoClose.Stop()
    # Fade out
    $fadeOut = New-Object Windows.Forms.Timer
    $fadeOut.Interval = 15
    $fadeOut.Add_Tick({{
        if ($f.Opacity -gt 0.05) {{
            $f.Opacity -= 0.06
        }} else {{
            $f.Close()
        }}
    }})
    $fadeOut.Start()
}})

$f.Add_Shown({{
    $fadeIn.Start()
    $autoClose.Start()
}})

[Windows.Forms.Application]::Run($f)
";
        }
    }
}
