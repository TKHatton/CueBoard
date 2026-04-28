namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class MinMaxCommand : CueBoardCommand
    {
        public MinMaxCommand()
            : base("Minimize", "Minimize the active window — Zoom shows its floating thumbnail", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Win+Down minimizes the active window. When Zoom is in front, this triggers
            // its floating thumbnail mode — which is the demo behavior Tyler wants.
            this.Keyboard?.SendWinDown();
            this.CueBoard?.Toast?.ShowToast("🪟", "Minimized", 1500);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawButton(imageSize, "MINIMIZE", new BitmapColor(42, 42, 53));
        }
    }
}
