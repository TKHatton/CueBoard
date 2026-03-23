namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Linq;
    using Loupedeck.CueBoardPlugin.Models;

    public class PreviewSummaryCommand : CueBoardCommand
    {
        public PreviewSummaryCommand()
            : base("Preview", "Show flag breakdown", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            // Nothing to do — just shows the breakdown
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var flags = this.CueBoard?.Flags;
            if (flags == null || flags.FlagCount == 0)
            {
                return this.DrawButton(imageSize, "PREVIEW\n(0)", new BitmapColor(80, 80, 80));
            }

            var allFlags = flags.GetFlags();
            var actions = allFlags.Count(f => f.Type == FlagType.ActionItem);
            var decisions = allFlags.Count(f => f.Type == FlagType.Decision);
            var followUps = allFlags.Count(f => f.Type == FlagType.FollowUp);
            var bookmarks = allFlags.Count(f => f.Type == FlagType.Bookmark);

            var text = $"A:{actions} D:{decisions}\nF:{followUps} B:{bookmarks}";

            return this.DrawButton(imageSize, text, new BitmapColor(100, 50, 150));
        }
    }
}
