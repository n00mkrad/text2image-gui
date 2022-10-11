using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class Dreambooth
    {
        static readonly string[] _validImgExtensions = new string[] { ".png", ".jpeg", ".jpg", ".jfif" };
        static readonly bool _useVisibleCmdWindow = false;
        static readonly float _learningRateMagicNumber = 0.18f;

        public static async Task<string> RunTraining(FileInfo baseModel, DirectoryInfo trainImgDir, string className, Enums.Dreambooth.TrainPreset preset)
        {
            try
            {
                Logger.ClearLogBox();

                bool showCmd = _useVisibleCmdWindow || OsUtils.ShowHiddenCmd();

                string name = trainImgDir.Name.Trunc(25, false);
                int cudaId = Config.GetInt("comboxCudaDevice") - 2;
                string logDir = Path.Combine(Paths.GetSessionDataPath(), "db", ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString());
                IoUtils.TryDeleteIfExists(logDir);
                Directory.CreateDirectory(logDir);

                Process p = OsUtils.NewProcess(!showCmd);
                p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat mb/envs/ldo && python {Constants.Dirs.RepoSd}/db/main.py -t " +
                    $"--base {WriteConfig(logDir, trainImgDir, preset).Wrap(true)} " +
                    $"--actual_resume {baseModel.FullName.Wrap(true)} " +
                    $"--name {name.Wrap()} " +
                    $"--logdir {logDir.Wrap(true)} " +
                    $"--gpus {cudaId}, " + // TODO: Support multi-GPU
                    $"--data_root {trainImgDir.FullName.Wrap(true)} " +
                    $"--reg_data_root {trainImgDir.FullName.Wrap(true)} " +
                    $"--class_word {className.Wrap()}";

                if (!showCmd)
                {
                    p.OutputDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Dreambooth); };
                    p.ErrorDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Dreambooth); };
                }

                Logger.Log($"Starting training on GPU {cudaId}.\nLog Folder: {logDir.Remove(Paths.GetExeDir())}");

                Logger.Log($"cmd {p.StartInfo.Arguments}", true);
                p.Start();

                if (!showCmd)
                {
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                }

                while (!p.HasExited) await Task.Delay(1);

                Logger.ClearLogBox();
                return ""; // outPath;
            }
            catch (Exception ex)
            {
                UiUtils.ShowMessageBox($"Training Error: {ex.Message}");
                Logger.Log(ex.StackTrace);
                return "";
            }
        }

        private static string WriteConfig (string logDir, DirectoryInfo trainDir, Enums.Dreambooth.TrainPreset preset)
        {
            string configPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.RepoSd, Constants.Dirs.Dreambooth, "configs", "stable-diffusion", "v1-finetune_unfrozen.yaml");
            var configLines = File.ReadAllLines(configPath).ToArray();

            var values = GetStepsAndLoggerIntervalAndLrMultiplier(preset);
            int targetSteps = values.Item1;
            int loggerInterval = values.Item2;
            float lrMultiplier = values.Item3;

            int trainImgs = IoUtils.GetFileInfosSorted(trainDir.FullName, false, "*.*").Where(x => _validImgExtensions.Contains(x.Extension.ToLower())).Count();
            double lr = trainImgs * _learningRateMagicNumber * 0.0000001 * lrMultiplier;
            configLines[1] = $"  base_learning_rate: {lr.ToString().Replace(",", ".")}";
            configLines[108] = $"      every_n_train_steps: {loggerInterval}";
            configLines[113] = $"        batch_frequency: {loggerInterval}";
            configLines[119] = $"    max_steps: {targetSteps}";

            string configOutPath = Path.Combine(logDir, "config.yaml");
            File.WriteAllLines(configOutPath, configLines);
            return configOutPath;
        }

        private static Tuple<int, int, float> GetStepsAndLoggerIntervalAndLrMultiplier (Enums.Dreambooth.TrainPreset preset)
        {
            if (preset == Enums.Dreambooth.TrainPreset.VeryHighQuality)
                return new Tuple<int, int, float> (4000, 1000, 1f);

            if (preset == Enums.Dreambooth.TrainPreset.HighQuality)
                return new Tuple<int, int, float>(2000, 500, 2f);

            if (preset == Enums.Dreambooth.TrainPreset.MedQuality)
                return new Tuple<int, int, float>(1000, 250, 4f);

            if (preset == Enums.Dreambooth.TrainPreset.LowQuality)
                return new Tuple<int, int, float>(250, 250, 16f);

            return new Tuple<int, int, float>(4000, 1000, 1f);
        }
    }
}
