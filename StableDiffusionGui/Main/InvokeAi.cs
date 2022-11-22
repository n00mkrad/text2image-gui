using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class InvokeAi
    {
        public enum FixAction { Upscale, FaceRestoration }

        /// <summary> Run InvokeAI post-processing (!fix), with log timeout </summary>
        /// <returns> Successful or not </returns>
        public static async Task<bool> RunFix(string imgPath, List<FixAction> actions)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Can't run post-processing while the program is still busy.");
                return false;
            }

            if (TtiProcess.CurrentStdInWriter == null)
            {
                UiUtils.ShowMessageBox("Can't run post-processing when Stable Diffusion is not loaded.");
                return false;
            }

            try
            {
                Program.SetState(Program.BusyState.PostProcessing);

                Logger.Log($"InvokeAI !fix: {string.Join(", ", actions.Select(x => x.ToString()))}", true);

                List<string> args = new List<string> { "!fix", imgPath.Wrap(true) };

                if (actions.Contains(FixAction.Upscale))
                    args.Add(Args.InvokeAi.GetUpscaleArgs(true));

                if (actions.Contains(FixAction.FaceRestoration))
                    args.Add(Args.InvokeAi.GetFaceRestoreArgs(true));

                await TtiProcess.WriteStdIn(string.Join(" ", args), true);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                Logger.Log(ex.StackTrace);
                Program.SetState(Program.BusyState.Standby);
                return false;
            }
        }

        public static async Task SwitchModel (string modelNameInYaml)
        {
            NmkdStopwatch timeoutSw = new NmkdStopwatch();
            await TtiProcess.WriteStdIn($"!reset");
            await TtiProcess.WriteStdIn($"!switch {modelNameInYaml}");

            Logger.Log("SwitchModel waiting...", true);

            while (true)
            {
                await Task.Delay(1);
                var last2 = Logger.GetLastLines(Constants.Lognames.Sd, 2, true).Select(l => l.Split("invoke> ").Last().Trim());

                if (last2.Where(l => l.StartsWith($"New model is current model")).Any())
                    return;

                if (last2.Where(l => l.StartsWith($"Changing model")).Any())
                    break;

                if(timeoutSw.ElapsedMs > 10000)
                {
                    Logger.Log($"Error switching model: Timed out. (1)");
                    return;
                }
            }

            Logger.Log("Loading model...");

            while (true)
            {
                await Task.Delay(10);

                if (Logger.GetLastLines(Constants.Lognames.Sd, 15, true).Where(l => l.Trim().EndsWith($" is not a known model name. Please check your models.yaml file")).Any())
                    break;

                if (Logger.GetLastLines(Constants.Lognames.Sd, 15, true).Where(l => l.Contains($" {modelNameInYaml} from ")).Any())
                    break;

                if (timeoutSw.ElapsedMs > 60000)
                {
                    Logger.Log($"Error switching model: Timed out. (2)");
                    return;
                }
            }

            while (true)
            {
                await Task.Delay(10);

                if (Logger.GetLastLines(Constants.Lognames.Sd, 5, true).Where(l => l.StartsWith(">> Setting Sampler to ")).Any())
                    break;

                if (timeoutSw.ElapsedMs > 60000)
                {
                    Logger.Log($"Error switching model: Timed out. (3)");
                    return;
                }
            }
        }
    }
}
