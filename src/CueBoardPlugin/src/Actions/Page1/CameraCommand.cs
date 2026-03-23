namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class CameraCommand : CueBoardCommand
    {
        public CameraCommand()
            : base("Camera On/Off", "Toggle Zoom video", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.CameraOn = !this.State.CameraOn;
            this.Keyboard.SendAltKey(KeyboardService.KEY_V);
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.CameraOn == true
                ? this.DrawIcon(imageSize, "camera-on.png")
                : this.DrawIcon(imageSize, "camera-off.png");
        }
    }
}
