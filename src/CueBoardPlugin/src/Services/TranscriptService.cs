namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class TranscriptLine
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public String Speaker { get; set; }
        public String Text { get; set; }
    }

    public class TranscriptService
    {
        private List<TranscriptLine> _lines = new List<TranscriptLine>();
        private String _loadedFile;

        public Boolean HasTranscript => this._lines.Count > 0;
        public Int32 LineCount => this._lines.Count;
        public IReadOnlyList<TranscriptLine> Lines => this._lines.AsReadOnly();

        /// <summary>
        /// Loads a transcript file. Auto-detects between WebVTT (.vtt) and Zoom Workplace's
        /// plain-text closed-caption format (meeting_saved_closed_caption.txt).
        /// </summary>
        public Boolean LoadVtt(String filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    PluginLog.Warning($"Transcript file not found: {filePath}");
                    return false;
                }

                var content = File.ReadAllText(filePath);

                // WebVTT files always begin with "WEBVTT" — pick parser by content sniff.
                this._lines = content.TrimStart().StartsWith("WEBVTT", StringComparison.OrdinalIgnoreCase)
                    ? ParseVtt(content)
                    : ParseZoomCaptionTxt(content);

                this._loadedFile = filePath;
                PluginLog.Info($"Transcript loaded: {this._lines.Count} lines from {filePath}");
                return this._lines.Count > 0;
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, $"Failed to load transcript: {filePath}");
                return false;
            }
        }

        /// <summary>
        /// Find transcript lines near a given timestamp (within a window of seconds).
        /// Returns lines sorted by start time.
        /// </summary>
        public List<TranscriptLine> GetLinesNear(TimeSpan timestamp, Int32 windowSeconds = 15)
        {
            var windowStart = timestamp.Add(TimeSpan.FromSeconds(-windowSeconds));
            var windowEnd = timestamp.Add(TimeSpan.FromSeconds(windowSeconds));

            if (windowStart < TimeSpan.Zero)
            {
                windowStart = TimeSpan.Zero;
            }

            return this._lines
                .Where(l => l.Start >= windowStart && l.Start <= windowEnd)
                .OrderBy(l => l.Start)
                .ToList();
        }

        /// <summary>
        /// Find the single closest transcript line to a given timestamp.
        /// </summary>
        public TranscriptLine GetClosestLine(TimeSpan timestamp)
        {
            if (this._lines.Count == 0)
            {
                return null;
            }

            return this._lines
                .OrderBy(l => Math.Abs((l.Start - timestamp).TotalSeconds))
                .First();
        }

        /// <summary>
        /// Attempts to auto-detect a transcript file in common Zoom locations. Picks the
        /// most recently modified file across all candidates. Looks for both WebVTT (.vtt)
        /// and Zoom Workplace's saved closed-caption .txt files.
        /// </summary>
        public static String AutoDetectVttFile()
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var searchPaths = new List<String>
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Zoom"),
                Path.Combine(userProfile, "OneDrive", "Documents", "Zoom"),
                Path.Combine(userProfile, "Documents", "Zoom"),
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Path.Combine(userProfile, "Downloads"),
            };

            FileInfo best = null;

            foreach (var searchPath in searchPaths)
            {
                if (!Directory.Exists(searchPath))
                {
                    continue;
                }

                try
                {
                    // .vtt anywhere under this root
                    foreach (var f in Directory.GetFiles(searchPath, "*.vtt", SearchOption.AllDirectories))
                    {
                        var fi = new FileInfo(f);
                        if ((DateTime.Now - fi.LastWriteTime).TotalHours < 24
                            && (best == null || fi.LastWriteTime > best.LastWriteTime))
                        {
                            best = fi;
                        }
                    }

                    // Zoom Workplace saves closed captions as a known filename
                    foreach (var f in Directory.GetFiles(searchPath, "meeting_saved_closed_caption*.txt", SearchOption.AllDirectories))
                    {
                        var fi = new FileInfo(f);
                        if ((DateTime.Now - fi.LastWriteTime).TotalHours < 24
                            && (best == null || fi.LastWriteTime > best.LastWriteTime))
                        {
                            best = fi;
                        }
                    }
                }
                catch
                {
                    // Skip directories we can't access
                }
            }

            return best?.FullName;
        }

        /// <summary>
        /// Loads a built-in demo transcript so the export linking demo works even when
        /// Zoom didn't produce a VTT file. Generates lines synthesized around the given
        /// flag timestamps so every flag click has something to scroll to.
        /// </summary>
        public void LoadDemoTranscript(IReadOnlyList<Models.MeetingFlag> flags, DateTime meetingStart)
        {
            this._lines.Clear();
            var speakers = new[] { "Sarah", "Mike", "Dev Team", "Tyler", "Acme Rep" };
            var fillerLines = new[]
            {
                "I think we should move forward with the proposed timeline.",
                "Just to clarify, the Q3 numbers are tracking ahead of plan.",
                "Can we circle back on the API migration after legal signs off?",
                "Agreed — let's lock that in and move to the next item.",
                "I'll own that and report back by end of week.",
                "Quick reminder, the budget cycle closes Friday.",
                "Good point. Let's document that and share with the team.",
                "From a technical standpoint that's the cleanest approach.",
                "Anyone have concerns about the rollout sequence?",
                "Let's take that offline and revisit next sync.",
            };

            var rng = new Random(42);
            var now = TimeSpan.FromSeconds(0);

            // Build a base timeline of background chatter every ~12 seconds
            var totalSeconds = flags.Count > 0
                ? (Int32)(flags[flags.Count - 1].Timestamp - meetingStart).TotalSeconds + 30
                : 600;

            for (var t = 0; t < totalSeconds; t += 12)
            {
                this._lines.Add(new TranscriptLine
                {
                    Start = TimeSpan.FromSeconds(t),
                    End = TimeSpan.FromSeconds(t + 8),
                    Speaker = speakers[rng.Next(speakers.Length)],
                    Text = fillerLines[rng.Next(fillerLines.Length)]
                });
            }

            // For each flag, ensure a transcript line exists at that exact second so the click anchor matches
            foreach (var flag in flags)
            {
                var rel = flag.Timestamp - meetingStart;
                var note = String.IsNullOrEmpty(flag.Note) ? fillerLines[rng.Next(fillerLines.Length)] : flag.Note;
                this._lines.Add(new TranscriptLine
                {
                    Start = rel,
                    End = rel.Add(TimeSpan.FromSeconds(6)),
                    Speaker = speakers[rng.Next(speakers.Length)],
                    Text = note
                });
            }

            this._lines = this._lines.OrderBy(l => l.Start).ToList();
            this._loadedFile = "(demo transcript)";
            PluginLog.Info($"Demo transcript synthesized: {this._lines.Count} lines around {flags.Count} flags");
        }

        /// <summary>
        /// Generates the full transcript as HTML for embedding in the export.
        /// Each line gets an ID based on its start time for scroll-to-anchor linking.
        /// </summary>
        public String GenerateTranscriptHtml()
        {
            if (!this.HasTranscript)
            {
                return "";
            }

            var html = new System.Text.StringBuilder();

            foreach (var line in this._lines)
            {
                var timeId = $"t-{(Int32)line.Start.TotalSeconds}";
                var timeStr = $"{(Int32)line.Start.TotalMinutes:D2}:{line.Start.Seconds:D2}";
                var speaker = String.IsNullOrEmpty(line.Speaker) ? "" : $"<span class='transcript-speaker'>{line.Speaker}:</span> ";

                html.Append($"<div class='transcript-line' id='{timeId}'>");
                html.Append($"<span class='transcript-time'>{timeStr}</span>");
                html.Append($"<span class='transcript-text'>{speaker}{line.Text}</span>");
                html.Append("</div>");
            }

            return html.ToString();
        }

        /// <summary>
        /// Parses WebVTT format into TranscriptLine objects.
        /// Handles both Zoom-style (with speaker names) and standard VTT format.
        /// </summary>
        private static List<TranscriptLine> ParseVtt(String content)
        {
            var lines = new List<TranscriptLine>();

            // VTT timestamp pattern: 00:00:00.000 --> 00:00:00.000
            var timestampRegex = new Regex(
                @"(\d{1,2}:\d{2}:\d{2}\.\d{3})\s*-->\s*(\d{1,2}:\d{2}:\d{2}\.\d{3})",
                RegexOptions.Compiled);

            // Speaker pattern: "Name: text" or just "text"
            var speakerRegex = new Regex(@"^([^:]{1,40}):\s*(.+)$", RegexOptions.Compiled);

            var rawLines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            TimeSpan currentStart = TimeSpan.Zero;
            TimeSpan currentEnd = TimeSpan.Zero;
            var textBuffer = new List<String>();
            var hasTimestamp = false;

            foreach (var rawLine in rawLines)
            {
                var trimmed = rawLine.Trim();

                // Skip WEBVTT header and NOTE blocks
                if (trimmed.StartsWith("WEBVTT") || trimmed.StartsWith("NOTE") || trimmed.StartsWith("Kind:") || trimmed.StartsWith("Language:"))
                {
                    continue;
                }

                // Check for timestamp line
                var tsMatch = timestampRegex.Match(trimmed);
                if (tsMatch.Success)
                {
                    // Save previous block if exists
                    if (hasTimestamp && textBuffer.Count > 0)
                    {
                        lines.Add(CreateLine(currentStart, currentEnd, textBuffer));
                    }

                    currentStart = ParseTimestamp(tsMatch.Groups[1].Value);
                    currentEnd = ParseTimestamp(tsMatch.Groups[2].Value);
                    textBuffer.Clear();
                    hasTimestamp = true;
                    continue;
                }

                // Skip numeric cue identifiers
                if (Int32.TryParse(trimmed, out _))
                {
                    continue;
                }

                // Empty line marks end of a cue block
                if (String.IsNullOrWhiteSpace(trimmed))
                {
                    if (hasTimestamp && textBuffer.Count > 0)
                    {
                        lines.Add(CreateLine(currentStart, currentEnd, textBuffer));
                        textBuffer.Clear();
                        hasTimestamp = false;
                    }
                    continue;
                }

                // It's a text line
                if (hasTimestamp)
                {
                    textBuffer.Add(trimmed);
                }
            }

            // Don't forget the last block
            if (hasTimestamp && textBuffer.Count > 0)
            {
                lines.Add(CreateLine(currentStart, currentEnd, textBuffer));
            }

            return lines;
        }

        private static TranscriptLine CreateLine(TimeSpan start, TimeSpan end, List<String> textLines)
        {
            var fullText = String.Join(" ", textLines);

            // Try to extract speaker name
            var speakerMatch = Regex.Match(fullText, @"^([^:]{1,40}):\s*(.+)$");
            if (speakerMatch.Success)
            {
                return new TranscriptLine
                {
                    Start = start,
                    End = end,
                    Speaker = speakerMatch.Groups[1].Value.Trim(),
                    Text = speakerMatch.Groups[2].Value.Trim()
                };
            }

            return new TranscriptLine
            {
                Start = start,
                End = end,
                Speaker = null,
                Text = fullText
            };
        }

        /// <summary>
        /// Parses Zoom Workplace's "meeting_saved_closed_caption.txt" format.
        /// Format per entry:
        ///   [Speaker name]
        ///   [HH:MM:SS]
        ///   [text — may span multiple lines]
        /// Entries are separated by blank lines (sometimes), or by the next speaker line.
        /// We use a lookahead heuristic: any non-timestamp line followed by a timestamp line
        /// is a speaker line; otherwise it's body text.
        /// </summary>
        private static List<TranscriptLine> ParseZoomCaptionTxt(String content)
        {
            var result = new List<TranscriptLine>();
            var tsRegex = new Regex(@"^(\d{1,2}):(\d{2}):(\d{2})(?:\.(\d{1,3}))?$", RegexOptions.Compiled);

            // Strip empty lines so the lookahead "next non-empty line is timestamp" works cleanly.
            var rawLines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            var lines = new List<String>();
            foreach (var l in rawLines)
            {
                var trimmed = l.Trim();
                if (!String.IsNullOrEmpty(trimmed))
                {
                    lines.Add(trimmed);
                }
            }

            String currentSpeaker = null;
            TimeSpan? currentTime = null;
            var currentText = new List<String>();

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var tsMatch = tsRegex.Match(line);

                if (tsMatch.Success)
                {
                    // Timestamp line — emit any pending entry, then start a new one.
                    EmitCaptionEntry(result, currentTime, currentSpeaker, currentText);
                    currentTime = new TimeSpan(0,
                        Int32.Parse(tsMatch.Groups[1].Value),
                        Int32.Parse(tsMatch.Groups[2].Value),
                        Int32.Parse(tsMatch.Groups[3].Value));
                    currentText.Clear();
                }
                else if (i + 1 < lines.Count && tsRegex.IsMatch(lines[i + 1]))
                {
                    // Speaker line (next line is a timestamp) — emit pending then update speaker.
                    EmitCaptionEntry(result, currentTime, currentSpeaker, currentText);
                    currentSpeaker = line;
                    currentText.Clear();
                }
                else
                {
                    // Body text — may be multi-line.
                    currentText.Add(line);
                }
            }

            // Final flush
            EmitCaptionEntry(result, currentTime, currentSpeaker, currentText);

            // Zoom caption timestamps are wall-clock (e.g., 22:58:21). The flag-link system
            // uses meeting-relative time, so shift everything so the first line is 0:00.
            if (result.Count > 0)
            {
                var origin = result[0].Start;
                foreach (var l in result)
                {
                    l.Start = l.Start - origin;
                    l.End = l.End - origin;
                }
            }
            return result;
        }

        private static void EmitCaptionEntry(List<TranscriptLine> sink, TimeSpan? time, String speaker, List<String> textLines)
        {
            if (!time.HasValue || textLines.Count == 0)
            {
                return;
            }
            sink.Add(new TranscriptLine
            {
                Start = time.Value,
                End = time.Value.Add(TimeSpan.FromSeconds(5)),
                Speaker = speaker,
                Text = String.Join(" ", textLines)
            });
        }

        private static TimeSpan ParseTimestamp(String ts)
        {
            var parts = ts.Split(':', '.');
            if (parts.Length >= 4)
            {
                return new TimeSpan(0,
                    Int32.Parse(parts[0]),
                    Int32.Parse(parts[1]),
                    Int32.Parse(parts[2]),
                    Int32.Parse(parts[3]));
            }
            return TimeSpan.Zero;
        }
    }
}
