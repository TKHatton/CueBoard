namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class RaiseHandCommand : CueBoardCommand
    {
        public RaiseHandCommand()
            : base("Raise Hand", "Raise/lower hand", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.HandRaised = !this.State.HandRaised;
            this.Keyboard.SendAltKey(KeyboardService.KEY_Y);
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.HandRaised == true
                ? this.DrawIcon(imageSize, "hand-up.png")
                : this.DrawIcon(imageSize, "hand-down.png");
        }
    }
}
