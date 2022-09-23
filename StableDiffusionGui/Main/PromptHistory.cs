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

        public static void Add(TtiSettings batch)
        {
            if (!Config.GetBool(Config.Key.checkboxEnableHistory))
                return;

            foreach (string prompt in batch.Prompts)
                History.Add(new TtiSettings() { Prompts = new string[] { prompt }, Implementation = batch.Implementation, Iterations = batch.Iterations, Params = batch.Params });

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
            string text = JsonConvert.SerializeObject(History, Formatting.Indented);
            File.WriteAllText(GetJsonPath(), text);
        }

        public static void Load()
        {
            if (!File.Exists(GetJsonPath()))
                return;

            string text = File.ReadAllText(GetJsonPath());
            History = JsonConvert.DeserializeObject<List<TtiSettings>>(text);
        }

        private static string GetJsonPath()
        {
            return Path.Combine(Paths.GetDataPath(), "promptHistory.json");
        }
    }
}
