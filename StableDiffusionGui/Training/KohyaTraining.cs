using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main.Utils;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZetaLongPaths;
using System.IO;
using System.Text.RegularExpressions;

namespace StableDiffusionGui.Training
{
    internal class KohyaTraining
    {
        static readonly string[] _validImageExts = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".bmp" };
        static readonly bool _useVisibleCmdWindow = false;

        public static async Task<string> TrainLora(Model baseModel, ZlpDirectoryInfo dataDir, string name, string singleCaption, KohyaSettings s, bool verboseFilename, string outDir = "")
        {
            try
            {
                if (!Directory.Exists(outDir))
                    outDir = Paths.GetLorasPath();

                Logger.ClearLogBox();

                var images = IoUtils.GetFileInfosSorted(dataDir.FullName).Where(f => _validImageExts.Contains(f.Extension.Lower())).ToList(); ;
                Assert.Check(images.Count > 0, "No valid images found!");

                if (singleCaption.IsEmpty())
                {
                    Assert.Check(CheckIfCaptionsExist(images), "At least one image file is missing a caption file!");
                }

                var tempFiles = new List<string>();

                if (singleCaption.IsNotEmpty())
                    tempFiles = CreateSingleTagTxts(images, singleCaption);
                
                int repeats = (int)Math.Ceiling((double)(s.Steps * s.BatchSize) / images.Count);

                TtiProcess.ProcessExistWasIntentional = true;
                ProcessManager.FindAndKillOrphans($"*{Constants.Dirs.SdRepo}*.py*{Paths.SessionTimestamp}*");

                bool showCmd = _useVisibleCmdWindow || OsUtils.ShowHiddenCmd();

                string timestamp = FormatUtils.GetUnixTimestamp();
                string logDir = Path.Combine(Paths.GetLogPath(), "_training");
                Directory.CreateDirectory(logDir);

                string configPath = Path.Combine(Paths.GetSessionDataPath(), $"loraCfg{timestamp}.toml");
                string configText = GetConfig(s.Resolution, dataDir.FullName, repeats);
                File.WriteAllText(configPath, configText);

                string lrStr = s.LearningRate.ToStringDot("0.######").Split('.').Last();
                string filename = "";

                if (verboseFilename)
                    filename = $"{name}_{s.Steps}s_lr{lrStr}_{s.Resolution}px_loha_nd{s.NetworkDim}c{s.ConvDim}";
                else
                    filename = $"{name}_{s.Steps}s_lr{lrStr}_{s.Resolution}px";

                string outPath = IoUtils.GetAvailablePath(Path.Combine(outDir, $"{filename}.safetensors"), "_{0}");
                filename = new FileInfo(outPath).GetNameNoExt();

                PatchScripts();
                Process proc = OsUtils.NewProcess(!showCmd);
                proc.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && " +
                    $"python \"{Constants.Dirs.SdRepo}\\sd-scripts\\train_network.py\" " +
                    $"--pretrained_model_name_or_path={baseModel.FullName.Wrap()} " +
                    $"--dataset_config={configPath.Wrap()} " +
                    $"--output_dir={outDir.Wrap()} " +
                    $"--output_name={filename} " +
                    $"--save_model_as=safetensors " +
                    $"--prior_loss_weight=1.0 " +
                    $"--max_train_steps={s.Steps} " +
                    $"--learning_rate={s.LearningRate.ToStringDot("0.########")} " +
                    $"--mixed_precision=\"fp16\" " +
                    $"--save_precision=\"fp16\" " +
                    $"--cache_latents " +
                    $"--gradient_checkpointing " +
                    $"--save_every_n_epochs=999 " +
                    $"--network_module=lycoris.kohya " +
                    $"--network_dim={s.NetworkDim} " +
                    $"--network_alpha={s.NetworkAlpha} " +
                    $"--seed={s.Seed} " +
                    $"--clip_skip={s.ClipSkip} " +
                    $"--network_args \"conv_dim={s.ConvDim}\" \"conv_alpha={s.ConvAlpha}\" \"dropout={s.Dropout}\" \"algo={s.Algo}\"";
                //    $"--base {configPath.Wrap(true)} " +
                //    $"--actual_resume {baseModel.FullName.Wrap(true)} " +
                //    $"--name {name.Wrap()} " +
                //    $"--logdir {logDir.Wrap(true)} " +
                //    $"--gpus {cudaId}, " + // TODO: Support multi-GPU
                //    $"--data_root {trainImgDir.FullName.Wrap(true)} " +
                //    $"--reg_data_root {trainImgDir.FullName.Wrap(true)} " +
                //    $"--class_word {singleTag.Wrap()} ";

                if (!showCmd)
                {
                    proc.OutputDataReceived += (sender, line) => { Log(line?.Data); };
                    proc.ErrorDataReceived += (sender, line) => { Log(line?.Data, true); };
                }

                Logger.Log($"Starting training.\nLoading...");

                DreamboothOutputHandler.Start();
                Logger.Log($"cmd {proc.StartInfo.Arguments}", true);
                proc.Start();
                OsUtils.AttachOrphanHitman(proc);

                if (!showCmd)
                {
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                }

                while (!proc.HasExited) await Task.Delay(100);

                tempFiles.ForEach(path => IoUtils.TryDeleteIfExists(path));

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

        private static string GetConfig(int res, string dataDir, int repeats, int batchSize = 4, bool buckets = true, string tagExt = ".txt")
        {
            string content = "" +
                $"[general]\n" +
                $"enable_bucket = {buckets.ToString().Lower()}\n\n" +
                $"[[datasets]]\n" +
                $"resolution = {res.Clamp(512, 2048)}\n" +
                $"batch_size = {batchSize.Clamp(1, 128)}\n\n" +
                $"  [[datasets.subsets]]\n" +
                $"  image_dir = '{dataDir}'\n" +
                $"  caption_extension = '{tagExt}'\n" +
                $"  num_repeats = {repeats}" +
                $"";

            return content;
        }

        /// <summary> Removes Japanese comments and prints from scripts, as this can cause text decoding errors </summary>
        private static void PatchScripts ()
        {
            string root = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "sd-scripts");

            var files = IoUtils.GetFileInfosSorted(root, true, "*.py");

            foreach(var file in files)
            {
                string text = File.ReadAllText(file.FullName);
                string newText = Regex.Replace(text, @"[^\u0000-\u007F]+", "...");

                if (newText == text)
                    continue;

                File.WriteAllText(file.FullName, newText);
                Logger.Log($"sd-scripts: Patched {file.FullName} chars", true);
            }
        }

        /// <summary> Creates a TXT for each image file with the provided tag. </summary>
        /// <returns> The paths of the created files </returns>
        private static List<string> CreateSingleTagTxts (List<ZlpFileInfo> imageFiles, string tag)
        {
            var paths = new List<string>();

            foreach(var imgFile in imageFiles)
            {
                string txtPath = Path.ChangeExtension(imgFile.FullName, "txt");
                File.WriteAllText(txtPath, tag);
                paths.Add(txtPath);
            }

            return paths;
        }

        /// <summary> Checks if every image has a tag file. </summary>
        private static bool CheckIfCaptionsExist(List<ZlpFileInfo> imageFiles, string ext = ".txt")
        {
            foreach (var imgFile in imageFiles)
            {
                string txtPath = Path.ChangeExtension(imgFile.FullName, "txt");

                if (!File.Exists(txtPath))
                    return false;
            }

            return true;
        }

        private static bool _hasErrored = false;

        public static void StartLogging()
        {
            _hasErrored = false;
        }

        public static void Log(string line, bool stdErr = false)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            Logger.Log(line, true, false, Constants.Lognames.Training);

            if (!Program.Busy)
                return;

            bool ellipsis = Logger.LastUiLine.EndsWith("...");

            bool replace = ellipsis;

           if (line.Contains("Load dataset config from"))
               Logger.Log("Loading dataset...", false, replace);
           
           if (line.Contains("import network module:"))
               Logger.Log($"Preparing training...", false, replace);
           
           string lastLogLines = string.Join("\n", Logger.GetLastLines(Constants.Lognames.Training, 6));
           
           if (line.Trim().StartsWith("steps:"))
           {
                int percent = line.Split("%").First().GetInt();
                int step = line.Split(" [").First().Split(' ').Last().Split("/").First().GetInt(true);
                int stepsGoal = line.Split(" [").First().Split('/').Last().GetInt(true);

                if (percent > 0 && percent <= 100)
                   Program.MainForm.SetProgress(percent);
           
               string speed = line.Contains(", loss=") ? line.Split(", loss=").First().Split(' ').Last() : "";
               int remainingMs = speed.IsNotEmpty() ? (stepsGoal - step) * FormatUtils.IterationsToMsPerIteration(speed) : 0;
           
               if ((stepsGoal - step) > 1)
                   Logger.Log($"Training (Step {step}/{stepsGoal} - {percent}%{(step >= 5 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")})...", false, replace);
           }
           // 
           // if (line.Contains("Saving"))
           //     Logger.Log($"Saving checkpoint...", false, replace);
           // 
           // if (line.Contains("Pruning..."))
           //     Logger.Log($"Pruning model...", false, replace);

            if (line.MatchesWildcard("*%|*/*[*B/s]*") && !line.Lower().Contains("it/s") && !line.Lower().Contains("s/it"))
            {
                Logger.Log($"Downloading required files... {line.Trunc(80)}", false, ellipsis);
            }

            if (!_hasErrored && line.Contains("CUDA out of memory"))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Your GPU ran out of VRAM!\n\n{line.Split("If reserved memory is").FirstOrDefault()}", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && (line.Contains("PytorchStreamReader failed reading zip archive") || line.Contains("UnpicklingError")))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Your model file seems to be damaged or incomplete!\n\n{line}", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && line.Contains("usage: "))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Invalid CLI syntax.", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && line.Lower().Contains("illegal memory access"))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Your GPU appears to be unstable! If you have an overclock enabled, please disable it!\n\n{line}", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && (line.Contains("RuntimeError") || line.Contains("ImportError") || line.Contains("OSError") || line.Contains("KeyError") || line.Contains("ModuleNotFoundError")))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Python Error:\n\n{line}", UiUtils.MessageType.Error);
            }
        }
    }
}