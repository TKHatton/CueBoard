namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class CaptionsCommand : CueBoardCommand
    {
        public CaptionsCommand()
            : base("Captions", "Toggle captions (simulated)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.CaptionsOn = !this.State.CaptionsOn;
            PluginLog.Info($"[SIMULATED] Captions: {(this.State.CaptionsOn ? "ON" : "OFF")}");
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.CaptionsOn == true
                ? this.DrawIcon(imageSize, "captions-on.png")
                : this.DrawIcon(imageSize, "captions-off.png");
        }
    }
}
