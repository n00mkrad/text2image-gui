using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
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
        public static RealtimeLoggerForm RealtimeLoggerForm;
        public static EasyDict<string, ConcurrentQueue<Entry>> CachedEntries = new EasyDict<string, ConcurrentQueue<Entry>>();
        public static EasyDict<string, ConcurrentQueue<string>> CachedLines = new EasyDict<string, ConcurrentQueue<string>>();

        private static string _lastUiLine = "";
        public static string LastUiLine { get { return _lastUiLine; } }
        private static string _lastFileLine = "";
        public static string LastFileLine { get { return _lastFileLine; } }
        private static string _lastLogLine = "";
        public static string LastLogLine { get { return _lastLogLine; } }

        private static long _currentId;
        private static ConcurrentQueue<Entry> _logQueue = new ConcurrentQueue<Entry>();


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

                if (!string.IsNullOrWhiteSpace(logName))
                    LogName = logName;
            }

            public string ToString(bool includeId, bool includeTimestamp, bool includeLogName = false)
            {
                var chunks = new List<string>();

                if (includeId)
                    chunks.Add($"[{Id.ToString().PadLeft(8, '0')}]");

                if (includeTimestamp)
                    chunks.Add($"[{TimestampDequeue}]");

                if (includeLogName)
                    chunks.Add($"[{Path.ChangeExtension(LogName, null)}]");

                chunks.Add(Message);

                return string.Join(" ", chunks);
            }
        }


        public static void Log(Entry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.Message))
                return;

            entry.TimeEnqueue = DateTime.Now;
            _logQueue.Enqueue(entry);
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
                if (_logQueue.Count >= 1)
                    ShowNext();
                else
                    await Task.Delay(1);
            }
        }

        public static void ShowNext()
        {
            if (_logQueue.TryDequeue(out Entry entry))
                Show(entry);
        }

        public static void Show(Entry entry)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.Message) || string.IsNullOrWhiteSpace(entry.LogName))
                return;

            entry.Id = _currentId;
            _currentId++;
            entry.TimeDequeue = DateTime.Now;
            entry.RepeatedMessage = entry.Message == LastLogLine;
            entry.RepeatedUiMessage = entry.Message == LastUiLine;

            entry.LogName = AddTxt(entry.LogName);

            if (entry.RepeatedUiMessage)
                entry.Hidden = true; // Never show the same line twice in UI, but log it to file

            _lastLogLine = entry.Message;

            if (!entry.Hidden)
                _lastUiLine = entry.Message;

            Console.WriteLine(entry.ToString(true, true, true));

            if(RealtimeLoggerForm != null)
                RealtimeLoggerForm.LogAppend(entry.Message.Replace("\n", Environment.NewLine), entry.ReplaceLastLine);

            if (!entry.Hidden)
                Program.MainForm.LogAppend(entry.Message.Replace("\n", Environment.NewLine), entry.ReplaceLastLine);

            LogToFile(entry);
        }

        public static void LogToFile(Entry entry)
        {
            string filename = entry.LogName;

            if (string.IsNullOrWhiteSpace(filename))
                filename = Constants.Lognames.General;

            filename = AddTxt(filename);

            try
            {
                bool firstLog = !CachedEntries.ContainsKey(filename) || CachedEntries[filename].Count <= 0;
                StoreLog(filename, entry);
                File.AppendAllText(Path.Combine(Paths.GetLogPath(), filename), $"{(firstLog ? "" : Environment.NewLine)}{entry.ToString(true, true)}");
                _lastFileLine = entry.Message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static string GetSessionLog(string filename)
        {
            filename = AddTxt(filename);
            return string.Join(Environment.NewLine, CachedLines.GetNoNull(filename, new ConcurrentQueue<string>()));
        }

        public static string EntriesToString(IEnumerable<Entry> entries, bool includeId = false, bool includeTimestamp = false, bool includeLogName = false)
        {
            string s = "";

            foreach (Entry e in entries)
                s += $"{(s == "" ? "" : Environment.NewLine)}{e.ToString(includeId, includeTimestamp, includeLogName)}";

            return s;
        }

        public static List<string> GetLastLines(string filename, int linesCount = 5)
        {
            filename = AddTxt(filename);
            return CachedLines.GetNoNull(filename, new ConcurrentQueue<string>()).AsEnumerable().Reverse().Take(linesCount).Reverse().ToList();
        }

        public static List<Entry> GetLastEntries(string filename, int entriesCount = 5)
        {
            filename = AddTxt(filename);
            return CachedEntries.GetNoNull(filename, new ConcurrentQueue<Entry>()).AsEnumerable().Reverse().Take(entriesCount).Reverse().ToList();
        }

        public static List<Entry> GetLastEntries(int entriesCount = 5)
        {
            var entries = new List<Entry>();

            if (entriesCount > 0)
            {
                foreach (var log in CachedEntries)
                    entries.AddRange(log.Value.Reverse().Take(entriesCount).Reverse());
            }
            else
            {
                foreach (var log in CachedEntries)
                    entries.AddRange(log.Value);
            }

            return entries;
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
            if (Program.MainForm != null)
                Program.MainForm.LogText = "";
        }

        public static string GetLastLine(bool includeHidden = false)
        {
            return includeHidden ? _lastLogLine : _lastUiLine;
        }

        public static string AddTxt(string name)
        {
            return name.EndsWith(".txt") ? name : $"{name}.txt"; // Add .txt if it's not already there
        }

        public static void StoreLog(string filename, Entry entry)
        {
            if (!CachedEntries.ContainsKey(filename) || CachedEntries[filename] == null)
                CachedEntries[filename] = new ConcurrentQueue<Entry>();

            CachedEntries[filename].Enqueue(entry);

            if (!CachedLines.ContainsKey(filename) || CachedLines[filename] == null)
                CachedLines[filename] = new ConcurrentQueue<string>();

            CachedLines[filename].Enqueue(entry.Message);
        }

        public static void Clear(string filename = "")
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                CachedEntries.Clear();
                CachedLines.Clear();
            }
            else
            {
                if (CachedEntries.ContainsKey(filename))
                    CachedEntries[filename] = new ConcurrentQueue<Entry>();

                if (CachedLines.ContainsKey(filename))
                    CachedLines[filename] = new ConcurrentQueue<string>();
            }
        }
    }
}
