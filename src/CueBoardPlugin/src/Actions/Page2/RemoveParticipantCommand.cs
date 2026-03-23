namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class RemoveParticipantCommand : CueBoardCommand
    {
        private Boolean _confirmPending = false;

        public RemoveParticipantCommand()
            : base("Remove", "Remove participant (simulated, 2-step)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (!this._confirmPending)
            {
                this._confirmPending = true;
                PluginLog.Info("[SIMULATED] Remove participant - press again to confirm");
            }
            else
            {
                this._confirmPending = false;
                PluginLog.Info("[SIMULATED] Participant removed!");
            }

            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this._confirmPending
                ? this.DrawButton(imageSize, "CONFIRM\nREMOVE?", new BitmapColor(220, 50, 50))
                : this.DrawIcon(imageSize, "remove.png");
        }
    }
}
