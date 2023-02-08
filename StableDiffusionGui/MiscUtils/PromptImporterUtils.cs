using StableDiffusionGui.Data;
using System;
using System.Linq;

namespace StableDiffusionGui.MiscUtils
{
    internal class PromptImporterUtils
    {
        internal static TtiSettings Import(string prompt)
        {
            if (prompt == null) return null;
            // Format:
            // 3 lines:
            // [0] "Positibe Prompt words ..."
            // [1] "Negative prompt: words words ..."
            // [2] "Key1: value1, Key2: value2, ..."

            // allows for prompts generated for either windows or linux
            // and also facilitates the split
            if (prompt.Contains('\r')) prompt = prompt.Replace("\r", "");

            var lines = prompt.Split('\n');

            var tti = new TtiSettings();
            // there are aways a first line
            tti.Prompts = new string[] { lines[0] };

            for (int i = 1; i < lines.Length; i++)
            {
                if (!lines[i].Contains(':')) continue; // ignore lines without :

                // Parse negative prompt
                if (lines[i].StartsWith("Negative prompt:", StringComparison.InvariantCultureIgnoreCase))
                {
                    var negative = lines[i].Substring(lines[i].IndexOf(':') + 1);
                    tti.NegativePrompt = negative;

                    continue;
                }
                // parse configs
                if (lines[i].Contains(',')) // contains ',' and ':'
                {
                    var pairs = lines[i].Split(',');
                    foreach(var p in pairs)
                    {
                        if (!p.Contains(':')) continue;

                        var kvp = p.Split(':');
                        tti.Params[kvp[0]] = kvp[1];
                    }
                }
            }

            if (tti.Params.ContainsKey("Steps"))
            {
                tti.Iterations = int.Parse(tti.Params["Steps"]);
            }

            return tti;
        }
    }
}
