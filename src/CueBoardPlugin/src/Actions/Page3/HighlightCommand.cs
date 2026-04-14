namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using Loupedeck.CueBoardPlugin.Models;

    public class HighlightCommand : CueBoardCommand
    {
        public HighlightCommand()
            : base("Highlight", "Mark a key moment as a highlight", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            if (flags == null)
            {
                return;
            }

            flags.AddFlag(FlagType.Highlight, DateTime.Now);
            this.CueBoard?.Toast?.HighlightAdded(flags.HighlightCount);
            this.CueBoard?.NotifyRefreshAllImages();
            PluginLog.Info($"Highlight added (total: {flags.HighlightCount})");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var count = this.CueBoard?.Flags?.HighlightCount ?? 0;

            if (count > 0)
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(new BitmapColor(245, 158, 11));
                builder.DrawText($"MARK\n({count})", BitmapColor.Black);
                return builder.ToImage();
            }

            return this.DrawIcon(imageSize, "highlight.png");
        }
    }
}
