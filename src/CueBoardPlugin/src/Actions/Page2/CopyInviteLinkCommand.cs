namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class CopyInviteLinkCommand : CueBoardCommand
    {
        public CopyInviteLinkCommand()
            : base("Copy Invite", "Copy meeting invite link to clipboard (Alt+I)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+I copies the meeting invite link to clipboard in Zoom
            this.Keyboard?.SendAltKey(KeyboardService.KEY_I);
            this.CueBoard?.Toast?.ShowToast("\uD83D\uDCCB", "Invite link copied!", 2000);
            PluginLog.Info("Copied invite link to clipboard (Alt+I)");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            // Match the dark theme with purple accent
            var builder = new BitmapBuilder(imageSize);
            builder.Clear(new BitmapColor(42, 42, 53)); // Dark background matching icons
            builder.DrawText("🔗", new BitmapColor(139, 92, 246), 28); // Purple link emoji
            return builder.ToImage();
        }
    }
}
