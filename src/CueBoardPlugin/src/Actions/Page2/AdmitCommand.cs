namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    using Loupedeck.CueBoardPlugin.Services;

    public class AdmitCommand : CueBoardCommand
    {
        public AdmitCommand()
            : base("Admit", "Open waiting room / participants to admit", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+U opens the participants panel where the Admit button appears
            this.Keyboard?.SendAltKey(KeyboardService.KEY_U);
            PluginLog.Info("Opened participants panel for admitting");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "admit.png");
        }
    }
}
