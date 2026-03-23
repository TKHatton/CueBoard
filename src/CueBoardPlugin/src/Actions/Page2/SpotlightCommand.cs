namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class SpotlightCommand : CueBoardCommand
    {
        public SpotlightCommand()
            : base("Spotlight", "Spotlight current speaker (simulated)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.SpotlightOn = !this.State.SpotlightOn;
            PluginLog.Info($"[SIMULATED] Spotlight: {(this.State.SpotlightOn ? "ON" : "OFF")}");
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.SpotlightOn == true
                ? this.DrawIcon(imageSize, "spotlight-on.png")
                : this.DrawIcon(imageSize, "spotlight-off.png");
        }
    }
}
