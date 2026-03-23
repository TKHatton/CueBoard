namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using Loupedeck.CueBoardPlugin.Models;

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
            : base("Flag Type", "Rotate to select flag type", "Meeting Intelligence", hasReset: false)
        {
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (this.State == null)
            {
                return;
            }

            var currentIdx = Array.IndexOf(FlagTypes, this.State.SelectedFlagType);
            var newIdx = Math.Clamp(currentIdx + (diff > 0 ? 1 : -1), 0, FlagTypes.Length - 1);
            this.State.SelectedFlagType = FlagTypes[newIdx];

            this.AdjustmentValueChanged();
            this.CueBoard?.NotifyRefreshAllImages();
        }

        protected override void RunCommand(String actionParameter)
        {
            // Dial press on Page 3 could also flag a moment
            if (this.State == null)
            {
                return;
            }

            this.CueBoard?.Flags?.AddFlag(this.State.SelectedFlagType, DateTime.Now);
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

            var idx = Array.IndexOf(FlagTypes, this.State.SelectedFlagType);
            return FlagTypeNames[idx >= 0 ? idx : 0];
        }
    }
}
