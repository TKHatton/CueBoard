namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class FullscreenCommand : CueBoardCommand
    {
        private Boolean _isFullscreen = false;

        public FullscreenCommand()
            : base("Fullscreen", "Toggle Zoom fullscreen mode (Alt+F)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+F toggles fullscreen in Zoom
            this.Keyboard?.SendAltKey(KeyboardService.KEY_F);

            this._isFullscreen = !this._isFullscreen;
            PluginLog.Info($"Fullscreen toggled (Alt+F): {(this._isFullscreen ? "ON" : "OFF")}");
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            // Match the dark theme with purple accent
            var builder = new BitmapBuilder(imageSize);
            builder.Clear(new BitmapColor(42, 42, 53)); // Dark background matching icons

            if (this._isFullscreen)
            {
                // Exit fullscreen indicator
                builder.DrawText("⊡", new BitmapColor(139, 92, 246), 36);
            }
            else
            {
                // Enter fullscreen indicator
                builder.DrawText("⊞", new BitmapColor(139, 92, 246), 36);
            }

            return builder.ToImage();
        }
    }
}
