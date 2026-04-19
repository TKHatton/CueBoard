namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class LockMeetingCommand : CueBoardCommand
    {
        public LockMeetingCommand()
            : base("Lock Meeting", "Lock or unlock the meeting (Alt+L)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            // Alt+L toggles meeting lock in Zoom
            this.Keyboard?.SendAltKey(KeyboardService.KEY_L);

            this.State.MeetingLocked = !this.State.MeetingLocked;
            PluginLog.Info($"Meeting {(this.State.MeetingLocked ? "LOCKED" : "UNLOCKED")} (Alt+L sent)");
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.MeetingLocked == true
                ? this.DrawIcon(imageSize, "lock-on.png")
                : this.DrawIcon(imageSize, "lock-off.png");
        }
    }
}
