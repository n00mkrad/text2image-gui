using Microsoft.VisualBasic;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class PostProcess
    {
        //public static string TempOutPath { get { return Path.Combine(Paths.GetSessionDataPath(), "out"); } }

        private static readonly int _maxPathLength = 255;
        private static List<string> outImgs;

        public static async Task PostProcLoop(string imagesDir, bool show)
        {
            await Task.Delay(1000);
            outImgs = new List<string>();
            DateTime exitTime = DateTime.MaxValue;
            bool procRunning = true;
            int totalImages = 0;

            while (true)
            {
                try
                {
                    var files = IoUtils.GetFileInfosSorted(imagesDir, false, "*.png");
                    // Consider only files that were part of this run
                    var images = files.Where(x => x.CreationTime > TextToImage.CurrentTask.StartTime).OrderBy(x => x.CreationTime).ToList();

                    if (procRunning && !TextToImage.CurrentTask.Processes.Any(x => x != null && !x.HasExited))
                    {
                        // Last process just quit.
                        Logger.Log("PostProcLoop observes all processes quit.", true);
                        procRunning = false;
                        exitTime = DateTime.Now;
                    }
                    
                    // If there is no work to do, nothing is running, and the file system has had time to settle down, halt.
                    if (!procRunning && !files.Any() && DateTime.Now - exitTime > TimeSpan.FromMilliseconds(500))
                        break;
                    
                    images = images.Where(x => (DateTime.Now - x.LastWriteTime).TotalMilliseconds > 500).ToList();

                    bool sub = TextToImage.CurrentTask.SubfoldersPerPrompt;

                    List<string> renamedImgPaths = new List<string>();
                    foreach (FileInfo img in images)
                    {
                        try
                        {
                            string number = $"-{(totalImages+1).ToString().PadLeft(TextToImage.CurrentTask.TargetImgCount.ToString().Length, '0')}";
                            bool inclPrompt = !sub && Config.GetBool("checkboxPromptInFilename");
                            string renamedPath = FormatUtils.GetExportFilename(img.FullName, DirForImage(img, imagesDir, sub), number, "png", _maxPathLength, inclPrompt, true, true, true);
                            img.MoveTo(renamedPath);
                            renamedImgPaths.Add(renamedPath);
                            totalImages++;
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

        private static string DirForImage(FileInfo img, string imagesDir, bool subfoldersPerPrompt)
        {
            if (!subfoldersPerPrompt)
            {
                return imagesDir;
            }
            string prompt = IoUtils.GetImageMetadata(img.FullName).Prompt;
            int pathBudget = 255 - img.Directory.FullName.Length - 65;
            string unixTimestamp = ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            string dirName = string.IsNullOrWhiteSpace(prompt)
                ? $"unknown_prompt_{unixTimestamp}"
                : FormatUtils.SanitizePromptFilename(prompt, pathBudget);
            return Directory.CreateDirectory(Path.Combine(imagesDir, dirName)).FullName;
        }
    }
}
