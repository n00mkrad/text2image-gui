using HTAlt;
using StableDiffusionGui.Data;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class Logger
    {
        public class Switches
        {
            public static bool LogFontLoader = false;
        }

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
        private static BlockingCollection<Entry> _logQueue = new BlockingCollection<Entry>();

        public static event EventHandler<string> MessageLogged;
        public static event EventHandler<string> MessageDequeued;

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

            public Entry(object message, bool hidden = false, bool replaceLastLine = false, string logName = "")
            {
                if (message == null)
                    return;

                Message = message.ToString().TrimEnd();
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


        private static void Log(Entry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.Message))
                return;

            MessageLogged?.Invoke(null, entry.Message);
            Console.WriteLine(entry.ToString(true, true, true));
            entry.TimeEnqueue = DateTime.Now;
            _logQueue.Add(entry);
        }

        public static void LogReplace(object msg, string filename = Constants.Lognames.General)
        {
            Log(new Entry(msg, false, true, filename));
        }

        public static void Log(object msg, bool hidden = false, bool replaceLastLine = false, string filename = Constants.Lognames.General)
        {
            Log(new Entry(msg, hidden, replaceLastLine, filename));
        }

        public static void LogIf(object msg, bool condition, bool hidden = true, bool replaceLastLine = false, string filename = Constants.Lognames.General)
        {
            if (condition)
                Log(new Entry(msg, hidden, replaceLastLine, filename));
        }

        public static void LogHidden(object msg, string filename = Constants.Lognames.General)
        {
            Log(new Entry(msg, true, false, filename));
        }

        public static void LogException(Exception ex, bool includeTrace = true, string prefix = "Exception:", string filename = Constants.Lognames.General)
        {
            string msg = $"{prefix} {ex.Message}{(includeTrace ? $"\n{ex.StackTrace}" : "")}".Trim();
            Log(new Entry($"[EX] {msg}", true, false, filename));
        }

        public static void QueueLoopOuter()
        {
            while (true)
                QueueLoop(); // Restarts loop in case it throws an exception
        }

        private static void QueueLoop()
        {
            try
            {
                foreach (Entry message in _logQueue.GetConsumingEnumerable())
                    Show(message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Logger Loop ObjectDisposedException: {ex.Message}\n{ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logger Loop Exception: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static void Show(Entry entry)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.Message) || string.IsNullOrWhiteSpace(entry.LogName))
                return;

            MessageDequeued?.Invoke(null, entry.Message);
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

            if (RealtimeLoggerForm != null && !RealtimeLoggerForm.IsDisposed)
                RealtimeLoggerForm.LogAppend(entry.Message.Replace("\n", Environment.NewLine));

            if (!entry.Hidden)
                Program.MainForm.LogAppend(entry.Message.Replace("\n", Environment.NewLine), entry.ReplaceLastLine);

            LogToFile(entry);
        }

        public static void LogToFile(Entry entry)
        {
            string filename = entry.LogName.IsEmpty() ? Constants.Lognames.General : entry.LogName;
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

        public static async Task<string> WaitForMessageAsync(string targetMessage, bool trim = true, bool contains = false, bool cancel = true)
        {
            var tcs = new TaskCompletionSource<bool>();
            var cts = new CancellationTokenSource(); 
            string matchingMsg = "";

            EventHandler<string> handler = (sender, message) =>
            {
                if (trim)
                    message = message.Trim();

                if((contains && message.Contains(targetMessage)) || (!contains && message == targetMessage))
                {
                    matchingMsg = message;
                    tcs.TrySetResult(true);
                }
            };

            MessageLogged += handler;

            while (!tcs.Task.IsCompleted)
            {
                if (cancel && TextToImage.Canceled)
                {
                    tcs.TrySetCanceled();
                    break;
                }

                await Task.Delay(100);
            }

            try
            {
                await tcs.Task;
            }
            catch (OperationCanceledException)
            {
                LogHidden("WaitForMessageAsync was cancelled.");
            }
            finally
            {
                MessageLogged -= handler;
            }

            return matchingMsg;
        }
    }
}
