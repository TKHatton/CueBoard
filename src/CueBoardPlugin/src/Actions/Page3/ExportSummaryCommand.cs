namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

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

            // Auto-detect a transcript file. If none exists, leave it empty — the export
            // HTML shows an honest banner instead of inventing fake speakers.
            var transcript = this.CueBoard?.Transcript;
            if (transcript != null && !transcript.HasTranscript)
            {
                var vttPath = TranscriptService.AutoDetectVttFile();
                if (vttPath != null)
                {
                    transcript.LoadVtt(vttPath);
                    PluginLog.Info($"Auto-detected transcript: {vttPath}");
                }
                else
                {
                    PluginLog.Info("No transcript found — export will show a load-transcript banner");
                }
            }

            var path = export.ExportToFile(flags.GetFlags(), state.MeetingStartTime, transcript);
            this.CueBoard?.Toast?.Exported(path);
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
