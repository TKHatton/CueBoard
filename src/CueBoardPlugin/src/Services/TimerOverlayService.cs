namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class TimerOverlayService
    {
        private Process _overlayProcess;

        public void ShowTimer(Int32 durationSeconds)
        {
            this.HideTimer();

            try
            {
                var script = GenerateTimerScript(durationSeconds);
                var scriptPath = Path.Combine(Path.GetTempPath(), "CueBoardTimer.ps1");
                File.WriteAllText(scriptPath, script);

                this._overlayProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-ExecutionPolicy Bypass -WindowStyle Hidden -File \"{scriptPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                this._overlayProcess.Start();
                PluginLog.Info($"Timer overlay launched: {durationSeconds} seconds");
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Failed to launch timer overlay");
            }
        }

        public void HideTimer()
        {
            try
            {
                if (this._overlayProcess != null && !this._overlayProcess.HasExited)
                {
                    this._overlayProcess.Kill();
                    this._overlayProcess.Dispose();
                    this._overlayProcess = null;
                    PluginLog.Info("Timer overlay closed");
                }
            }
            catch
            {
            }
        }

        private static String GenerateTimerScript(Int32 totalSeconds)
        {
            return @"
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$seconds = " + totalSeconds + @"
$end = (Get-Date).AddSeconds($seconds)

$f = New-Object Windows.Forms.Form
$f.Text = 'CueBoard Timer'
$f.Width = 300
$f.Height = 100
$f.FormBorderStyle = 'None'
$f.BackColor = [Drawing.Color]::FromArgb(25, 25, 35)
$f.ForeColor = [Drawing.Color]::White
$f.TopMost = $true
$f.StartPosition = 'Manual'
$screenW = [Windows.Forms.Screen]::PrimaryScreen.WorkingArea.Width
$f.Location = New-Object Drawing.Point(($screenW - 320) / 2, 30)
$f.ShowInTaskbar = $false
$f.Opacity = 0.95

$title = New-Object Windows.Forms.Label
$title.Text = 'CUEBOARD TIMER'
$title.Font = New-Object Drawing.Font('Segoe UI', 9, [Drawing.FontStyle]::Bold)
$title.ForeColor = [Drawing.Color]::FromArgb(139, 92, 246)
$title.Location = New-Object Drawing.Point(10, 6)
$title.AutoSize = $true
$f.Controls.Add($title)

$closeBtn = New-Object Windows.Forms.Label
$closeBtn.Text = 'X'
$closeBtn.Font = New-Object Drawing.Font('Segoe UI', 12, [Drawing.FontStyle]::Bold)
$closeBtn.ForeColor = [Drawing.Color]::FromArgb(150, 150, 150)
$closeBtn.Location = New-Object Drawing.Point(270, 2)
$closeBtn.Size = New-Object Drawing.Size(25, 25)
$closeBtn.TextAlign = 'MiddleCenter'
$closeBtn.Cursor = [Windows.Forms.Cursors]::Hand
$closeBtn.Add_Click({ $f.Close() })
$closeBtn.Add_MouseEnter({ $closeBtn.ForeColor = [Drawing.Color]::White })
$closeBtn.Add_MouseLeave({ $closeBtn.ForeColor = [Drawing.Color]::FromArgb(150, 150, 150) })
$f.Controls.Add($closeBtn)

$lbl = New-Object Windows.Forms.Label
$lbl.Font = New-Object Drawing.Font('Consolas', 32, [Drawing.FontStyle]::Bold)
$lbl.ForeColor = [Drawing.Color]::White
$lbl.Location = New-Object Drawing.Point(10, 28)
$lbl.Size = New-Object Drawing.Size(280, 55)
$f.Controls.Add($lbl)

$drag = $false
$dragPt = [Drawing.Point]::Empty
$f.Add_MouseDown({ param($s,$e) $script:drag=$true; $script:dragPt=$e.Location })
$f.Add_MouseUp({ $script:drag=$false })
$f.Add_MouseMove({ param($s,$e) if($script:drag){ $f.Location = [Drawing.Point]::new($f.Location.X+$e.X-$script:dragPt.X, $f.Location.Y+$e.Y-$script:dragPt.Y) } })
$lbl.Add_MouseDown({ param($s,$e) $script:drag=$true; $script:dragPt=$e.Location })
$lbl.Add_MouseUp({ $script:drag=$false })
$lbl.Add_MouseMove({ param($s,$e) if($script:drag){ $f.Location = [Drawing.Point]::new($f.Location.X+$e.X-$script:dragPt.X, $f.Location.Y+$e.Y-$script:dragPt.Y) } })

$t = New-Object Windows.Forms.Timer
$t.Interval = 250
$t.Add_Tick({
    $left = ($end - (Get-Date)).TotalSeconds
    if ($left -le 0) {
        $lbl.Text = '0:00'
        $lbl.ForeColor = [Drawing.Color]::FromArgb(230, 57, 70)
        $title.Text = 'TIME IS UP!'
        $t.Stop()
        $ct = New-Object Windows.Forms.Timer
        $ct.Interval = 10000
        $ct.Add_Tick({ $f.Close() })
        $ct.Start()
    } else {
        $m = [Math]::Floor($left / 60)
        $s = [Math]::Floor($left % 60)
        $lbl.Text = ('{0}:{1:D2}' -f [int]$m, [int]$s)
        if ($left -le 10) { $lbl.ForeColor = [Drawing.Color]::FromArgb(230, 57, 70) }
        elseif ($left -le 30) { $lbl.ForeColor = [Drawing.Color]::FromArgb(243, 156, 18) }
        else { $lbl.ForeColor = [Drawing.Color]::White }
    }
})

$left = ($end - (Get-Date)).TotalSeconds
$m = [Math]::Floor($left / 60)
$s = [Math]::Floor($left % 60)
$lbl.Text = ('{0}:{1:D2}' -f [int]$m, [int]$s)

$t.Start()
[Windows.Forms.Application]::Run($f)
";
        }
    }
}
