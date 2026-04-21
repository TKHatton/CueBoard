namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class CaptionsCommand : CueBoardCommand
    {
        public CaptionsCommand()
            : base("Captions", "Toggle Zoom closed captions (Ctrl+C)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            // Ctrl+C toggles captions in Zoom (confirmed shortcut)
            this.Keyboard?.SendCtrlKey(KeyboardService.KEY_C);

            this.State.CaptionsOn = !this.State.CaptionsOn;
            PluginLog.Info($"Captions toggled (Ctrl+C): {(this.State.CaptionsOn ? "ON" : "OFF")}");
            this.CueBoard?.Toast?.ShowToast("💬", this.State.CaptionsOn ? "Captions ON" : "Captions OFF", 2000);
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
