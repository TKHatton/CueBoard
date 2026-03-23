namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class ShareCommand : CueBoardCommand
    {
        public ShareCommand()
            : base("Screen Share", "Start/Stop screen share", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.IsSharing = !this.State.IsSharing;
            this.Keyboard.SendAltKey(KeyboardService.KEY_S);
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.IsSharing == true
                ? this.DrawIcon(imageSize, "share-on.png")
                : this.DrawIcon(imageSize, "share-off.png");
        }
    }
}
