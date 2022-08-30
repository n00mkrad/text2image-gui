using StableDiffusionGui.Io;
using StableDiffusionGui.IO;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcess
    {
        public static void Start(string outPath)
        {
            Program.MainForm.SetWorking(true);
            TextToImage.CurrentTask = new Data.TtiTaskInfo { StartTime = DateTime.Now, OutPath = outPath };
        }

        public static void Finish()
        {
            ImagePreview.SetImages(TextToImage.CurrentTask.OutPath, true, TextToImage.CurrentTask.TargetImgCount);

            PostProcess(TextToImage.CurrentTask.OutPath, true, TextToImage.CurrentTask.TargetImgCount);

            Logger.Log($"Done!");
            Program.MainForm.SetWorking(false);
        }

        private static readonly int _maxPathLength = 255;

        public static void PostProcess(string imagesDir, bool show, int amount = -1, string pattern = "*.png", bool recursive = false)
        {
            try
            {
                var images = IoUtils.GetFileInfosSorted(imagesDir, recursive, pattern).Where(x => x.CreationTime > TextToImage.CurrentTask.StartTime).OrderBy(x => x.CreationTime).ToList(); // Find images and sort by date, newest to oldest

                Logger.Log($"Found {images.Count} images created after start time.", true);

                //if (amount > 0)
                //    images = images.Take(amount).ToList();

                List<string> renamedImgPaths = new List<string>();

                for (int i = 0; i < images.Count; i++)
                {
                    var img = images[i];
                    string number = $"-{(i + 1).ToString().PadLeft(images.Count.ToString().Length, '0')}";
                    string renamedPath = FormatUtils.GetExportFilename(img.FullName, imagesDir, number, pattern.Remove("*").Split('.').Last(), _maxPathLength, true, true, true, true);
                    img.MoveTo(renamedPath);
                    renamedImgPaths.Add(renamedPath);
                }

                ImagePreview.SetImages(renamedImgPaths, show);
            }
            catch (Exception ex)
            {
                Logger.Log($"Image post-processing error:\n{ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        public static async Task RunStableDiffusion(string[] prompts, string initImg, string embedding, float[] initStrengths, int iterations, int steps, float[] scales, long seed, string sampler, Size res, string outPath)
        {
            Start(outPath);
            long startSeed = seed;

            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts.txt");
            string promptFileContent = "";

            foreach (string prompt in prompts)
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach (float scale in scales)
                    {
                        foreach (float strength in initStrengths)
                        {
                            string init = File.Exists(initImg) ? $"--init_img {initImg.Wrap()} --strength {strength.ToStringDot("0.0000")}" : "";
                            promptFileContent += $"{prompt} {init} -n {1} -s {steps} -C {scale.ToStringDot()} -A {sampler} -W {res.Width} -H {res.Height} -S {seed}\n";
                            TextToImage.CurrentTask.TargetImgCount++;
                        }
                    }

                    seed++;
                }
            }

            File.WriteAllText(promptFilePath, promptFileContent);

            Logger.Log($"Preparing to run Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            if (!string.IsNullOrWhiteSpace(embedding))
            {
                if (!File.Exists(embedding))
                    embedding = "";
                else
                    Logger.Log($"Using fine-tuned model: {Path.GetFileName(embedding)}");
            }

            string strengths = File.Exists(initImg) ? $" and {initStrengths.Length} strengths" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{strengths} each = {TextToImage.CurrentTask.TargetImgCount} images total.");

            Process dream = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            TextToImage.CurrentTask.Processes.Add(dream);

            dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mc\\Scripts\\activate.bat\" ldo && " +
                $"python \"{Paths.GetDataPath()}/repo/scripts/dream.py\" -o {outPath.Wrap()} --from_file={promptFilePath.Wrap()} {(!string.IsNullOrWhiteSpace(embedding) ? $"--embedding_path {embedding.Wrap()}" : "")}";

            Logger.Log("cmd.exe " + dream.StartInfo.Arguments, true);

            if (!OsUtils.ShowHiddenCmd())
            {
                dream.OutputDataReceived += (sender, line) => { LogOutput(line.Data); };
                dream.ErrorDataReceived += (sender, line) => { LogOutput(line.Data, true); };
            }

            Logger.Log("Loading...");
            dream.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                dream.BeginOutputReadLine();
                dream.BeginErrorReadLine();
            }

            while (!dream.HasExited) await Task.Delay(1);

            Finish();
        }

        public static async Task RunStableDiffusionCli(string outPath)
        {
            if (Program.Busy)
                return;

            string batPath = Path.Combine(Paths.GetSessionDataPath(), "dream.bat");

            string batText = $"@echo off\n title Dream.py CLI && cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mc\\Scripts\\activate.bat\" ldo && " +
                $"python \"{Paths.GetDataPath()}/repo/scripts/dream.py\" -o {outPath.Wrap()} {(Config.GetBool("checkboxFullPrecision") ? "--full_precision" : "")}";

            File.WriteAllText(batPath, batText);
            Process.Start(batPath);
        }

        static void LogOutput(string line, bool stdErr = false)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            //Stopwatch sw = new Stopwatch();
            //sw.Restart();

            //lastLogName = ai.LogFilename;
            Logger.Log(line, true, false, "sd");

            if (line.Contains("Setting Sampler"))
            {
                Logger.Log("Generating...");
                Program.MainForm.SetProgress((int)Math.Round(((float)1 / TextToImage.CurrentTask.TargetImgCount) * 100f));
            }

            if (line.Contains("image(s) generated in "))
            {
                var split = line.Split("image(s) generated in ");
                TextToImage.CurrentTask.ImgCount += split[0].GetInt();
                Program.MainForm.SetProgress((int)Math.Round(((float)(TextToImage.CurrentTask.ImgCount + 1) / TextToImage.CurrentTask.TargetImgCount) * 100f));

                int lastMsPerImg = $"{split[1].Remove(".").Remove("s")}0".GetInt();
                int remainingMs = (TextToImage.CurrentTask.TargetImgCount - TextToImage.CurrentTask.ImgCount) * lastMsPerImg;

                Logger.Log($"Generated {split[0].GetInt()} image in {split[1]} ({TextToImage.CurrentTask.ImgCount}/{TextToImage.CurrentTask.TargetImgCount})" +
                    $"{(TextToImage.CurrentTask.ImgCount > 1 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")}", false, Logger.LastUiLine.Contains("Generated"));
                ImagePreview.SetImages(TextToImage.CurrentTask.OutPath, true, TextToImage.CurrentTask.ImgCount);
            }
        }

        public static void Kill()
        {
            if (TextToImage.CurrentTask != null)
            {
                foreach (var process in TextToImage.CurrentTask.Processes.Where(x => x != null && !x.HasExited))
                {
                    try
                    {
                        OsUtils.KillProcessTree(process.Id);
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"Failed to kill process tree: {e.Message}", true);
                    }
                }
            }
        }
    }
}
