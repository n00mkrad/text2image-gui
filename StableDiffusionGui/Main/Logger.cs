using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using DT = System.DateTime;

namespace StableDiffusionGui.Main
{
    internal class Logger
    {
        public static TextBox Textbox;
        public static TextBox TextboxDebug;
        private static string _file;
        public static long Id;
        public static Dictionary<string, string> SessionLogs = new Dictionary<string, string>();
        private static string _lastUiLine = "";
        public static string LastUiLine { get { return _lastUiLine; } }
        private static string _lastFileLine = "";
        public static string LastFileLine { get { return _lastFileLine; } }
        private static string _lastLogLine = "";
        public static string LastLogLine { get { return _lastLogLine; } }
        private static Dictionary<string, DT> _lastEntryTimestampPerLog = new Dictionary<string, DT>();

        public class Entry
        {
            public string Message { get; set; } = "";
            public bool Hidden { get; set; } = false;
            public bool ReplaceLastLine { get; set; } = false;
            public string LogName { get; set; } = Constants.Lognames.General;

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
        }

        private static ConcurrentQueue<Entry> logQueue = new ConcurrentQueue<Entry>();

        public static void Log(Entry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.Message))
                return;

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

        public static async Task QueueLoop ()
        {
            while (true)
            {
                if(logQueue.Count >= 1)
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

            string msg = entry.Message;

            bool repeated = msg == LastLogLine;
            bool repeatedUi = msg == LastLogLine;

            if (repeatedUi)
                entry.Hidden = true; // Never show the same line twice in UI, but log it to file

            _lastLogLine = msg;

            if (!entry.Hidden)
                _lastUiLine = msg;

            _lastEntryTimestampPerLog[entry.LogName] = DT.Now;

            Console.WriteLine(msg);

            try
            {
                if (!entry.Hidden && entry.ReplaceLastLine)
                {
                    Textbox.Suspend();
                    string[] lines = Textbox.Text.SplitIntoLines();
                    Textbox.Text = string.Join(Environment.NewLine, lines.Take(lines.Count() - 1).ToArray());
                }
            }
            catch { }

            msg = msg.Replace("\n", Environment.NewLine);

            if (!entry.Hidden && Textbox != null && !Textbox.IsDisposed)
                Textbox.AppendText((Textbox.Text.Length > 1 ? Environment.NewLine : "") + msg);

            if (repeated)
                msg = "\"";

            if (entry.ReplaceLastLine)
            {
                Textbox.Resume();
                msg = $"[REPL] {msg}";
            }

            if (!entry.Hidden)
                msg = $"[UI] {msg}";

            if (TextboxDebug != null && !TextboxDebug.IsDisposed)
                TextboxDebug.AppendText($"[{entry.LogName}] {msg}{Environment.NewLine}");

            LogToFile(msg, false, entry.LogName);
        }

        public static void LogToFile(string logStr, bool noLineBreak, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                filename = Constants.Lognames.General;

            if (Path.GetExtension(filename) != ".txt")
                filename = Path.ChangeExtension(filename, "txt");

            _file = Path.Combine(Paths.GetLogPath(), filename);
            //logStr = logStr.Replace(Environment.NewLine, " ").TrimWhitespaces();
            string time = DT.Now.ToString("MM-dd-yyyy HH:mm:ss");

            try
            {
                string appendStr = noLineBreak ? $" {logStr}" : $"{Environment.NewLine}[{Id.ToString().PadLeft(8, '0')}] [{time}]: {logStr}";
                SessionLogs[filename] = (SessionLogs.ContainsKey(filename) ? SessionLogs[filename] : "") + appendStr;
                File.AppendAllText(_file, appendStr);
                Id++;
                _lastFileLine = logStr;
            }
            catch
            {
                // this if fine, i forgot why
            }
        }

        public static string GetSessionLog(string filename)
        {
            if (!filename.Contains(".txt"))
                filename = Path.ChangeExtension(filename, "txt");

            if (SessionLogs.ContainsKey(filename))
                return SessionLogs[filename];
            else
                return "";
        }

        public static List<string> GetLastLines(string filename, int linesCount = 5, bool stripTimestamp = false)
        {
            var lines = GetSessionLog(filename).SplitIntoLines();

            if (stripTimestamp)
                return lines.Reverse().Take(linesCount).Reverse().Select(line => line.Contains("]: ") ? line.Substring(line.IndexOf("]: ") + 1).Substring(2) : line).ToList();
            else
                return lines.Reverse().Take(linesCount).Reverse().ToList();
        }

        public static void LogIfLastLineDoesNotContainMsg(string s, bool hidden = false, bool replaceLastLine = false, string filename = "")
        {
            if (!GetLastLine().Contains(s))
                Log(s, hidden, replaceLastLine, filename);
        }

        public static void WriteToFile(string content, bool append, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                filename = Constants.Lognames.General;

            if (Path.GetExtension(filename) != ".txt")
                filename = Path.ChangeExtension(filename, "txt");

            _file = Path.Combine(Paths.GetLogPath(), filename);

            string time = DT.Now.Month + "-" + DT.Now.Day + "-" + DT.Now.Year + " " + DT.Now.Hour + ":" + DT.Now.Minute + ":" + DT.Now.Second;

            try
            {
                if (append)
                    File.AppendAllText(_file, Environment.NewLine + time + ":" + Environment.NewLine + content);
                else
                    File.WriteAllText(_file, Environment.NewLine + time + ":" + Environment.NewLine + content);
            }
            catch
            {

            }
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

        public static TimeSpan GetTimeSinceLastEntry (string logName)
        {
            if (_lastEntryTimestampPerLog.ContainsKey(logName))
                return DT.Now - _lastEntryTimestampPerLog[logName];
            else
                return DT.MaxValue - DT.MinValue;
        }
    }
}
