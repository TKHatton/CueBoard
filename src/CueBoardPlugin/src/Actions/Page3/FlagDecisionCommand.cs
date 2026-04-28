namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Linq;
    using Loupedeck.CueBoardPlugin.Models;

    public class FlagDecisionCommand : CueBoardCommand
    {
        public FlagDecisionCommand()
            : base("Flag Decision", "One-tap flag for a decision recorded", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            if (flags == null)
            {
                return;
            }

            flags.AddFlag(FlagType.Decision, DateTime.Now);
            this.CueBoard?.Toast?.FlagAdded("Decision");
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var count = this.CueBoard?.Flags?.GetFlags().Count(f => f.Type == FlagType.Decision) ?? 0;

            var builder = new BitmapBuilder(imageSize);
            builder.Clear(new BitmapColor(52, 152, 219));
            builder.DrawText(count > 0 ? $"DECISION\n({count})" : "DECISION", BitmapColor.White);
            return builder.ToImage();
        }
    }
}
