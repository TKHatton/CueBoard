namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class ClearTimerCommand : CueBoardCommand
    {
        public ClearTimerCommand()
            : base("Clear Timer", "Reset timer to zero", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            this.CueBoard?.Timer?.Reset();
            this.CueBoard?.NotifyRefreshAllImages();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "timer-clear.png");
        }
    }
}
