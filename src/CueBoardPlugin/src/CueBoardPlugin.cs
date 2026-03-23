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

        // Event for commands to subscribe to for global refresh
        public event Action RefreshAllImages;

        public void NotifyRefreshAllImages() => this.RefreshAllImages?.Invoke();

        public CueBoardPlugin()
        {
            PluginLog.Init(this.Log);
            PluginResources.Init(this.Assembly);
        }

        public override void Load()
        {
            this.Keyboard = new KeyboardService();
            this.ZoomDetection = new ZoomDetectionService();
            this.State = new SessionState();
            this.Flags = new FlagService();
            this.Timer = new TimerService();
            this.Export = new ExportService();
            this.TimerOverlay = new TimerOverlayService();

            // Refresh all button images every second when timer is running
            this.Timer.TimerTick += (remaining) =>
            {
                this.NotifyRefreshAllImages();
            };

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

            PluginLog.Info("CueBoard plugin loaded successfully");
        }

        public override void Unload()
        {
            this.Timer?.Dispose();
            PluginLog.Info("CueBoard plugin unloaded");
        }
    }
}
