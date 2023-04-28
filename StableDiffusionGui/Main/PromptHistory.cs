using Newtonsoft.Json;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StableDiffusionGui.Main
{
    internal class PromptHistory
    {
        public static List<TtiSettings> History = new List<TtiSettings>();

        private static readonly int _maxEntries = 1000;

        public static void Add(TtiSettings batch)
        {
            if (!Config.Instance.EnablePromptHistory)
                return;

            foreach (string prompt in batch.Prompts.Distinct())
            {
                TtiSettings newEntry = batch.ToJson().FromJson<TtiSettings>(); // Clone
                newEntry.Prompts = prompt.AsArray();

                if (History.Count < 1 || (History.Count >= 1 && !newEntry.EqualsWithoutPrompts(History.First())))
                    History.Insert(0, newEntry);
            }

            if (History.Count > _maxEntries)
                History = History.Take(_maxEntries).ToList();

            Save();
        }

        public static void Delete(List<TtiSettings> promptsToDelete)
        {
            History = History.Except(promptsToDelete).ToList();
            Save();
        }

        public static void DeleteAll()
        {
            History.Clear();
            Save();
        }

        public static void Save()
        {
            string text = History.ToJson(true);
            File.WriteAllText(GetJsonPath(), text);
        }

        public static void Load()
        {
            if (!File.Exists(GetJsonPath()))
                return;

            string text = File.ReadAllText(GetJsonPath());
            History = text.FromJson<List<TtiSettings>>(NullValueHandling.Ignore, DefaultValueHandling.Include, true, true);
        }

        private static string GetJsonPath()
        {
            return Path.Combine(Paths.GetDataPath(), Constants.Files.PromptHistory);
        }
    }
}
