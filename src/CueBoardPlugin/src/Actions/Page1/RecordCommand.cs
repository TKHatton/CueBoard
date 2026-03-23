namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class RecordCommand : CueBoardCommand
    {
        public RecordCommand()
            : base("Record", "Start/Stop Zoom recording", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.IsRecording = !this.State.IsRecording;
            this.Keyboard.SendAltKey(KeyboardService.KEY_R);
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.IsRecording == true
                ? this.DrawIcon(imageSize, "record-on.png")
                : this.DrawIcon(imageSize, "record-off.png");
        }
    }
}
