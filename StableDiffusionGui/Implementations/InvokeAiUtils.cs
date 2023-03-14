using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Main.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StableDiffusionGui.Implementations
{
    internal class InvokeAiUtils
    {
        public static string ModelsYamlPath { get { return Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "invoke", "configs", "models.yaml"); } }
        private static EasyDict<string, Enums.Models.Format> _modelFormatCache = new EasyDict<string, Enums.Models.Format>();

        public static async Task<Model> ConvertVae(Model vae, bool print = true)
        {
            string outPath = Path.ChangeExtension(vae.FullName, null);

            if (DetectModelFormatCached(vae.FullName) == Enums.Models.Format.Diffusers) // Is already correct format
                return vae;

            if (DetectModelFormatCached(outPath) == Enums.Models.Format.Diffusers) // Conversion already exists at output path
                return new Model(outPath, Enums.Models.Format.Diffusers);

            if (print)
                Logger.Log($"VAE '{vae.FormatIndependentName.Trunc(50)}' is in legacy format, converting to Diffusers format...");

            await ConvertModels.ConvVaePytorchDiffusers(vae.FullName, outPath);
            _modelFormatCache.Clear(); // Clear model type detection cache because we just converted one
            Model convertedVae = new Model(outPath);

            if (print)
                Logger.Log($"Converted '{vae.FormatIndependentName.Trunc(50)}' to Diffusers format.", false, Logger.LastUiLine.EndsWith("converting to Diffusers format..."));

            return convertedVae;
        }

        private static Enums.Models.Format DetectModelFormatCached (string path)
        {
            if (!_modelFormatCache.ContainsKey(path))
                _modelFormatCache[path] = Models.DetectModelFormat(path);

            return _modelFormatCache[path];
        }

        /// <summary> Writes all models into models.yml for InvokeAI to use </summary>
        public static async Task WriteModelsYamlAll(Model selectedMdl, Model selectedVae, List<Model> cachedModels = null, List<Model> cachedModelsVae = null, bool quiet = false)
        {
            try
            {
                _modelFormatCache.Clear();

                if (cachedModels == null || cachedModels.Count < 1)
                    cachedModels = Models.GetModels(Enums.Models.Type.Normal);

                if (cachedModelsVae == null || cachedModelsVae.Count < 1)
                    cachedModelsVae = Models.GetModels(Enums.Models.Type.Vae);

                if (!Config.Get<bool>(Config.Keys.DisablePickleScanner))
                {
                    if (!quiet)
                        Logger.Log($"Preparing model files...");

                    var pickleScanResults = await TtiUtils.VerifyModelsWithPseudoHash(cachedModels.Concat(cachedModelsVae));
                    var cachedModelsUnsafe = cachedModels.Concat(cachedModelsVae).Where(model => !pickleScanResults.GetNoNull(IoUtils.GetPseudoHash(model.FullName), false)).ToList();

                    cachedModels = cachedModels.Except(cachedModelsUnsafe).ToList();
                    cachedModelsVae = cachedModelsVae.Except(cachedModelsUnsafe).ToList();

                    if (cachedModelsUnsafe.Any())
                    {
                        if (!quiet)
                            Logger.Log($"Warning: The following model files were disabled because they are either corrupted, incompatible, or malicious:\n" +
                            $"{string.Join("\n", cachedModelsUnsafe.Select(model => model.Name))}");

                        if (cachedModelsUnsafe.Select(m => m.FullName).Contains(selectedMdl.FullName))
                            TextToImage.Cancel("Selected model can not be loaded because it is either corruped or contains malware.", true);
                    }
                }

                string text = "";

                cachedModelsVae.Insert(0, null); // Insert null entry, for looping
                string dataPath = Paths.GetDataPath();//.Replace("\\", "/").TrimEnd('/');

                foreach (Model mdl in cachedModels)
                {
                    bool inpaint = mdl.Name.MatchesWildcard("*-inpainting.*");

                    foreach (Model mdlVae in cachedModelsVae)
                    {
                        var vae = mdlVae == null ? null : mdlVae.Format == Enums.Models.Format.Diffusers ? mdlVae : await ConvertVae(mdlVae, !quiet);
                        string configFile = File.Exists(mdl.FullName + ".yaml") ? $"{(mdl.FullName + ".yaml").Wrap(true)} # custom" : $"configs/stable-diffusion/{(inpaint ? "v1-inpainting-inference" : "v1-inference")}.yaml";
                        var properties = new List<string>();

                        if (mdl.Format != Enums.Models.Format.Diffusers)
                            properties.Add($"config: {configFile}"); // Neeed to specify config path for ckpt models
                        else if (mdl.Format == Enums.Models.Format.Diffusers)
                            properties.Add($"format: diffusers"); // Need to specify format for diffusers models

                        properties.Add($"{(mdl.Format == Enums.Models.Format.Diffusers ? "path" : "weights")}: {mdl.FullName.Replace(dataPath, "../..").Wrap(true)}"); // Weights path, use relative path if possible

                        if (vae != null && vae.FullName.IsNotEmpty())
                            properties.Add($"vae: {vae.FullName.Replace(dataPath, "../..").Wrap(true)}");

                        string res = mdl.Name.Contains("768") ? "768" : "512";
                        properties.Add($"width: {res}");
                        properties.Add($"height: {res}");

                        if (IsModelDefault(mdl, vae, selectedMdl, selectedVae))
                            properties.Add($"default: true");

                        text += $"{GetMdlNameForYaml(mdl, vae)}:\n    {string.Join("\n    ", properties)}\n\n";
                    } 
                }

                File.WriteAllText(ModelsYamlPath, text);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error writing model list: {ex.Message}.", true);
                Logger.Log(ex.StackTrace, true);
                TextToImage.Cancel($"Error writing model list: {ex.Message.Trunc(200)}.\nCheck logs for details.", true);
            }
        }

        private static bool IsModelDefault(Model mdl, Model vae, Model selectedMdl, Model selectedVae)
        {
            if (mdl == null || selectedMdl == null)
                return false;

            bool mdlMatch = mdl.FullName == selectedMdl.FullName;
            bool vaeMatch;

            if (selectedVae == null)
                vaeMatch = vae == null;
            else
                vaeMatch = vae != null && selectedVae.FormatIndependentName == vae.FormatIndependentName;

            return mdlMatch && vaeMatch;
        }

        public static string GetMdlNameForYaml(Model mdl, Model vae)
        {
            return $"{mdl.FormatIndependentName}{(vae == null ? "" : $"-{vae.FormatIndependentName}")}";
        }

        public static string GetModelsYamlHash(IoUtils.Hash hashType = IoUtils.Hash.CRC32, bool ignoreDefaultKey = true)
        {
            var lines = File.ReadAllLines(ModelsYamlPath);

            if (ignoreDefaultKey)
                lines = lines.Where(l => !l.Contains("    default: ")).ToArray();

            string contentStr = string.Join(Environment.NewLine, lines);
            return IoUtils.GetHash(contentStr, hashType, false);
        }

        public static string ConvertAttentionSyntax(string prompt)
        {
            if (!prompt.Contains("(") && !prompt.Contains("{")) // Skip if no parentheses/curly brackets were used
                return prompt;

            if (PromptUsesNewAttentionSyntax(prompt))
                return prompt;

            prompt = prompt.Replace("\\(", "escapedParenthesisOpen").Replace("\\)", "escapedParenthesisClose");

            var parentheses = Regex.Matches(prompt, @"\(((?>[^()]+|\((?<n>)|\)(?<-n>))+(?(n)(?!)))\)"); // Find parenthesis pairs

            for (int i = 0; i < parentheses.Count; i++)
            {
                string match = parentheses[i].Value;

                if (match.MatchesRegex(@":\d.\d+\)"))
                    continue;

                int count = match.Where(c => c == ')').Count();
                string converted = $"({match.Remove("(").Remove(")")}){new string('+', count)}";
                prompt = prompt.Replace(match, converted);
            }

            var curlyBrackets = Regex.Matches(prompt, @"\{((?>[^{}]+|\{(?<n>)|\}(?<-n>))+(?(n)(?!)))\}"); // Find curly bracket pairs

            for (int i = 0; i < curlyBrackets.Count; i++)
            {
                string match = curlyBrackets[i].Value;
                int count = match.Where(c => c == '}').Count();
                string converted = $"({match.Remove("{").Remove("}")}){new string('-', count)}";
                prompt = prompt.Replace(match, converted);
            }

            var weightsInsideParentheses = Regex.Matches(prompt, @":\d.\d+\)"); // Detect A1111 float weighted parentheses

            for (int i = 0; i < weightsInsideParentheses.Count; i++)
            {
                string match = weightsInsideParentheses[i].Value;
                float weight = match.TrimStart(':').TrimEnd(')').GetFloat();
                prompt = prompt.Replace(match, $"){weight.ToStringDot("0.#")}");
            }

            prompt = prompt.Replace("escapedParenthesisOpen", "\\(").Replace("escapedParenthesisClose", "\\)");

            return prompt;
        }

        private static bool PromptUsesNewAttentionSyntax(string p)
        {
            bool newSyntax = false;

            if (p.Contains(")+") || p.Contains(")-")) // Detect +/- weighted parentheses
                newSyntax = true;

            if (Regex.Matches(p, @"\)\d").Count >= 1 || Regex.Matches(p, @"\)\d.\d+").Count >= 1) // Detect int or float weighted parentheses
                newSyntax = true;

            if (p.Contains(".blend(") || p.Contains(".swap(")) // Check for blend and swap commands
                newSyntax = true;

            return newSyntax;
        }
    }
}
