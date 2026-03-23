namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class LockMeetingCommand : CueBoardCommand
    {
        public LockMeetingCommand()
            : base("Lock Meeting", "Lock/unlock meeting (simulated)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.MeetingLocked = !this.State.MeetingLocked;
            PluginLog.Info($"[SIMULATED] Meeting {(this.State.MeetingLocked ? "LOCKED" : "UNLOCKED")}");
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
