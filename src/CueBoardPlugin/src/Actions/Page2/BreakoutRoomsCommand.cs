namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class BreakoutRoomsCommand : CueBoardCommand
    {
        public BreakoutRoomsCommand()
            : base("Breakout Rooms", "Open Zoom breakout rooms panel", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.Keyboard == null)
            {
                return;
            }

            // Alt+B opens the Breakout Rooms panel in Zoom (host only)
            this.Keyboard.SendAltKey(KeyboardService.KEY_B);
            this.CueBoard?.Toast?.ShowToast("\uD83D\uDCE6", "Breakout Rooms opened", 2000);
            PluginLog.Info("Breakout Rooms panel opened (Alt+B)");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            // Breakout rooms uses the view-gallery icon (grid of 4 rooms)
            return this.DrawIcon(imageSize, "view-gallery.png");
        }
    }
}
