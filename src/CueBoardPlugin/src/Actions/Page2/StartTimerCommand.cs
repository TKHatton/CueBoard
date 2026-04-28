namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class StartTimerCommand : CueBoardCommand
    {
        public StartTimerCommand()
            : base("Timer", "Start/pause countdown timer", "Operator Mode")
        {
            this.EnableTimerTickUpdates();
        }

        protected override void RunCommand(String actionParameter)
        {
            var timer = this.CueBoard?.Timer;
            if (timer == null)
            {
                return;
            }

            // Update internal state and refresh the device button BEFORE spawning the
            // PowerShell overlay process — the spawn can take 200-400ms and was making
            // the button feel laggy / unresponsive on press.
            Int32 overlaySeconds = 0;
            Boolean shouldShowOverlay = false;

            if (timer.RemainingSeconds <= 0 && !timer.IsRunning && !timer.IsPaused)
            {
                timer.Start();
                overlaySeconds = timer.DurationMinutes * 60;
                shouldShowOverlay = true;
            }
            else if (timer.IsRunning)
            {
                timer.Pause();
            }
            else if (timer.IsPaused)
            {
                timer.Start();
                overlaySeconds = timer.RemainingSeconds;
                shouldShowOverlay = true;
            }

            this.ActionImageChanged();

            if (shouldShowOverlay)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    this.CueBoard?.TimerOverlay?.ShowTimer(overlaySeconds);
                });
            }
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var timer = this.CueBoard?.Timer;
            if (timer == null)
            {
                return this.DrawButton(imageSize, "TIMER", new BitmapColor(80, 80, 80));
            }

            if (timer.IsRunning)
            {
                var remaining = timer.GetDisplayTime();
                return this.DrawButton(imageSize, remaining, new BitmapColor(220, 140, 30));
            }

            if (timer.IsPaused && timer.RemainingSeconds > 0)
            {
                var remaining = timer.GetDisplayTime();
                return this.DrawButton(imageSize, $"❚❚ {remaining}", new BitmapColor(120, 80, 30));
            }

            // Idle — show clock icon
            return this.DrawIcon(imageSize, "timer.png");
        }
    }
}
