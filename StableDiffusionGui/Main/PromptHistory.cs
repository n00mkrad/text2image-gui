using Newtonsoft.Json;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class PromptHistory
    {
        public static List<TtiSettings> Prompts = new List<TtiSettings>();

        public static void Add(TtiSettings batch)
        {
            foreach (string prompt in batch.Prompts)
                Prompts.Add(new TtiSettings() { Prompts = new string[] { prompt }, Implementation = batch.Implementation, Iterations = batch.Iterations, OutDir = batch.OutDir, Params = batch.Params });

            Save();
        }

        public static void Delete(List<TtiSettings> promptsToDelete)
        {
            Prompts = Prompts.Except(promptsToDelete).ToList();
            Save();
        }

        public static void DeleteAll()
        {
            Prompts.Clear();
            Save();
        }

        public static void Save()
        {
            string text = JsonConvert.SerializeObject(Prompts, Formatting.Indented);
            File.WriteAllText(GetJsonPath(), text);
        }

        public static void Load()
        {
            if (!File.Exists(GetJsonPath()))
                return;

            string text = File.ReadAllText(GetJsonPath());
            Prompts = JsonConvert.DeserializeObject<List<TtiSettings>>(text);
        }

        private static string GetJsonPath()
        {
            return Path.Combine(Paths.GetDataPath(), "promptHistory.json");
        }
    }
}
