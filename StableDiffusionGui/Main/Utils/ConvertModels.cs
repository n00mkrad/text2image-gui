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
using static StableDiffusionGui.Main.Enums.Models;

namespace StableDiffusionGui.Main.Utils
{
    internal class ConvertModels
    {
        private static string _ckptConfigPath;

        /// <summary> Converts model weights </summary>
        /// <returns> A model class of the newly created model, or null if it failed </returns>
        public static async Task<Model> Convert(Format formatIn, Format formatOut, Model model, bool fp16, bool safeDiffusers, string customOutPath = "", bool quiet = false, bool? deleteInput = null)
        {
            try
            {
                if (deleteInput == null)
                    deleteInput = Config.Instance.ConvertModelsDeleteInput;

                if (!quiet)
                {
                    Logger.ClearLogBox();
                    Logger.Log($"Converting model '{model.Name}' - This could take a few minutes...");
                }

                string filename = model.IsDirectory ? model.Name : Path.GetFileNameWithoutExtension(model.Name);
                string outPath = customOutPath;

                List<string> outLines = new List<string>();

                if (outPath.IsEmpty())
                {
                    switch (formatOut)
                    {
                        case Format.Pytorch: outPath = GetOutputPath(model, ".ckpt"); break;
                        case Format.Diffusers: outPath = GetOutputPath(model, ".diff"); break;
                        case Format.DiffusersOnnx: outPath = GetOutputPath(model, ".onnx"); break;
                        case Format.Safetensors: outPath = GetOutputPath(model, ".safetensors"); break;
                    }
                }

                Assert.Check(!File.Exists(outPath) && !Directory.Exists(outPath), $"Can't convert model because a file or directory already exists at the output path: {outPath}");
                Assert.Check(IoUtils.GetFreeDiskSpaceGb(outPath) >= 5f, $"Not enough disk space on {Path.GetPathRoot(outPath)}.");

                _ckptConfigPath = TtiUtils.GetCkptConfig(model, false);

                if(formatIn == Format.Diffusers)
                {
                    Models.SetDiffusersClipSkip(model, 0); // Reset CLIP Skip
                    Models.HotswapDiffusersVae(model, null); // Reset custom VAE
                }

                // Pytorch -> Diffusers
                if (formatIn == Format.Pytorch && formatOut == Format.Diffusers)
                {
                    await ConvPytorchDiffusers(model.FullName, outPath, false, safeDiffusers, fp16);
                }
                // Pytorch -> Diffusers -> Diffusers ONNX
                else if (formatIn == Format.Pytorch && formatOut == Format.DiffusersOnnx)
                {
                    string tempPath = Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
                    await ConvPytorchDiffusers(model.FullName, tempPath); // We need to convert to diffusers first...
                    await ConvDiffusersOnnx(tempPath, outPath, true, fp16); // ...then we can convert to ONNX
                }
                // Pytorch -> Safetensors
                else if (formatIn == Format.Pytorch && formatOut == Format.Safetensors)
                {
                    string tempPath = Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
                    await ConvPytorchDiffusers(model.FullName, tempPath);
                    await ConvDiffusersSafetensors(tempPath, outPath, true, fp16);
                }
                // Diffusers -> Pytorch
                else if (formatIn == Format.Diffusers && formatOut == Format.Pytorch)
                {
                    await ConvDiffusersPytorch(model.FullName, outPath, false, fp16);
                }
                // Diffusers -> Diffusers ONNX
                else if (formatIn == Format.Diffusers && formatOut == Format.DiffusersOnnx)
                {
                    await ConvDiffusersOnnx(model.FullName, outPath, false, fp16);
                }
                // Diffusers -> Safetensors
                else if (formatIn == Format.Diffusers && formatOut == Format.Safetensors)
                {
                    await ConvDiffusersSafetensors(model.FullName, outPath, false, fp16);
                }
                // Safetensors -> Pytorch
                else if (formatIn == Format.Safetensors && formatOut == Format.Pytorch)
                {
                    string tempPath = Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
                    await ConvSafetensorsDiffusers(model.FullName, tempPath);
                    await ConvDiffusersPytorch(tempPath, outPath, true, fp16);
                }
                // Safetensors -> Diffusers
                else if (formatIn == Format.Safetensors && formatOut == Format.Diffusers)
                {
                    await ConvSafetensorsDiffusers(model.FullName, outPath, false, fp16);
                }
                // Safetensors -> Diffusers ONNX
                else if (formatIn == Format.Safetensors && formatOut == Format.DiffusersOnnx)
                {
                    string tempPath = GetTempPath();
                    await ConvSafetensorsDiffusers(model.FullName, tempPath, false);
                    await ConvDiffusersOnnx(tempPath, outPath, true, fp16);
                }
                else
                {
                    throw new Exception($"Conversion from {formatIn} to {formatOut} not implemented!");
                }

                if (File.Exists(outPath) || Directory.Exists(outPath))
                {
                    if (!quiet)
                        Logger.Log($"Done. Saved converted model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");

                    if (deleteInput == true)
                    {
                        bool deleteSuccess = IoUtils.TryDeleteIfExists(model.FullName);

                        if (!quiet)
                            Logger.Log($"{(deleteSuccess ? "Deleted" : "Failed to delete")} input file '{model.Name}'.");
                    }

                    var mdl = Models.GetModelsAll().Where(m => m.FullName == outPath).FirstOrDefault();

                    if (mdl == null)
                        return new Model(outPath, formatOut, Enums.Models.Type.Normal);
                    else
                        return mdl;
                }
                else
                {
                    throw new Exception($"Failed to convert model.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Conversion Error: {ex.Message}", quiet);
                Logger.Log(ex.StackTrace, true);
                return null;
            }
        }

        private static string GetTempPath()
        {
            return Path.Combine(Paths.GetSessionDataPath(), $"conv-temp-{FormatUtils.GetUnixTimestamp()}");
        }

        private static async Task ConvPytorchDiffusers(string inPath, string outPath, bool deleteInput = false, bool safetensors = true, bool fp16 = true)
        {
            string filename = Path.GetFileName(inPath).Lower();
            bool nai = filename.Contains("animefull") || filename.Contains("novel");
            string firstTryScript = nai ? "convert_original_stable_diffusion_to_diffusers_old" : "convert_original_stable_diffusion_to_diffusers";
            string secondTryScript = nai ? "convert_original_stable_diffusion_to_diffusers" : "convert_original_stable_diffusion_to_diffusers_old";

            string output = await RunPython($"python repo/scripts/diff/{firstTryScript}.py --checkpoint_path {inPath.Wrap(true)} --dump_path {outPath.Wrap(true)} " +
                        $"--original_config_file {_ckptConfigPath.Wrap(true)} {(safetensors ? "--to_safetensors" : "")} {(fp16 ? "--half" : "")}");

            if(!Directory.Exists(outPath) || output.SplitIntoLines().Last().Contains("KeyError:"))
            {
                await RunPython($"python repo/scripts/diff/{secondTryScript}.py --checkpoint_path {inPath.Wrap(true)} --dump_path {outPath.Wrap(true)} " +
                        $"--original_config_file {_ckptConfigPath.Wrap(true)} {(safetensors ? "--to_safetensors" : "")} {(fp16 ? "--half" : "")}");
            }

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvDiffusersOnnx(string inPath, string outPath, bool deleteInput = false, bool fp16 = true)
        {
            await RunPython($"python repo/scripts/diff/convert_stable_diffusion_checkpoint_to_onnx.py --model_path {inPath.Wrap(true)} --output_path {outPath.Wrap(true)} {(fp16 ? "--fp16" : "")}");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvDiffusersSafetensors(string inPath, string outPath, bool deleteInput = false, bool fp16 = true)
        {
            await RunPython($"python repo/scripts/diff/convert_diffusers_to_original_stable_diffusion.py --model_path {inPath.Wrap(true)} --checkpoint_path {outPath.Wrap(true)} {(fp16 ? "--half" : "")} --use_safetensors");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvDiffusersPytorch(string inPath, string outPath, bool deleteInput = false, bool fp16 = true)
        {
            await RunPython($"python repo/scripts/diff/convert_diffusers_to_original_stable_diffusion.py --model_path {inPath.Wrap(true)} --checkpoint_path {outPath.Wrap(true)} {(fp16 ? "--half" : "")}");

            if (deleteInput)
                IoUtils.TryDeleteIfExists(inPath);
        }

        private static async Task ConvSafetensorsDiffusers(string inPath, string outPath, bool deleteInput = false, bool fp16 = true)
        {
            if(Path.GetFileNameWithoutExtension(inPath).Lower().Contains("xl") && IoUtils.GetFilesize(inPath) > 5 * 1024 * 1024 * 1024L)
            {
                await RunPython($"python repo/scripts/diff/convert_sdxl_safetensors_to_diffusers.py --checkpoint_path {inPath.Wrap(true)} --dump_path {outPath.Wrap(true)} --to_safetensors {(fp16 ? "--half" : "")}");
            }
            else
            {
                await RunPython($"python repo/scripts/diff/convert_original_stable_diffusion_to_diffusers.py --from_safetensors --checkpoint_path {inPath.Wrap(true)} --dump_path {outPath.Wrap(true)} " +
                       $"--original_config_file {_ckptConfigPath.Wrap(true)} --to_safetensors {(fp16 ? "--half" : "")}");
            }

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

        private static async Task<string> RunPython(string cmd)
        {
            string output = "";

            void PythonLog(string log)
            {
                if (log == null) return;
                Logger.Log(log, true, false, Constants.Lognames.Convert);
                output += $"{log.Trim('\n')}\n";
            }

            Process py = OsUtils.NewProcess(true, logAction: PythonLog);
            py.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && {Constants.Files.VenvActivate} && {cmd}";

            Logger.Log($"cmd {py.StartInfo.Arguments}", true);
            OsUtils.StartProcess(py, killWithParent: true);
            await OsUtils.WaitForProcessExit(py);
            return output.TrimEnd('\n');
        }

        private static string GetOutputPath(Model model, string extension, bool noOverwrite = true)
        {
            string desiredPath = Path.Combine(model.Directory.FullName, model.Name) + extension;
            return IoUtils.GetAvailablePath(desiredPath, "_{0}");
        }
    }
}
