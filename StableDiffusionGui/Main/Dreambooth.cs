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
        static readonly bool _onlySaveFinalCkpt = true;
        static readonly float _learningRateMagicNumber = 0.18f;

        public static int CurrentTargetSteps;

        public static async Task<string> RunTraining(FileInfo baseModel, DirectoryInfo trainImgDir, string className, Enums.Dreambooth.TrainPreset preset, float lrMultiplier = 1f)
        {
            CurrentTargetSteps = 0;

            try
            {
                Logger.ClearLogBox();

                TtiProcess.ProcessExistWasIntentional = true;
                ProcessManager.FindAndKillOrphans($"*dream.py*{Paths.SessionTimestamp}*");

                bool showCmd = _useVisibleCmdWindow || OsUtils.ShowHiddenCmd();

                string name = trainImgDir.Name.Trunc(25, false);
                int cudaId = (Config.GetInt("comboxCudaDevice") - 2).Clamp(0, 64);
                long timestamp = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                string logDir = Path.Combine(Paths.GetSessionDataPath(), "db", timestamp.ToString());
                IoUtils.TryDeleteIfExists(logDir);
                Directory.CreateDirectory(logDir);

                string configPath = WriteConfig(logDir, trainImgDir, preset, lrMultiplier);

                if (!File.Exists(configPath))
                    throw new Exception("Could not create training config.");

                string outPath = Path.Combine(Paths.GetModelsPath(), $"dreambooth-{className}-{CurrentTargetSteps}step-{timestamp}.ckpt");

                Process db = OsUtils.NewProcess(!showCmd);
                db.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat mb/envs/ldo && python {Constants.Dirs.RepoSd}/db/main.py -t " +
                    $"--base {configPath.Wrap(true)} " +
                    $"--actual_resume {baseModel.FullName.Wrap(true)} " +
                    $"--name {name.Wrap()} " +
                    $"--logdir {logDir.Wrap(true)} " +
                    $"--gpus {cudaId}, " + // TODO: Support multi-GPU
                    $"--data_root {trainImgDir.FullName.Wrap(true)} " +
                    $"--reg_data_root {trainImgDir.FullName.Wrap(true)} " +
                    $"--class_word {className.Wrap()} " +
                    $"&& python {Constants.Dirs.RepoSd}/scripts/prune_model.py -i {Path.Combine(logDir, "checkpoints", "last.ckpt").Wrap(true)} -o {outPath.Wrap(true)}";

                if (!showCmd)
                {
                    db.OutputDataReceived += (sender, line) => { DreamboothOutputHandler.Log(line?.Data); };
                    db.ErrorDataReceived += (sender, line) => { DreamboothOutputHandler.Log(line?.Data, true); };
                }

                Logger.Log($"Starting training on GPU {cudaId}.\nLog Folder: {logDir.Remove(Paths.GetExeDir())}\nLoading...");

                DreamboothOutputHandler.Start();
                Logger.Log($"cmd {db.StartInfo.Arguments}", true);
                db.Start();
                OsUtils.AttachOrphanHitman(db);

                if (!showCmd)
                {
                    db.BeginOutputReadLine();
                    db.BeginErrorReadLine();
                }

                while (!db.HasExited) await Task.Delay(1);

                IoUtils.TryDeleteIfExists(Path.Combine(logDir, "checkpoints", "last.ckpt"));
                IoUtils.TryDeleteIfExists(Path.Combine(logDir, "testtube"));

                // Logger.ClearLogBox();
                return outPath;
            }
            catch (Exception ex)
            {
                UiUtils.ShowMessageBox($"Training Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                return "";
            }

            Program.MainForm.SetProgress(0);
        }

        private static string WriteConfig (string logDir, DirectoryInfo trainDir, Enums.Dreambooth.TrainPreset preset, float userlrMult)
        {
            string configPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.RepoSd, Constants.Dirs.Dreambooth, "configs", "stable-diffusion", "v1-finetune_unfrozen.yaml");
            var configLines = File.ReadAllLines(configPath).ToArray();

            var values = GetStepsAndLoggerIntervalAndLrMultiplier(preset);
            int targetSteps = values.Item1;
            int loggerInterval = values.Item2;
            float lrMultiplier = values.Item3;

            CurrentTargetSteps = targetSteps;

            var filesInTrainDir = IoUtils.GetFileInfosSorted(trainDir.FullName, false, "*.*");
            int trainImgs = filesInTrainDir.Where(x => _validImgExtensions.Contains(x.Extension.ToLower())).Count();

            if (trainImgs < 1)
            {
                Logger.Log($"Error: Training folder does not contain any valid images. Currently supported are {string.Join(", ", _validImgExtensions.Select(x => x.Substring(1).ToUpperInvariant()))}.");
                return "";
            }

            if (filesInTrainDir.Count() > trainImgs)
            {
                Logger.Log($"Error: Training folder contains invalid files. Currently supported are {string.Join(", ", _validImgExtensions.Select(x => x.Substring(1).ToUpperInvariant()))}.");
                return "";
            }

            double lr = trainImgs * _learningRateMagicNumber * 0.0000001 * lrMultiplier * userlrMult;
            string lrStr = lr.ToString().Replace(",", ".");

            configLines[1] = $"  base_learning_rate: {lrStr}";
            configLines[95] = $"        repeats: 100";
            configLines[108] = $"      every_n_train_steps: {(_onlySaveFinalCkpt ? targetSteps + 1 : loggerInterval)}";
            configLines[113] = $"        batch_frequency: {loggerInterval}";
            configLines[119] = $"    max_steps: {targetSteps}";

            Logger.Log($"Training on {trainImgs} images to {targetSteps} steps using learning rate of {lrStr}.");

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
