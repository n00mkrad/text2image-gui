using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.Models;

namespace StableDiffusionGui.Main.Utils
{
    internal class ConvertModels
    {
        /// <summary> Converts model weights </summary>
        /// <returns> A model class of the newly created model, or null if it failed </returns>
        public static async Task<Model> Convert (Format formatIn, Format formatOut, Model model, bool fp16, bool quiet = false)
        {
            try
            {
                Logger.ClearLogBox();
                Logger.Log($"Converting model '{model.Name}' - This could take a few minutes...");

                string filename = model.IsDirectory ? model.Name : Path.GetFileNameWithoutExtension(model.Name);
                string outPath = "";

                List<string> outLines = new List<string>();

                if (formatIn == Format.Pytorch && formatOut == Format.Diffusers)
                {
                    outPath = GetOutputPath(model, "_diff");
                    await RunPython($"python repo/scripts/diff/convert_original_stable_diffusion_to_diffusers.py --checkpoint_path {model.FullName.Wrap(true)} --dump_path {outPath.Wrap(true)} " +
                        $"--original_config_file {Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "configs", "stable-diffusion", "v1-inference.yaml").Wrap(true)}");
                }
                else if (formatIn == Format.Pytorch && formatOut == Format.DiffusersOnnx)
                {
                    string tempPath = Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
                    outPath = GetOutputPath(model, "_onnx");
                    await RunPython($"python repo/scripts/diff/convert_original_stable_diffusion_to_diffusers.py --checkpoint_path {model.FullName.Wrap(true)} --dump_path {tempPath.Wrap(true)} " +
                        $"--original_config_file {Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "configs", "stable-diffusion", "v1-inference.yaml").Wrap(true)}");
                    await RunPython($"python repo/scripts/diff/convert_stable_diffusion_checkpoint_to_onnx.py --model_path {tempPath.Wrap(true)} --output_path {outPath.Wrap(true)} ");
                    IoUtils.TryDeleteIfExists(tempPath);
                }
                else if (formatIn == Format.Diffusers && formatOut == Format.Pytorch)
                {
                    outPath = GetOutputPath(model, "_ckpt.ckpt");
                    await RunPython($"python repo/scripts/diff/convert_diffusers_to_original_stable_diffusion.py --model_path {model.FullName.Wrap(true)} --checkpoint_path {outPath.Wrap(true)}");
                }
                else if (formatIn == Format.Diffusers && formatOut == Format.DiffusersOnnx)
                {
                    outPath = GetOutputPath(model, "_onnx");
                    await RunPython($"python repo/scripts/diff/convert_stable_diffusion_checkpoint_to_onnx.py --model_path {model.FullName.Wrap(true)} --output_path {outPath.Wrap(true)} ");
                }
                else if (formatIn == Format.Pytorch && formatOut == Format.Safetensors)
                {
                    outPath = GetOutputPath(model, "_sft.safetensors");
                    await RunPython($"python repo/scripts/ckpt_to_st.py -i {model.FullName.Wrap(true)} -o {outPath.Wrap(true)}");
                }
                else if (formatIn == Format.Safetensors && formatOut == Format.Pytorch)
                {
                    outPath = GetOutputPath(model, "_ckpt.ckpt");
                    await RunPython($"python repo/scripts/st_to_ckpt.py -i {model.FullName.Wrap(true)} -o {outPath.Wrap(true)}");
                }

                Logger.ClearLogBox();

                if (File.Exists(outPath) || Directory.Exists(outPath))
                {
                    Logger.Log($"Done. Saved converted model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");

                    if (Config.Get<bool>(Config.Keys.ConvertModelsDeleteInput))
                    {
                        bool deleteSuccess = IoUtils.TryDeleteIfExists(model.FullName);
                        Logger.Log($"{(deleteSuccess ? "Deleted" : "Failed to delete")} input file '{model.Name}'.");
                    }

                    return Paths.GetModelsAll().Where(m => m.FullName == outPath).FirstOrDefault();
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

        private static async Task RunPython (string cmd)
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
    }
}
