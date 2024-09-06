using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.MiscUtils
{
    internal class FormatUtils
    {
        public static string Bytes(long sizeBytes)
        {
            try
            {
                string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
                if (sizeBytes == 0)
                    return "0" + suf[0];
                long bytes = Math.Abs(sizeBytes);
                int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                double num = Math.Round(bytes / Math.Pow(1024, place), 1);
                return ($"{Math.Sign(sizeBytes) * num} {suf[place]}");
            }
            catch
            {
                return "N/A B";
            }
        }

        public static string Time(long milliseconds, bool allowMs = true)
        {
            return Time(TimeSpan.FromMilliseconds(milliseconds), allowMs);
        }

        public static string Time(TimeSpan span, bool allowMs = true)
        {
            if (span.TotalHours >= 1f)
                return span.ToString(@"hh\:mm\:ss");

            if (span.TotalMinutes >= 1f)
                return span.ToString(@"mm\:ss");

            if (span.TotalSeconds >= 1f || !allowMs)
            {
                string format = span.TotalSeconds < 10f ? @"%s\.f" : @"%s";
                return span.ToString(format) + "s";
            }

            return span.ToString(@"fff") + "ms";
        }

        public static string Time(Stopwatch sw)
        {
            return Time(sw.ElapsedMilliseconds);
        }

        public static long TimestampToSecs(string timestamp, bool hasMilliseconds = true)
        {
            try
            {
                string[] values = timestamp.Split(':');
                int hours = int.Parse(values[0]);
                int minutes = int.Parse(values[1]);
                int seconds = int.Parse(values[2].Split('.')[0]);
                long secs = hours * 3600 + minutes * 60 + seconds;

                if (hasMilliseconds)
                {
                    int milliseconds = int.Parse(values[2].Split('.')[1].Substring(0, 2)) * 10;

                    if (milliseconds >= 500)
                        secs++;
                }

                return secs;
            }
            catch (Exception e)
            {
                Logger.Log($"TimestampToSecs({timestamp}) Exception: {e.Message}", true);
                return 0;
            }
        }

        public static long TimestampToMs(string timestamp)
        {
            try
            {
                string[] values = timestamp.Split(':');
                int hours = int.Parse(values[0]);
                int minutes = int.Parse(values[1]);
                int seconds = int.Parse(values[2].Split('.')[0]);
                long ms = 0;

                if (timestamp.Contains("."))
                {
                    int milliseconds = int.Parse(values[2].Split('.')[1].Substring(0, 2)) * 10;
                    ms = hours * 3600000 + minutes * 60000 + seconds * 1000 + milliseconds;
                }
                else
                {
                    ms = hours * 3600000 + minutes * 60000 + seconds * 1000;
                }

                return ms;
            }
            catch (Exception e)
            {
                Logger.Log($"MsFromTimeStamp({timestamp}) Exception: {e.Message}", true);
                return 0;
            }
        }

        public static string SecsToTimestamp(long seconds)
        {
            return (new DateTime(1970, 1, 1)).AddSeconds(seconds).ToString("HH:mm:ss");
        }

        public static string MsToTimestamp(long milliseconds)
        {
            return (new DateTime(1970, 1, 1)).AddMilliseconds(milliseconds).ToString("HH:mm:ss");
        }

        public static string Ratio(long numFrom, long numTo)
        {
            float ratio = ((float)numFrom / (float)numTo) * 100f;
            return ratio.ToString("0.00") + "%";
        }

        public static int RatioInt(long numFrom, long numTo)
        {
            double ratio = Math.Round(((float)numFrom / (float)numTo) * 100f);
            return (int)ratio;
        }

        public static string RatioIntStr(long numFrom, long numTo)
        {
            double ratio = Math.Round(((float)numFrom / (float)numTo) * 100f);
            return ratio + "%";
        }

        public static string ConcatStrings(string[] strings, char delimiter = ',', bool distinct = false)
        {
            string outStr = "";

            strings = strings.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            if (distinct)
                strings = strings.Distinct().ToArray();

            for (int i = 0; i < strings.Length; i++)
            {
                outStr += strings[i];
                if (i + 1 != strings.Length)
                    outStr += delimiter;
            }

            return outStr;
        }

        public static string CapsIfShort(string codec, int capsIfShorterThan = 5)
        {
            if (codec.Length < capsIfShorterThan)
                return codec.ToUpper();
            else
                return codec.ToTitleCase();
        }

        public static string SanitizePromptFilename(string prompt, int pathBudget = 64)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return "";

            prompt = prompt.Trim().Replace(" ", "_"); // Replace all spaces by underscores...
            return new Regex(@"[^a-zA-Z0-9 ._]").Replace(prompt, "").Trunc(pathBudget - 1, false); // ...remove special chars
        }

        public static string GetPromptWithoutModifiers(string prompt)
        {
            List<char> final = new List<char>();
            string noBrackets = Regex.Replace(prompt, @"(\[(?:\[??[^\[]*?\]))", "").Remove("[").Remove("]"); // Remove square brackets with contents

            bool ignore = false;

            foreach (char c in noBrackets)
            {
                if (c == ':')
                    ignore = true; // Ignore this characater and any that follow up...

                if (c == ' ' && ignore)
                    ignore = false; // ...until there is a space

                if (!ignore)
                    final.Add(c);
            }

            return string.Join("", final).Trim();
        }

        public static int IterationsToMsPerIteration(string s)
        {
            bool its = s.EndsWith("it/s");

            if (its) // iterations per second
                return (1000000f / (s.Remove(".").Remove("it/s") + "0").GetInt()).RoundToInt();
            else // seconds per iteration
                return (s.Remove(".").Remove("it/s") + "0").GetInt();
        }

        public static string ConvertTextEncoding(string s, Encoding sourceEncoding = null, Encoding targetEncoding = null)
        {
            sourceEncoding = null ?? Encoding.UTF8;
            targetEncoding = null ?? Encoding.Default; // ANSI

            string output = targetEncoding.GetString(Encoding.Convert(sourceEncoding, targetEncoding, sourceEncoding.GetBytes(s)));

            return output;
        }

        public static bool StringContainsNonAsciiChars(string str)
        {
            return GetNonAsciiChars(str).Count > 0;
        }

        public static List<char> GetNonAsciiChars (string originalString)
        {
            string asciiString = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(originalString));
            return originalString.ToList().Where(c => !asciiString.Contains(c)).ToList();
        }

        public static long GetUnixTime()
        {
            return ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds);
        }

        public static string GetUnixTimestamp ()
        {
            return GetUnixTime().ToString();
        }

        public static string GetEmbeddingFormat (Implementation imp)
        {
            if (imp == Implementation.InvokeAi)
                return new InvokeAi().GetEmbeddingStringFormat();
            else if (imp == Implementation.Comfy)
                return new Comfy().GetEmbeddingStringFormat();

            return "";
        }

        private static readonly Regex _multiSpacePattern = new Regex(@"\s+", RegexOptions.Compiled);

        public static string SanitizePrompt(string prompt, Implementation imp = (Implementation)(-1))
        {
            if (imp == (Implementation)(-1))
                imp = Config.Instance.Implementation;

            prompt = prompt.Remove("\""); // Don't allow "
            prompt = _multiSpacePattern.Replace(prompt, " "); // Don't allow multiple consecutive spaces

            if (imp == Implementation.InvokeAi)
                prompt = InvokeAiUtils.ConvertAttentionSyntax(prompt); // Convert old (multi-bracket) emphasis/attention syntax to new one (with +/-)
            if (imp == Implementation.InvokeAi)
                prompt = ComfyUtils.SanitizePrompt(prompt); // Convert Invoke embbedding syntax to Comfy (Does not convert attention syntax currently)

            return prompt;
        }

        public static string NormalizePath (string path)
        {
            return path.Replace("/", "\\").Trim().TrimEnd('\\');
        }

        public static string PrintValues<T>(T[] objs, string format = "{0}", string floatFormat = "0.###")
        {
            if (objs == null || objs.Length == 0)
                return "";

            bool isFloatNum = typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal);
            string firstVal = isFloatNum ? firstVal = string.Format(format, string.Format("{0:" + floatFormat + "}", objs[0])) : string.Format(format, objs[0]);
            return objs.Length == 1 ? firstVal : $"{firstVal}...";
        }
    }
}
