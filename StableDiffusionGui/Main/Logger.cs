using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private static string _lastUi = "";
        public static string LastUiLine { get { return _lastUi; } }
        private static string _lastLog = "";
        public static string LastLogLine { get { return _lastLog; } }
        private static Dictionary<string, DT> _lastEntryTimestampPerLog = new Dictionary<string, DT>();

        public struct Entry
        {
            public string logMessage;
            public bool hidden;
            public bool replaceLastLine;
            public string filename;

            public Entry(string logMessageArg, bool hiddenArg = false, bool replaceLastLineArg = false, string filenameArg = "")
            {
                logMessage = logMessageArg;
                hidden = hiddenArg;
                replaceLastLine = replaceLastLineArg;
                filename = filenameArg;
            }
        }

        private static ConcurrentQueue<Entry> logQueue = new ConcurrentQueue<Entry>();

        public static void Log(string msg, bool hidden = false, bool replaceLastLine = false, string filename = Constants.Lognames.Session)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return;

            logQueue.Enqueue(new Entry(msg, hidden, replaceLastLine, filename));
            ShowNext();
        }

        public static void ShowNext()
        {
            if (logQueue.TryDequeue(out Entry entry))
                Show(entry);
        }

        public static void Show(Entry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.logMessage))
                return;

            string msg = entry.logMessage;

            if (msg == LastUiLine)
                entry.hidden = true; // Never show the same line twice in UI, but log it to file

            _lastLog = msg;

            if (!entry.hidden)
                _lastUi = msg;

            _lastEntryTimestampPerLog[entry.filename] = DT.Now;

            Console.WriteLine(msg);

            try
            {
                if (!entry.hidden && entry.replaceLastLine)
                {
                    Textbox.Suspend();
                    string[] lines = Textbox.Text.SplitIntoLines();
                    Textbox.Text = string.Join(Environment.NewLine, lines.Take(lines.Count() - 1).ToArray());
                }
            }
            catch { }
            msg = msg.Replace("\n", Environment.NewLine);

            if (!entry.hidden && Textbox != null && !Textbox.IsDisposed)
                Textbox.AppendText((Textbox.Text.Length > 1 ? Environment.NewLine : "") + msg);

            if (entry.replaceLastLine)
            {
                Textbox.Resume();
                msg = "[REPL] " + msg;
            }

            if (!entry.hidden)
                msg = "[UI] " + msg;

            if (TextboxDebug != null && !TextboxDebug.IsDisposed)
                TextboxDebug.AppendText($"[{entry.filename}] {msg}{Environment.NewLine}");

            LogToFile(msg, false, entry.filename);
        }

        public static void LogToFile(string logStr, bool noLineBreak, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                filename = Constants.Lognames.Session;

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
                filename = Constants.Lognames.Session;

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
            return includeHidden ? _lastLog : _lastUi;
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
