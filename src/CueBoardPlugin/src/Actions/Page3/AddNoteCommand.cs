namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class AddNoteCommand : CueBoardCommand
    {
        private Boolean _noteAdding = false;

        public AddNoteCommand()
            : base("Add Note", "Type a private note for the last flag", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            var dialog = this.CueBoard?.InputDialog;
            if (flags == null || dialog == null || flags.FlagCount == 0)
            {
                return;
            }

            if (this._noteAdding)
            {
                return; // Already showing a dialog
            }

            this._noteAdding = true;
            this.ActionImageChanged();

            dialog.ShowInputDialogAsync("Add Note", "Type your note...").ContinueWith(task =>
            {
                var text = task.Result;
                if (!String.IsNullOrEmpty(text))
                {
                    flags.NoteLastFlag(text);
                    this.CueBoard?.Toast?.NoteAdded(text);
                    PluginLog.Info($"Note added: {text}");
                }

                this._noteAdding = false;
                this.ActionImageChanged();
                this.CueBoard?.NotifyRefreshAllImages();
            });
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (this._noteAdding)
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(new BitmapColor(13, 148, 136));
                builder.DrawText("NOTE\nTYPING...");
                return builder.ToImage();
            }

            var lastFlag = this.CueBoard?.Flags?.GetLastFlag();
            if (lastFlag != null && !String.IsNullOrEmpty(lastFlag.Note))
            {
                var builder = new BitmapBuilder(imageSize);
                builder.Clear(new BitmapColor(30, 60, 55));
                builder.DrawText("NOTE\nSet");
                return builder.ToImage();
            }

            return this.DrawIcon(imageSize, "note.png");
        }
    }
}
