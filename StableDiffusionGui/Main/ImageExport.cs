using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.Export;

namespace StableDiffusionGui.Main
{
    internal class ImageExport
    {
        public static Stopwatch TimeSinceLastImage = new Stopwatch();
        public static RollingAverage RollingAvg = new RollingAverage(10);

        private static readonly int _maxPathLength = 230;
        private static readonly int _minimumImageAgeMs = 200;
        private static readonly int _loopWaitBeforeStartMs = 1000;
        private static readonly int _loopWaitTimeMs = 200;

        public static List<string> _outImgs = new List<string>();
        private static ConfigInstance _config = null;
        private static TtiTaskInfo _currTask = null;
        private static TtiSettings _currSettings = null;

        public static void Init(bool clearImages)
        {
            _config = TextToImage.CurrentTask.Config;
            _currTask = TextToImage.CurrentTask;
            _currSettings = TextToImage.CurrentTaskSettings;
            RollingAvg.Reset();
            TimeSinceLastImage.Restart();

            if (clearImages)
                _outImgs.Clear();
        }

        public static async Task ExportLoop(string imagesDir, int startingImgCount, int targetImgCount)
        {
            Logger.Log("ExportLoop START", true);

            await Task.Delay(_loopWaitBeforeStartMs);

            while (!TextToImage.Canceled)
            {
                try
                {
                    var files = IoUtils.GetFileInfosSorted(imagesDir, false, "*.png");
                    bool running = TtiProcess.CurrentProcess != null && !TtiProcess.CurrentProcess.HasExited;

                    if (_currSettings.Implementation.Supports(ImplementationInfo.Feature.InteractiveCli))
                        running = (_currTask.ImgCount - startingImgCount) < targetImgCount;

                    if (!running && !TtiUtils.ImportBusy && !files.Any())
                    {
                        Logger.Log($"ExportLoop: Breaking. Process running: {running} - Any files exist: {files.Any()}", true);
                        break;
                    }

                    var images = files.Where(x => x.CreationTime > _currTask.StartTime).OrderBy(x => x.CreationTime).ToList(); // Find images and sort by date, newest to oldest
                    images = images.Where(x => !IoUtils.IsFileLocked(x)).ToList(); // Ignore files that are still in use
                    images = images.Where(x => (DateTime.Now - x.LastWriteTime).TotalMilliseconds >= _minimumImageAgeMs).ToList(); // Wait a certain time to make sure python is done writing to it

                    if (_currSettings.Implementation == Enums.StableDiffusion.Implementation.InvokeAi)
                    {
                        var lastLines = TtiProcessOutputHandler.LastMessages.Take(3);
                        images = images.Where(img => lastLines.Any(l => l.Contains(img.Name))).ToList(); // Only take image if it was written into SD log. Avoids copying too early (post-proc etc)
                    }

                    if (images.Any())
                        Export(images);

                    await Task.Delay(_loopWaitTimeMs);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Image export error:\n{ex.Message}");
                    Logger.Log($"{ex.StackTrace}", true);
                    break;
                }
            }

            Logger.Log("ExportLoop END", true);
        }

        public static void Export(string path)
        {
            Export(new ZlpFileInfo(path).AsList());
        }

        public static void Export(List<ZlpFileInfo> images)
        {
            Dictionary<string, string> imageDirMap = new Dictionary<string, string>();

            if (_currTask.SubfoldersPerPrompt)
            {
                foreach (var img in images)
                {
                    var imgTimeSinceLastWrite = DateTime.Now - img.LastWriteTime;
                    string prompt = IoUtils.GetImageMetadata(img.FullName).CombinedPrompt;
                    int pathBudget = _maxPathLength - img.Directory.FullName.Length - 65;
                    prompt = _currTask.IgnoreWildcardsForFilenames && _currSettings.ProcessedAndRawPrompts.ContainsKey(prompt) ? _currSettings.ProcessedAndRawPrompts[prompt] : prompt;
                    string dirName = string.IsNullOrWhiteSpace(prompt) ? $"unknown_prompt_{FormatUtils.GetUnixTimestamp()}" : FormatUtils.SanitizePromptFilename(FormatUtils.GetPromptWithoutModifiers(prompt), pathBudget);
                    string subDirPath = _config.FolderPerSession ? Path.Combine(_currTask.OutDir, Paths.SessionTimestamp, dirName) : Path.Combine(_currTask.OutDir, dirName);
                    imageDirMap[img.FullName] = Directory.CreateDirectory(subDirPath).FullName;
                }
            }

            _currTask.ImgCount += images.Count;
            ExportLog(images.Count);
            List<string> renamedImgPaths = new List<string>();

            for (int i = 0; i < images.Count; i++)
            {
                try
                {
                    var img = images[i];
                    string num = _currTask.ImgCount.ToString().PadLeft(_currTask.TargetImgCount.ToString().Length, '0');
                    string dir = _currTask.SubfoldersPerPrompt ? imageDirMap[img.FullName] : _currTask.OutDir;
                    string renamedPath = GetExportFilename(img.FullName, dir, num, "png", _maxPathLength, _config.PromptInFilename, _config.SeedInFilename, _config.ScaleInFilename, _config.SamplerInFilename, _config.ModelInFilename);
                    OverlayMaskIfExists(img.FullName);
                    Logger.Log($"ImageExport: Moving {img.Name} => {renamedPath}", true);
                    img.MoveTo(renamedPath);
                    renamedImgPaths.Add(renamedPath);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to move image ({ex.Message})", true);
                }
            }

            _outImgs.AddRange(renamedImgPaths);

            if (_outImgs.Count > 0)
                ImageViewer.SetImages(_outImgs.Where(x => File.Exists(x)).ToList(), ImageViewer.ImgShowMode.ShowLast);
        }

        private static void ExportLog(int amount)
        {
            if (amount < 1 || TextToImage.Canceled)
                return;

            int imgCount = TextToImage.CurrentTask.ImgCount + TextToImage.CompletedTasks.Sum(t => t.ImgCount);
            int targetImgCount = TextToImage.CurrentTask.TargetImgCount + TextToImage.CompletedTasks.Sum(t => t.TargetImgCount) + MainUi.Queue.Sum(s => s.GetTargetImgCount(TextToImage.CurrentTask.Config));
            Program.MainForm.SetProgress((int)Math.Round(((float)imgCount / targetImgCount) * 100f));

            int lastMsPerImg = (int)TimeSinceLastImage.ElapsedMilliseconds;
            RollingAvg.AddDataPoint(lastMsPerImg);
            int remainingMs = (TextToImage.CurrentTask.TargetImgCount - TextToImage.CurrentTask.ImgCount) * (int)RollingAvg.GetAverage();
            int remainingMsTotal = (targetImgCount - imgCount) * (int)RollingAvg.GetAverage();

            string imgCountStr = $"{TextToImage.CurrentTask.ImgCount}/{TextToImage.CurrentTask.TargetImgCount}";

            if (TextToImage.IsRunningQueue)
                imgCountStr += $" of this task - {imgCount}/{targetImgCount} total";

            string etaStr = "";

            if (imgCount > 2 && remainingMsTotal > 2000)
            {
                etaStr += $" - ETA: {FormatUtils.Time(remainingMs, false)}";

                if (TextToImage.IsRunningQueue && imgCount > 4)
                {
                    if (remainingMsTotal != remainingMs)
                        etaStr += $" for this task - {FormatUtils.Time(remainingMsTotal, false)} for entire queue";
                }
            }

            string imgsStr = amount > 1 ? $"{amount} images" : "image";
            Logger.Log($"Generated {imgsStr} in {FormatUtils.Time(lastMsPerImg)} ({imgCountStr}){etaStr}", false, Program.MainForm.LogText.EndsWith("...") || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"));

            TimeSinceLastImage.Restart();
        }

        public static string GetExportFilename(string filePath, string parentDir, string suffix, string ext, int pathLimit, bool inclPrompt, bool inclSeed, bool inclScale, bool inclSampler, bool inclModel)
        {
            try
            {
                ext = ext.StartsWith(".") ? ext : $".{ext}"; // Ensure extension always starts with a dot
                string timestamp = GetExportTimestamp(Config.Instance.FilenameTimestampMode);
                int pathBudget = pathLimit - (parentDir.Length + 1) - (timestamp.Length + 1) - (suffix.Length + 1) - 4; // Remove 4 for extension
                var meta = IoUtils.GetImageMetadata(filePath);
                var filenameChunks = new List<string> { timestamp, suffix };

                string seed = meta.Seed.ToString();

                if (inclSeed && (pathBudget - seed.Length > 0))
                {
                    pathBudget -= (seed.Length + 1);
                    filenameChunks.Add(seed);
                }

                string scale = $"scale{meta.Scale.ToStringDot("0.00")}";

                if (inclScale && (pathBudget - scale.Length > 0))
                {
                    pathBudget -= (scale.Length + 1);
                    filenameChunks.Add(scale);
                }

                string sampler = meta.Sampler;

                if (inclSampler && (pathBudget - sampler.Length > 0))
                {
                    pathBudget -= (sampler.Length + 1);
                    filenameChunks.Add(sampler);
                }

                if (inclPrompt && pathBudget >= 4) // Including prompt with less than 4 chars is kinda pointless
                {
                    string prompt = FormatUtils.SanitizePromptFilename(meta.Prompt, pathBudget);
                    filenameChunks.Insert(2, prompt); // Insert after timestamp & suffix
                    pathBudget -= (prompt.Length + 1);
                }

                string model = $"{Path.ChangeExtension(TextToImage.CurrentTaskSettings.Model, null).Trim().Trunc(20, false)}";

                if (inclModel && model.Length > 1 && (pathBudget - model.Length > 0))
                {
                    pathBudget -= (model.Length + 1);
                    filenameChunks.Add(model);
                }

                string finalPath = Path.Combine(parentDir, $"{string.Join("-", filenameChunks.Where(s => !string.IsNullOrWhiteSpace(s)))}{ext}");
                return IoUtils.GetAvailablePath(finalPath).Replace(@"\\", "/").Replace(@"\", "/");
            }
            catch (Exception ex)
            {
                Logger.Log($"GetExportFilename Error: {ex.Message}\n{ex.StackTrace}");
                return "";
            }
        }

        private static string GetExportTimestamp(FilenameTimestamp timestampMode)
        {
            switch (timestampMode)
            {
                case FilenameTimestamp.None: return "";
                case FilenameTimestamp.Date: return DateTime.Now.ToString("yyyy-MM-dd");
                case FilenameTimestamp.DateTime: return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                case FilenameTimestamp.UnixEpoch: return FormatUtils.GetUnixTimestamp();
                default: return FormatUtils.GetUnixTimestamp();
            }
        }

        private static void OverlayMaskIfExists(string imgPath, bool copyMetadata = true)
        {
            if (TextToImage.CurrentTaskSettings.Implementation.Supports(ImplementationInfo.Feature.NativeInpainting))
                return; // InvokeAI has proper built-in inpainting - Skip for this implementation

            string maskPath = Inpainting.MaskedImagePath;

            if (!File.Exists(maskPath))
                return;

            ImageMetadata meta = null;

            if (copyMetadata)
                meta = IoUtils.GetImageMetadata(imgPath); // Save metadata as it gets lost otherwise

            ImgUtils.Overlay(imgPath, maskPath); // Actually do the overlaying

            if (meta != null)
                IoUtils.SetImageMetadata(imgPath, meta.ParsedText); // Put metadata back in
        }
    }
}
