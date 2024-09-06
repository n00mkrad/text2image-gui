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
using ZetaLongPaths;
using StableDiffusionGui.Data;
using Newtonsoft.Json.Converters;
using System.Collections.Concurrent;
using System.Reflection;
using StableDiffusionGui.Extensions;
using Newtonsoft.Json.Serialization;

namespace StableDiffusionGui
{
    public static class ExtensionMethods
    {
        public static string TrimNumbers(this string s, bool allowDotComma = false, bool allowScientific = false)
        {
            if (!allowDotComma)
                s = Regex.Replace(s, $"[^0-9.{(allowScientific ? "e" : "")}]", "");
            else
                s = Regex.Replace(s, $"[^.,0-9-{(allowScientific ? "e" : "")}]", "");

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
                if (allowScientificNotation && CouldBeScientificNotation(str))
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

        private static bool CouldBeScientificNotation(string s)
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
                if (string.IsNullOrWhiteSpace(str))
                    return false;

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
                s += " ";

            if (backslashToSlash)
                s = s.Replace(@"\", "/");

            return s;
        }

        public static string GetParentDirOfFile(this string path)
        {
            try
            {
                return new ZlpFileInfo(path).Directory.FullName;
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

        public static string FilenameSuffix(this string path, string suffix)
        {
            string filename = Path.ChangeExtension(path, null);
            string ext = Path.GetExtension(path);
            return filename + suffix + ext;
        }

        public static string ToStringDot(this float f, string format = "0.######")
        {
            if (string.IsNullOrWhiteSpace(format))
                return f.ToString().Replace(",", ".");
            else
                return f.ToString(format).Replace(",", ".");
        }

        public static string[] Split(this string str, string trimStr)
        {
            if (str == null)
                return new string[0];

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

        public static int RoundMod(this int n, int mod = 2)     // Round to a number that's divisible by mod
        {
            int a = (n / mod) * mod;    // Smaller multiple
            int b = a + mod;   // Larger multiple
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
                return string.Empty;
            }
        }

        public static string GetReverse(this Dictionary<string, string> dict, string key, bool returnKeyInsteadOfEmptyString = false, bool ignoreCase = false)
        {
            var reversedDict = dict.ToDictionary(pair => pair.Value, pair => pair.Key);
            return reversedDict.Get(key, returnKeyInsteadOfEmptyString, ignoreCase);
        }

        public static string Get(this Dictionary<string, string> dict, string key, bool returnKeyInsteadOfEmptyString = false, bool ignoreCase = false)
        {
            if (key == null)
                key = "";

            for (int i = 0; i < dict.Count; i++)
            {
                if (ignoreCase)
                {
                    if (key.Lower() == dict.ElementAt(i).Key.Lower())
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

        public static void FillFromEnum<TEnum>(this ComboBox comboBox, Dictionary<string, string> stringMap = null, int defaultIndex = -1, IEnumerable<TEnum> exclusionList = null) where TEnum : Enum
        {
            if (stringMap == null)
                stringMap = new Dictionary<string, string>();

            if (exclusionList == null)
                exclusionList = new List<TEnum>();

            comboBox.Items.Clear();
            var entriesToAdd = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Except(exclusionList);
            comboBox.Items.AddRange(entriesToAdd.Select(x => stringMap.Get(x.ToString(), true)).ToArray());

            if (defaultIndex >= 0)
                comboBox.SelectedIndex = defaultIndex;
        }

        public static void SetWithEnum<TEnum>(this ComboBox comboBox, TEnum e, bool ignoreCase = true, Dictionary<string, string> stringMap = null)
        {
            comboBox.SetWithText(e.ToString(), ignoreCase, stringMap);
        }

        public static void SetWithText(this ComboBox comboBox, string str, bool ignoreCase = true, Dictionary<string, string> stringMap = null)
        {
            if (comboBox.RequiresInvoke(new Action<ComboBox, string, bool, Dictionary<string,string>>(SetWithText), comboBox, str, ignoreCase, stringMap))
                return;

            if (stringMap == null)
                stringMap = new Dictionary<string, string>();

            str = stringMap.Get(str, true, true);

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (ignoreCase)
                {
                    if (comboBox.Items[i].ToString().Lower() == str.Lower())
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

        public static void SafeInvoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static string GetTextSafe(this Control control)
        {
            if (control.InvokeRequired)
            {
                return (string)control.Invoke(new Func<string>(() => control.Text));
            }
            else
            {
                return control.Text;
            }
        }

        public static EasyDict<V, K> SwapKeysValues<V, K>(this EasyDict<K, V> dict)
        {
            EasyDict<V, K> result = new EasyDict<V, K>();

            foreach (var pair in dict)
            {
                if (!result.ContainsKey(pair.Value))
                    result.Add(pair.Value, pair.Key);
            }

            return result;
        }

        public static string Lower(this string s)
        {
            if (s == null)
                return s;

            return s.ToLowerInvariant();
        }

        public static T FromJson<T>(this Dictionary<string, string> jsonDict, string key, T fallbackValue = default(T))
        {
            if (jsonDict.ContainsKey(key))
            {
                string valueStr = jsonDict.Get(key);

                if (valueStr.IsEmpty())
                    return fallbackValue;

                var value = valueStr.FromJson<T>();
                return value != null ? value : fallbackValue;
            }

            return fallbackValue;
        }

        public static T FromJson<T>(this string s)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                    return default(T);

                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to deserialize ({ex.Message}): \n'{s.Trunc(1000)}'", true);
                if (Program.Debug) Logger.Log(ex.StackTrace, true);
                return default(T);
            }
        }

        public static T FromJson<T>(this string s, NullValueHandling nullHandling, DefaultValueHandling defHandling, bool useTolerantEnumConv, bool useNullToEmptyStringConv, IContractResolver contractResolver = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                    return default(T);

                var settings = new JsonSerializerSettings();

                if (useTolerantEnumConv)
                    settings.Converters.Add(new Serialization.JsonUtils.TolerantEnumConverter()); // Fallback to first enum entry instead of throwing an error

                if (useNullToEmptyStringConv)
                    settings.Converters.Add(new Serialization.JsonUtils.NullToEmptyStringConverter()); // Deserialize null as empty string instead of null

                settings.NullValueHandling = nullHandling;
                settings.DefaultValueHandling = defHandling;

                if(contractResolver != null)
                    settings.ContractResolver = contractResolver;

                return JsonConvert.DeserializeObject<T>(s, settings);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to deserialize ({ex.Message}): \n'{s.Trunc(1000)}'", true);
                if (Program.Debug) Logger.Log(ex.StackTrace, true);
                return default(T);
            }
        }

        public static string ToJson(this object o, bool indent = false, bool ignoreErrors = true)
        {
            var settings = new JsonSerializerSettings();

            if (ignoreErrors)
                settings.Error = (s, e) => { e.ErrorContext.Handled = true; };

            // Serialize enums as strings.
            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(o, indent ? Formatting.Indented : Formatting.None, settings);
        }

        public static string NullToEmpty(this string s)
        {
            if (s == null)
                return "";
            else
                return s;
        }

        public static string GetNameNoExt(this FileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.Name);
        }

        public static string NameNoExt(this ZlpFileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.Name);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        
        // Prettier ToString alternative
        public static string AsString(this Size s, string delimiter = "x", bool swapWidthHeight = false, bool returnEmptyIf0x0 = false)
        {
            if(s == null)
                return returnEmptyIf0x0 ? "" : "0x0";

            if (returnEmptyIf0x0 && s.IsEmpty)
                return "";

            if (swapWidthHeight)
                return $"{s.Height}{delimiter}{s.Width}";
            else
                return $"{s.Width}{delimiter}{s.Height}";
        }

        // Prettier ToString alternative
        public static string AsString(this System.Drawing.Imaging.PixelFormat pixFmt)
        {
            return pixFmt.ToString().Replace("Format", "").Replace("bpp", "-bit ").Replace("PArgb", "Pre-multiplied ARGB ").Replace("Argb", "ARGB ").Replace("Rgb", "RGB ");
        }

        /// <summary> Shortcut for !string.IsNullOrWhiteSpace </summary>
        /// <returns> If the string is NOT null or whitespace </returns>
        public static bool IsNotEmpty(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary> Shortcut for string.IsNullOrWhiteSpace </summary>
        /// <returns> If the string is null or whitespace </returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static ImplementationInfo GetInfo(this Enums.StableDiffusion.Implementation imp)
        {
            return new ImplementationInfo(imp);
        }

        public static bool Supports(this Enums.StableDiffusion.Implementation imp, ImplementationInfo.Feature feature)
        {
            return new ImplementationInfo(imp).SupportedFeatures.Contains(feature);
        }

        public static bool IsUnset<TEnum>(this TEnum myEnum)
        {
            return myEnum.Equals(Enum.ToObject(typeof(TEnum), -1));
        }

        public static void RunInTryCatch(this Action action, string errorPrefix = "", string guiErrorPrefix = "")
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (guiErrorPrefix.IsNotEmpty())
                    Logger.Log($"{guiErrorPrefix}: {ex.Message}");

                Logger.LogException(ex, true, errorPrefix.IsNotEmpty() ? $"{errorPrefix} " : "");
            }
        }

        public static T[] AsArray<T>(this T obj)
        {
            return new[] { obj };
        }

        public static List<T> AsList<T> (this T obj)
        {
            return obj.AsArray().ToList();
        }

        public static void Clear<T> (this ConcurrentQueue<T> queue)
        {
            while (queue.TryDequeue(out _))
            {
                // Do nothing, just keep dequeuing until the queue is empty
            }
        }

        public static int CalculateReadTimeMs(this string text, float humanReadSpeedInWpm = 200.0f)
        {
            int wordCount = text.Trim().Split(' ').Length;
            double minutes = wordCount / humanReadSpeedInWpm;
            int milliseconds = (int)(minutes * 60000);
            return milliseconds;
        }

        public static string Append (this string s, string text, bool comma)
        {
            if (comma && !s.TrimEnd().EndsWith(",") && s.IsNotEmpty())
                return $"{s.TrimEnd()}, {text}";

            return $"{s.TrimEnd()} {text}";
        }

        public static string AsString<T> (this IEnumerable<T> list, string delimiter = ", ")
        {
            return string.Join(delimiter, list);
        }

        public static bool Contains(this string s, string toCheck, StringComparison comp)
        {
            return s?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool IsDisposed(this Image image)
        {
            try
            {
                if(image == null)
                    return true;

                return image.PixelFormat == System.Drawing.Imaging.PixelFormat.DontCare;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
        }

        public static List<KeyValuePair<string, object>> GetPublicVariableValues (this object obj, bool basic = true)
        {
            List<KeyValuePair<string, object>> variables = new List<KeyValuePair<string, object>>();

            // Retrieve all public instance fields
            var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var s = basic && !field.FieldType.IsPrimitive ? (object)(field.GetValue(obj) != null ? "..." : "null") : field.GetValue(obj);
                variables.Add(new KeyValuePair<string, object>(field.Name, s));
            }

            // Retrieve all public instance properties
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);  // Ensure property can be read
            foreach (var property in properties)
            {
                var s = basic && !property.PropertyType.IsPrimitive ? (object)(property.GetValue(obj) != null ? "..." : "null") : property.GetValue(obj);
                variables.Add(new KeyValuePair<string, object>(property.Name, s));
            }

            return variables;
        }

        public static string GetPublicVariablesString(this object obj, bool includeClassName = true)
        {
            var variables = obj.GetPublicVariableValues();
            string variablesString = string.Join(", ", variables.Select(v => $"{v.Key}: {v.Value}"));

            if (includeClassName)
                return $"{obj.GetType().Name}: {variablesString}";
            else
                return variablesString;
        }
    }
}
