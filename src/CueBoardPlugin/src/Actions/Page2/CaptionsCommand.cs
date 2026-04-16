namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class CaptionsCommand : CueBoardCommand
    {
        public CaptionsCommand()
            : base("Captions", "Toggle Zoom closed captions (Alt+F2)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            // Send the actual Zoom shortcut so captions really toggle in the meeting
            this.Keyboard?.SendAltF2();

            this.State.CaptionsOn = !this.State.CaptionsOn;
            PluginLog.Info($"Captions toggled (Alt+F2): {(this.State.CaptionsOn ? "ON" : "OFF")}");
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
