namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class ChatCommand : CueBoardCommand
    {
        public ChatCommand()
            : base("Open Chat", "Open Zoom chat panel", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            this.Keyboard?.SendAltKey(KeyboardService.KEY_H);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "chat.png");
        }
    }
}
