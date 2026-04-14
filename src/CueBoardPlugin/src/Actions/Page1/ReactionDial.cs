namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class ReactionDial : CueBoardAdjustment
    {
        private static readonly String[] ReactionNames = { "Clap", "Thumbs Up", "Heart", "Laugh", "Tada" };
        private static readonly UInt16[] ReactionKeys =
        {
            KeyboardService.KEY_4, KeyboardService.KEY_5, KeyboardService.KEY_6,
            KeyboardService.KEY_7, KeyboardService.KEY_9
        };

        public ReactionDial()
            : base("Reaction Selector", "Rotate to select reaction, press to send", "Live Controls", hasReset: false)
        {
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            if (this.State == null)
            {
                return;
            }

            var newIndex = this.State.SelectedReactionIndex + (diff > 0 ? 1 : -1);
            this.State.SelectedReactionIndex = Math.Clamp(newIndex, 0, ReactionNames.Length - 1);
            this.CueBoard?.Toast?.ShowToast("\uD83D\uDE00", $"Reaction: {ReactionNames[this.State.SelectedReactionIndex]}", 1500);
            this.AdjustmentValueChanged();

            // Also update the reaction button image
            this.CueBoard?.NotifyRefreshAllImages();
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null || this.Keyboard == null)
            {
                return;
            }

            var idx = this.State.SelectedReactionIndex;
            this.Keyboard.SendAltShiftKey(ReactionKeys[idx]);
            PluginLog.Info($"Dial sent reaction: {ReactionNames[idx]}");
        }

        protected override String GetAdjustmentValue(String actionParameter)
        {
            var idx = this.State?.SelectedReactionIndex ?? 0;
            return ReactionNames[idx];
        }
    }
}
