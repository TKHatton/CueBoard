namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class TimerDial : CueBoardAdjustment
    {
        public TimerDial()
            : base("Timer Duration", "Rotate to adjust timer, press to start/pause", "Operator Mode", hasReset: true)
        {
            this.EnableTimerTickUpdates();
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            var timer = this.CueBoard?.Timer;
            if (timer == null)
            {
                return;
            }

            timer.AdjustDuration(diff > 0 ? 1 : -1);
            this.AdjustmentValueChanged();
            this.CueBoard?.NotifyRefreshAllImages();
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
                timer.Start();
                this.CueBoard?.TimerOverlay?.ShowTimer(timer.DurationMinutes * 60);
            }
            else
            {
                timer.Pause();
            }

            this.AdjustmentValueChanged();
            this.CueBoard?.NotifyRefreshAllImages();
        }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var timer = this.CueBoard?.Timer;
            if (timer == null)
            {
                return "0:00";
            }

            return timer.IsRunning ? timer.GetDisplayTime() : $"{timer.DurationMinutes}:00";
        }
    }
}
