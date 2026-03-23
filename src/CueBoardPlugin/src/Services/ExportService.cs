namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Loupedeck.CueBoardPlugin.Models;

    public class ExportService
    {
        public String ExportToFile(IReadOnlyList<MeetingFlag> flags, DateTime meetingStart)
        {
            var html = this.GenerateHtml(flags, meetingStart);

            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "CueBoard");

            Directory.CreateDirectory(folder);

            var fileName = $"meeting-{meetingStart:yyyy-MM-dd-HHmmss}.html";
            var filePath = Path.Combine(folder, fileName);

            File.WriteAllText(filePath, html, Encoding.UTF8);
            PluginLog.Info($"Summary exported to {filePath}");

            // Auto-open in browser
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                PluginLog.Warning($"Could not open export: {ex.Message}");
            }

            return filePath;
        }

        private String GenerateHtml(IReadOnlyList<MeetingFlag> flags, DateTime meetingStart)
        {
            var duration = DateTime.Now - meetingStart;
            var durationMins = (Int32)duration.TotalMinutes;

            var flagsHtml = new StringBuilder();

            if (flags.Count == 0)
            {
                flagsHtml.Append("<div class='empty'>No flags were recorded during this meeting.</div>");
            }
            else
            {
                var groupedFlags = flags.GroupBy(f => f.Type).OrderBy(g => g.Key);

                foreach (var group in groupedFlags)
                {
                    var typeName = this.GetFlagTypeName(group.Key);
                    var colorClass = this.GetFlagColorClass(group.Key);
                    var flagList = group.ToList();

                    flagsHtml.Append($"<div class='section'>");
                    flagsHtml.Append($"<div class='section-header {colorClass}'>");
                    flagsHtml.Append($"<span class='section-icon'>{this.GetFlagIcon(group.Key)}</span>");
                    flagsHtml.Append($"<span>{typeName}</span>");
                    flagsHtml.Append($"<span class='count'>{flagList.Count}</span>");
                    flagsHtml.Append($"</div>");

                    foreach (var flag in flagList)
                    {
                        var relativeTime = flag.Timestamp - meetingStart;
                        var timeStr = $"{(Int32)relativeTime.TotalMinutes:D2}:{relativeTime.Seconds:D2}";

                        flagsHtml.Append($"<div class='flag-item'>");
                        flagsHtml.Append($"<span class='timestamp'>{timeStr}</span>");
                        flagsHtml.Append($"<span class='flag-detail'>Flagged moment</span>");

                        if (!String.IsNullOrEmpty(flag.AssignedTo))
                        {
                            flagsHtml.Append($"<span class='assigned'>→ {flag.AssignedTo}</span>");
                        }
                        if (!String.IsNullOrEmpty(flag.Note))
                        {
                            flagsHtml.Append($"<div class='note'>{flag.Note}</div>");
                        }

                        flagsHtml.Append($"</div>");
                    }

                    flagsHtml.Append($"</div>");
                }
            }

            // Summary counts
            var actionCount = flags.Count(f => f.Type == FlagType.ActionItem);
            var decisionCount = flags.Count(f => f.Type == FlagType.Decision);
            var followUpCount = flags.Count(f => f.Type == FlagType.FollowUp);
            var bookmarkCount = flags.Count(f => f.Type == FlagType.Bookmark);

            return $@"<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>CueBoard Meeting Summary</title>
<style>
  * {{ margin: 0; padding: 0; box-sizing: border-box; }}
  body {{ font-family: 'Segoe UI', -apple-system, sans-serif; background: #0f0f13; color: #e0e0e0; min-height: 100vh; }}
  .container {{ max-width: 700px; margin: 0 auto; padding: 40px 24px; }}
  .header {{ text-align: center; margin-bottom: 40px; }}
  .logo {{ font-size: 14px; font-weight: 700; letter-spacing: 3px; color: #8B5CF6; text-transform: uppercase; margin-bottom: 8px; }}
  .title {{ font-size: 28px; font-weight: 700; color: #fff; margin-bottom: 4px; }}
  .subtitle {{ font-size: 14px; color: #888; }}
  .stats {{ display: flex; gap: 12px; justify-content: center; margin: 24px 0; flex-wrap: wrap; }}
  .stat {{ background: #1a1a24; border-radius: 12px; padding: 16px 20px; text-align: center; min-width: 100px; }}
  .stat-value {{ font-size: 28px; font-weight: 700; color: #fff; }}
  .stat-label {{ font-size: 11px; color: #888; text-transform: uppercase; letter-spacing: 1px; margin-top: 4px; }}
  .stat.purple .stat-value {{ color: #8B5CF6; }}
  .stat.blue .stat-value {{ color: #3498DB; }}
  .stat.green .stat-value {{ color: #2ECC71; }}
  .stat.orange .stat-value {{ color: #F39C12; }}
  .divider {{ height: 1px; background: #2a2a35; margin: 32px 0; }}
  .section {{ margin-bottom: 24px; }}
  .section-header {{ display: flex; align-items: center; gap: 10px; padding: 12px 16px; border-radius: 10px; font-weight: 600; font-size: 15px; margin-bottom: 8px; }}
  .section-header.action {{ background: #1a1528; color: #8B5CF6; }}
  .section-header.decision {{ background: #152028; color: #3498DB; }}
  .section-header.followup {{ background: #1a2818; color: #2ECC71; }}
  .section-header.bookmark {{ background: #28201a; color: #F39C12; }}
  .section-icon {{ font-size: 18px; }}
  .count {{ margin-left: auto; background: rgba(255,255,255,0.1); padding: 2px 10px; border-radius: 20px; font-size: 13px; }}
  .flag-item {{ display: flex; align-items: center; gap: 12px; padding: 10px 16px; border-left: 2px solid #2a2a35; margin-left: 20px; }}
  .flag-item:hover {{ background: #1a1a24; border-radius: 0 8px 8px 0; }}
  .timestamp {{ font-family: 'Consolas', monospace; font-size: 13px; color: #8B5CF6; background: #1a1528; padding: 3px 8px; border-radius: 6px; white-space: nowrap; }}
  .flag-detail {{ color: #ccc; font-size: 14px; }}
  .assigned {{ color: #F39C12; font-size: 13px; }}
  .note {{ color: #aaa; font-size: 13px; font-style: italic; margin-top: 4px; padding-left: 80px; }}
  .empty {{ text-align: center; color: #666; padding: 40px; font-style: italic; }}
  .footer {{ text-align: center; margin-top: 48px; padding-top: 24px; border-top: 1px solid #2a2a35; }}
  .footer-logo {{ font-size: 12px; font-weight: 700; letter-spacing: 2px; color: #8B5CF6; }}
  .footer-text {{ font-size: 11px; color: #555; margin-top: 4px; }}
</style>
</head>
<body>
<div class='container'>
  <div class='header'>
    <div class='logo'>CueBoard</div>
    <div class='title'>Meeting Summary</div>
    <div class='subtitle'>{meetingStart:dddd, MMMM d, yyyy} · {meetingStart:h:mm tt} · {durationMins} min</div>
  </div>

  <div class='stats'>
    <div class='stat purple'>
      <div class='stat-value'>{flags.Count}</div>
      <div class='stat-label'>Total Flags</div>
    </div>
    <div class='stat blue'>
      <div class='stat-value'>{actionCount}</div>
      <div class='stat-label'>Actions</div>
    </div>
    <div class='stat green'>
      <div class='stat-value'>{decisionCount}</div>
      <div class='stat-label'>Decisions</div>
    </div>
    <div class='stat orange'>
      <div class='stat-value'>{followUpCount + bookmarkCount}</div>
      <div class='stat-label'>Follow-Ups</div>
    </div>
  </div>

  <div class='divider'></div>

  {flagsHtml}

  <div class='footer'>
    <div class='footer-logo'>CUEBOARD</div>
    <div class='footer-text'>Meeting Control Surface for Logitech MX Creative Console</div>
  </div>
</div>
</body>
</html>";
        }

        private String GetFlagTypeName(FlagType type)
        {
            switch (type)
            {
                case FlagType.ActionItem: return "Action Items";
                case FlagType.Decision: return "Decisions";
                case FlagType.FollowUp: return "Follow-Ups";
                case FlagType.Bookmark: return "Bookmarks";
                default: return type.ToString();
            }
        }

        private String GetFlagColorClass(FlagType type)
        {
            switch (type)
            {
                case FlagType.ActionItem: return "action";
                case FlagType.Decision: return "decision";
                case FlagType.FollowUp: return "followup";
                case FlagType.Bookmark: return "bookmark";
                default: return "action";
            }
        }

        private String GetFlagIcon(FlagType type)
        {
            switch (type)
            {
                case FlagType.ActionItem: return "⚡";
                case FlagType.Decision: return "✓";
                case FlagType.FollowUp: return "→";
                case FlagType.Bookmark: return "★";
                default: return "●";
            }
        }
    }
}
