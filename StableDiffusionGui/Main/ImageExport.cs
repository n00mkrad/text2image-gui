using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class ImageExport
    {
        private static readonly int _maxPathLength = 255;
        private static readonly int _minimumImageAgeMs = 200;
        private static readonly int _loopWaitBeforeStartMs = 1000;
        private static readonly int _loopWaitTimeMs = 200;

        public static async Task ExportLoop(string imagesDir, bool show)
        {
            Logger.Log("ExportLoop START", true);
            List<string> outImgs = new List<string>();

            await Task.Delay(_loopWaitBeforeStartMs);

            while (!TextToImage.Canceled)
            {
                try
                {
                    var files = IoUtils.GetFileInfosSorted(imagesDir, false, "*.png");
                    bool running = IoUtils.GetFileInfosSorted(Paths.GetSessionDataPath(), false, "prompts*.*").Any();

                    // TODO: Adapt for new dream.py implementation
                    if (TextToImage.LastTaskSettings.Implementation == Implementation.StableDiffusion)
                        running = TextToImage.CurrentTask.TargetImgCount > 0 && TextToImage.CurrentTask.ImgCount < TextToImage.CurrentTask.TargetImgCount;

                    if (!running && !files.Any())
                    {
                        Logger.Log($"ExportLoop: Breaking. Process running: {running} - Any files exist: {files.Any()}", true);
                        break;
                    }

                    var images = files.Where(x => x.CreationTime > TextToImage.CurrentTask.StartTime).OrderBy(x => x.CreationTime).ToList(); // Find images and sort by date, newest to oldest
                    images = images.Where(x => !IoUtils.IsFileLocked(x)).ToList(); // Ignore files that are still in use
                    images = images.Where(x => (DateTime.Now - x.LastWriteTime).TotalMilliseconds >= _minimumImageAgeMs).ToList(); // Wait a certain time to make sure python is done writing to it

                    if (TextToImage.LastTaskSettings.Implementation == Implementation.StableDiffusion)
                    {
                        string log = Logger.GetSessionLog(Constants.Lognames.Sd);
                        images = images.Where(img => log.Contains(img.Name)).ToList(); // Only take image if it was written into SD log. Avoids copying too early (post-proc etc)
                    }

                    bool sub = TextToImage.CurrentTask.SubfoldersPerPrompt;
                    Dictionary<string, string> imageDirMap = new Dictionary<string, string>();

                    if (sub)
                    {
                        foreach (var img in images)
                        {
                            var imgTimeSinceLastWrite = DateTime.Now - img.LastWriteTime;
                            string prompt = IoUtils.GetImageMetadata(img.FullName).Prompt;
                            int pathBudget = 255 - img.Directory.FullName.Length - 65;
                            string unixTimestamp = ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
                            string dirName = string.IsNullOrWhiteSpace(prompt) ? $"unknown_prompt_{unixTimestamp}" : FormatUtils.SanitizePromptFilename(FormatUtils.GetPromptWithoutModifiers(prompt), pathBudget);
                            imageDirMap[img.FullName] = Directory.CreateDirectory(Path.Combine(TextToImage.CurrentTask.OutDir, dirName)).FullName;
                        }
                    }

                    List<string> renamedImgPaths = new List<string>();

                    bool inclPrompt = !sub && Config.GetBool("checkboxPromptInFilename");
                    bool inclSeed = Config.GetBool("checkboxSeedInFilename");
                    bool inclScale = Config.GetBool("checkboxScaleInFilename");
                    bool inclSampler = Config.GetBool("checkboxSamplerInFilename");
                    bool inclModel = Config.GetBool("checkboxModelInFilename");

                    for (int i = 0; i < images.Count; i++)
                    {
                        try
                        {
                            var img = images[i];
                            string number = $"-{(TextToImage.CurrentTask.ImgCount).ToString().PadLeft(TextToImage.CurrentTask.TargetImgCount.ToString().Length, '0')}";
                            string parentDir = sub ? imageDirMap[img.FullName] : TextToImage.CurrentTask.OutDir;
                            string renamedPath = FormatUtils.GetExportFilename(img.FullName, parentDir, number, "png", _maxPathLength, inclPrompt, inclSeed, inclScale, inclSampler, inclModel);
                            OverlayMaskIfExists(img.FullName);
                            Logger.Log($"ImageExport: Trying to move {img.Name} => {renamedPath}", true);
                            img.MoveTo(renamedPath);
                            renamedImgPaths.Add(renamedPath);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"Failed to move image - Will retry in next loop iteration. ({ex.Message})", true);
                        }
                    }

                    outImgs.AddRange(renamedImgPaths);

                    if (outImgs.Count > 0 && show)
                        ImagePreview.SetImages(outImgs.Where(x => File.Exists(x)).ToList(), ImagePreview.ImgShowMode.ShowLast);

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

        private static void OverlayMaskIfExists(string imgPath, bool copyMetadata = true)
        {
            string maskPath = InpaintingUtils.MaskedImagePath;

            if (!File.Exists(maskPath))
                return;

            ImageMetadata meta = null;

            if (copyMetadata)
                meta = IoUtils.GetImageMetadata(imgPath);

            ImgUtils.Overlay(imgPath, maskPath);

            if (meta != null)
                IoUtils.SetImageMetadata(imgPath, meta.ParsedText);
        }

    }
}
