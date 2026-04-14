namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class ParticipantsCommand : CueBoardCommand
    {
        public ParticipantsCommand()
            : base("Participants", "Open participant panel", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Alt+U opens/closes the participants panel in Zoom
            this.Keyboard?.SendAltKey(KeyboardService.KEY_U);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "participants.png");
        }
    }
}
