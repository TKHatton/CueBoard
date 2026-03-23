namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class AssignCommand : CueBoardCommand
    {
        public AssignCommand()
            : base("Assign", "Assign last flag to a person (simulated)", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            PluginLog.Info("[SIMULATED] Assign flag to participant");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "assign.png");
        }
    }
}
