namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Loupedeck.CueBoardPlugin.Models;

    public class FlagService
    {
        private readonly List<MeetingFlag> _flags = new List<MeetingFlag>();

        public Int32 FlagCount => this._flags.Count;

        public Int32 HighlightCount => this._flags.Count(f => f.Type == FlagType.Highlight);

        public void AddFlag(FlagType type, DateTime timestamp)
        {
            this._flags.Add(new MeetingFlag(type, timestamp));
            PluginLog.Info($"Flag added: {type} at {timestamp:HH:mm:ss} (total: {this.FlagCount})");
        }

        public MeetingFlag GetLastFlag()
        {
            return this._flags.Count > 0 ? this._flags[this._flags.Count - 1] : null;
        }

        public void AssignLastFlag(String assignee)
        {
            var last = this.GetLastFlag();
            if (last != null)
            {
                last.AssignedTo = assignee;
                PluginLog.Info($"Flag assigned to: {assignee}");
            }
        }

        public void NoteLastFlag(String note)
        {
            var last = this.GetLastFlag();
            if (last != null)
            {
                last.Note = note;
                PluginLog.Info($"Note added to flag: {note}");
            }
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

        public void LoadDemoData(DateTime meetingStart)
        {
            this._flags.Clear();

            var f1 = new MeetingFlag(FlagType.ActionItem, meetingStart.AddMinutes(3).AddSeconds(22))
                { AssignedTo = "Sarah", Note = "Send Q3 report by Friday" };
            var f2 = new MeetingFlag(FlagType.ActionItem, meetingStart.AddMinutes(8).AddSeconds(45))
                { AssignedTo = "Dev Team", Note = "Review API migration plan" };
            var f3 = new MeetingFlag(FlagType.Decision, meetingStart.AddMinutes(12).AddSeconds(10))
                { Note = "Approved vendor switch to Acme Corp" };
            var f4 = new MeetingFlag(FlagType.Decision, meetingStart.AddMinutes(18).AddSeconds(33))
                { Note = "Budget increase approved for Q4" };
            var f5 = new MeetingFlag(FlagType.FollowUp, meetingStart.AddMinutes(22).AddSeconds(5))
                { AssignedTo = "Mike", Note = "Schedule meeting with legal" };
            var f6 = new MeetingFlag(FlagType.Bookmark, meetingStart.AddMinutes(25).AddSeconds(40))
                { Note = "Good discussion on remote work policy" };
            var f7 = new MeetingFlag(FlagType.Highlight, meetingStart.AddMinutes(15).AddSeconds(12));
            var f8 = new MeetingFlag(FlagType.Highlight, meetingStart.AddMinutes(30).AddSeconds(8));

            this._flags.AddRange(new[] { f1, f2, f3, f4, f5, f6, f7, f8 });
            PluginLog.Info($"Demo data loaded: {this.FlagCount} flags");
        }
    }
}
