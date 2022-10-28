using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            try
            {
                string[] split = prompt.Split(_identifier);
                int[] identifierIndexes = prompt.Select((b, i) => b.Equals(_identifier) ? i : -1).Where(i => i != -1).ToArray();

                for (int i = 1; i < split.Length; i++) // Start at 1 because we never need the first entry as it can't be a wildcard
                {
                    SortMode sort = SortMode.Shuffle;
                    int identifierIndex = identifierIndexes[i - 1];

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

                    string rest = "";
                    var breakingDelimMatches = Regex.Matches(split[i], regex).Cast<Match>(); // If this has 0 matches, we are at the end of the string, see below...

                    if (breakingDelimMatches.Count() > 0)
                    {
                        string breakingDelimiter = breakingDelimMatches.Count() > 0 ? breakingDelimMatches.First().Value : ""; // First char that no longer belongs to wildcard name
                        rest = breakingDelimiter + string.Join(breakingDelimiter, split[i].Split(breakingDelimiter).Skip(1));
                    }

                    string wildcardName = wordSplit.FirstOrDefault().Trim();
                    string wildcardPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Wildcards, wildcardName + ".txt");

                    if (File.Exists(wildcardPath))
                    {
                        var list = GetWildcardList(wildcardPath, iterations, sort);
                        string replacement = list.ElementAt(GetWildcardIndex(wildcardName, true));
                        split[i] = replacement + rest; // Pick random line, insert back into word array
                        Logger.Log($"Filled wildcard '{wildcardName}' with '{replacement}' (Order: {sort})", true);
                    }
                    else
                    {
                        Logger.Log($"No wildcard file found for '{wildcardName}'", true);
                    }
                }

                return string.Join("", split);
            }
            catch(Exception ex)
            {
                Logger.Log($"Wildcard Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                return prompt;
            }
        }

        private enum SortMode { Shuffle, Sequential, Alphabetically }

        private static List<string> GetWildcardList(string wildcardPath, int listSize, SortMode sortMode)
        {
            string id = wildcardPath + sortMode.ToString(); // Cache per sort mode

            if(_cachedWildcardLists.ContainsKey(id))
                return _cachedWildcardLists[id];

            var linesSrc = File.ReadAllLines(wildcardPath).Where(line => !string.IsNullOrWhiteSpace(line)); // Read all lines from wildcard file
            List<string> list = new List<string>(linesSrc);

            while (list.Count < listSize) // Clone list until it's longer than the desired size (will not run at all if it's already long enough)
                list.AddRange(linesSrc);

            if (sortMode == SortMode.Shuffle)
                list = list.OrderBy(a => _random.Next()).ToList(); // Shuffle list optionally
            else if (sortMode == SortMode.Alphabetically)
                list = list.OrderBy(a => a).ToList(); // Sort list optionally

            list = list.Take(listSize).ToList(); // Trim to the exact desired size

            _cachedWildcardLists[id] = list; // Cache until reset
            return list;
        }

        private static int GetWildcardIndex (string wildcard, bool increment)
        {
            if (!_wildcardIndex.ContainsKey(wildcard))
                _wildcardIndex[wildcard] = 0;

            int index = _wildcardIndex[wildcard];

            if (increment)
                _wildcardIndex[wildcard] = _wildcardIndex[wildcard] + 1;

            return index;
        }

        /// <summary> Resets randomization and saved indexes </summary>
        public static void Reset()
        {
            _cachedWildcardLists.Clear();
            _wildcardIndex.Clear();
        }
    }
}
