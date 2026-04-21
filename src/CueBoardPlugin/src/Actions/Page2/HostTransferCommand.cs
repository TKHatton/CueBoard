namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class HostTransferCommand : CueBoardCommand
    {
        public HostTransferCommand()
            : base("Host Transfer", "Transfer host role (simulated)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            PluginLog.Info("[SIMULATED] Host role transferred");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "host-transfer.png");
        }
    }
}
