using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Main
{
    internal class Logger
    {
        public static TextBox Textbox;
        public static TextBox TextboxDebug;

        private static long _currentId;

        public static Dictionary<string, List<Entry>> SessionLogs = new Dictionary<string, List<Entry>>();
        private static string _lastUiLine = "";
        public static string LastUiLine { get { return _lastUiLine; } }
        private static string _lastFileLine = "";
        public static string LastFileLine { get { return _lastFileLine; } }
        private static string _lastLogLine = "";
        public static string LastLogLine { get { return _lastLogLine; } }

        public class Entry
        {
            public string Message { get; set; } = "";
            public bool Hidden { get; set; } = false;
            public bool ReplaceLastLine { get; set; } = false;
            public string LogName { get; set; } = Constants.Lognames.General;
            public long Id { get; set; } = -1;
            public bool RepeatedMessage = false;
            public bool RepeatedUiMessage = false;
            public DateTime TimeEnqueue { get; set; }
            public DateTime TimeDequeue { get; set; }
            public string TimestampEnqueue { get { return GetTimestamp(TimeEnqueue); } }
            public string TimestampDequeue { get { return GetTimestamp(TimeDequeue); } }

            public Entry()
            {

            }

            public Entry(string message, bool hidden = false, bool replaceLastLine = false, string logName = "")
            {
                Message = message;
                Hidden = hidden;
                ReplaceLastLine = replaceLastLine;
                LogName = logName;
            }

            public string ToString(bool includeIndex, bool includeTimestamp, bool includeLogName = false)
            {
                var chunks = new List<string>();

                if (includeIndex)
                    chunks.Add($"[{_currentId.ToString().PadLeft(8, '0')}]");

                if (includeTimestamp)
                    chunks.Add($"[{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")}]:");

                if (includeLogName)
                    chunks.Add($"[{LogName}]");

                return string.Join(" ", chunks);
            }
        }

        private static ConcurrentQueue<Entry> logQueue = new ConcurrentQueue<Entry>();

        public static void Log(Entry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.Message))
                return;

            entry.TimeEnqueue = DateTime.Now;
            logQueue.Enqueue(entry);
        }

        public static void LogReplace(string msg, string filename = Constants.Lognames.General)
        {
            Log(new Entry(msg, false, true, filename));
        }

        public static void Log(string msg, bool hidden = false, bool replaceLastLine = false, string filename = Constants.Lognames.General)
        {
            Log(new Entry(msg, hidden, replaceLastLine, filename));
        }

        public static void LogHidden(string msg, string filename = Constants.Lognames.General)
        {
            Log(new Entry(msg, true, false, filename));
        }

        public static async Task QueueLoop()
        {
            while (true)
            {
                if (logQueue.Count >= 1)
                    ShowNext();
            }
        }

        public static void ShowNext()
        {
            if (logQueue.TryDequeue(out Entry entry))
                Show(entry);
        }

        public static void Show(Entry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.Message))
                return;

            entry.Id = _currentId;
            entry.TimeDequeue = DateTime.Now;
            entry.RepeatedMessage = entry.Message == LastLogLine;
            entry.RepeatedUiMessage = entry.Message == LastLogLine;

            if (entry.RepeatedUiMessage)
                entry.Hidden = true; // Never show the same line twice in UI, but log it to file

            _lastLogLine = entry.Message;

            if (!entry.Hidden)
                _lastUiLine = entry.Message;

            Console.WriteLine(entry.ToString(true, true, true));

            try
            {
                if (!entry.Hidden && entry.ReplaceLastLine)
                {
                    Textbox.Suspend();
                    string[] lines = Textbox.Text.SplitIntoLines();
                    Textbox.Text = string.Join(Environment.NewLine, lines.Take(lines.Length - 1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging Error: {ex.Message}\n{ex.StackTrace}");
            }

            if (!entry.Hidden && Textbox != null && !Textbox.IsDisposed)
                Textbox.AppendText((string.IsNullOrWhiteSpace(Textbox.Text) ? "" : Environment.NewLine) + entry.Message.Replace("\n", Environment.NewLine));

            if (entry.ReplaceLastLine)
                Textbox.Resume();

            LogToFile(entry);
        }

        public static void LogToFile(Entry entry)
        {
            string filename = entry.LogName;

            if (string.IsNullOrWhiteSpace(filename))
                filename = Constants.Lognames.General;

            if (Path.GetExtension(filename) != ".txt")
                filename = Path.ChangeExtension(filename, "txt");

            try
            {
                bool firstLog = !SessionLogs.ContainsKey(filename) || SessionLogs[filename].Count <= 0;
                StoreLog(filename, entry);
                File.AppendAllText(Path.Combine(Paths.GetLogPath(), filename), $"{(firstLog ? "" : Environment.NewLine)}{entry.ToString(true, true)}");
                _currentId++;
                _lastFileLine = entry.Message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void StoreLog(string logName, Entry entry)
        {
            if (!SessionLogs.ContainsKey(logName) || SessionLogs[logName] == null)
                SessionLogs[logName] = new List<Entry>();

            SessionLogs[logName].Add(entry);
        }

        public static List<Entry> GetSessionLogEntries(string filename)
        {
            if (!filename.Contains(".txt"))
                filename = Path.ChangeExtension(filename, "txt");

            if (SessionLogs.ContainsKey(filename))
                return SessionLogs[filename];
            else
                return new List<Entry>();
        }

        public static string GetSessionLog(string filename)
        {
            if (!filename.Contains(".txt"))
                filename = Path.ChangeExtension(filename, "txt");

            if (SessionLogs.ContainsKey(filename))
                return EntriesToString(SessionLogs[filename]);
            else
                return "";
        }

        private static string EntriesToString(IEnumerable<Entry> entries, bool includeIndex = false, bool includeTimestamp = false, bool includeLogName = false)
        {
            string s = "";

            foreach (Entry e in entries)
                s += $"{(s == "" ? "" : Environment.NewLine)}{e.ToString(includeIndex, includeTimestamp, includeLogName)}";

            return s;
        }

        public static List<string> GetLastLines(string filename, int linesCount = 5, bool stripTimestamp = false)
        {
            return SessionLogs[filename].AsEnumerable().Reverse().Take(linesCount).Reverse().Select(l => l.ToString(false, !stripTimestamp, false)).ToList();
        }

        public static void LogIfLastLineDoesNotContainMsg(string s, bool hidden = false, bool replaceLastLine = false, string filename = "")
        {
            if (!GetLastLine().Contains(s))
                Log(s, hidden, replaceLastLine, filename);
        }

        private static string GetTimestamp(DateTime t)
        {
            if (t == DateTime.MinValue)
                t = DateTime.Now;

            return t.ToString("MM-dd-yyyy HH:mm:ss");
        }

        public static void ClearLogBox()
        {
            Textbox.Text = "";
        }

        public static string GetLastLine(bool includeHidden = false)
        {
            return includeHidden ? _lastLogLine : _lastUiLine;
        }

        public static void RemoveLastLine()
        {
            Textbox.Text = Textbox.Text.Remove(Textbox.Text.LastIndexOf(Environment.NewLine));
        }
    }
}
