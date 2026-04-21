namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class MinMaxCommand : CueBoardCommand
    {
        public MinMaxCommand()
            : base("Min/Max Window", "Minimize or maximize Zoom window", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Win+Down minimizes the active window
            this.Keyboard?.SendWinDown();
            PluginLog.Info("Window minimized (Win+Down)");
            this.CueBoard?.Toast?.ShowToast("🪟", "Window minimized", 2000);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "minmax.png");
        }
    }
}
