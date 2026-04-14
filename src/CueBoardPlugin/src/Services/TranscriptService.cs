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
        /// Loads a VTT (WebVTT) transcript file. Returns true on success.
        /// Zoom saves these as .vtt files in the meeting recording folder.
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
                this._lines = ParseVtt(content);
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
        /// Attempts to auto-detect a VTT file in common Zoom recording locations.
        /// Returns the path if found, null otherwise.
        /// </summary>
        public static String AutoDetectVttFile()
        {
            var searchPaths = new List<String>
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Zoom"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
            };

            foreach (var searchPath in searchPaths)
            {
                if (!Directory.Exists(searchPath))
                {
                    continue;
                }

                // Find the most recently modified .vtt file
                try
                {
                    var vttFiles = Directory.GetFiles(searchPath, "*.vtt", SearchOption.AllDirectories);
                    if (vttFiles.Length > 0)
                    {
                        var newest = vttFiles
                            .Select(f => new FileInfo(f))
                            .OrderByDescending(f => f.LastWriteTime)
                            .First();

                        // Only return if modified in the last 24 hours
                        if ((DateTime.Now - newest.LastWriteTime).TotalHours < 24)
                        {
                            return newest.FullName;
                        }
                    }
                }
                catch
                {
                    // Skip directories we can't access
                }
            }

            return null;
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
