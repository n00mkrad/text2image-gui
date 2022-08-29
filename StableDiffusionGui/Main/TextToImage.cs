using HTAlt;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TextToImage
    {
        public enum Implementation { StableDiffusion }

        private static int _currentImgCount;
        private static int _currentTargetImgCount;
        private static string _currentOutPath;

        public class TtiSettings
        {
            public Implementation Implementation { get; set; } = Implementation.StableDiffusion;
            public string[] Prompts { get; set; } = new string[] { "" };
            public int Iterations { get; set; } = 1;
            public string OutPath { get; set; } = "";
            public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
        }

        public static async Task RunTti(TtiSettings s)
        {
            Program.MainForm.SetWorking(true);

            if (s.Implementation == Implementation.StableDiffusion)
                await RunStableDiffusion(s.Prompts, s.Iterations, s.Params["steps"].GetInt(), s.Params["scales"].Replace(" ", "").Split(",").Select(x => x.GetFloat()).ToArray(), s.Params["seed"].GetInt(), s.Params["sampler"], FormatUtils.ParseSize(s.Params["res"]), s.OutPath);

            Program.MainForm.SetWorking(false);
        }

        public static async Task RunStableDiffusion(string[] prompts, int iterations, int steps, float[] scales, int seed, string sampler, Size res, string outPath)
        {
            _currentOutPath = outPath;
            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts.txt");
            string promptFileContent = "";

            _currentImgCount = 0;
            _currentTargetImgCount = 0;

            foreach (string prompt in prompts)
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach(float scale in scales)
                    {
                        promptFileContent += $"{prompt} -n {1} -s {steps} -C {scale.ToStringDot()} -A {sampler} -W {res.Width} -H {res.Height} -S {seed}\n";
                        _currentTargetImgCount++;
                    }

                    seed++;
                }
            }

            File.WriteAllText(promptFilePath, promptFileContent);

            Logger.Log($"Preparing to run Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {string.Join(", ", scales.Select(x => x.ToStringDot()))}, {res.Width}x{res.Height}, Starting Seed: {seed}");
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")} each = {prompts.Length * iterations * scales.Length} images total.");

            Process dream = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());

            dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mc\\Scripts\\activate.bat\" ldo && " +
                $"python \"{Paths.GetDataPath()}/repo/scripts/dream.py\" -o {outPath.Wrap()} --from_file={promptFilePath.Wrap()}";

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

            ImagePreview.SetImages(outPath, true, _currentTargetImgCount);
            Logger.Log($"Done");
        }

        public static async Task RunStableDiffusionCli (string outPath)
        {
            if (Program.Busy)
                return;

            Process dream = OsUtils.NewProcess(false);

            dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mc\\Scripts\\activate.bat\" ldo && " +
                $"python \"{Paths.GetDataPath()}/repo/scripts/dream.py\" -o {outPath.Wrap()}";

            dream.Start();
        }

        static void LogOutput(string line, bool stdErr = false)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            //Stopwatch sw = new Stopwatch();
            //sw.Restart();

            //lastLogName = ai.LogFilename;
            Logger.Log(line, true, false, "sd");

            if (line.Contains("Initialization done!"))
            {
                Logger.Log("Generating...");
                Program.MainForm.SetProgress((int)Math.Round(((float)1 / _currentTargetImgCount) * 100f));
            }

            if (line.Contains("image(s) generated in "))
            {
                var split = line.Split("image(s) generated in ");
                _currentImgCount += split[0].GetInt();
                Program.MainForm.SetProgress((int)Math.Round(((float)(_currentImgCount+1) / _currentTargetImgCount) * 100f));

                int lastMsPerImg = $"{split[1].Remove(".").Remove("s")}0".GetInt();
                int remainingMs = (_currentTargetImgCount - _currentImgCount) * lastMsPerImg;

                Logger.Log($"Generated {split[0].GetInt()} image in {split[1]} ({_currentImgCount}/{_currentTargetImgCount})" +
                    $"{(_currentImgCount > 1 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")}", false, Logger.LastUiLine.Contains("Generated"));
                ImagePreview.SetImages(_currentOutPath, true, _currentImgCount);
            }
        }
    }
}
