namespace Loupedeck.CueBoardPlugin.Actions.Page3
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Loupedeck.CueBoardPlugin.Models;

    /// <summary>
    /// Post-meeting handoff: opens the user's default mail client with a draft
    /// containing the meeting flags grouped by type, ready to send to attendees.
    /// Pure mailto: — no integrations, no API keys.
    /// </summary>
    public class EmailSummaryCommand : CueBoardCommand
    {
        public EmailSummaryCommand()
            : base("Email Summary", "Open an email draft with all flagged moments", "Meeting Intelligence")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            var flags = this.CueBoard?.Flags;
            var state = this.State;
            if (flags == null || state == null)
            {
                return;
            }

            var meetingStart = state.MeetingStartTime;
            var subject = $"Meeting Recap — {meetingStart:MMM d, yyyy h:mm tt}";
            var body = this.BuildBody(flags, meetingStart);

            // mailto: payload — RFC-2368, percent-encoded
            var mailto = "mailto:?subject=" + Uri.EscapeDataString(subject)
                       + "&body=" + Uri.EscapeDataString(body);

            try
            {
                Process.Start(new ProcessStartInfo(mailto) { UseShellExecute = true });
                this.CueBoard?.Toast?.ShowToast("\u2709", "Email draft opened");
                PluginLog.Info($"Email draft opened with {flags.FlagCount} flags");
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Failed to open email draft");
                this.CueBoard?.Toast?.ShowToast("\u26A0", "No mail client found");
            }
        }

        private String BuildBody(Services.FlagService flags, DateTime meetingStart)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Meeting Recap — {meetingStart:dddd, MMMM d, yyyy} at {meetingStart:h:mm tt}");
            sb.AppendLine();

            var all = flags.GetFlags();
            if (all.Count == 0)
            {
                sb.AppendLine("No flags were captured during this meeting.");
                sb.AppendLine();
                sb.AppendLine("— Sent from CueBoard");
                return sb.ToString();
            }

            AppendSection(sb, "Action Items", all, FlagType.ActionItem, meetingStart);
            AppendSection(sb, "Decisions", all, FlagType.Decision, meetingStart);
            AppendSection(sb, "Follow-Ups", all, FlagType.FollowUp, meetingStart);
            AppendSection(sb, "Bookmarks", all, FlagType.Bookmark, meetingStart);
            AppendSection(sb, "Highlights", all, FlagType.Highlight, meetingStart);

            sb.AppendLine("— Sent from CueBoard");
            return sb.ToString();
        }

        private static void AppendSection(StringBuilder sb, String header,
            System.Collections.Generic.IReadOnlyList<MeetingFlag> all, FlagType type, DateTime meetingStart)
        {
            var items = all.Where(f => f.Type == type).ToList();
            if (items.Count == 0)
            {
                return;
            }

            sb.AppendLine(header.ToUpperInvariant());
            foreach (var flag in items)
            {
                var rel = flag.Timestamp - meetingStart;
                var ts = $"{(Int32)rel.TotalMinutes:D2}:{rel.Seconds:D2}";

                var line = $"  [{ts}]";
                if (!String.IsNullOrEmpty(flag.Note))
                {
                    line += " " + flag.Note;
                }
                if (!String.IsNullOrEmpty(flag.AssignedTo))
                {
                    line += $" (-> {flag.AssignedTo})";
                }
                sb.AppendLine(line);
            }
            sb.AppendLine();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            // Soft blue — reads as "send / share"
            return this.DrawButton(imageSize, "EMAIL\nRECAP", new BitmapColor(52, 152, 219));
        }
    }
}
