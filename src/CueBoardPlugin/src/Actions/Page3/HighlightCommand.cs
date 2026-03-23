namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class HighlightCommand : CueBoardCommand
    {
        public HighlightCommand()
            : base("Highlight", "Mark a light highlight (simulated)", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            PluginLog.Info("[SIMULATED] Highlight added");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "highlight.png");
        }
    }
}
