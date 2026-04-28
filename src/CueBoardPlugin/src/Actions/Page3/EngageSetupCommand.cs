namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Opens the engage page hosted on GitHub Pages in setup mode (no URL fragment).
    /// The host fills in word cloud prompt, poll questions, rating prompt, and feedback
    /// email. Clicking "Generate &amp; Copy Share Link" inside the page builds a URL with
    /// the config encoded in the fragment, copies it to clipboard, and saves to localStorage.
    /// During the meeting, InteractivePollCommand reads that localStorage and jumps to play.
    /// </summary>
    public class EngageSetupCommand : CueBoardCommand
    {
        private const String EngageSetupUrl = "https://tkhatton.github.io/CueBoard/engage.html";

        public EngageSetupCommand()
            : base("Engage Setup", "Pre-meeting: configure word cloud + poll questions", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            try
            {
                Process.Start(new ProcessStartInfo(EngageSetupUrl) { UseShellExecute = true });
                this.CueBoard?.Toast?.ShowToast("⚙", "Setup page opened", 2000);
                PluginLog.Info($"Engage setup opened: {EngageSetupUrl}");
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Failed to open setup page");
                try
                {
                    Process.Start(new ProcessStartInfo("cmd.exe", $"/c start \"\" \"{EngageSetupUrl}\"")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                }
                catch
                {
                }
            }
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawButton(imageSize, "ENGAGE\nSETUP", new BitmapColor(99, 102, 241));
        }
    }
}
