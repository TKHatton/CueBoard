namespace Loupedeck.CueBoardPlugin.Actions
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public abstract class CueBoardAdjustment : PluginDynamicAdjustment
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
                        this._cueBoard.RefreshAllImages += () => this.AdjustmentValueChanged();
                        this._subscribedToRefresh = true;
                    }
                }
                return this._cueBoard;
            }
        }

        protected KeyboardService Keyboard => this.CueBoard?.Keyboard;
        protected SessionState State => this.CueBoard?.State;

        protected CueBoardAdjustment(String displayName, String description, String groupName, Boolean hasReset)
            : base(displayName, description, groupName, hasReset)
        {
        }
    }
}
