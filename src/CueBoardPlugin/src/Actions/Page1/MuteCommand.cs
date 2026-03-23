namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class MuteCommand : CueBoardCommand
    {
        public MuteCommand()
            : base("Mute / Unmute", "Toggle Zoom microphone", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.IsMuted = !this.State.IsMuted;
            this.Keyboard.SendAltKey(KeyboardService.KEY_A);
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (this.State == null)
            {
                return this.DrawButton(imageSize, "MUTE", new BitmapColor(80, 80, 80));
            }

            return this.State.IsMuted
                ? this.DrawIcon(imageSize, "mute-on.png")
                : this.DrawIcon(imageSize, "mute-off.png");
        }
    }
}
