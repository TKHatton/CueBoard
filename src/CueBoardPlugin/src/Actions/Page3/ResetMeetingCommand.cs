namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class ResetMeetingCommand : CueBoardCommand
    {
        private Boolean _confirmPending = false;
        private DateTime _confirmStarted;

        public ResetMeetingCommand()
            : base("Reset Meeting", "Clear all flags and start fresh", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            if (this._confirmPending && (DateTime.Now - this._confirmStarted).TotalSeconds < 3)
            {
                // Confirmed — reset everything
                this.CueBoard?.Flags?.Clear();
                this.State.IsAssignMode = false;
                this.State.SelectedParticipantIndex = 0;
                this.State.MeetingStartTime = DateTime.Now;
                this._confirmPending = false;
                this.CueBoard?.Toast?.MeetingReset();
                this.CueBoard?.NotifyRefreshAllImages();
                PluginLog.Info("Meeting reset — all flags cleared, timer restarted");
            }
            else
            {
                // First press — enter confirm mode
                this._confirmPending = true;
                this._confirmStarted = DateTime.Now;
                this.ActionImageChanged();

                // Auto-cancel after 3 seconds
                System.Threading.Tasks.Task.Delay(3200).ContinueWith(_ =>
                {
                    if (this._confirmPending)
                    {
                        this._confirmPending = false;
                        this.ActionImageChanged();
                    }
                });
            }
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var builder = new BitmapBuilder(imageSize);

            if (this._confirmPending)
            {
                builder.Clear(new BitmapColor(231, 76, 60));
                builder.DrawText("TAP TO\nRESET", BitmapColor.White);
            }
            else
            {
                builder.Clear(new BitmapColor(42, 42, 53));
                builder.DrawText("RESET\nMTG", new BitmapColor(180, 180, 180));
            }

            return builder.ToImage();
        }
    }
}
