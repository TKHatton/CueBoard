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

            if (!timer.IsRunning)
            {
                // Starting the timer — launch overlay on screen
                timer.Start();
                this.CueBoard?.TimerOverlay?.ShowTimer(timer.DurationMinutes * 60);
            }
            else
            {
                // Pausing
                timer.Pause();
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

            if (timer.RemainingSeconds > 0)
            {
                var remaining = timer.GetDisplayTime();
                return this.DrawButton(imageSize, $"PAUSED\n{remaining}", new BitmapColor(120, 80, 30));
            }

            return this.DrawIcon(imageSize, "timer.png");
        }
    }
}
