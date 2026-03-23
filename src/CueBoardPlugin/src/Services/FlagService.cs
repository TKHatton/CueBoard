namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Collections.Generic;
    using Loupedeck.CueBoardPlugin.Models;

    public class FlagService
    {
        private readonly List<MeetingFlag> _flags = new List<MeetingFlag>();

        public Int32 FlagCount => this._flags.Count;

        public void AddFlag(FlagType type, DateTime timestamp)
        {
            this._flags.Add(new MeetingFlag(type, timestamp));
            PluginLog.Info($"Flag added: {type} at {timestamp:HH:mm:ss} (total: {this.FlagCount})");
        }

        public MeetingFlag RemoveLastFlag()
        {
            if (this._flags.Count == 0)
            {
                return null;
            }

            var last = this._flags[this._flags.Count - 1];
            this._flags.RemoveAt(this._flags.Count - 1);
            PluginLog.Info($"Flag removed: {last.Type} (remaining: {this.FlagCount})");
            return last;
        }

        public IReadOnlyList<MeetingFlag> GetFlags() => this._flags.AsReadOnly();

        public void Clear()
        {
            this._flags.Clear();
            PluginLog.Info("All flags cleared");
        }
    }
}
