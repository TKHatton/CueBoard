namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public class InputDialogService
    {
        /// <summary>
        /// Shows a dark-theme input dialog on the user's screen (private, topmost).
        /// Returns the typed text, or null if cancelled/closed.
        /// Runs asynchronously so it doesn't block the plugin service.
        /// </summary>
        public Task<String> ShowInputDialogAsync(String title, String placeholder)
        {
            return Task.Run(() =>
            {
                try
                {
                    var resultFile = Path.Combine(Path.GetTempPath(), $"CueBoardInput_{Guid.NewGuid():N}.txt");
                    var script = GenerateInputScript(title, placeholder, resultFile);
                    var scriptPath = Path.Combine(Path.GetTempPath(), "CueBoardInput.ps1");
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
                    process.WaitForExit(30000); // 30 second timeout

                    if (!process.HasExited)
                    {
                        process.Kill();
                        process.Dispose();
                        return null;
                    }

                    process.Dispose();

                    if (File.Exists(resultFile))
                    {
                        var result = File.ReadAllText(resultFile).Trim();
                        File.Delete(resultFile);
                        PluginLog.Info($"Input dialog returned: {result}");
                        return String.IsNullOrWhiteSpace(result) ? null : result;
                    }

                    PluginLog.Info("Input dialog cancelled");
                    return null;
                }
                catch (Exception ex)
                {
                    PluginLog.Error(ex, "Failed to show input dialog");
                    return null;
                }
            });
        }

        private static String GenerateInputScript(String title, String placeholder, String resultFile)
        {
            // Escape single quotes for PowerShell
            var safeTitle = title.Replace("'", "''");
            var safePlaceholder = placeholder.Replace("'", "''");
            var safeResultFile = resultFile.Replace("'", "''");

            return $@"
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$resultFile = '{safeResultFile}'

$f = New-Object Windows.Forms.Form
$f.Text = '{safeTitle}'
$f.Width = 420
$f.Height = 150
$f.FormBorderStyle = 'None'
$f.BackColor = [Drawing.Color]::FromArgb(25, 25, 35)
$f.ForeColor = [Drawing.Color]::White
$f.TopMost = $true
$f.StartPosition = 'Manual'
$screenW = [Windows.Forms.Screen]::PrimaryScreen.WorkingArea.Width
$f.Location = New-Object Drawing.Point(($screenW - 440) / 2, 30)
$f.ShowInTaskbar = $false
$f.Opacity = 0.97

# Title label
$titleLbl = New-Object Windows.Forms.Label
$titleLbl.Text = '{safeTitle}'
$titleLbl.Font = New-Object Drawing.Font('Segoe UI', 10, [Drawing.FontStyle]::Bold)
$titleLbl.ForeColor = [Drawing.Color]::FromArgb(139, 92, 246)
$titleLbl.Location = New-Object Drawing.Point(14, 10)
$titleLbl.AutoSize = $true
$f.Controls.Add($titleLbl)

# Close button
$closeBtn = New-Object Windows.Forms.Label
$closeBtn.Text = 'X'
$closeBtn.Font = New-Object Drawing.Font('Segoe UI', 11, [Drawing.FontStyle]::Bold)
$closeBtn.ForeColor = [Drawing.Color]::FromArgb(150, 150, 150)
$closeBtn.Location = New-Object Drawing.Point(390, 6)
$closeBtn.Size = New-Object Drawing.Size(22, 22)
$closeBtn.TextAlign = 'MiddleCenter'
$closeBtn.Cursor = [Windows.Forms.Cursors]::Hand
$closeBtn.Add_Click({{ $f.Close() }})
$closeBtn.Add_MouseEnter({{ $closeBtn.ForeColor = [Drawing.Color]::White }})
$closeBtn.Add_MouseLeave({{ $closeBtn.ForeColor = [Drawing.Color]::FromArgb(150, 150, 150) }})
$f.Controls.Add($closeBtn)

# Text input
$txt = New-Object Windows.Forms.TextBox
$txt.Font = New-Object Drawing.Font('Segoe UI', 12)
$txt.Location = New-Object Drawing.Point(14, 42)
$txt.Size = New-Object Drawing.Size(310, 30)
$txt.BackColor = [Drawing.Color]::FromArgb(40, 40, 55)
$txt.ForeColor = [Drawing.Color]::White
$txt.BorderStyle = 'FixedSingle'
$f.Controls.Add($txt)

# OK button
$okBtn = New-Object Windows.Forms.Button
$okBtn.Text = 'OK'
$okBtn.Font = New-Object Drawing.Font('Segoe UI', 10, [Drawing.FontStyle]::Bold)
$okBtn.Location = New-Object Drawing.Point(334, 40)
$okBtn.Size = New-Object Drawing.Size(70, 34)
$okBtn.BackColor = [Drawing.Color]::FromArgb(139, 92, 246)
$okBtn.ForeColor = [Drawing.Color]::White
$okBtn.FlatStyle = 'Flat'
$okBtn.FlatAppearance.BorderSize = 0
$okBtn.Cursor = [Windows.Forms.Cursors]::Hand
$okBtn.Add_Click({{
    if ($txt.Text.Trim() -ne '') {{
        [IO.File]::WriteAllText($resultFile, $txt.Text.Trim())
    }}
    $f.Close()
}})
$f.Controls.Add($okBtn)

# Placeholder text
$placeholderText = '{safePlaceholder}'
$txt.ForeColor = [Drawing.Color]::FromArgb(120, 120, 140)
$txt.Text = $placeholderText
$txt.Add_GotFocus({{
    if ($txt.Text -eq $placeholderText) {{
        $txt.Text = ''
        $txt.ForeColor = [Drawing.Color]::White
    }}
}})
$txt.Add_LostFocus({{
    if ($txt.Text.Trim() -eq '') {{
        $txt.ForeColor = [Drawing.Color]::FromArgb(120, 120, 140)
        $txt.Text = $placeholderText
    }}
}})

# Enter key submits
$txt.Add_KeyDown({{ param($s, $e)
    if ($e.KeyCode -eq 'Return') {{
        $e.SuppressKeyPress = $true
        if ($txt.Text.Trim() -ne '' -and $txt.Text -ne $placeholderText) {{
            [IO.File]::WriteAllText($resultFile, $txt.Text.Trim())
        }}
        $f.Close()
    }}
    if ($e.KeyCode -eq 'Escape') {{
        $f.Close()
    }}
}})

# Hint label
$hint = New-Object Windows.Forms.Label
$hint.Text = 'Enter to submit · Esc to cancel'
$hint.Font = New-Object Drawing.Font('Segoe UI', 8)
$hint.ForeColor = [Drawing.Color]::FromArgb(100, 100, 120)
$hint.Location = New-Object Drawing.Point(14, 82)
$hint.AutoSize = $true
$f.Controls.Add($hint)

# Draggable
$drag = $false
$dragPt = [Drawing.Point]::Empty
$f.Add_MouseDown({{ param($s,$e) $script:drag=$true; $script:dragPt=$e.Location }})
$f.Add_MouseUp({{ $script:drag=$false }})
$f.Add_MouseMove({{ param($s,$e) if($script:drag){{ $f.Location = [Drawing.Point]::new($f.Location.X+$e.X-$script:dragPt.X, $f.Location.Y+$e.Y-$script:dragPt.Y) }} }})

# Focus the text field on load
$f.Add_Shown({{ $txt.Focus(); if ($txt.Text -eq $placeholderText) {{ $txt.SelectAll() }} }})

[Windows.Forms.Application]::Run($f)
";
        }
    }
}
