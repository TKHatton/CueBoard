namespace Loupedeck.CueBoardPlugin.Actions
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public abstract class CueBoardAdjustment : PluginDynamicAdjustment
    {
        private CueBoardPlugin _cueBoard;
        private Boolean _subscribedToRefresh = false;
        private Boolean _subscribedToTimerTick = false;
        private Boolean _wantsTimerTick = false;

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
                        PluginLog.Info($"[CueBoardAdjustment] {this.GetType().Name} registered — subscribed to RefreshAllImages");
                    }
                    if (this._cueBoard != null && this._wantsTimerTick && !this._subscribedToTimerTick)
                    {
                        this._cueBoard.TimerDisplayChanged += () => this.AdjustmentValueChanged();
                        this._subscribedToTimerTick = true;
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
            PluginLog.Info($"[CueBoardAdjustment] Constructing: {displayName} (group: {groupName}, hasReset: {hasReset})");
        }

        /// <summary>
        /// Call in constructor of dials that need per-second timer updates.
        /// </summary>
        protected void EnableTimerTickUpdates()
        {
            this._wantsTimerTick = true;
        }
    }
}
