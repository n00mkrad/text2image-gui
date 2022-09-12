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
    internal class PostProcess
    {
        private static readonly int _maxPathLength = 255;
        private static List<string> outImgs;

        public static async Task PostProcLoop(string imagesDir, bool show)
        {
            await Task.Delay(1000);
            outImgs = new List<string>();

            while (true)
            {
                try
                {
                    var files = IoUtils.GetFileInfosSorted(imagesDir, false, "*.png");

                    //bool procRunning = TextToImage.CurrentTask.Processes.Where(x => x != null && !x.HasExited).Any();
                    bool procRunning = IoUtils.GetAmountOfFiles(Paths.GetSessionDataPath(), false, "prompts.txt") == 1;

                    if (!procRunning && !files.Any())
                        break;

                    var images = files.Where(x => x.CreationTime > TextToImage.CurrentTask.StartTime).OrderBy(x => x.CreationTime).ToList(); // Find images and sort by date, newest to oldest
                    images = images.Where(x => (DateTime.Now - x.LastWriteTime).TotalMilliseconds > 500).ToList();

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
                            string dirName = string.IsNullOrWhiteSpace(prompt) ? $"unknown_prompt_{unixTimestamp}" : FormatUtils.SanitizePromptFilename(prompt, pathBudget);
                            imageDirMap[img.FullName] = Directory.CreateDirectory(Path.Combine(TextToImage.CurrentTask.OutPath, dirName)).FullName;
                        }
                    }

                    List<string> renamedImgPaths = new List<string>();

                    for (int i = 0; i < images.Count; i++)
                    {
                        try
                        {
                            var img = images[i];
                            string number = $"-{(TextToImage.CurrentTask.ImgCount).ToString().PadLeft(TextToImage.CurrentTask.TargetImgCount.ToString().Length, '0')}";
                            bool inclPrompt = !sub && Config.GetBool("checkboxPromptInFilename");
                            string renamedPath = FormatUtils.GetExportFilename(img.FullName, sub ? imageDirMap[img.FullName] : TextToImage.CurrentTask.OutPath, number, "png", _maxPathLength, inclPrompt, true, true, true);
                            Logger.Log($"PostProcessing: Trying to move {img.Name} => {renamedPath}", true);
                            img.MoveTo(renamedPath);
                            renamedImgPaths.Add(renamedPath);
                        }
                        catch(Exception ex)
                        {
                            Logger.Log($"Failed to move image - Will retry in next loop iteration. ({ex.Message})", true);
                        }
                    }

                    outImgs.AddRange(renamedImgPaths);

                    if(outImgs.Count > 0)
                        ImagePreview.SetImages(outImgs, ImagePreview.ImgShowMode.ShowLast);

                    await Task.Delay(1001);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Image post-processing error:\n{ex.Message}");
                    Logger.Log($"{ex.StackTrace}", true);
                    break;
                }
            }

            Logger.Log("PostProcLoop end.", true);
        }

    }
}
