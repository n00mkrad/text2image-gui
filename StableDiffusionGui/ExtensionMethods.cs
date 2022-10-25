using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using Newtonsoft.Json;
using StableDiffusionGui.Main;

namespace StableDiffusionGui
{
    public static class ExtensionMethods
    {
        public static string TrimNumbers(this string s, bool allowDotComma = false, bool allowScientific = false)
        {
            if (!allowDotComma)
                s = Regex.Replace(s, $"[^0-9{(allowScientific ? "e" : "")}]", "");
            else
                s = Regex.Replace(s, $"[^.,0-9{(allowScientific ? "e" : "")}]", "");

            return s.Trim();
        }

        public static int GetInt(this TextBox textbox)
        {
            return GetInt(textbox.Text);
        }

        public static int GetInt(this ComboBox combobox)
        {
            return GetInt(combobox.Text);
        }

        public static int GetInt(this string str, bool allowScientificNotation = true)
        {
            if (str == null || str.Length < 1)
                return 0;

            str = str.Trim();

            try
            {
                if(allowScientificNotation && CouldBeScientificNotation(str))
                    return int.Parse(str.TrimNumbers(true, true), NumberStyles.Float, CultureInfo.InvariantCulture);

                if (str.Length >= 2 && str[0] == '-' && str[1] != '-')
                    return int.Parse("-" + str.TrimNumbers());
                else
                    return int.Parse(str.TrimNumbers());
            }
            catch
            {
                return 0;
            }
        }

        private static bool CouldBeScientificNotation (string s)
        {
            if (!(s.ToLowerInvariant().Contains("e+") || s.ToLowerInvariant().Contains("e-")))
                return false;

            if (s[0] == 'e' || s.Last() == '+' || s.Last() == '-') // e must be in the middle, can't be first char (and +- can't be last)
                return false;

            return true;
        }

        public static long GetLong(this string str)
        {
            if (str == null || str.Length < 1)
                return 0;

            str = str.Trim();

            try
            {
                if (str.Length >= 2 && str[0] == '-' && str[1] != '-')
                    return long.Parse("-" + str.TrimNumbers());
                else
                    return long.Parse(str.TrimNumbers());
            }
            catch
            {
                return 0;
            }
        }

        public static bool GetBool(this string str)
        {
            try
            {
                return bool.Parse(str);
            }
            catch
            {
                return false;
            }
        }

        public static float GetFloat(this TextBox textbox)
        {
            return GetFloat(textbox.Text);
        }

        public static float GetFloat(this ComboBox combobox)
        {
            return GetFloat(combobox.Text);
        }

        public static float GetFloat(this string str)
        {
            if (str.Length < 1 || str == null)
                return 0f;

            string num = str.TrimNumbers(true).Replace(",", ".");
            float value;
            float.TryParse(num, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
            return value;
        }

        public static string Wrap(this string path, bool backslashToSlash = false, bool addSpaceFront = false, bool addSpaceEnd = false)
        {
            string s = "\"" + path + "\"";

            if (addSpaceFront)
                s = " " + s;

            if (addSpaceEnd)
                s = s + " ";

            if (backslashToSlash)
                s = s.Replace(@"\", "/");

            return s;
        }

        public static string GetParentDirOfFile(this string path)
        {
            try
            {
                return new FileInfo(path).Directory.FullName;
            }
            catch (Exception ex)
            {
                return path;
            }
        }

        public static int RoundToInt(this float f)
        {
            return (int)Math.Round(f);
        }

        public static int RoundToInt(this double d)
        {
            return (int)Math.Round(d);
        }

        public static long RoundToLong(this double d)
        {
            return (long)Math.Round(d);
        }

        public static int Clamp(this int i, int min, int max)
        {
            if (i < min)
                i = min;

            if (i > max)
                i = max;

            return i;
        }

        public static float Clamp(this float i, float min, float max)
        {
            if (i < min)
                i = min;

            if (i > max)
                i = max;

            return i;
        }

        public static long Clamp(this long i, long min, long max)
        {
            if (i < min) i = min;
            if (i > max) i = max;
            return i;
        }

        public static string[] SplitIntoLines(this string str)
        {
            return Regex.Split(str, "\r\n|\r|\n");
        }

        public static string Trunc(this string inStr, int maxChars, bool addEllipsis = true)
        {
            if (string.IsNullOrWhiteSpace(inStr))
                return "";

            string str = inStr.Length <= maxChars ? inStr : inStr.Substring(0, maxChars);

            if (addEllipsis && inStr.Length > maxChars)
                str += "…";

            return str;
        }

        public static string StripBadChars(this string str)
        {
            string outStr = Regex.Replace(str, @"[^\u0020-\u007E]", string.Empty);
            outStr = outStr.Remove("(").Remove(")").Remove("[").Remove("]").Remove("{").Remove("}").Remove("%").Remove("'").Remove("~");
            return outStr;
        }

        public static string StripNumbers(this string str)
        {
            return new string(str.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
        }

        public static string Remove(this string str, string stringToRemove)
        {
            if (str == null || stringToRemove == null)
                return str;

            return str.Replace(stringToRemove, "");
        }

        public static string TrimWhitespaces(this string str)
        {
            if (str == null) return str;
            var newString = new StringBuilder();
            bool previousIsWhitespace = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsWhiteSpace(str[i]))
                {
                    if (previousIsWhitespace)
                        continue;
                    previousIsWhitespace = true;
                }
                else
                {
                    previousIsWhitespace = false;
                }
                newString.Append(str[i]);
            }
            return newString.ToString();
        }

        public static string ReplaceLast(this string str, string stringToReplace, string replaceWith)
        {
            int place = str.LastIndexOf(stringToReplace);

            if (place == -1)
                return str;

            return str.Remove(place, stringToReplace.Length).Insert(place, replaceWith);
        }

        public static string[] SplitBy(this string str, string splitBy)
        {
            return str.Split(new string[] { splitBy }, StringSplitOptions.None);
        }

        public static string RemoveComments(this string str)
        {
            return str.Split('#')[0].SplitBy("//")[0];
        }

        public static string FilenameSuffix(this string path, string suffix)
        {
            string filename = Path.ChangeExtension(path, null);
            string ext = Path.GetExtension(path);
            return filename + suffix + ext;
        }

        public static string ToStringDot(this float f, string format = "")
        {
            if (string.IsNullOrWhiteSpace(format))
                return f.ToString().Replace(",", ".");
            else
                return f.ToString(format).Replace(",", ".");
        }

        public static string[] Split(this string str, string trimStr)
        {
            return str.Split(new string[] { trimStr }, StringSplitOptions.None);
        }

        public static bool MatchesWildcard(this string str, string wildcard)
        {
            WildcardPattern pattern = new WildcardPattern(wildcard);
            return pattern.IsMatch(str);
        }

        public static bool MatchesRegex(this string str, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(str);
        }

        public static int RoundMod(this int n, int mod = 2)     // Round to a number that's divisible by 2 (for h264 etc)
        {
            int a = (n / 2) * 2;    // Smaller multiple
            int b = a + 2;   // Larger multiple
            return (n - a > b - n) ? b : a; // Return of closest of two
        }

        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        public static string ToStringShort(this Size s, string separator = "x")
        {
            return $"{s.Width}{separator}{s.Height}";
        }

        public static bool IsConcatFile(this string filePath)
        {
            try
            {
                return Path.GetExtension(filePath)?.Lower() == ".concat";
            }
            catch
            {
                return false;
            }
        }

        public static string GetConcStr(this string filePath, int rate = -1)
        {
            string rateStr = rate >= 0 ? $"-r {rate} " : "";
            return filePath.IsConcatFile() ? $"{rateStr}-safe 0 -f concat " : "";
        }

        public static string GetFfmpegInputArg(this string filePath)
        {
            return $"{(filePath.IsConcatFile() ? filePath.GetConcStr() : "")} -i {filePath.Wrap()}";
        }

        public static int CountOccurences(this List<string> list, string stringToLookFor)
        {
            int count = 0;

            foreach (string s in list)
                if (s == stringToLookFor)
                    count++;

            return count;
        }

        public static string CleanString(this string str)
        {
            try
            {
                return Regex.Replace(str.Trim(), @"[^\w\._-]", "", RegexOptions.None, TimeSpan.FromSeconds(1));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static string Get(this Dictionary<string, string> dict, string key, bool returnKeyInsteadOfEmptyString = false, bool ignoreCase = false)
        {
            for(int i = 0; i < dict.Count; i++)
            {
                if (ignoreCase)
                {
                    if(key.Lower() == dict.ElementAt(i).Key.Lower())
                        return dict.ElementAt(i).Value;
                }
                else
                {
                    if (key == dict.ElementAt(i).Key)
                        return dict.ElementAt(i).Value;
                }
            }

            if (returnKeyInsteadOfEmptyString)
                return key;
            else
                return "";
        }

        public static void FillFromEnum<TEnum>(this ComboBox comboBox, Dictionary<string, string> stringMap = null, int defaultIndex = -1) where TEnum : Enum
        {
            if (stringMap == null)
                stringMap = new Dictionary<string, string>();

            comboBox.Items.Clear();
            comboBox.Items.AddRange(Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(x => stringMap.Get(x.ToString(), true)).ToArray());

            if (defaultIndex >= 0)
                comboBox.SelectedIndex = defaultIndex;
        }

        public static void SetIfTextMatches(this ComboBox comboBox, string str, bool ignoreCase = true, Dictionary<string, string> stringMap = null)
        {
            if (stringMap == null)
                stringMap = new Dictionary<string, string>();

            str = stringMap.Get(str, true, true);
            
            for(int i = 0; i < comboBox.Items.Count; i++)
            {
                if (ignoreCase)
                {
                    if(comboBox.Items[i].ToString().Lower() == str.Lower())
                    {
                        comboBox.SelectedIndex = i;
                        return;
                    }
                }
                else
                {
                    if (comboBox.Items[i].ToString() == str)
                    {
                        comboBox.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        public static Dictionary<V, K> SwapKeysValues<V, K> (this Dictionary<K, V> dict)
        {
            Dictionary<V, K> result = new Dictionary<V, K>();

            foreach(var pair in dict)
            {
                if(!result.ContainsKey(pair.Value))
                    result.Add(pair.Value, pair.Key);
            }

            return result;
        }

        public static string Lower (this string s)
        {
            if (s == null)
                return s;

            return s.ToLowerInvariant();
        }

        public static T FromJson<T> (this string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch(Exception ex)
            {
                Logger.Log($"Failed to deserialize ({ex.Message}):\n{s.Trunc(1000)}", true);
                return default(T);
            }
        }

        public static string ToJson(this object o, Formatting format = Formatting.None)
        {
            return JsonConvert.SerializeObject(o, format);
        }
    }
}
