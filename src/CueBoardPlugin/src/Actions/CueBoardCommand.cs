namespace Loupedeck.CueBoardPlugin.Actions
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public abstract class CueBoardCommand : PluginDynamicCommand
    {
        private CueBoardPlugin _cueBoard;
        private Boolean _subscribedToRefresh = false;

        protected CueBoardPlugin CueBoard
        {
            get
            {
                if (this._cueBoard == null)
                {
                    this._cueBoard = this.Plugin as CueBoardPlugin;
                    if (this._cueBoard != null && !this._subscribedToRefresh)
                    {
                        this._cueBoard.RefreshAllImages += () => this.ActionImageChanged();
                        this._subscribedToRefresh = true;
                    }
                }
                return this._cueBoard;
            }
        }

        protected KeyboardService Keyboard => this.CueBoard?.Keyboard;
        protected SessionState State => this.CueBoard?.State;

        protected CueBoardCommand(String displayName, String description, String groupName)
            : base(displayName, description, groupName)
        {
        }

        protected BitmapImage DrawButton(PluginImageSize imageSize, String text, BitmapColor bgColor)
        {
            return this.DrawButton(imageSize, text, bgColor, BitmapColor.White);
        }

        protected BitmapImage DrawButton(PluginImageSize imageSize, String text, BitmapColor bgColor, BitmapColor textColor)
        {
            var builder = new BitmapBuilder(imageSize);
            builder.Clear(bgColor);
            builder.DrawText(text, textColor);
            return builder.ToImage();
        }

        protected BitmapImage DrawIcon(PluginImageSize imageSize, String iconFileName)
        {
            try
            {
                return PluginResources.ReadImage(iconFileName);
            }
            catch
            {
                // Fallback to text if icon not found
                return this.DrawButton(imageSize, iconFileName, new BitmapColor(80, 80, 80));
            }
        }
    }
}
