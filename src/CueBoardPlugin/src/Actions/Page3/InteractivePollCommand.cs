namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Opens the LIVE engagement page hosted on GitHub Pages, with ?play=1 so the page
    /// reads the host's saved config from localStorage, redirects into play mode, and
    /// copies the share URL to the clipboard for pasting in Zoom chat.
    ///
    /// Pre-meeting setup happens via EngageSetupCommand on the same hosted page.
    /// </summary>
    public class InteractivePollCommand : CueBoardCommand
    {
        // The engage page lives on GitHub Pages so attendees on any device can join.
        private const String EngageUrlPlay = "https://tkhatton.github.io/CueBoard/engage.html?play=1";

        public InteractivePollCommand()
            : base("Interactive Poll", "Open the live engagement page and copy share link to clipboard", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            try
            {
                Process.Start(new ProcessStartInfo(EngageUrlPlay) { UseShellExecute = true });
                this.CueBoard?.Toast?.ShowToast("📊", "Engage open - link copied", 3000);
                PluginLog.Info($"Engagement page opened: {EngageUrlPlay}");
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Failed to open engagement page");
                try
                {
                    Process.Start(new ProcessStartInfo("cmd.exe", $"/c start \"\" \"{EngageUrlPlay}\"")
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
            return this.DrawIcon(imageSize, "poll.png");
        }
    }
}
