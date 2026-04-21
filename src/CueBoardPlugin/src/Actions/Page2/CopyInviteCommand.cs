namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class CopyInviteCommand : CueBoardCommand
    {
        public CopyInviteCommand()
            : base("Copy Invite", "Copy meeting invite link to clipboard (Alt+I)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+I copies the meeting invite link to clipboard
            this.Keyboard?.SendAltKey(KeyboardService.KEY_I);
            this.CueBoard?.Toast?.ShowToast("🔗", "Invite link copied", 2000);
            PluginLog.Info("Copied meeting invite link (Alt+I)");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var builder = new BitmapBuilder(imageSize);
            builder.Clear(new BitmapColor(42, 42, 53));
            builder.DrawText("🔗", new BitmapColor(139, 92, 246), 48);
            return builder.ToImage();
        }
    }
}
