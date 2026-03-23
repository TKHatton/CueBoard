namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class MuteAllCommand : CueBoardCommand
    {
        public MuteAllCommand()
            : base("Mute All", "Mute all participants (host)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            this.Keyboard?.SendAltKey(KeyboardService.KEY_M);
            PluginLog.Info("Mute All sent");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "mute-all.png");
        }
    }
}
