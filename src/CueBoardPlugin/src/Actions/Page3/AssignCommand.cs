namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class AssignCommand : CueBoardCommand
    {
        private Boolean _addingNew = false;

        public AssignCommand()
            : base("Assign", "Assign last flag to a person via dial (+ to add new)", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            if (flags == null || this.State == null || flags.FlagCount == 0 || this._addingNew)
            {
                return;
            }

            if (!this.State.IsAssignMode)
            {
                // Enter assign mode
                this.State.IsAssignMode = true;
                this.State.SelectedParticipantIndex = 0;
                PluginLog.Info("Entered assign mode — use dial to select participant");
            }
            else
            {
                // Check if user is on the "+" option (past the end of the list)
                if (this.State.SelectedParticipantIndex >= this.State.Participants.Count)
                {
                    // Spawn input dialog to add a new participant
                    var dialog = this.CueBoard?.InputDialog;
                    if (dialog == null)
                    {
                        return;
                    }

                    this._addingNew = true;
                    this.ActionImageChanged();

                    dialog.ShowInputDialogAsync("Add Participant", "Name or team...").ContinueWith(task =>
                    {
                        var name = task.Result;
                        if (!String.IsNullOrEmpty(name))
                        {
                            this.State.AddParticipant(name);
                            flags.AssignLastFlag(name);
                            this.CueBoard?.Toast?.Assigned(name);
                            PluginLog.Info($"New participant added and assigned: {name}");
                        }

                        this._addingNew = false;
                        this.State.IsAssignMode = false;
                        this.ActionImageChanged();
                        this.CueBoard?.NotifyRefreshAllImages();
                    });
                }
                else
                {
                    // Confirm assignment from existing list
                    var name = this.State.Participants[this.State.SelectedParticipantIndex];
                    flags.AssignLastFlag(name);
                    this.CueBoard?.Toast?.Assigned(name);
                    this.State.IsAssignMode = false;
                    PluginLog.Info($"Assigned to: {name}");
                }
            }

            this.CueBoard?.NotifyRefreshAllImages();
        }

        /// <summary>
        /// Gets the display name for the current dial position.
        /// Returns participant name or "+" for the add-new option.
        /// </summary>
        public String GetCurrentSelectionName()
        {
            if (this.State == null)
            {
                return "+";
            }

            return this.State.SelectedParticipantIndex >= this.State.Participants.Count
                ? "+"
                : this.State.Participants[this.State.SelectedParticipantIndex];
        }

        /// <summary>
        /// Total number of dial positions (participants + the "+" option).
        /// </summary>
        public Int32 TotalDialPositions => (this.State?.Participants.Count ?? 0) + 1;

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (this._addingNew)
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(new BitmapColor(139, 92, 246));
                builder.DrawText("ASSIGN\nTYPING...");
                return builder.ToImage();
            }

            if (this.State != null && this.State.IsAssignMode)
            {
                var name = this.GetCurrentSelectionName();
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(new BitmapColor(139, 92, 246));
                builder.DrawText($"ASSIGN\n{name}\n[DIAL]");
                return builder.ToImage();
            }

            var lastFlag = this.CueBoard?.Flags?.GetLastFlag();
            if (lastFlag != null && !String.IsNullOrEmpty(lastFlag.AssignedTo))
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(new BitmapColor(50, 40, 70));
                builder.DrawText($"ASSIGN\n-> {lastFlag.AssignedTo}");
                return builder.ToImage();
            }

            return this.DrawIcon(imageSize, "assign.png");
        }
    }
}
