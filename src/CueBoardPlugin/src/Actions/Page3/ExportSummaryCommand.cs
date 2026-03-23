namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class ExportSummaryCommand : CueBoardCommand
    {
        private Boolean _justExported = false;

        public ExportSummaryCommand()
            : base("Export Summary", "Export meeting summary as Markdown", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            var export = this.CueBoard?.Export;
            var state = this.State;
            if (flags == null || export == null || state == null)
            {
                return;
            }

            var path = export.ExportToFile(flags.GetFlags(), state.MeetingStartTime);
            PluginLog.Info($"Exported to: {path}");

            this._justExported = true;
            this.ActionImageChanged();

            // Reset the "SAVED!" indicator after 2 seconds
            System.Threading.Tasks.Task.Delay(2000).ContinueWith(_ =>
            {
                this._justExported = false;
                this.ActionImageChanged();
            });
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this._justExported
                ? this.DrawIcon(imageSize, "export-done.png")
                : this.DrawIcon(imageSize, "export.png");
        }
    }
}
