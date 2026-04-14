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
            this.ShowToast("⚡", $"{typeName} Flagged");

        public void Assigned(String personName) =>
            this.ShowToast("→", $"Assigned to {personName}");

        public void NoteAdded(String preview) =>
            this.ShowToast("📝", preview.Length > 40 ? preview.Substring(0, 40) + "..." : preview);

        public void HighlightAdded(Int32 count) =>
            this.ShowToast("★", $"Highlight #{count}");

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
