namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class StartTimerCommand : CueBoardCommand
    {
        public StartTimerCommand()
            : base("Timer", "Start/pause countdown timer", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var timer = this.CueBoard?.Timer;
            if (timer == null)
            {
                return;
            }

            if (timer.RemainingSeconds <= 0 && !timer.IsRunning && !timer.IsPaused)
            {
                // Timer is idle or expired — start fresh
                timer.Start();
                this.CueBoard?.TimerOverlay?.ShowTimer(timer.DurationMinutes * 60);
            }
            else if (timer.IsRunning)
            {
                // Running → Pause
                timer.Pause();
            }
            else if (timer.IsPaused)
            {
                // Paused → Resume
                timer.Start();
                this.CueBoard?.TimerOverlay?.ShowTimer(timer.RemainingSeconds);
            }

            this.ActionImageChanged();
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

            // Idle — show duration that will start
            return this.DrawButton(imageSize, $"▶ {timer.DurationMinutes}:00", new BitmapColor(80, 80, 80));
        }
    }
}
