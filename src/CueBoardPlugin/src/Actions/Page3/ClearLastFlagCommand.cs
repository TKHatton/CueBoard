namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;

    public class ClearLastFlagCommand : CueBoardCommand
    {
        public ClearLastFlagCommand()
            : base("Clear Last", "Undo last flag", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var removed = this.CueBoard?.Flags?.RemoveLastFlag();
            if (removed != null)
            {
                this.CueBoard?.Toast?.FlagCleared(removed.Type.ToString());
                PluginLog.Info($"Removed flag: {removed.Type}");
            }

            this.CueBoard?.NotifyRefreshAllImages();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "clear-last.png");
        }
    }
}
