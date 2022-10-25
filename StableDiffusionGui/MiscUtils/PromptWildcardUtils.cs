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

        public static string ApplyWildcards(string prompt, int iterations)
        {
            string[] split = prompt.Split('+');

            for (int i = 0; i < split.Length; i++)
            {
                if (i == 0 || string.IsNullOrWhiteSpace(split[i]))
                    continue;

                string regex = @"[^a-z-]";
                var wordSplit = Regex.Split(split[i], regex);
                string breakingDelimiter = Regex.Matches(split[i], regex).Cast<Match>().First().Value;

                string wildcardName = wordSplit.FirstOrDefault().Trim();
                string rest = breakingDelimiter + string.Join(breakingDelimiter, split[i].Split(breakingDelimiter).Skip(1));

                string wildcardPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Wildcards, wildcardName + ".txt");

                if (File.Exists(wildcardPath))
                {
                    var list = GetWildcardList(wildcardPath, iterations, true);
                    split[i] = list.ElementAt(GetWildcardIndex(wildcardName, true)) + rest; // Pick random line, insert back into word array
                    Logger.Log($"Filled wildcard '{wildcardName}' with '{split[i]}'", true);
                }
                else
                {
                    Logger.Log($"No wildcard file found for '{wildcardName}'", true);
                }
            }

            return string.Join("", split);
        }

        private static List<string> GetWildcardList(string wildcardPath, int listSize, bool shuffle)
        {
            if(_cachedWildcardLists.ContainsKey(wildcardPath))
                return _cachedWildcardLists[wildcardPath];

            var linesSrc = File.ReadAllLines(wildcardPath).Where(line => !string.IsNullOrWhiteSpace(line)); // Read all lines from wildcard file
            List<string> list = new List<string>(linesSrc);

            while (list.Count < listSize) // Clone list until it's longer than the desired size (will not run at all if it's already long enough)
                list.AddRange(linesSrc);

            if (shuffle)
                list = list.OrderBy(a => _random.Next()).ToList(); // Shuffle list optionally

            list = list.Take(listSize).ToList(); // Trim to the exact desired size

            _cachedWildcardLists[wildcardPath] = list; // Cache until reset
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
