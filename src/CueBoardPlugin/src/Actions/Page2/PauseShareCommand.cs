namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class PauseShareCommand : CueBoardCommand
    {
        private Boolean _isPaused = false;

        public PauseShareCommand()
            : base("Pause Share", "Pause or resume screen sharing (Alt+T)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+T pauses/resumes screen sharing in Zoom
            this.Keyboard?.SendAltKey(KeyboardService.KEY_T);

            this._isPaused = !this._isPaused;
            PluginLog.Info($"Screen share {(this._isPaused ? "PAUSED" : "RESUMED")} (Alt+T)");
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            // Use share icons - off when paused, on when sharing
            return this._isPaused
                ? this.DrawIcon(imageSize, "share-off.png")
                : this.DrawIcon(imageSize, "share-on.png");
        }
    }
}
