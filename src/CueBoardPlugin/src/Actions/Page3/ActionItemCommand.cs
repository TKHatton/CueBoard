namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using Loupedeck.CueBoardPlugin.Models;

    /// <summary>
    /// One-tap Action Item: creates an ActionItem flag at the current moment AND
    /// pops a dialog so the user can type the action in the same gesture.
    /// Distinct from FlagMoment (which uses whatever flag type is selected on the dial)
    /// and AddNote (which only attaches text to the most recent existing flag).
    /// </summary>
    public class ActionItemCommand : CueBoardCommand
    {
        private Boolean _capturing = false;

        public ActionItemCommand()
            : base("Action Item", "Capture an action item with a typed description", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            var dialog = this.CueBoard?.InputDialog;
            if (flags == null || dialog == null || this._capturing)
            {
                return;
            }

            // Stamp the moment first — if the user cancels the dialog they still get the flag.
            flags.AddFlag(FlagType.ActionItem, DateTime.Now);
            this.CueBoard?.Toast?.FlagAdded("Action");

            this._capturing = true;
            this.ActionImageChanged();

            dialog.ShowInputDialogAsync("Action Item", "What's the action?").ContinueWith(task =>
            {
                var text = task.Result;
                if (!String.IsNullOrEmpty(text))
                {
                    flags.NoteLastFlag(text);
                    this.CueBoard?.Toast?.NoteAdded(text);
                    PluginLog.Info($"Action item captured: {text}");
                }

                this._capturing = false;
                this.ActionImageChanged();
                this.CueBoard?.NotifyRefreshAllImages();
            });
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (this._capturing)
            {
                return this.DrawButton(imageSize, "ACTION\nTYPING...", new BitmapColor(139, 92, 246));
            }

            return this.DrawButton(imageSize, "ACTION\nITEM", new BitmapColor(139, 92, 246));
        }
    }
}
