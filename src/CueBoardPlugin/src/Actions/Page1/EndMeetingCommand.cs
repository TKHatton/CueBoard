namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class EndMeetingCommand : CueBoardCommand
    {
        public EndMeetingCommand()
            : base("End Meeting", "End meeting for all", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            this.Keyboard?.SendAltKey(KeyboardService.KEY_Q);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "end-meeting.png");
        }
    }
}
