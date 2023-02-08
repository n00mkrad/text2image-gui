using StableDiffusionGui.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StableDiffusionGui.MiscUtils
{
    internal class PromptImporterUtils
    {
        internal static TtiSettings Import(string prompt, TtiSettings referenceTti)
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

            var tti = new TtiSettings() // copy values
            {
                Prompts = new string[] { "" },
                NegativePrompt = "",
                Implementation = referenceTti.Implementation,
                Iterations = referenceTti.Iterations,
                Params = new Dictionary<string, string>(referenceTti.Params),
            };
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
                    foreach (var p in pairs)
                    {
                        if (!p.Contains(':')) continue;

                        var kvp = p.Split(':');
                        var key = kvp[0].ToLower().Trim(); // parser uses lowercase
                        var value = kvp[1].Trim();
                        // internaly some values are parsed with different names or formats
                        value = converParams(ref key, value);

                        tti.Params[key] = value;
                    }
                }
            }

            if (tti.Params.ContainsKey("Steps"))
            {
                tti.Iterations = int.Parse(tti.Params["Steps"]);
            }

            return tti;
        }
        private static string converParams(ref string key, string value)
        {
            switch (key)
            {
                case "steps":
                    // Steps is a List of Floats
                    return $"[ {value} ]";
                case "size":
                    var sizeSplit = value.Split('x');
                    return $"{{ \"width\": \"{sizeSplit[0]}\", \"height\": \"{sizeSplit[1]}\" }}";

                case "cfg scale":
                    // Steps is a List of Floats
                    key = "scales";
                    return $"[ {value} ]";

                default: return value;
            }
        }
    }
}
