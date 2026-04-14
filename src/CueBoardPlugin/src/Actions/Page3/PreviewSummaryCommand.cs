namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Linq;
    using Loupedeck.CueBoardPlugin.Models;

    public class PreviewSummaryCommand : CueBoardCommand
    {
        private DateTime _lastPressTime = DateTime.MinValue;
        private Boolean _demoLoaded = false;

        public PreviewSummaryCommand()
            : base("Preview", "Show flag breakdown (double-press for demo data)", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var now = DateTime.Now;

            // Double-press within 2 seconds loads demo data
            if ((now - this._lastPressTime).TotalSeconds < 2)
            {
                this.CueBoard?.Flags?.LoadDemoData(this.State?.MeetingStartTime ?? DateTime.Now);
                this._demoLoaded = true;
                this.CueBoard?.NotifyRefreshAllImages();
                PluginLog.Info("Demo data loaded via Preview double-press");

                // Reset indicator after 2 seconds
                System.Threading.Tasks.Task.Delay(2000).ContinueWith(_ =>
                {
                    this._demoLoaded = false;
                    this.ActionImageChanged();
                });

                this._lastPressTime = DateTime.MinValue;
                return;
            }

            this._lastPressTime = now;
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (this._demoLoaded)
            {
                return this.DrawButton(imageSize, "DEMO\nLOADED", new BitmapColor(139, 92, 246));
            }

            var flags = this.CueBoard?.Flags;
            if (flags == null || flags.FlagCount == 0)
            {
                return this.DrawIcon(imageSize, "preview.png");
            }

            var builder = new BitmapBuilder(imageSize);
            builder.Clear(new BitmapColor(100, 50, 150));
            builder.DrawText($"VIEW\n({flags.FlagCount})", BitmapColor.White);
            return builder.ToImage();
        }
    }
}
