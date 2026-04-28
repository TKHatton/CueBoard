namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Linq;
    using Loupedeck.CueBoardPlugin.Models;

    public class FlagFollowUpCommand : CueBoardCommand
    {
        public FlagFollowUpCommand()
            : base("Flag Follow-Up", "One-tap flag for a follow-up needed", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            if (flags == null)
            {
                return;
            }

            flags.AddFlag(FlagType.FollowUp, DateTime.Now);
            this.CueBoard?.Toast?.FlagAdded("Follow-Up");
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var count = this.CueBoard?.Flags?.GetFlags().Count(f => f.Type == FlagType.FollowUp) ?? 0;

            var builder = new BitmapBuilder(imageSize);
            builder.Clear(new BitmapColor(46, 204, 113));
            builder.DrawText(count > 0 ? $"FOLLOW\n({count})" : "FOLLOW\nUP", BitmapColor.White);
            return builder.ToImage();
        }
    }
}
