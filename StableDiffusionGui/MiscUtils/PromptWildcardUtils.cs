using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
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
        private enum Order { Shuffle, Sequential, Alphabetical }

        private static Random _random = new Random();
        private static readonly char _identifier = '~';
        private static readonly string _placeholderPattern = ">>>{0}<<<";

        public static List<string> ApplyWildcardsAll(string prompt, int iterations, bool runAllCombinations)
        {
            bool verboseLog = iterations == 1 || Program.Debug;
            List<string> outList = new List<string>();

            try
            {
                if (!prompt.Contains(_identifier))
                    return Enumerable.Repeat(prompt, iterations).ToList();

                string[] split = prompt.Split(_identifier);
                int[] identifierIndexes = prompt.Select((b, i) => b.Equals(_identifier) ? i : -1).Where(i => i != -1).ToArray(); // Store char index (position in string) of each identifier
                int wildcardIndex = 0;

                Dictionary<int, List<string>> lists = new Dictionary<int, List<string>>();

                for (int i = 1; i < split.Length; i++) // Start at 1 because we never need the first entry as it can't be a wildcard
                {
                    Order sort = Order.Shuffle;
                    int identifierIndex = identifierIndexes[i - 1];

                    if ((identifierIndex + 1) >= prompt.Length)
                        break;

                    if ((identifierIndex + 1) <= prompt.Length && prompt[identifierIndex + 1] == _identifier) // Char after identifier is also identifier => Sequential
                    {
                        sort = Order.Sequential;
                        i++;

                        if ((identifierIndex + 2) <= prompt.Length && prompt[identifierIndex + 2] == _identifier) // 2 chars after identifier are also identifier => Alphabetically
                        {
                            sort = Order.Alphabetical;
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
                        var list = GetWildcardList(wildcardIndex, wildcardName, runAllCombinations ? -1 : iterations, sort);
                        lists[wildcardIndex] = list;
                        split[i] = string.Format(_placeholderPattern, wildcardIndex);
                        if (verboseLog) Logger.Log($"Found Inline Wildcard '{wildcardName}' ({sort})", true);
                    }
                    else
                    {
                        string wildcardPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Wildcards, wildcardName + ".txt");

                        if (File.Exists(wildcardPath))
                        {
                            var list = GetWildcardList(wildcardIndex, wildcardPath, runAllCombinations ? -1 : iterations, sort);
                            lists[wildcardIndex] = list;
                            split[i] = string.Format(_placeholderPattern, wildcardIndex) + rest;
                            if (verboseLog) Logger.Log($"Found File Wildcard '{wildcardName}' ({sort})", true);
                        }
                        else
                        {
                            Logger.Log($"No wildcard file found for '{wildcardName}'", true);
                        }
                    }

                    wildcardIndex++;
                }

                prompt = string.Join("", split);
                Dictionary<int, int> indexes = lists.ToDictionary(x => x.Key, x => 0);

                if (runAllCombinations)
                {
                    var combinations = GetAllPossibleCombinations(lists);
                    Logger.Log($"Possible combinations: {string.Join(" * ", lists.Select(x => x.Value.Count().ToString()))} = {combinations.Count()}");

                    for (int comboIdx = 0; comboIdx < combinations.Count(); comboIdx++)
                    {
                        string processedPrompt = prompt;

                        for (int listIdx = 0; listIdx < lists.Count; listIdx++)
                            processedPrompt = processedPrompt.Replace(string.Format(_placeholderPattern, listIdx), combinations.ElementAt(comboIdx).ElementAt(listIdx));

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
                            processedPrompt = processedPrompt.Replace(string.Format(_placeholderPattern, listIdx), lists[listIdx][indexes[listIdx]]);
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

        private static IEnumerable<IEnumerable<string>> GetAllPossibleCombinations(Dictionary<int, List<string>> lists)
        {
            IEnumerable<IEnumerable<string>> combos = new string[][] { new string[0] };

            foreach (var inner in lists.Select(x => x.Value))
                combos = from c in combos
                         from i in inner
                         select c.Append(i);

            return combos;
        }

        private static List<string> GetWildcardList(int index, string wildcardStringOrFilePath, int listSize, Order sortMode)
        {
            if (File.Exists(wildcardStringOrFilePath))
            {
                IEnumerable<string> validLines = File.ReadAllLines(wildcardStringOrFilePath);
                return GetWildcardList(validLines, listSize, sortMode);
            }
            else
            {
                IEnumerable<string> validPhrases = wildcardStringOrFilePath.Split(',');
                return GetWildcardList(validPhrases, listSize, sortMode);
            }
        }

        private static List<string> GetWildcardList(IEnumerable<string> lines, int listSize, Order sortMode)
        {
            if (lines.Count() <= 0)
                return new List<string>();

            if (!Config.Instance.WildcardAllowEmptyEntries)
                lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)); // Filter out empty entries optionally

            if (sortMode == Order.Alphabetical)
                lines = lines.OrderBy(a => a).ToList(); // Sort list optionally
            if (sortMode == Order.Shuffle)
                lines = lines.OrderBy(a => _random.Next()).ToList(); // Shuffle list optionally

            List<string> list = new List<string>(lines);

            while (listSize > 0 && list.Count < listSize) // Clone list until it's longer than the desired size (will not run at all if it's already long enough)
                list.AddRange(lines);

            if (listSize > 0)
                list = list.Take(listSize).ToList(); // Trim to the exact desired size

            return list;
        }
    }
}
