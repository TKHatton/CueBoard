namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class ReactionCommand : CueBoardCommand
    {
        private static readonly String[] ReactionNames = { "Clap", "Thumbs Up", "Heart", "Laugh", "Tada" };
        private static readonly UInt16[] ReactionKeys =
        {
            KeyboardService.KEY_4, KeyboardService.KEY_5, KeyboardService.KEY_6,
            KeyboardService.KEY_7, KeyboardService.KEY_9
        };

        public ReactionCommand()
            : base("Reaction", "Send selected reaction", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null || this.Keyboard == null)
            {
                return;
            }

            var idx = this.State.SelectedReactionIndex;
            this.Keyboard.SendAltShiftKey(ReactionKeys[idx]);
            PluginLog.Info($"Sent reaction: {ReactionNames[idx]}");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "reaction.png");
        }
    }
}
