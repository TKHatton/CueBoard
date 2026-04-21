namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class AdmitCommand : CueBoardCommand
    {
        public AdmitCommand()
            : base("Admit", "Open participants panel to admit waiting guests (Alt+U)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+U opens the participants panel where the Admit button appears for waiting room
            this.Keyboard?.SendAltKey(KeyboardService.KEY_U);
            this.CueBoard?.Toast?.ShowToast("👥", "Opened panel - click Admit", 2500);
            PluginLog.Info("Opened participants panel for admitting (Alt+U)");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "admit.png");
        }
    }
}
