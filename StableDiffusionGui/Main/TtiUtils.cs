using ImageMagick;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Installation;
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

        public static async Task<OrderedDictionary> CreateResizedInitImagesIfNeeded(List<string> initImgPaths, Size targetSize, bool print = false)
        {
            ImportBusy = true;
            Logger.Log($"Importing initialization images...", false, Logger.LastUiLine.EndsWith("..."));

            var sourceAndImportedPaths = new ConcurrentDictionary<string, string>(initImgPaths.ToDictionary(x => x, x => ""));
            int imgsSucessful = 0;
            int imgsResized = 0;
            string initImgsDir = Directory.CreateDirectory(Path.Combine(Paths.GetSessionDataPath(), "inits")).FullName;

            var opts = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Task parallelTask = Task.Run(async () => Parallel.For(0, sourceAndImportedPaths.Count, opts, async i =>
            {
                var pair = sourceAndImportedPaths.ElementAt(i);
                int index = initImgPaths.IndexOf(pair.Key);
                MagickImage img = new MagickImage(pair.Key) { Format = MagickFormat.Png24, Quality = 30 };

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
                        Size scaleSize = Config.Get<bool>(Config.Keys.InitImageRetainAspectRatio) ? ImgMaths.FitIntoFrame(new Size(img.Width, img.Height), targetSize) : targetSize;
                        img = ImgUtils.ScaleAndPad(img, scaleSize, targetSize);
                        string resizedImgPath = Path.Combine(initImgsDir, $"{index}.png");
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

            int thresh = 55;

            if (words > thresh)
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} is very long (>{thresh} words).\n\nThe AI might ignore parts of your prompt. Shorten the prompt to avoid this.");

            var imp = ConfigParser.CurrentImplementation;

            if (imp == Implementation.OptimizedSd && prompts.Where(x => x.MatchesRegex(@"(?:(?!\[)(?:.|\n))*\[(?:(?!\])(?:.|\n))*\]")).Any())
            {
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} contains square brackets used for exclusion words.\n\n" +
                    $"This is not supported with your current implementation ({Strings.Implementation.Get(imp.ToString(), true)}).");
            }
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
            string name = Config.Get<string>(Config.Keys.Model);
            var imp = ConfigParser.CurrentImplementation;

            if (string.IsNullOrWhiteSpace(name))
            {
                TextToImage.Cancel($"No Stable Diffusion model file has been set.\nPlease set one in the settings, or quick-switch with Ctrl+M.", true);
                return null;
            }
            else
            {
                var model = cachedModels == null ? Models.GetModel(name, false, Enums.Models.Type.Normal, imp) : Models.GetModel(cachedModels, name, false, Enums.Models.Type.Normal, imp);

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

            var model = cachedModels == null ? Models.GetModel(name, false, Enums.Models.Type.Normal, imp) : Models.GetModel(cachedModels, name, false, Enums.Models.Type.Normal, imp);

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
                movePath = IoUtils.GetAvailableFilePath(InvokeAi.PostProcessMovePaths[key]);
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

                Ui.MainFormUtils.ImageViewer.AppendImage(movePath, Ui.MainFormUtils.ImageViewer.ImgShowMode.ShowLast, false);
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
    }
}
