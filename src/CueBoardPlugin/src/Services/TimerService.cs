namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using Timer = System.Timers.Timer;

    public class TimerService : IDisposable
    {
        private readonly Timer _timer;
        private DateTime _startTime;
        private Int32 _totalSeconds;

        public Int32 DurationMinutes { get; private set; } = 5;
        public Int32 RemainingSeconds { get; private set; } = 0;
        public Boolean IsRunning { get; private set; } = false;

        public event Action<Int32> TimerTick;
        public event Action TimerExpired;

        public TimerService()
        {
            this._timer = new Timer(1000);
            this._timer.Elapsed += (s, e) => this.OnTick();
            this._timer.AutoReset = true;
        }

        public void AdjustDuration(Int32 deltaMins)
        {
            if (this.IsRunning)
            {
                return;
            }

            this.DurationMinutes = Math.Clamp(this.DurationMinutes + deltaMins, 1, 60);
            this.RemainingSeconds = this.DurationMinutes * 60;
            PluginLog.Info($"Timer duration set to {this.DurationMinutes} minutes");
        }

        public void Start()
        {
            if (this.IsRunning)
            {
                this.Pause();
                return;
            }

            this._totalSeconds = this.DurationMinutes * 60;
            this.RemainingSeconds = this._totalSeconds;
            this._startTime = DateTime.UtcNow;
            this.IsRunning = true;
            this._timer.Start();
            PluginLog.Info($"Timer started: {this.DurationMinutes} minutes");
        }

        public void Pause()
        {
            this.IsRunning = false;
            this._timer.Stop();
            PluginLog.Info($"Timer paused at {this.RemainingSeconds} seconds remaining");
        }

        public void Reset()
        {
            this.IsRunning = false;
            this._timer.Stop();
            this.RemainingSeconds = 0;
            this.DurationMinutes = 5;
            PluginLog.Info("Timer reset");
        }

        private void OnTick()
        {
            if (!this.IsRunning)
            {
                return;
            }

            var elapsed = (Int32)(DateTime.UtcNow - this._startTime).TotalSeconds;
            this.RemainingSeconds = Math.Max(0, this._totalSeconds - elapsed);
            this.TimerTick?.Invoke(this.RemainingSeconds);

            if (this.RemainingSeconds <= 0)
            {
                this.IsRunning = false;
                this._timer.Stop();
                this.TimerExpired?.Invoke();
                PluginLog.Info("Timer expired!");
            }
        }

        public String GetDisplayTime()
        {
            var mins = this.RemainingSeconds / 60;
            var secs = this.RemainingSeconds % 60;
            return $"{mins}:{secs:D2}";
        }

        public void Dispose()
        {
            this._timer?.Stop();
            this._timer?.Dispose();
        }
    }
}
