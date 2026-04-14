namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using Loupedeck.CueBoardPlugin.Models;
    using Loupedeck.CueBoardPlugin.Services;

    public class FlagTypeDial : CueBoardAdjustment
    {
        private static readonly FlagType[] FlagTypes =
        {
            FlagType.ActionItem, FlagType.Decision, FlagType.FollowUp, FlagType.Bookmark
        };

        private static readonly String[] FlagTypeNames =
        {
            "Action Item", "Decision", "Follow-Up", "Bookmark"
        };

        public FlagTypeDial()
            : base("Flag Type", "Rotate to select flag type or participant", "Meeting Intelligence", hasReset: false)
        {
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (this.State == null)
            {
                return;
            }

            var step = diff > 0 ? 1 : -1;

            if (this.State.IsAssignMode)
            {
                // Participants + 1 for the "+" add-new option
                var max = this.State.Participants.Count; // index == Count means "+"
                this.State.SelectedParticipantIndex = Math.Clamp(this.State.SelectedParticipantIndex + step, 0, max);

                // Toast so user sees the selection on screen
                var assignName = this.State.SelectedParticipantIndex >= this.State.Participants.Count
                    ? "+ Add New"
                    : this.State.Participants[this.State.SelectedParticipantIndex];
                this.CueBoard?.Toast?.ShowToast("\u2192", $"Assign: {assignName}", 1500);
            }
            else
            {
                var currentIdx = Array.IndexOf(FlagTypes, this.State.SelectedFlagType);
                var newIdx = Math.Clamp(currentIdx + step, 0, FlagTypes.Length - 1);
                this.State.SelectedFlagType = FlagTypes[newIdx];

                // Toast so user sees the flag type on screen
                this.CueBoard?.Toast?.ShowToast("\u25C6", $"Flag type: {FlagTypeNames[newIdx]}", 1500);
            }

            this.AdjustmentValueChanged();
            this.CueBoard?.NotifyRefreshAllImages();
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            if (this.State.IsAssignMode)
            {
                // If on "+" option, let AssignCommand handle the input dialog
                if (this.State.SelectedParticipantIndex >= this.State.Participants.Count)
                {
                    // Do nothing — user should press the Assign button to trigger the dialog
                    return;
                }

                var name = this.State.Participants[this.State.SelectedParticipantIndex];
                this.CueBoard?.Flags?.AssignLastFlag(name);
                this.CueBoard?.Toast?.Assigned(name);
                this.State.IsAssignMode = false;
                PluginLog.Info($"Dial confirmed assign: {name}");
                this.AdjustmentValueChanged();
                this.CueBoard?.NotifyRefreshAllImages();
                return;
            }

            // Default: dial press flags a moment
            var idx = Array.IndexOf(FlagTypes, this.State.SelectedFlagType);
            var typeName = idx >= 0 ? FlagTypeNames[idx] : "Flag";
            this.CueBoard?.Flags?.AddFlag(this.State.SelectedFlagType, DateTime.Now);
            this.CueBoard?.Toast?.FlagAdded(typeName);
            this.AdjustmentValueChanged();
            this.CueBoard?.NotifyRefreshAllImages();
            PluginLog.Info($"Dial flagged: {this.State.SelectedFlagType}");
        }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            if (this.State == null)
            {
                return "Action Item";
            }

            if (this.State.IsAssignMode)
            {
                return this.State.SelectedParticipantIndex >= this.State.Participants.Count
                    ? "+ Add New"
                    : this.State.Participants[this.State.SelectedParticipantIndex];
            }

            var idx = Array.IndexOf(FlagTypes, this.State.SelectedFlagType);
            return FlagTypeNames[idx >= 0 ? idx : 0];
        }
    }
}
