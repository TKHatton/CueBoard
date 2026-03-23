namespace Loupedeck.CueBoardPlugin.Models
{
    using System;

    public class MeetingFlag
    {
        public FlagType Type { get; set; }
        public DateTime Timestamp { get; set; }
        public String AssignedTo { get; set; }
        public String Note { get; set; }

        public MeetingFlag(FlagType type, DateTime timestamp)
        {
            this.Type = type;
            this.Timestamp = timestamp;
        }
    }
}
