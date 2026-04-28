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
        public String ExportToFile(IReadOnlyList<MeetingFlag> flags, DateTime meetingStart, TranscriptService transcript = null)
        {
            var html = this.GenerateHtml(flags, meetingStart, transcript);

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
                PluginLog.Warning($"UseShellExecute failed: {ex.Message}");
                try
                {
                    Process.Start(new ProcessStartInfo("cmd.exe", $"/c start \"\" \"{filePath}\"")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                }
                catch (Exception ex2)
                {
                    PluginLog.Warning($"cmd fallback also failed: {ex2.Message}");
                }
            }

            return filePath;
        }

        private String GenerateHtml(IReadOnlyList<MeetingFlag> flags, DateTime meetingStart, TranscriptService transcript)
        {
            var duration = DateTime.Now - meetingStart;
            var durationMins = (Int32)duration.TotalMinutes;
            var hasTranscript = transcript != null && transcript.HasTranscript;

            var actionCount = flags.Count(f => f.Type == FlagType.ActionItem);
            var decisionCount = flags.Count(f => f.Type == FlagType.Decision);
            var followUpCount = flags.Count(f => f.Type == FlagType.FollowUp);
            var bookmarkCount = flags.Count(f => f.Type == FlagType.Bookmark);
            var highlightCount = flags.Count(f => f.Type == FlagType.Highlight);
            var assignedFlags = flags.Where(f => !String.IsNullOrEmpty(f.AssignedTo)).ToList();

            // Quick Summary
            var summaryParts = new List<String>();
            if (actionCount > 0) summaryParts.Add($"{actionCount} action item{(actionCount > 1 ? "s" : "")} flagged");
            if (decisionCount > 0) summaryParts.Add($"{decisionCount} key decision{(decisionCount > 1 ? "s" : "")} recorded");
            if (followUpCount > 0) summaryParts.Add($"{followUpCount} follow-up{(followUpCount > 1 ? "s" : "")} needed");
            if (highlightCount > 0) summaryParts.Add($"{highlightCount} highlight{(highlightCount > 1 ? "s" : "")} captured");
            if (bookmarkCount > 0) summaryParts.Add($"{bookmarkCount} moment{(bookmarkCount > 1 ? "s" : "")} bookmarked");
            if (assignedFlags.Count > 0)
            {
                var names = assignedFlags.Select(f => f.AssignedTo).Distinct().ToList();
                summaryParts.Add($"{assignedFlags.Count} item{(assignedFlags.Count > 1 ? "s" : "")} assigned to {String.Join(", ", names)}");
            }
            var summaryHtml = summaryParts.Count > 0
                ? $"<div class='summary'>{String.Join(". ", summaryParts)}.</div>" : "";

            // Build flag sections with transcript context
            var flagsHtml = new StringBuilder();
            if (flags.Count == 0)
            {
                flagsHtml.Append("<div class='empty'>No flags were recorded during this meeting.</div>");
            }
            else
            {
                foreach (var group in flags.GroupBy(f => f.Type).OrderBy(g => g.Key))
                {
                    var typeName = this.GetFlagTypeName(group.Key);
                    var colorClass = this.GetFlagColorClass(group.Key);
                    var flagList = group.ToList();

                    flagsHtml.Append($"<div class='section'>");
                    flagsHtml.Append($"<div class='section-header {colorClass}'>");
                    flagsHtml.Append($"<span class='section-icon'>{this.GetFlagIcon(group.Key)}</span>");
                    flagsHtml.Append($"<span>{typeName}</span>");
                    flagsHtml.Append($"<span class='count'>{flagList.Count}</span>");
                    flagsHtml.Append("</div>");

                    foreach (var flag in flagList)
                    {
                        var relativeTime = flag.Timestamp - meetingStart;
                        var timeSec = (Int32)relativeTime.TotalSeconds;
                        var timeStr = $"{(Int32)relativeTime.TotalMinutes:D2}:{relativeTime.Seconds:D2}";

                        flagsHtml.Append("<div class='flag-item'>");

                        if (hasTranscript)
                        {
                            flagsHtml.Append($"<span class='timestamp clickable' onclick='scrollToTranscript({timeSec})' title='Click to see transcript'>{timeStr}</span>");
                        }
                        else
                        {
                            flagsHtml.Append($"<span class='timestamp'>{timeStr}</span>");
                        }

                        flagsHtml.Append($"<span class='flag-detail'>{this.GetFlagDescription(flag.Type)}</span>");

                        if (!String.IsNullOrEmpty(flag.AssignedTo))
                            flagsHtml.Append($"<span class='assigned'>-&gt; {flag.AssignedTo}</span>");

                        // Inline transcript snippet
                        if (hasTranscript)
                        {
                            var closest = transcript.GetClosestLine(relativeTime);
                            if (closest != null)
                            {
                                var speaker = String.IsNullOrEmpty(closest.Speaker) ? "" : $"<strong>{closest.Speaker}:</strong> ";
                                flagsHtml.Append($"<div class='transcript-snippet'>{speaker}{closest.Text}</div>");
                            }
                        }

                        if (!String.IsNullOrEmpty(flag.Note))
                            flagsHtml.Append($"<div class='note'>{flag.Note}</div>");

                        flagsHtml.Append("</div>");
                    }
                    flagsHtml.Append("</div>");
                }
            }

            // Transcript panel HTML
            var transcriptPanelHtml = "";
            if (hasTranscript)
            {
                transcriptPanelHtml = $@"
    <div id='transcript-panel' class='transcript-panel'>
      <div class='transcript-panel-header'>
        <span>Transcript</span>
        <span class='transcript-close' onclick='toggleTranscript()'>&#10005;</span>
      </div>
      <div class='transcript-body'>
        {transcript.GenerateTranscriptHtml()}
      </div>
    </div>";
            }

            var transcriptBtnHtml = hasTranscript
                ? "<button class='action-btn' onclick='toggleTranscript()'>&#128196; Transcript</button>"
                : "";

            var noTranscriptBanner = hasTranscript ? "" : @"
    <div class='no-transcript-banner'>
      <strong>No transcript loaded.</strong> Your flags are still timestamped below.
      To enable transcript linking: in Zoom Workplace, open the Transcript panel and click
      <strong>Save transcript</strong> before exporting — CueBoard auto-detects the file.
      Or use the <strong>Load Transcript</strong> button on CueBoard to pick a .vtt manually.
    </div>";

            return $@"<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>CueBoard Meeting Summary</title>
<style>
  * {{ margin: 0; padding: 0; box-sizing: border-box; }}
  body {{ font-family: 'Segoe UI', -apple-system, sans-serif; background: #0f0f13; color: #e0e0e0; min-height: 100vh; }}
  .container {{ max-width: 700px; margin: 0 auto; padding: 40px 24px; transition: margin-right 0.3s; }}
  body.transcript-open .container {{ margin-right: 380px; }}
  .header {{ text-align: center; margin-bottom: 32px; }}
  .logo {{ font-size: 14px; font-weight: 700; letter-spacing: 3px; color: #8B5CF6; text-transform: uppercase; margin-bottom: 8px; }}
  .title {{ font-size: 28px; font-weight: 700; color: #fff; margin-bottom: 4px; }}
  .subtitle {{ font-size: 14px; color: #888; }}

  /* Action buttons */
  .actions {{ display: flex; gap: 8px; justify-content: center; margin: 16px 0 24px; flex-wrap: wrap; }}
  .action-btn {{ background: #1a1a24; border: 1px solid #2a2a35; color: #ccc; padding: 8px 16px; border-radius: 8px; cursor: pointer; font-size: 13px; font-family: inherit; transition: all 0.2s; }}
  .action-btn:hover {{ background: #252530; color: #fff; border-color: #8B5CF6; }}

  /* Stats */
  .stats {{ display: flex; gap: 12px; justify-content: center; margin: 24px 0; flex-wrap: wrap; }}
  .stat {{ background: #1a1a24; border-radius: 12px; padding: 16px 20px; text-align: center; min-width: 85px; }}
  .stat-value {{ font-size: 28px; font-weight: 700; color: #fff; }}
  .stat-label {{ font-size: 11px; color: #888; text-transform: uppercase; letter-spacing: 1px; margin-top: 4px; }}
  .stat.purple .stat-value {{ color: #8B5CF6; }}
  .stat.blue .stat-value {{ color: #3498DB; }}
  .stat.green .stat-value {{ color: #2ECC71; }}
  .stat.orange .stat-value {{ color: #F39C12; }}
  .stat.gold .stat-value {{ color: #F59E0B; }}
  .stat.teal .stat-value {{ color: #0D9488; }}

  .summary {{ background: #1a1a24; border-radius: 12px; padding: 20px; margin-bottom: 24px; line-height: 1.8; color: #ccc; font-size: 14px; border-left: 3px solid #8B5CF6; }}
  .no-transcript-banner {{ background: #28201a; border-radius: 12px; padding: 16px 20px; margin-bottom: 16px; line-height: 1.6; color: #d4b48f; font-size: 13px; border-left: 3px solid #F39C12; }}
  .no-transcript-banner strong {{ color: #fff; }}
  .divider {{ height: 1px; background: #2a2a35; margin: 32px 0; }}

  /* Flag sections */
  .section {{ margin-bottom: 24px; }}
  .section-header {{ display: flex; align-items: center; gap: 10px; padding: 12px 16px; border-radius: 10px; font-weight: 600; font-size: 15px; margin-bottom: 8px; }}
  .section-header.action {{ background: #1a1528; color: #8B5CF6; }}
  .section-header.decision {{ background: #152028; color: #3498DB; }}
  .section-header.followup {{ background: #1a2818; color: #2ECC71; }}
  .section-header.bookmark {{ background: #28201a; color: #F39C12; }}
  .section-header.highlight {{ background: #28241a; color: #F59E0B; }}
  .section-icon {{ font-size: 18px; }}
  .count {{ margin-left: auto; background: rgba(255,255,255,0.1); padding: 2px 10px; border-radius: 20px; font-size: 13px; }}

  .flag-item {{ display: flex; align-items: center; gap: 12px; padding: 10px 16px; border-left: 2px solid #2a2a35; margin-left: 20px; flex-wrap: wrap; }}
  .flag-item:hover {{ background: #1a1a24; border-radius: 0 8px 8px 0; }}
  .timestamp {{ font-family: 'Consolas', monospace; font-size: 13px; color: #8B5CF6; background: #1a1528; padding: 3px 8px; border-radius: 6px; white-space: nowrap; }}
  .timestamp.clickable {{ cursor: pointer; transition: all 0.2s; }}
  .timestamp.clickable:hover {{ background: #8B5CF6; color: #fff; }}
  .flag-detail {{ color: #ccc; font-size: 14px; }}
  .assigned {{ color: #F39C12; font-size: 13px; font-weight: 600; }}
  .note {{ width: 100%; color: #ddd; font-size: 13px; margin-top: 8px; padding: 8px 12px; padding-left: 80px; border-left: 2px solid #8B5CF6; background: rgba(139, 92, 246, 0.06); border-radius: 0 6px 6px 0; line-height: 1.4; }}
  .transcript-snippet {{ width: 100%; color: #999; font-size: 12px; font-style: italic; margin-top: 4px; padding-left: 80px; }}
  .transcript-snippet strong {{ color: #bbb; font-style: normal; }}
  .empty {{ text-align: center; color: #666; padding: 40px; font-style: italic; }}

  /* Transcript panel */
  .transcript-panel {{ position: fixed; top: 0; right: -370px; width: 360px; height: 100vh; background: #12121a; border-left: 1px solid #2a2a35; transition: right 0.3s; z-index: 100; display: flex; flex-direction: column; }}
  .transcript-panel.open {{ right: 0; }}
  .transcript-panel-header {{ display: flex; justify-content: space-between; align-items: center; padding: 16px 20px; border-bottom: 1px solid #2a2a35; font-weight: 700; color: #8B5CF6; font-size: 14px; letter-spacing: 1px; }}
  .transcript-close {{ cursor: pointer; color: #888; font-size: 18px; padding: 4px; }}
  .transcript-close:hover {{ color: #fff; }}
  .transcript-body {{ flex: 1; overflow-y: auto; padding: 12px 0; }}
  .transcript-line {{ padding: 6px 20px; font-size: 13px; line-height: 1.5; border-left: 3px solid transparent; transition: all 0.2s; }}
  .transcript-line:hover {{ background: #1a1a24; }}
  .transcript-line.highlight {{ background: rgba(139, 92, 246, 0.15); border-left-color: #8B5CF6; }}
  .transcript-time {{ font-family: 'Consolas', monospace; font-size: 11px; color: #666; margin-right: 8px; }}
  .transcript-text {{ color: #ccc; }}
  .transcript-speaker {{ color: #8B5CF6; font-weight: 600; }}

  .footer {{ text-align: center; margin-top: 48px; padding-top: 24px; border-top: 1px solid #2a2a35; }}
  .footer-logo {{ font-size: 12px; font-weight: 700; letter-spacing: 2px; color: #8B5CF6; }}
  .footer-text {{ font-size: 11px; color: #555; margin-top: 4px; }}

  @media print {{
    .actions, .transcript-panel {{ display: none !important; }}
    body {{ background: #fff; color: #222; }}
    .container {{ max-width: 100%; margin: 0; }}
    .stat {{ border: 1px solid #ddd; }}
    .section-header {{ border: 1px solid #ddd; }}
  }}
</style>
</head>
<body>
<div class='container'>
  <div class='header'>
    <div class='logo'>CueBoard</div>
    <div class='title'>Meeting Summary</div>
    <div class='subtitle'>{meetingStart:dddd, MMMM d, yyyy} &middot; {meetingStart:h:mm tt} &middot; {durationMins} min</div>
  </div>

  <div class='actions'>
    <button class='action-btn' onclick='window.print()'>&#128424; Print</button>
    <button class='action-btn' onclick='emailSummary()'>&#9993; Email</button>
    <button class='action-btn' onclick='downloadFile()'>&#11015; Download</button>
    {transcriptBtnHtml}
  </div>

  <div class='stats'>
    <div class='stat purple'><div class='stat-value'>{flags.Count}</div><div class='stat-label'>Total</div></div>
    <div class='stat blue'><div class='stat-value'>{actionCount}</div><div class='stat-label'>Actions</div></div>
    <div class='stat green'><div class='stat-value'>{decisionCount}</div><div class='stat-label'>Decisions</div></div>
    <div class='stat teal'><div class='stat-value'>{followUpCount}</div><div class='stat-label'>Follow-Ups</div></div>
    <div class='stat orange'><div class='stat-value'>{bookmarkCount}</div><div class='stat-label'>Bookmarks</div></div>
    <div class='stat gold'><div class='stat-value'>{highlightCount}</div><div class='stat-label'>Highlights</div></div>
  </div>

  {summaryHtml}
  {noTranscriptBanner}
  <div class='divider'></div>
  {flagsHtml}

  <div class='footer'>
    <div class='footer-logo'>CUEBOARD</div>
    <div class='footer-text'>Meeting Control Surface for Logitech MX Creative Console</div>
  </div>
</div>

{transcriptPanelHtml}

<script>
function toggleTranscript() {{
  var panel = document.getElementById('transcript-panel');
  if (panel) {{
    panel.classList.toggle('open');
    document.body.classList.toggle('transcript-open');
  }}
}}

function scrollToTranscript(seconds) {{
  var panel = document.getElementById('transcript-panel');
  if (panel && !panel.classList.contains('open')) {{
    panel.classList.add('open');
    document.body.classList.add('transcript-open');
  }}

  // Find closest transcript line and scroll to it
  var target = document.getElementById('t-' + seconds);
  if (!target) {{
    // Find nearest
    var allLines = document.querySelectorAll('.transcript-line');
    var closest = null;
    var closestDiff = 999999;
    allLines.forEach(function(el) {{
      var id = el.id;
      if (id && id.startsWith('t-')) {{
        var t = parseInt(id.substring(2));
        var diff = Math.abs(t - seconds);
        if (diff < closestDiff) {{
          closestDiff = diff;
          closest = el;
        }}
      }}
    }});
    target = closest;
  }}

  if (target) {{
    // Remove previous highlights
    document.querySelectorAll('.transcript-line.highlight').forEach(function(el) {{
      el.classList.remove('highlight');
    }});
    target.classList.add('highlight');
    target.scrollIntoView({{ behavior: 'smooth', block: 'center' }});
  }}
}}

function emailSummary() {{
  var subject = encodeURIComponent(document.title);
  var body = encodeURIComponent('Meeting summary attached. Open the HTML file in your browser to view.\n\nExported by CueBoard - Meeting Control Surface');
  window.location.href = 'mailto:?subject=' + subject + '&body=' + body;
}}

function downloadFile() {{
  var html = document.documentElement.outerHTML;
  var blob = new Blob([html], {{ type: 'text/html' }});
  var a = document.createElement('a');
  a.href = URL.createObjectURL(blob);
  a.download = document.title.replace(/[^a-zA-Z0-9]/g, '_') + '.html';
  a.click();
}}
</script>
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
                case FlagType.Highlight: return "Highlights";
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
                case FlagType.Highlight: return "highlight";
                default: return "action";
            }
        }

        private String GetFlagIcon(FlagType type)
        {
            switch (type)
            {
                case FlagType.ActionItem: return "&#9889;";
                case FlagType.Decision: return "&#10003;";
                case FlagType.FollowUp: return "&#8594;";
                case FlagType.Bookmark: return "&#9733;";
                case FlagType.Highlight: return "&#9670;";
                default: return "&#9679;";
            }
        }

        private String GetFlagDescription(FlagType type)
        {
            switch (type)
            {
                case FlagType.ActionItem: return "Action item flagged";
                case FlagType.Decision: return "Decision recorded";
                case FlagType.FollowUp: return "Follow-up needed";
                case FlagType.Bookmark: return "Bookmarked for reference";
                case FlagType.Highlight: return "Key moment highlighted";
                default: return "Flagged moment";
            }
        }
    }
}
