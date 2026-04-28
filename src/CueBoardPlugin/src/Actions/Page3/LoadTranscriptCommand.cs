namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Opens a file picker so the user can select a VTT transcript manually.
    /// Backup for when Zoom auto-detection misses the file (different folder, different
    /// 24-hour window, or transcript isn't generated automatically by Zoom plan).
    /// </summary>
    public class LoadTranscriptCommand : CueBoardCommand
    {
        public LoadTranscriptCommand()
            : base("Load Transcript", "Pick a VTT transcript file to link with flags", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var transcript = this.CueBoard?.Transcript;
            if (transcript == null)
            {
                return;
            }

            try
            {
                var path = ShowFilePicker();
                if (String.IsNullOrEmpty(path))
                {
                    this.CueBoard?.Toast?.ShowToast("✖", "Cancelled", 1500);
                    return;
                }

                if (transcript.LoadVtt(path))
                {
                    this.CueBoard?.Toast?.ShowToast("📜", $"Transcript loaded ({transcript.LineCount} lines)", 2500);
                    PluginLog.Info($"Manually loaded transcript: {path}");
                }
                else
                {
                    this.CueBoard?.Toast?.ShowToast("⚠", "Could not parse VTT", 2500);
                }
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Manual transcript load failed");
                this.CueBoard?.Toast?.ShowToast("⚠", "Load failed", 2000);
            }
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawButton(imageSize, "LOAD\nVTT", new BitmapColor(70, 70, 90));
        }

        private static String ShowFilePicker()
        {
            var tempOut = Path.Combine(Path.GetTempPath(), $"cueboard_picked_{Guid.NewGuid():N}.txt");
            var script = @"
Add-Type -AssemblyName System.Windows.Forms
$dlg = New-Object System.Windows.Forms.OpenFileDialog
$dlg.Filter = 'Transcript files (*.vtt;*.srt)|*.vtt;*.srt|All files (*.*)|*.*'
$dlg.Title = 'Pick a transcript file'
if ($dlg.ShowDialog() -eq 'OK') {
    Set-Content -Path '" + tempOut.Replace("'", "''") + @"' -Value $dlg.FileName -NoNewline
}
";

            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -STA -Command \"{script.Replace("\"", "\\\"")}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit(60000);
            }

            try
            {
                if (File.Exists(tempOut))
                {
                    var picked = File.ReadAllText(tempOut, Encoding.UTF8).Trim();
                    File.Delete(tempOut);
                    return picked;
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
