namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;

    public class MinMaxCommand : CueBoardCommand
    {
        private Boolean _isMinimized = false;

        public MinMaxCommand()
            : base("Min/Max Window", "Minimize or maximize the active window", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this._isMinimized)
            {
                this.Keyboard?.SendWinUp();
                PluginLog.Info("Window maximized");
            }
            else
            {
                this.Keyboard?.SendWinDown();
                PluginLog.Info("Window minimized");
            }

            this._isMinimized = !this._isMinimized;
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (this._isMinimized)
            {
                return this.DrawButton(imageSize, "MAX ↑", new BitmapColor(50, 160, 80));
            }

            return this.DrawButton(imageSize, "MIN ↓", new BitmapColor(50, 120, 180));
        }
    }
}
