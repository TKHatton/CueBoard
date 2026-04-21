namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class RenameCommand : CueBoardCommand
    {
        public RenameCommand()
            : base("Rename", "Rename yourself in the meeting (Alt+N)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+N opens the rename dialog in Zoom
            this.Keyboard?.SendAltKey(KeyboardService.KEY_N);
            this.CueBoard?.Toast?.ShowToast("✏️", "Rename dialog opened", 2000);
            PluginLog.Info("Opened rename dialog (Alt+N)");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var builder = new BitmapBuilder(imageSize);
            builder.Clear(new BitmapColor(42, 42, 53));
            builder.DrawText("✏️", new BitmapColor(139, 92, 246), 48);
            return builder.ToImage();
        }
    }
}
