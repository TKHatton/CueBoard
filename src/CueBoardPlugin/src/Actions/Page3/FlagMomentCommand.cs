namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class FlagMomentCommand : CueBoardCommand
    {
        public FlagMomentCommand()
            : base("Flag Moment", "Flag a key moment with current type", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            if (flags == null || this.State == null)
            {
                return;
            }

            flags.AddFlag(this.State.SelectedFlagType, DateTime.Now);
            this.CueBoard?.Toast?.FlagAdded(this.GetShortTypeName());
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var count = this.CueBoard?.Flags?.FlagCount ?? 0;

            if (count > 0)
            {
                // Show count when flags exist
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(new BitmapColor(139, 92, 246));
                builder.DrawText($"FLAG\n({count})", BitmapColor.White);
                return builder.ToImage();
            }

            return this.DrawIcon(imageSize, "flag.png");
        }

        private String GetShortTypeName()
        {
            if (this.State == null)
            {
                return "";
            }

            switch (this.State.SelectedFlagType)
            {
                case Models.FlagType.ActionItem: return "Action";
                case Models.FlagType.Decision: return "Decision";
                case Models.FlagType.FollowUp: return "Follow-Up";
                case Models.FlagType.Bookmark: return "Bookmark";
                default: return "";
            }
        }
    }
}
