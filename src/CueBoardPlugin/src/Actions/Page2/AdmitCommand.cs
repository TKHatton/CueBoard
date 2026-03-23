namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class AdmitCommand : CueBoardCommand
    {
        public AdmitCommand()
            : base("Admit", "Admit from waiting room (simulated)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            PluginLog.Info("[SIMULATED] Admitted participant from waiting room");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "admit.png");
        }
    }
}
