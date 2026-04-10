namespace Loupedeck.CueBoardPlugin
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class CueBoardPlugin : Plugin
    {
        public override Boolean UsesApplicationApiOnly => true;
        public override Boolean HasNoApplication => true;

        public KeyboardService Keyboard { get; private set; }
        public ZoomDetectionService ZoomDetection { get; private set; }
        public SessionState State { get; private set; }
        public FlagService Flags { get; private set; }
        public TimerService Timer { get; private set; }
        public ExportService Export { get; private set; }
        public TimerOverlayService TimerOverlay { get; private set; }

        // Event for timer-related actions only (fires every second during countdown)
        public event Action TimerDisplayChanged;

        // Event for all buttons (fires on meaningful state changes only)
        public event Action RefreshAllImages;

        public void NotifyTimerDisplayChanged() => this.TimerDisplayChanged?.Invoke();
        public void NotifyRefreshAllImages() => this.RefreshAllImages?.Invoke();

        public CueBoardPlugin()
        {
            PluginLog.Init(this.Log);
            PluginResources.Init(this.Assembly);
            PluginLog.Info("CueBoard plugin constructed");
        }

        public override void Load()
        {
            PluginLog.Info("CueBoard plugin Load() starting...");

            this.Keyboard = new KeyboardService();
            this.ZoomDetection = new ZoomDetectionService();
            this.State = new SessionState();
            this.Flags = new FlagService();
            this.Timer = new TimerService();
            this.Export = new ExportService();
            this.TimerOverlay = new TimerOverlayService();

            // Only refresh timer-related buttons each second (not all 32+)
            this.Timer.TimerTick += (remaining) =>
            {
                this.NotifyTimerDisplayChanged();
            };

            // On timer expiry, refresh everything (state changed meaningfully)
            this.Timer.TimerExpired += () =>
            {
                this.NotifyRefreshAllImages();
                PluginLog.Info("Timer expired — all buttons refreshed");
            };

            // Close timer overlay when timer resets
            this.State.StateChanged += () =>
            {
                if (!this.Timer.IsRunning && this.Timer.RemainingSeconds <= 0)
                {
                    this.TimerOverlay?.HideTimer();
                }
            };

            PluginLog.Info("CueBoard plugin loaded successfully — all services initialized");
        }

        public override void Unload()
        {
            PluginLog.Info("CueBoard plugin Unload() called — shutting down");
            this.Timer?.Dispose();
            PluginLog.Info("CueBoard plugin unloaded");
        }
    }
}
