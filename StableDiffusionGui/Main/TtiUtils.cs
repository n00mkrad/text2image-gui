using ImageMagick;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Main
{
    internal class TtiUtils
    {
        public static async Task<Dictionary<string, string>> CreateResizedInitImagesIfNeeded(List<string> initImgPaths, Size targetSize, bool print = false)
        {
            Logger.Log($"Importing initialization images...");

            Dictionary<string, string> sourceAndImportedPaths = initImgPaths.ToDictionary(x => x, x => ""); // Dictionary key = original path, Value is imported path
            int imgsSucessful = 0;
            int imgsResized = 0;

            var opts = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Task parallelTask = Task.Run(async () => Parallel.For(0, sourceAndImportedPaths.Count, opts, async index =>
            {
                var pair = sourceAndImportedPaths.ElementAt(index);
                MagickImage img = new MagickImage(pair.Key) { Format = MagickFormat.Png24, Quality = 30 };

                if (img.Width == targetSize.Width && img.Height == targetSize.Height) // Size already matches
                {
                    Logger.Log($"Init img '{Path.GetFileName(pair.Key)}' has correct dimensions ({img.Width}x{img.Height}).", true);
                    sourceAndImportedPaths[pair.Key] = pair.Key; // Don't do anything, just assign the same input path as import path
                    Interlocked.Increment(ref imgsSucessful);
                }
                else // Needs to be resized
                {
                    try
                    {
                        Logger.Log($"Init img '{Path.GetFileName(pair.Key)}' has incorrect dimensions ({img.Width}x{img.Height}), resizing to {targetSize.Width}x{targetSize.Height}.", true);
                        img.Scale(new MagickGeometry(targetSize.Width, targetSize.Height) { IgnoreAspectRatio = true });
                        string initImgsDir = Directory.CreateDirectory(Path.Combine(Paths.GetSessionDataPath(), "inits")).FullName;
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

            Logger.Log($"Imported {imgsSucessful} images{(imgsResized > 0 ? $" - {imgsResized} were resized to {targetSize.Width}x{targetSize.Height}" : "")}.", false, Logger.LastUiLine.EndsWith("..."));
            return sourceAndImportedPaths;
        }

        /// <returns> Amount of removed images </returns>
        public static int CleanInitImageList()
        {
            if (MainUi.CurrentInitImgPaths == null)
                return 0;

            var modifiedList = MainUi.CurrentInitImgPaths.Where(path => File.Exists(path)).ToList();
            int removed = modifiedList.Count - MainUi.CurrentInitImgPaths.Count;

            if (MainUi.CurrentInitImgPaths.Count < 1)
            {
                MainUi.CurrentInitImgPaths = null;
                Logger.Log($"{(removed == 1 ? "Initialization image was cleared because the file no longer exists." : "Initialization images were cleared because the files no longer exist.")}");
            }
            else if (removed > 0)
            {
                MainUi.CurrentInitImgPaths = modifiedList;
                Logger.Log($"{removed} initialization image were removed because the files no longer exist.");
            }

            return removed;
        }

        /// <returns> Path to resized image </returns>
        public static string ResizeInitImg(string path, Size targetSize, bool print = false)
        {
            string outPath = Path.Combine(Paths.GetSessionDataPath(), "init.bmp");
            Image resized = ImgUtils.ResizeImage(IoUtils.GetImage(path), targetSize.Width, targetSize.Height);
            resized.Save(outPath, System.Drawing.Imaging.ImageFormat.Bmp);

            if (print)
                Logger.Log($"Resized init image to {targetSize.Width}x{targetSize.Height}.");

            return outPath;
        }

        public static void WriteModelsYaml(string mdlName, string vaeName = "", string keyName = "default")
        {
            var mdl = Paths.GetModel(mdlName, false, Enums.StableDiffusion.ModelType.Normal);
            var vae = Paths.GetModel(vaeName, false, Enums.StableDiffusion.ModelType.Vae);
            WriteModelsYaml(mdl, vae, keyName);
        }

        public static void WriteModelsYaml(FileInfo mdl, FileInfo vae, string keyName = "default")
        {
            string text = $"{keyName}:\n" +
                $"    config: configs/stable-diffusion/v1-inference.yaml\n" +
                $"    weights: {(mdl == null ? $"unknown{Constants.FileExts.SdModel}" : mdl.FullName.Wrap(true))}\n" +
                $"    {(vae != null && File.Exists(vae.FullName) ? $"vae: {vae.FullName.Wrap(true)}" : "")}\n" +
                $"    description: Current NMKD SD GUI model\n" +
                $"    width: 512\n" +
                $"    height: 512\n" +
                $"    default: true";

            File.WriteAllText(Path.Combine(Paths.GetDataPath(), Constants.Dirs.RepoSd, "configs", "models.yaml"), text);
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

            if (Config.GetBool("checkboxOptimizedSd") && prompts.Where(x => x.MatchesRegex(@"(?:(?!\[)(?:.|\n))*\[(?:(?!\])(?:.|\n))*\]")).Any())
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} contains square brackets used for exclusion words.\n\nThis is currently not supported in Low Memory Mode.");

            if (MainUi.CurrentEmbeddingPath != null && MainUi.CurrentEmbeddingPath.ToLowerInvariant().EndsWith(".pt") && prompts.Any(x => !x.Contains("*")))
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} does not contain a concept placeholder (*).\n\nIt will not use your loaded concept.");
        }

        // public static string GetCudaDevice(string arg)
        // {
        //     int opt = Config.GetInt("comboxCudaDevice");
        // 
        //     if (opt == 0)
        //         return "";
        //     else if (opt == 1)
        //         return $"{arg} cpu";
        //     else
        //         return $"{arg} cuda:{opt - 2}";
        // }

        public static void SoftCancelDreamPy()
        {
            var childProcesses = OsUtils.GetChildProcesses(TtiProcess.CurrentProcess);

            foreach (System.Diagnostics.Process p in childProcesses)
                OsUtils.SendCtrlC(p.Id);
        }

        /// <summary> Checks if Stable Diffusion model exists </summary>
        /// <returns> Model FileInfo, if it exists - null if not </returns>
        public static FileInfo CheckIfCurrentSdModelExists()
        {
            string savedModelFileName = Config.Get(Config.Key.comboxSdModel);

            if (string.IsNullOrWhiteSpace(savedModelFileName))
            {
                TextToImage.Cancel($"No Stable Diffusion model file has been set.\nPlease set one in the settings.");
                new SettingsForm().ShowDialogForm(0.5f);
                return null;
            }
            else
            {
                var model = Paths.GetModel(savedModelFileName);

                if (model == null)
                {
                    TextToImage.Cancel($"Stable Diffusion model file {savedModelFileName.Wrap()} not found.\nPossibly it was moved, renamed, or deleted.");
                    return null;
                }
                else
                {
                    return model;
                }
            }
        }

        public static string GetEnvVarsSd(bool allCudaDevices = false, string baseDir = ".")
        {
            string path = OsUtils.GetTemporaryPathVariable(new string[] { $"{baseDir}/{Constants.Dirs.Conda}", $"{baseDir}/{Constants.Dirs.Conda}/Scripts", $"{baseDir}/{Constants.Dirs.Conda}/condabin", $"{baseDir}/{Constants.Dirs.Conda}/Library/bin" });

            int cudaDeviceOpt = Config.GetInt("comboxCudaDevice");
            string devicesArg = ""; // Don't set env var if cudaDeviceOpt == 0 (=> automatic)

            if (!allCudaDevices && cudaDeviceOpt > 0)
            {
                if (cudaDeviceOpt == 1) // CPU
                    devicesArg = $" && SET CUDA_VISIBLE_DEVICES=\"\""; // Set env var to empty string
                else
                    devicesArg = $" && SET CUDA_VISIBLE_DEVICES={cudaDeviceOpt - 2}"; // Set env var to selected GPU ID (-2 because the first two options are Automatic and CPU)
            }

            return $"SET PATH={path}{devicesArg}";
        }

        public static bool ModelFilesizeValid(string path, Enums.StableDiffusion.ModelType type = Enums.StableDiffusion.ModelType.Normal)
        {
            if (!File.Exists(path))
                return false;

            return ModelFilesizeValid(new FileInfo(path));
        }

        public static bool ModelFilesizeValid(FileInfo file, Enums.StableDiffusion.ModelType type = Enums.StableDiffusion.ModelType.Normal)
        {
            try
            {
                if (type == Enums.StableDiffusion.ModelType.Normal)
                    return file.Length > 2010000000;
            }
            catch
            {
                return true;
            }

            return true;
        }

        public static void ExportPostprocessedImage(string sourceImgPath, string processedImgPath)
        {
            string ext = Path.GetExtension(sourceImgPath);
            string movePath = GetUniquePath(Path.ChangeExtension(sourceImgPath, null) + $".fix" + ext);

            try
            {
                File.Move(processedImgPath, movePath);

                var meta = IoUtils.GetImageMetadata(sourceImgPath);
                IoUtils.SetImageMetadata(movePath, meta.ParsedText);

                ImagePreview.AppendImage(movePath, ImagePreview.ImgShowMode.ShowLast, false);
                Logger.Log($"Saved post-processed image as '{Path.GetFileName(movePath)}'.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to save post-processed image: {ex.Message}");
                Logger.Log($"From '{processedImgPath}' to '{movePath}' - Trace:\n{ex.StackTrace}", true);
            }

            Program.MainForm.SetWorking(Program.BusyState.Standby);
        }

        private static string GetUniquePath(string preferredPath, string separator = "", int maxTries = 1000)
        {
            if (!File.Exists(preferredPath))
                return preferredPath;

            string pathNoExt = Path.ChangeExtension(preferredPath, null);
            string ext = Path.GetExtension(preferredPath);

            int counter = 1;

            while (File.Exists($"{pathNoExt}{separator}{counter}{ext}"))
            {
                counter++;

                if (counter >= maxTries)
                    return "";
            }

            return $"{pathNoExt}{separator}{counter}{ext}";
        }
    }
}
