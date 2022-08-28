using HTAlt;
using StableDiffusionGui.Io;
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
        public static async Task RunStableDiffusion(string prompt, int iterations, int steps, float scale, Size res, string outPath)
        {
            Logger.Log($"Preparing to run Stable Diffusion - {iterations} Iterations, {steps} Steps, Scale {scale.ToStringDot()}, {res.Width}x{res.Height}");

            string promptFilePath = Path.Combine(Paths.GetDataPath(), "prompt.txt");
            string promptFileContent = $"{prompt} -n {iterations} -s {steps} -C {scale.ToStringDot()} -W {res.Width} -H {res.Height}";

            File.WriteAllText(promptFilePath, promptFileContent);

            Process dream = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());

            dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mc\\Scripts\\activate.bat\" ldo && " +
                $"python \"{Paths.GetDataPath()}/repo/scripts/dream.py\" -o {outPath.Wrap()} --from_file={promptFilePath.Wrap()}";

            Logger.Log("cmd.exe " + dream.StartInfo.Arguments, true);

            if (!OsUtils.ShowHiddenCmd())
            {
                dream.OutputDataReceived += (sender, line) => { LogOutput(line.Data); };
                dream.ErrorDataReceived += (sender, line) => { LogOutput(line.Data, true); };
            }

            dream.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                dream.BeginOutputReadLine();
                dream.BeginErrorReadLine();
            }

            while (!dream.HasExited) await Task.Delay(1);

            ImagePreview.SetImages(outPath, true, iterations);
            Logger.Log($"Done");
        }

        static void LogOutput(string line, bool stdErr = false)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            //Stopwatch sw = new Stopwatch();
            //sw.Restart();

            //lastLogName = ai.LogFilename;
            Logger.Log(line, true, false, "sd");
        }
    }
}
