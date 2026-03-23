namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class AddNoteCommand : CueBoardCommand
    {
        public AddNoteCommand()
            : base("Add Note", "Add a note to last flag (simulated)", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            PluginLog.Info("[SIMULATED] Note added to last flag");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "note.png");
        }
    }
}
