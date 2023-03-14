using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.Models;

namespace StableDiffusionGui.Main.Utils
{
    internal class ConvertModels
    {
        private static string _ckptConfigPath;

        /// <summary> Converts model weights </summary>
        /// <returns> A model class of the newly created model, or null if it failed </returns>
        public static async Task<Model> Convert(Format formatIn, Format formatOut, Model model, bool fp16, bool quiet = false)
        {
            try
            {
                Logger.ClearLogBox();
                Logger.Log($"Converting model '{model.Name}' - This could take a few minutes...");

                string filename = model.IsDirectory ? model.Name : Path.GetFileNameWithoutExtension(model.Name);
                string outPath = "";

                List<string> outLines = new List<string>();

                switch (formatOut)
                {
                    case Format.Pytorch: outPath = GetOutputPath(model, ".ckpt"); break;
                    case Format.Diffusers: outPath = GetOutputPath(model, ".diff"); break;
                    case Format.DiffusersOnnx: outPath = GetOutputPath(model, ".onnx"); break;
                    case Format.Safetensors: outPath = GetOutputPath(model, ".safetensors"); break;
                }

                if (File.Exists(outPath) || Directory.Exists(outPath))
                {
                    Logger.Log($"Can't convert model because a file or directory already exists at the output path: {outPath}");
                    return null;
                }

                _ckptConfigPath = TtiUtils.GetCkptConfig(model, false);
                PatchConversionScripts();

                // Pytorch -> Diffusers
                if (formatIn == Format.Pytorch && formatOut == Format.Diffusers)
                {
                    await ConvPytorchDiffusers(model.FullName, outPath);
                }
                // Pytorch -> Diffusers -> Diffusers ONNX
                else if (formatIn == Format.Pytorch && formatOut == Format.DiffusersOnnx)
                {
                    string tempPath = Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
                    await ConvPytorchDiffusers(model.FullName, tempPath); // We need to convert to diffusers first...
                    await ConvDiffusersOnnx(tempPath, outPath, true); // ...then we can convert to ONNX
                }
                // Pytorch -> Safetensors
                else if (formatIn == Format.Pytorch && formatOut == Format.Safetensors)
                {
                    await ConvPytorchSafetensors(model.FullName, outPath);
                }
                // Diffusers -> Pytorch
                else if (formatIn == Format.Diffusers && formatOut == Format.Pytorch)
                {
                    await ConvDiffusersPytorch(model.FullName, outPath);
                }
                // Diffusers -> Diffusers ONNX
                else if (formatIn == Format.Diffusers && formatOut == Format.DiffusersOnnx)
                {
                    await ConvDiffusersOnnx(model.FullName, outPath);
                }
                // Diffusers -> Safetensors
                else if (formatIn == Format.Diffusers && formatOut == Format.Safetensors)
                {
                    string tempPath = Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
                    await ConvDiffusersPytorch(model.FullName, tempPath);
                    await ConvPytorchSafetensors(tempPath, outPath, true);
                }
                // Safetensors -> Pytorch
                else if (formatIn == Format.Safetensors && formatOut == Format.Pytorch)
                {
                    string tempPath = Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
                    await ConvSafetensorsDiffusers(model.FullName, tempPath);
                    await ConvDiffusersPytorch(tempPath, outPath);
                }
                // Safetensors -> Diffusers
                else if (formatIn == Format.Safetensors && formatOut == Format.Diffusers)
                {
                    await ConvSafetensorsDiffusers(model.FullName, outPath);
                }
                // Safetensors -> Diffusers ONNX
                else if (formatIn == Format.Safetensors && formatOut == Format.DiffusersOnnx)
                {
                    string tempPath = GetTempPath();
                    await ConvSafetensorsDiffusers(model.FullName, tempPath, true);
                    await ConvDiffusersOnnx(tempPath, outPath, true);
                }
                else
                {
                    Logger.Log($"Error: Conversion from {formatIn} to {formatOut} not implemented!");
                }

                if (File.Exists(outPath) || Directory.Exists(outPath))
                {
                    Logger.Log($"Done. Saved converted model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");

                    if (Config.Get<bool>(Config.Keys.ConvertModelsDeleteInput))
                    {
                        bool deleteSuccess = IoUtils.TryDeleteIfExists(model.FullName);
                        Logger.Log($"{(deleteSuccess ? "Deleted" : "Failed to delete")} input file '{model.Name}'.");
                    }

                    return Models.GetModelsAll().Where(m => m.FullName == outPath).FirstOrDefault();
                }
                else
                {
                    Logger.Log($"Failed to convert model.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Conversion Error: {ex.Message}");
                Logger.Log(ex.StackTrace);
                return null;
            }

            return null;
        }

        private static string GetTempPath()
        {
            return Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
        }

        private static async Task ConvPytorchDiffusers(string inPath, string outPath, bool deleteInput = false)
        {
            await RunPython($"python repo/scripts/diff/convert_original_stable_diffusion_to_diffusers.py --checkpoint_path {inPath.Wrap(true)} --dump_path {outPath.Wrap(true)} " +
                        $"--original_config_file {_ckptConfigPath.Wrap(true)}");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvDiffusersOnnx(string inPath, string outPath, bool deleteInput = false)
        {
            await RunPython($"python repo/scripts/diff/convert_stable_diffusion_checkpoint_to_onnx.py --model_path {inPath.Wrap(true)} --output_path {outPath.Wrap(true)} ");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvPytorchSafetensors(string inPath, string outPath, bool deleteInput = false)
        {
            await RunPython($"python repo/scripts/ckpt_to_st.py -i {inPath.Wrap(true)} -o {outPath.Wrap(true)}");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvDiffusersPytorch(string inPath, string outPath, bool deleteInput = false)
        {
            await RunPython($"python repo/scripts/diff/convert_diffusers_to_original_stable_diffusion.py --model_path {inPath.Wrap(true)} --checkpoint_path {outPath.Wrap(true)}");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvSafetensorsDiffusers(string inPath, string outPath, bool deleteInput = false)
        {
            await RunPython($"python repo/scripts/diff/convert_original_stable_diffusion_to_diffusers.py --from_safetensors --checkpoint_path {inPath.Wrap(true)} --dump_path {outPath.Wrap(true)} " +
                        $"--original_config_file {_ckptConfigPath.Wrap(true)}");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        public static async Task<string> ConvVaePytorchDiffusers(string inPath, string outPath = "", bool deleteInput = false)
        {
            if (outPath.IsEmpty())
                outPath = Path.ChangeExtension(inPath, null);

            await RunPython($"python repo/scripts/diff/convert_vae_pt_to_diffusers.py --vae_pt_path {inPath.Wrap(true)} --dump_path {outPath.Wrap(true)}");

            if (deleteInput && Directory.Exists(outPath))
                IoUtils.TryDeleteIfExists(inPath);

            return Directory.Exists(outPath) ? outPath : "";
        }

        private static async Task RunPython(string cmd)
        {
            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && {cmd}";

            if (!OsUtils.ShowHiddenCmd())
            {
                p.OutputDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Convert); };
                p.ErrorDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Convert); };
            }

            Logger.Log($"cmd {p.StartInfo.Arguments}", true);
            p.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }

            while (!p.HasExited) await Task.Delay(1);
        }

        private static string GetOutputPath(Model model, string extension, bool noOverwrite = true)
        {
            if (model.IsDirectory)
            {
                string filename = model.Name;
                string pathNoExt = Path.Combine(model.Directory.FullName, filename);
                string path = pathNoExt + extension;

                if (Directory.Exists(path) && noOverwrite)
                {
                    for (int attempt = 0; attempt < 10000; attempt++)
                    {
                        path = $"{pathNoExt}_{attempt + 1}{extension}";

                        if (!File.Exists(path))
                            return path;
                    }

                    Logger.Log($"Failed to get unique directory! Using '{path}'", true);
                }

                return path;
            }
            else
            {
                string filename = Path.GetFileNameWithoutExtension(model.Name);
                string pathNoExt = Path.Combine(model.Directory.FullName, filename);
                string path = pathNoExt + extension;

                if (File.Exists(path) && noOverwrite)
                {
                    for (int attempt = 0; attempt < 10000; attempt++)
                    {
                        path = $"{pathNoExt}_{attempt + 1}{extension}";

                        if (!File.Exists(path))
                            return path;
                    }

                    Logger.Log($"Failed to get unique filename! Using '{path}'", true);
                }

                return path;
            }
        }

        private static void PatchConversionScripts()
        {
            string marker = "# PATCHED BY NMKD SD GUI";
            string diffusersPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Lib", "site-packages", "diffusers");
            string convertScriptPath = Path.Combine(diffusersPath, "pipelines", "stable_diffusion", "convert_from_ckpt.py");
            string text = File.ReadAllText(convertScriptPath);

            if (text.SplitIntoLines()[0].Trim() != marker)
            {
                text = text.Replace(".parms.", ".params.");
                text = text.Replace("model_type == \"FrozenCLIPEmbedder\"", "model_type.endswith(\"FrozenCLIPEmbedder\")");
                text = text.Replace("safety_checker = StableDiffusionSafetyChecker.from_pretrained(\"CompVis/stable-diffusion-safety-checker\")", "safety_checker = None");
                text = text.Replace("feature_extractor = AutoFeatureExtractor.from_pretrained(\"CompVis/stable-diffusion-safety-checker\")", "feature_extractor = None");
                text = text.Replace("feature_extractor=feature_extractor,", "feature_extractor=feature_extractor, requires_safety_checker=False,");
                File.WriteAllText(convertScriptPath, $"{marker}{Environment.NewLine}{text}");
                Logger.Log($"Patched {Path.GetFileName(convertScriptPath)}", true);
            }
        }
    }
}
