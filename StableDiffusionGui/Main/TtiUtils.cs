using ImageMagick;
using StableDiffusionGui.Data;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using Path = System.IO.Path;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Main
{
    internal class TtiUtils
    {
        public static bool ImportBusy;

        /// <summary> Resizes init images to <paramref name="targetSize"/>. If <paramref name="extendGravity"/> is specified, cropping/extending will be used instead of scaling. </summary>
        public static async Task<OrderedDictionary> CreateResizedInitImagesIfNeeded(List<string> initImgPaths, Size targetSize, Gravity extendGravity = (Gravity)(-1))
        {
            ImportBusy = true;
            Logger.Log($"Importing initialization images...", false, Logger.LastUiLine.EndsWith("..."));

            var sourceAndImportedPaths = new ConcurrentDictionary<string, string>(initImgPaths.ToDictionary(x => x, x => ""));
            int imgsSucessful = 0;
            int imgsResized = 0;
            string initImgsDir = Directory.CreateDirectory(Path.Combine(Paths.GetSessionDataPath(), "inits")).FullName;

            var opts = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Task parallelTask = Task.Run(async () => Parallel.For(0, sourceAndImportedPaths.Count, opts, i =>
            {
                var pair = sourceAndImportedPaths.ElementAt(i);
                int index = initImgPaths.IndexOf(pair.Key);
                MagickImage img = new MagickImage(pair.Key) { Format = extendGravity != (Gravity)(-1) ? MagickFormat.Png32 : MagickFormat.Png24, Quality = 30 };

                if (targetSize.IsEmpty || (img.Width == targetSize.Width && img.Height == targetSize.Height)) // Size already matches
                {
                    sourceAndImportedPaths[pair.Key] = pair.Key; // Don't do anything, just assign the same input path as import path
                    Interlocked.Increment(ref imgsSucessful);
                }
                else // Needs to be resized
                {
                    try
                    {
                        Logger.Log($"Init img '{Path.GetFileName(pair.Key)}' has bad dimensions ({img.Width}x{img.Height}), resizing to {targetSize.Width}x{targetSize.Height}.", true);
                        string resizedImgPath = Path.Combine(initImgsDir, $"{index}.png");

                        if (extendGravity != (Gravity)(-1)) // Extend/Crop
                        {
                            img = ImgUtils.ResizeCanvas(img, targetSize, extendGravity); // TODO: Gravity needs to be a variable passed from UI
                        }
                        else // Resize (Fit)
                        {
                            Size scaleSize = Config.Get<bool>(Config.Keys.InitImageRetainAspectRatio) ? ImgMaths.FitIntoFrame(new Size(img.Width, img.Height), targetSize) : targetSize;
                            img = ImgUtils.ScaleAndPad(img, scaleSize, targetSize);
                        }

                        img.Write(resizedImgPath);
                        img.Dispose();
                        sourceAndImportedPaths[pair.Key] = resizedImgPath;
                        Interlocked.Increment(ref imgsSucessful);
                        Interlocked.Increment(ref imgsResized);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Failed to resize image: {ex.Message}\n{ex.StackTrace}", true);
                    }
                }
            }));

            while (!parallelTask.IsCompleted)
                await Task.Delay(1);

            Logger.Log($"Imported {imgsSucessful} image(s){(imgsResized > 0 ? $" - {imgsResized} were resized to {targetSize.Width}x{targetSize.Height}" : "")}.", false, Logger.LastUiLine.EndsWith("..."));

            var sorted = new OrderedDictionary();

            for (int i = 0; i < initImgPaths.Count(); i++)
                sorted.Add(initImgPaths[i], sourceAndImportedPaths[initImgPaths[i]]); // Add images in the correct order, since multithreading messes the order up

            ImportBusy = false;
            return sorted;
        }

        /// <returns> Amount of removed images </returns>
        public static int CleanInitImageList()
        {
            var modifiedList = MainUi.CurrentInitImgPaths.Where(path => File.Exists(path)).ToList();
            int removed = modifiedList.Count - MainUi.CurrentInitImgPaths.Count;

            if (removed > 0)
            {
                MainUi.CurrentInitImgPaths = modifiedList;
                Logger.Log($"{removed} initialization images were removed because the files no longer exist.");
            }

            return removed;
        }

        public static void ShowPromptWarnings(List<string> prompts)
        {
            string longest = prompts.OrderByDescending(s => s.Length).First();
            longest = Regex.Replace(longest, @"(\[(?:\[??[^\[]*?\]))", "").Remove("[").Remove("]"); // Remove square brackets and contents

            char[] delimiters = new char[] { ' ', '\r', '\n' };
            int words = longest.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

            int thresh = 120;

            if (words > thresh)
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} is very long (>{thresh} words).\n\nThe AI might ignore parts of your prompt. Shorten the prompt to avoid this.");
        }

        public static void SoftCancelInvokeAi()
        {
            OsUtils.SendCtrlC(TtiProcess.CurrentProcess.Id);

            var childProcesses = OsUtils.GetChildProcesses(TtiProcess.CurrentProcess);

            foreach (System.Diagnostics.Process p in childProcesses)
                OsUtils.SendCtrlC(p.Id);
        }

        /// <summary> Checks if Stable Diffusion model exists </summary>
        /// <returns> Model ZlpFileInfo , if it exists - null if not </returns>
        public static Model CheckIfCurrentSdModelExists(List<Model> cachedModels = null)
        {
            string name = "";

            if (!CurrentSdModelExists())
                Config.Set(Config.Keys.Model, "");
            else
                name = Config.Get<string>(Config.Keys.Model);

            var imp = ConfigParser.CurrentImplementation;

            if (string.IsNullOrWhiteSpace(name))
            {
                TextToImage.Cancel($"No Stable Diffusion model file has been set.\nPlease set one in the settings, or quick-switch with Ctrl+M.", true);
                return null;
            }
            else
            {
                var model = cachedModels == null ? Models.GetModel(name, Enums.Models.Type.Normal, imp) : Models.GetModel(cachedModels, name, Enums.Models.Type.Normal, imp);

                if (model == null)
                {
                    TextToImage.Cancel($"Stable Diffusion model file {name.Wrap()} not found.\nPossibly it was moved, renamed, or deleted.", true);
                    return null;
                }
                else
                {
                    return model;
                }
            }
        }

        public static bool CurrentSdModelExists(List<Model> cachedModels = null)
        {
            string name = Config.Get<string>(Config.Keys.Model);
            var imp = ConfigParser.CurrentImplementation;

            if (string.IsNullOrWhiteSpace(name))
                return false;

            var model = cachedModels == null ? Models.GetModel(name, Enums.Models.Type.Normal, imp) : Models.GetModel(cachedModels, name, Enums.Models.Type.Normal, imp);

            return model != null;
        }

        public static Dictionary<string, string> GetEnvVarsSd(bool allCudaDevices = false, string baseDir = ".")
        {
            var envVars = new Dictionary<string, string>();

            string p = OsUtils.GetPathVar(new string[] {
                    Path.Combine(baseDir, Constants.Dirs.SdVenv, "Scripts"),
                    Path.Combine(baseDir, Constants.Dirs.Python, "Scripts"),
                    Path.Combine(baseDir, Constants.Dirs.Python),
                    Path.Combine(baseDir, Constants.Dirs.Git, "cmd")
                });

            envVars["PATH"] = p;

            int cudaDeviceOpt = Config.Get<int>(Config.Keys.CudaDeviceIdx);

            if (!allCudaDevices && cudaDeviceOpt > 0)
            {
                if (cudaDeviceOpt == 1) // CPU
                    envVars["CUDA_VISIBLE_DEVICES"] = "-1";
                else
                    envVars["CUDA_VISIBLE_DEVICES"] = $"{cudaDeviceOpt - 2}"; // Set env var to selected GPU ID (-2 because the first two options are Automatic and CPU)
            }

            if (!Directory.Exists(Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".cache", "huggingface", "transformers")))
                envVars["TRANSFORMERS_CACHE"] = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, Constants.Dirs.Cache.Transformers);

            if (!Directory.Exists(Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".cache", "huggingface")))
                envVars["HF_HOME"] = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, Constants.Dirs.Cache.Hf);

            if (!Directory.Exists(Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".cache", "torch")))
                envVars["TORCH_HOME"] = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, Constants.Dirs.Cache.TorchHub);

            return envVars;
        }

        public static string GetEnvVarsSdCommand(bool allCudaDevices = false, string baseDir = ".")
        {
            Dictionary<string, string> envVars = GetEnvVarsSd(allCudaDevices, baseDir);
            List<string> cmds = envVars.Select(x => $"SET \"{x.Key}={x.Value}\"").ToList();
            return string.Join(" && ", cmds);
        }

        public static void ApplyEnvVarsSd(System.Diagnostics.Process p, bool allCudaDevices = false, string baseDir = ".")
        {
            Dictionary<string, string> envVars = GetEnvVarsSd(allCudaDevices, baseDir);

            foreach (var envVar in envVars)
                p.StartInfo.EnvironmentVariables[envVar.Key] = envVar.Value;
        }

        public static bool ModelFilesizeValid(string path, Enums.Models.Type type = Enums.Models.Type.Normal)
        {
            if (!File.Exists(path))
                return false;

            return ModelFilesizeValid(new ZlpFileInfo(path).Length);
        }

        public static bool ModelFilesizeValid(Model model, Enums.Models.Type type = Enums.Models.Type.Normal)
        {
            if (!File.Exists(model.FullName))
                return false;

            return ModelFilesizeValid(model.Size);
        }

        public static bool ModelFilesizeValid(long size, Enums.Models.Type type = Enums.Models.Type.Normal)
        {
            try
            {
                if (type == Enums.Models.Type.Normal)
                    return size > 2000 * 1024 * 1024;
                else if (type == Enums.Models.Type.Vae)
                    return size > 100 * 1024 * 1024 && size < 1500 * 1024 * 1024;
            }
            catch
            {
                return true;
            }

            return true;
        }

        public static void ExportPostprocessedImage(string sourceImgPath, string processedImgPath)
        {
            string movePath = "";

            try
            {
                string key = new FileInfo(processedImgPath).Name.Split('.')[0];
                movePath = IoUtils.GetAvailablePath(InvokeAi.PostProcessMovePaths[key]);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to save post-processed image: {ex.Message}");
                return;
            }

            try
            {
                File.Move(processedImgPath, movePath);

                var meta = IoUtils.GetImageMetadata(sourceImgPath);
                IoUtils.SetImageMetadata(movePath, meta.ParsedText);

                ImageViewer.AppendImage(movePath, ImageViewer.ImgShowMode.ShowLast, false);
                // OsUtils.ShowNotification("Stable Diffusion GUI", $"Saved post-processed image as '{Path.GetFileName(movePath)}'.", false, 2.5f); // WHY DOES THIS NOT WORK - Threading problem?
                Logger.Log($"Saved post-processed image as '{Path.GetFileName(movePath)}'.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to save post-processed image: {ex.Message}");
                Logger.Log($"From '{processedImgPath}' to '{movePath}' - Trace:\n{ex.StackTrace}", true);
            }

            Program.SetState(Program.BusyState.Standby);
        }

        public static async Task<EasyDict<string, bool>> VerifyModelsWithPseudoHash(IEnumerable<Model> models)
        {
            var safeModels = Config.Get<string>(Config.Keys.SafeModels).FromJson<EasyDict<string, bool>>();

            if (safeModels == null)
                safeModels = new EasyDict<string, bool>();

            foreach (Model m in models)
            {
                string pseudoHash = IoUtils.GetPseudoHash(m.FullName);

                if (m.Format == Enums.Models.Format.Pytorch)
                {
                    bool safe = safeModels.ContainsKey(pseudoHash) ? safeModels[pseudoHash] : await OsUtils.ScanPickle(m.FullName);

                    if (safe) // Only save safe models to force re-checking of unsafe models
                        safeModels[pseudoHash] = safe;
                }
                else
                {
                    safeModels[pseudoHash] = true;
                }
            }

            Config.Set(Config.Keys.SafeModels, safeModels.ToJson());
            return safeModels;
        }

        /// <summary> Finds the model config file for a given ckpt, either for use with models.yaml (<paramref name="modelsYamlFormat"/> == true) or as full path. </summary>
        public static string GetCkptConfig(Model mdl, bool modelsYamlFormat)
        {
            if (mdl.Format == Enums.Models.Format.Diffusers || mdl.Format == Enums.Models.Format.DiffusersOnnx)
                return "";

            bool inpaint = mdl.FormatIndependentName.EndsWith("inpainting");

            if(mdl.LoadArchitecture != Enums.Models.SdArch.Automatic)
            {
                string file = inpaint ? "v1-inpainting-inference" : "v1-inference";

                if (mdl.LoadArchitecture == Enums.Models.SdArch.V2)
                    file = "v2-inference";
                else if (mdl.LoadArchitecture == Enums.Models.SdArch.V2V)
                    file = "v2-inference-v";

                if (modelsYamlFormat)
                    return $"configs/stable-diffusion/{file}.yaml"; // Return relative path for models.yaml
                else
                    return Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "invoke", "invokeai", "configs", "stable-diffusion", $"{file}.yaml"); // Return full path
            }

            var custConfigs = new List<string> { $"{Path.ChangeExtension(mdl.FullName, null)}.yaml", $"{mdl.FullName}.yaml", $"{Path.ChangeExtension(mdl.FullName, null)}.yml", $"{mdl.FullName}.yml" }.Where(path => File.Exists(path));

            if (custConfigs.Any())
            {
                if (modelsYamlFormat)
                    return $"{custConfigs.First().Wrap(true)} # custom"; // Return formatted path for models.yaml
                else
                    return custConfigs.First(); // Return path
            }

            return "";
        }
    }
}
