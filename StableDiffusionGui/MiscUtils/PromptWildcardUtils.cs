using Microsoft.VisualBasic;
using StableDiffusionGui.Main;
using StableDiffusionGui.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Constants = StableDiffusionGui.Main.Constants;
using Path = System.IO.Path;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.MiscUtils
{
    internal class PromptWildcardUtils
    {
        private static Random _random = new Random();
        private static Dictionary<string, List<string>> _cachedWildcardLists = new Dictionary<string, List<string>>(); // Cache until reset, to avoid re-shuffling
        private static Dictionary<string, int> _wildcardIndex = new Dictionary<string, int>(); // Store current index for each wildcard file

        private static readonly char _identifier = '~';


        public static string ApplyWildcards(string prompt, int iterations)
        {
            bool verboseLog = iterations == 1 || Debugger.IsAttached;

            try
            {
                string[] split = prompt.Split(_identifier);
                int[] identifierIndexes = prompt.Select((b, i) => b.Equals(_identifier) ? i : -1).Where(i => i != -1).ToArray();
                int wildcardIndex = 0;

                for (int i = 1; i < split.Length; i++) // Start at 1 because we never need the first entry as it can't be a wildcard
                {
                    SortMode sort = SortMode.Shuffle;
                    int identifierIndex = identifierIndexes[i - 1];

                    if ((identifierIndex + 1) >= prompt.Length)
                        break;

                    if ((identifierIndex + 1) <= prompt.Length && prompt[identifierIndex + 1] == _identifier)
                    {
                        sort = SortMode.Sequential;
                        i++;

                        if ((identifierIndex + 2) <= prompt.Length && prompt[identifierIndex + 2] == _identifier)
                        {
                            sort = SortMode.Alphabetically;
                            i++;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(split[i]))
                            continue;
                    }

                    string regex = @"(?i)[^a-z-0-9]"; // A-Z, case insensitive, numbers
                    var wordSplit = Regex.Split(split[i], regex);

                    bool inline = false;
                    string rest = "";
                    var breakingDelimMatches = Regex.Matches(split[i], regex).Cast<Match>(); // If this has 0 matches, we are at the end of the string, see below...

                    if (breakingDelimMatches.Count() > 0)
                    {
                        string breakingDelimiter = breakingDelimMatches.Count() > 0 ? breakingDelimMatches.First().Value : ""; // First char that no longer belongs to wildcard name
                        rest = breakingDelimiter + string.Join(breakingDelimiter, split[i].Split(breakingDelimiter).Skip(1));
                        inline = split[i].Contains(",") && !split[i].Contains(" ");
                    }

                    string wildcardName = inline ? split[i] : wordSplit.FirstOrDefault().Trim();

                    if (!string.IsNullOrWhiteSpace(wildcardName))
                        continue;

                    if (inline)
                    {
                        var list = GetWildcardListFromString(wildcardIndex, wildcardName, iterations, sort);
                        string replacement = list.ElementAt(GetWildcardValueIndex(wildcardIndex, wildcardName, true)).Replace(".", " ");
                        split[i] = replacement; // Pick random phrase, insert back into word array
                        if (verboseLog) Logger.Log($"Wildcard '{wildcardName}' => '{replacement}' ({sort})", true);
                    }
                    else
                    {
                        string wildcardPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Wildcards, wildcardName + ".txt");

                        if (File.Exists(wildcardPath))
                        {
                            var list = GetWildcardListFromFile(wildcardIndex, wildcardPath, iterations, sort);
                            string replacement = list.ElementAt(GetWildcardValueIndex(wildcardIndex, wildcardName, true));
                            split[i] = replacement + rest; // Pick random line, insert back into word array
                            if (verboseLog) Logger.Log($"Wildcard '{wildcardName}' => '{replacement}' ({sort})", true);
                        }
                        else
                        {
                            Logger.Log($"No wildcard file found for '{wildcardName}'", true);
                        }
                    }

                    wildcardIndex++;
                }

                return string.Join("", split);
            }
            catch (Exception ex)
            {
                Logger.Log($"Wildcard Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                return prompt;
            }
        }

        public static List<string> ApplyWildcardsAll(string prompt, int iterations, bool runAllCombinations)
        {
            bool verboseLog = iterations == 1 || Debugger.IsAttached;
            List<string> outList = new List<string>();

            try
            {
                string[] split = prompt.Split(_identifier);
                int[] identifierIndexes = prompt.Select((b, i) => b.Equals(_identifier) ? i : -1).Where(i => i != -1).ToArray();
                int wildcardIndex = 0;

                Dictionary<int, List<string>> lists = new Dictionary<int, List<string>>();
                Dictionary<int, int> indexes = new Dictionary<int, int>();

                for (int i = 1; i < split.Length; i++) // Start at 1 because we never need the first entry as it can't be a wildcard
                {
                    SortMode sort = SortMode.Shuffle;
                    int identifierIndex = identifierIndexes[i - 1];

                    if ((identifierIndex + 1) >= prompt.Length)
                        break;

                    if ((identifierIndex + 1) <= prompt.Length && prompt[identifierIndex + 1] == _identifier)
                    {
                        sort = SortMode.Sequential;
                        i++;

                        if ((identifierIndex + 2) <= prompt.Length && prompt[identifierIndex + 2] == _identifier)
                        {
                            sort = SortMode.Alphabetically;
                            i++;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(split[i]))
                            continue;
                    }

                    string regex = @"(?i)[^a-z-0-9]"; // A-Z, case insensitive, numbers
                    var wordSplit = Regex.Split(split[i], regex);

                    bool inline = false;
                    string rest = "";
                    var breakingDelimMatches = Regex.Matches(split[i], regex).Cast<Match>(); // If this has 0 matches, we are at the end of the string, see below...

                    if (breakingDelimMatches.Count() > 0)
                    {
                        string breakingDelimiter = breakingDelimMatches.Count() > 0 ? breakingDelimMatches.First().Value : ""; // First char that no longer belongs to wildcard name
                        rest = breakingDelimiter + string.Join(breakingDelimiter, split[i].Split(breakingDelimiter).Skip(1));
                        inline = split[i].Contains(",") && !split[i].Contains(" ");
                    }

                    string wildcardName = inline ? split[i] : wordSplit.FirstOrDefault().Trim();

                    if (string.IsNullOrWhiteSpace(wildcardName))
                        continue;

                    if (inline)
                    {
                        var list = GetWildcardListFromString(wildcardIndex, wildcardName, runAllCombinations ? -1 : iterations, sort);
                        lists[wildcardIndex] = list;
                        //string replacement = list.ElementAt(GetWildcardValueIndex(wildcardIndex, wildcardName, true)).Replace(".", " ");
                        string replacement = $">>>{wildcardIndex}<<<";
                        split[i] = replacement; // Pick random phrase, insert back into word array
                        if (verboseLog) Logger.Log($"Wildcard '{wildcardName}' => '{replacement}' ({sort})", true);
                    }
                    else
                    {
                        string wildcardPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Wildcards, wildcardName + ".txt");

                        if (File.Exists(wildcardPath))
                        {
                            var list = GetWildcardListFromFile(wildcardIndex, wildcardPath, runAllCombinations ? -1 : iterations, sort);
                            lists[wildcardIndex] = list;
                            //string replacement = list.ElementAt(GetWildcardValueIndex(wildcardIndex, wildcardName, true));
                            string replacement = $">>>{wildcardIndex}<<<";
                            split[i] = replacement + rest; // Pick random line, insert back into word array
                            if (verboseLog) Logger.Log($"Wildcard '{wildcardName}' => '{replacement}' ({sort})", true);
                        }
                        else
                        {
                            Logger.Log($"No wildcard file found for '{wildcardName}'", true);
                        }
                    }

                    wildcardIndex++;
                }

                prompt = string.Join("", split);
                indexes = lists.ToDictionary(x => x.Key, x => 0);

                if (runAllCombinations)
                {
                    IEnumerable<IEnumerable<string>> combos = new string[][] { new string[0] };
                    foreach (var inner in lists.Select(x => x.Value)) combos = from c in combos from i in inner select c.Append(i);

                    Logger.Log($"Possible combinations: {string.Join(" * ", lists.Select(x => x.Value.Count().ToString()))} = {combos.Count()}");

                    for(int comboIdx = 0; comboIdx < combos.Count(); comboIdx++)
                    {
                        string processedPrompt = prompt;

                        for (int listIdx = 0; listIdx < lists.Count; listIdx++)
                            processedPrompt = processedPrompt.Replace($">>>{listIdx}<<<", combos.ElementAt(comboIdx).ElementAt(listIdx));
                        
                        outList.Add(processedPrompt);
                    }
                }
                else
                {
                    for (int i = 0; i < iterations; i++)
                    {
                        string processedPrompt = prompt;

                        for (int listIdx = 0; listIdx < lists.Count; listIdx++)
                        {
                            processedPrompt = processedPrompt.Replace($">>>{listIdx}<<<", lists[listIdx][indexes[listIdx]]);
                            indexes[listIdx] += 1;
                        }

                        outList.Add(processedPrompt);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Wildcard Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }

            return outList;
        }

        private enum SortMode { Shuffle, Sequential, Alphabetically }

        private static List<string> GetWildcardListFromFile(int index, string wildcardPath, int listSize, SortMode sortMode)
        {
            string id = $"{index}-file-{wildcardPath}-{sortMode}"; // Cache per index & sort mode
            return GetWildcardList(id, File.ReadAllLines(wildcardPath).Where(line => !string.IsNullOrWhiteSpace(line)), listSize, sortMode);
        }

        private static List<string> GetWildcardListFromString(int index, string wildcardStr, int listSize, SortMode sortMode)
        {
            string id = $"{index}-string-{wildcardStr}-{sortMode}"; // Cache per index & sort mode
            List<string> split = wildcardStr.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            return GetWildcardList(id, split, listSize, sortMode);
        }

        private static List<string> GetWildcardList(string id, IEnumerable<string> lines, int listSize, SortMode sortMode)
        {
            if (_cachedWildcardLists.ContainsKey(id))
                return _cachedWildcardLists[id];

            if (lines.Count() <= 0)
                return new List<string>();

            var linesSrc = lines.Where(line => !string.IsNullOrWhiteSpace(line)); // Read all lines from wildcard file

            if (sortMode == SortMode.Alphabetically)
                linesSrc = linesSrc.OrderBy(a => a).ToList(); // Sort list optionally
            if (sortMode == SortMode.Shuffle)
                linesSrc = linesSrc.OrderBy(a => _random.Next()).ToList(); // Shuffle list optionally

            List<string> list = new List<string>(linesSrc);

            while (listSize > 0 && list.Count < listSize) // Clone list until it's longer than the desired size (will not run at all if it's already long enough)
                list.AddRange(linesSrc);

            if (listSize > 0)
                list = list.Take(listSize).ToList(); // Trim to the exact desired size

            _cachedWildcardLists[id] = list; // Cache until reset
            return list;
        }

        private static int GetWildcardValueIndex(int keyIndex, string wildcard, bool increment)
        {
            string key = $"{keyIndex}_{wildcard}";

            if (!_wildcardIndex.ContainsKey(key))
                _wildcardIndex[key] = 0;

            int valueIndex = _wildcardIndex[key];

            if (increment)
                _wildcardIndex[key] = _wildcardIndex[key] + 1;

            return valueIndex;
        }

        /// <summary> Resets randomization and saved indexes </summary>
        public static void Reset()
        {
            _cachedWildcardLists.Clear();
            _wildcardIndex.Clear();
        }
    }
}
