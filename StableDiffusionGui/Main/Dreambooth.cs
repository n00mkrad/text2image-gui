using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main.Utils;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZetaLongPaths;

namespace StableDiffusionGui.Main
{
    internal class Dreambooth
    {
        static readonly bool _useVisibleCmdWindow = false;
        static readonly bool _onlySaveFinalCkpt = true;
        static readonly float _learningRateMagicNumber = 0.18f;

        public static int CurrentTargetSteps;

        public static async Task<string> TrainDreamboothLegacy(Model baseModel, ZlpDirectoryInfo trainImgDir, string className, Enums.Training.TrainPreset preset, float lrMult = 1f, float stepsMult = 1f)
        {
            CurrentTargetSteps = 0;

            try
            {
                Logger.ClearLogBox();

                TtiProcess.ProcessExistWasIntentional = true;
                ProcessManager.FindAndKillOrphans($"*{Constants.Dirs.SdRepo}*.py*{Paths.SessionTimestamp}*");

                string name = trainImgDir.Name.Trunc(25, false);
                int cudaId = (Config.Instance.CudaDeviceIdx - 2).Clamp(0, 64);
                string timestamp = FormatUtils.GetUnixTimestamp();
                string logDir = Path.Combine(Paths.GetSessionDataPath(), "db", timestamp);
                IoUtils.TryDeleteIfExists(logDir);
                Directory.CreateDirectory(logDir);

                string configPath = await WriteConfig(logDir, trainImgDir, preset, lrMult, stepsMult);

                if (!File.Exists(configPath))
                    throw new Exception("Could not create training config.");

                string outPath = Path.Combine(Paths.GetModelsPath(false), $"dreambooth-{className}-{CurrentTargetSteps}step-{timestamp}.diff");

                Process py = OsUtils.NewProcess(true, logAction: DreamboothOutputHandler.Log);
                py.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && python {Constants.Dirs.SdRepo}/db/main.py -t " +
                    $"--base {configPath.Wrap(true)} " +
                    $"--actual_resume {baseModel.FullName.Wrap(true)} " +
                    $"--name {name.Wrap()} " +
                    $"--logdir {logDir.Wrap(true)} " +
                    $"--gpus {cudaId}, " + // TODO: Support multi-GPU
                    $"--data_root {trainImgDir.FullName.Wrap(true)} " +
                    $"--reg_data_root {trainImgDir.FullName.Wrap(true)} " +
                    $"--class_word {className.Wrap()} ";

                Logger.Log($"Starting training on GPU {cudaId}.\nLog Folder: {logDir.Remove(Paths.GetExeDir())}\nLoading...");

                DreamboothOutputHandler.Start();
                Logger.Log($"cmd {py.StartInfo.Arguments}", true);
                OsUtils.StartProcess(py, killWithParent: true);

                await OsUtils.WaitForProcessExit(py, 100);

                Model finalCkpt = new Model(Path.Combine(logDir, "checkpoints", "last.ckpt"));
                await ConvertModels.Convert(Enums.Models.Format.Pytorch, Enums.Models.Format.Diffusers, finalCkpt, true, true, outPath);

                IoUtils.TryDeleteIfExists(finalCkpt.FullName);
                IoUtils.TryDeleteIfExists(Path.Combine(logDir, "testtube"));

                Program.MainForm.SetProgress(0);
                return outPath;
            }
            catch (Exception ex)
            {
                UiUtils.ShowMessageBox($"Training Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                Program.MainForm.SetProgress(0);
                return "";
            }
        }

        private static async Task<string> WriteConfig (string logDir, ZlpDirectoryInfo trainDir, Enums.Training.TrainPreset preset, float userlrMult, float userStepsMult)
        {
            string configPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, Constants.Dirs.Dreambooth, "configs", "stable-diffusion", "v1-finetune_unfrozen.yaml");
            var configLines = File.ReadAllLines(configPath).ToArray();

            var values = GetStepsAndLoggerIntervalAndLrMultiplier(preset);
            int targetSteps = (values.Item1 * userStepsMult).RoundToInt();
            int loggerInterval = values.Item2;
            float lrMultiplier = values.Item3;

            CurrentTargetSteps = targetSteps;

            var filesInTrainDir = IoUtils.GetFileInfosSorted(trainDir.FullName, false, "*.*");
            int trainImgs = filesInTrainDir.Where(x => Constants.FileExts.ValidImages.Contains(x.Extension.Lower())).Count();

            if (trainImgs < 1)
            {
                Logger.Log($"Error: Training folder does not contain any valid images. Currently supported are {string.Join(", ", Constants.FileExts.ValidImages.Select(x => x.Substring(1).ToUpperInvariant()))}.");
                return "";
            }

            var validateResult1Valid2ContainsFixable = await ValidateImages(trainDir.FullName);

            if(!validateResult1Valid2ContainsFixable.Item1)
            {
                if (validateResult1Valid2ContainsFixable.Item2)
                {
                    DialogResult dialogResult = UiUtils.ShowMessageBox($"Your training folder contains files in the correct format, but they need to be resized.\n" +
                    $"Do you want to do this automatically now? The files will be overwritten!", UiUtils.MessageType.Warning.ToString(), MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        await FormatImages(trainDir.FullName);
                        Logger.Log("Images have been formatted. Checking dataset again...");

                        if (!(await ValidateImages(trainDir.FullName)).Item1)
                            return "";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
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

        public static Tuple<int, int, float> GetStepsAndLoggerIntervalAndLrMultiplier (Enums.Training.TrainPreset preset)
        {
            if (preset == Enums.Training.TrainPreset.VeryHighQuality)
                return new Tuple<int, int, float> (4000, 1000, 1f);

            if (preset == Enums.Training.TrainPreset.HighQuality)
                return new Tuple<int, int, float>(2000, 500, 2f);

            if (preset == Enums.Training.TrainPreset.MedQuality)
                return new Tuple<int, int, float>(1000, 250, 4f);

            if (preset == Enums.Training.TrainPreset.LowQuality)
                return new Tuple<int, int, float>(250, 250, 16f);

            return new Tuple<int, int, float>(4000, 1000, 1f);
        }

        /// <returns> Bool1: All Valid - Bool2: Contains fixable images </returns>
        public static async Task<Tuple<bool, bool>> ValidateImages (string dir)
        {
            var files = IoUtils.GetFileInfosSorted(dir, false, "*.*").Where(x => Constants.FileExts.ValidImages.Contains(x.Extension.Lower())).ToList();

            bool allValid = true;
            bool containsFixableImages = false;

            foreach(var file in files)
            {
                // Check extension
                if (!Constants.FileExts.ValidImages.Contains(file.Extension.ToLowerInvariant()))
                {
                    Logger.Log($"File {file.Name} is invalid: Invalid file type. Supported are {string.Join(", ", Constants.FileExts.ValidImages.Select(x => x.Substring(1).ToUpperInvariant()))}");
                    allValid = false;
                    continue;
                }

                Image img = null;

                try
                {
                    img = IoUtils.GetImage(file.FullName);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error loading {file.Name}: {ex.Message}");
                }

                // Check if image can be loaded
                if (img == null)
                {
                    Logger.Log($"Image {file.Name} is invalid: Can't load image.");
                    allValid = false;
                    continue;
                }

                // Check dimensions
                if (img.Width != 512 || img.Height != 512)
                {
                    Logger.Log($"Image {file.Name} is invalid: Resolution is {img.Width}x{img.Height}, but training currently requires 512x512 images.");
                    containsFixableImages = true;
                    allValid = false;
                    continue;
                }
            }

            return new Tuple<bool, bool> (allValid, containsFixableImages);
        }

        public static async Task FormatImages (string dir)
        {
            var files = IoUtils.GetFileInfosSorted(dir, false, "*.*").Where(x => Constants.FileExts.ValidImages.Contains(x.Extension.Lower())).ToList();

            var opts = new ParallelOptions { MaxDegreeOfParallelism = (Environment.ProcessorCount / 2f).RoundToInt().Clamp(1, 12) }; // Thread count = Half the threads on this CPU, clamped to 1-12 (should be plenty for this...)
            int count = 0;

            Task imageProcessingTask = Task.Run(async () => Parallel.ForEach(files, opts, async file => {
                var scaledImg = await ImgUtils.Scale(file.FullName, ImgUtils.ScaleMode.LongerSide, 512, false);
                ImgUtils.Pad(scaledImg, new Size(512, 512), true);
                int currentCount = Interlocked.Increment(ref count);
                Logger.Log($"Processed {currentCount}/{files.Count} images...", false, Logger.LastUiLine.EndsWith("..."));
            }));

            Logger.Log($"Processed {files.Count} images.", false, Logger.LastUiLine.EndsWith("..."));

            while (!imageProcessingTask.IsCompleted) await Task.Delay(1);
        }
    }
}
