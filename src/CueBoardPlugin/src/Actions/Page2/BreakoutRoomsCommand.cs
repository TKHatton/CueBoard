namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class BreakoutRoomsCommand : CueBoardCommand
    {
        public BreakoutRoomsCommand()
            : base("Breakout Rooms", "Open participants panel to access breakout rooms", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+U opens the participants panel where Breakout Rooms button appears
            // Zoom has no direct keyboard shortcut for breakout rooms
            this.Keyboard?.SendAltKey(KeyboardService.KEY_U);
            this.CueBoard?.Toast?.ShowToast("\uD83D\uDCE6", "Click 'Breakout Rooms' in the toolbar", 3000);
            PluginLog.Info("Opened participants panel for breakout rooms (Alt+U)");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            // Using view-gallery icon (grid of rooms concept)
            return this.DrawIcon(imageSize, "view-gallery.png");
        }
    }
}
