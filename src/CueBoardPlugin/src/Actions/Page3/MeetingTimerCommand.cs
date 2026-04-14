namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class MeetingTimerCommand : CueBoardCommand
    {
        private System.Timers.Timer _displayTimer;

        public MeetingTimerCommand()
            : base("Meeting Timer", "Shows elapsed meeting time", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Informational button — press refreshes display
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            // Always show the clock icon
            return this.DrawIcon(imageSize, "timer.png");
        }
    }
}
