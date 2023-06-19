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
using System.Threading.Tasks;
using ZetaLongPaths;
using System.IO;
using System.Text.RegularExpressions;
using StableDiffusionGui.Forms;

namespace StableDiffusionGui.Training
{
    internal class KohyaTraining
    {
        static readonly string[] _validImageExts = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".bmp" };
        static readonly bool _useVisibleCmdWindow = false;
        private static string _currentArchivalLogDir = "";

        public static async Task<string> TrainLora(Model baseModel, ZlpDirectoryInfo dataDir, string name, string caption, KohyaSettings s, bool verboseFilename, string outDir = "")
        {
            try
            {
                if (!Directory.Exists(outDir))
                    outDir = Paths.GetLorasPath();

                Logger.ClearLogBox();

                var images = IoUtils.GetFileInfosSorted(dataDir.FullName).Where(f => _validImageExts.Contains(f.Extension.Lower())).ToList();
                Assert.Check(images.Count > 0, "No valid images found!");

                int repeats = (int)Math.Ceiling((double)(s.Steps * s.BatchSize) / images.Count);

                TtiProcess.ProcessExistWasIntentional = true;
                bool showCmd = _useVisibleCmdWindow || OsUtils.ShowHiddenCmd();

                string timestamp = FormatUtils.GetUnixTimestamp();
                string configPath = Path.Combine(Paths.GetSessionDataPath(), $"loraCfg{timestamp}.toml");
                string configText = GetConfig(s.Resolution, dataDir.FullName, repeats);
                File.WriteAllText(configPath, configText);

                string lrStr = s.LearningRate.ToStringDot("0.######").Split('.').Last();
                string filename = $"{name}_{s.Steps}s_lr{lrStr}_{s.Resolution}px";

                if (verboseFilename)
                    filename += $"_loha_nd{s.NetworkDim}c{s.ConvDim}";

                string outPath = IoUtils.GetAvailablePath(Path.Combine(outDir, $"{filename}.safetensors"), "_{0}");
                filename = new FileInfo(outPath).GetNameNoExt();
                _currentArchivalLogDir = IoUtils.CreateDir(Path.Combine(Paths.GetLogPath(), $"train_{filename}"), true).FullName;
                string captionsDir = CreateDataset(images, caption);

                if(s.AgumentColor || s.AugmentFlip)
                {
                    s.CacheLatents = false;
                    Logger.Log($"Warning: Latents Caching has been disabled because it cannot be used with Color/Flip augmentations. This will increase VRAM usage.");
                }

                string args = $"--pretrained_model_name_or_path={baseModel.FullName.Wrap()} " +
                    $"--dataset_config={configPath.Wrap()} " +
                    $"--output_dir={outDir.Wrap()} " +
                    $"--output_name={filename} " +
                    $"--save_model_as=safetensors " +
                    $"--prior_loss_weight=1.0 " +
                    $"--max_train_steps={s.Steps} " +
                    $"--learning_rate={s.LearningRate.ToStringDot("0.########")} " +
                    $"--mixed_precision={s.TrainFormat} " +
                    $"--save_precision={s.TrainFormat} " +
                    $"{(s.CacheLatents ? "--cache_latents" : "")} " +
                    $"{(s.GradientCheckpointing ? "--gradient_checkpointing" : "")} " +
                    $"{(s.AugmentFlip ? "--flip_aug" : "")} " +
                    $"{(s.AgumentColor ? "--color_aug" : "")} " +
                    $"{(s.ShuffleCaption ? "--shuffle_caption" : "")} " +
                    $"--save_every_n_epochs=999 " +
                    $"--network_module=lycoris.kohya " +
                    $"--network_dim={s.NetworkDim} " +
                    $"--network_alpha={s.NetworkAlpha} " +
                    $"--seed={s.Seed} " +
                    $"--clip_skip={s.ClipSkip} " +
                    $"--network_args \"conv_dim={s.ConvDim}\" \"conv_alpha={s.ConvAlpha}\" \"dropout={s.Dropout}\" \"algo={s.Algo}\"";

                if (InputUtils.IsHoldingShift)
                {
                    var form = new PromptForm("Edit LoRA Training Command", "LoRA Training Arguments:", args, 5f);

                    if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        args = form.EnteredText.Trim();
                }

                PatchScripts();
                Process py = OsUtils.NewProcess(!showCmd);
                py.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && " +
                    $"python \"{Constants.Dirs.SdRepo}\\sd-scripts\\train_network.py\" {args}";

                if (!showCmd)
                {
                    py.OutputDataReceived += (sender, line) => { Log(line?.Data); };
                    py.ErrorDataReceived += (sender, line) => { Log(line?.Data, true); };
                }

                Logger.Log($"Starting training.\nLoading...");

                DreamboothOutputHandler.Start();
                Logger.Log($"cmd {py.StartInfo.Arguments}", true);
                py.Start();
                TtiProcess.CurrentProcess = py;
                OsUtils.AttachOrphanHitman(py);
                StartLogging();

                if (!showCmd)
                {
                    py.BeginOutputReadLine();
                    py.BeginErrorReadLine();
                }

                string log = $"Base Model:\n{baseModel.FullName}\n\nSteps:\n{s.Steps}\n\nLearning Rate:\n{s.LearningRate.ToStringDot("0.########")}\n\nFull Command:\ncmd {py.StartInfo.Arguments}\n\n";
                File.WriteAllText(Path.Combine(_currentArchivalLogDir, "info.txt"), log);
                var sw = new NmkdStopwatch();
                while (!py.HasExited) await Task.Delay(100);

                if(!TextToImage.Canceled)
                Logger.Log($"Training has finished after {FormatUtils.Time(sw.ElapsedMilliseconds)}.");

                IoUtils.TryDeleteIfExists(captionsDir);

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

        /// <summary> Removes Japanese (or rather all non-UTF8) comments and prints from scripts, as this can cause text decoding errors </summary>
        private static void PatchScripts()
        {
            string root = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "sd-scripts");

            var files = IoUtils.GetFileInfosSorted(root, true, "*.py");
            var regex = new Regex(@"[^\u0000-\u007F]+", RegexOptions.Compiled);

            foreach (var file in files)
            {
                string text = File.ReadAllText(file.FullName);
                string newText = regex.Replace(text, "...");

                if (file.Name == "train_util.py")
                {
                    newText = newText.Replace("base_name = os.path.splitext(img_path)[0]", "base_name = os.path.join(os.path.dirname(img_path), \"captions\", os.path.splitext(os.path.basename(img_path))[0])"); // Use subfolder for captions
                    newText = newText.Replace("logging_dir=logging_dir,", ""); // Seems to break with accelerate==0.20.3
                }

                if (newText == text)
                    continue;

                File.WriteAllText(file.FullName, newText);
                Logger.Log($"sd-scripts: Patched {file.FullName} chars", true);
            }
        }

        private static string CreateDataset(List<ZlpFileInfo> imageFiles, string caption)
        {
            if (imageFiles == null || !imageFiles.Any())
                return null;

            string tagsDir = Path.Combine(imageFiles.First().Directory.FullName, "captions");
            IoUtils.DeleteIfExists(tagsDir);

            if (caption != null && caption.IsEmpty())
                return null;

            Directory.CreateDirectory(tagsDir);

            foreach (var imgFile in imageFiles)
            {
                string txtPathTarget = Path.Combine(tagsDir, $"{Path.GetFileNameWithoutExtension(imgFile.FullName)}.txt");

                if(caption == null) // No caption from GUI, use TXTs
                {
                    string txtPathSource = Path.ChangeExtension(imgFile.FullName, "txt");

                    if (File.Exists(txtPathSource))
                        IoUtils.CopyTo(txtPathSource, tagsDir); // Copy existing tags
                }
                else
                {
                    File.WriteAllText(txtPathTarget, caption); // Write new tag file for all
                }
            }

            IoUtils.CopyDir(tagsDir, Path.Combine(_currentArchivalLogDir, "captions"));
            File.WriteAllText(Path.Combine(_currentArchivalLogDir, "images.txt"), string.Join("\n", imageFiles.Select(f => f.FullName)));

            return tagsDir;
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

            if (line.Contains("caching latents."))
                Logger.Log($"Caching dataset latents...", false, replace);

            if (line.Contains("import network module:"))
                Logger.Log($"Preparing training...", false, replace);

            // string lastLogLines = string.Join("\n", Logger.GetLastLines(Constants.Lognames.Training, 6));

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
                    Logger.Log($"Training (Step {step}/{stepsGoal} - {percent}%{(step >= 10 && remainingMs > 3000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")})...", false, replace);
            }

            if (line.Contains("saving checkpoint:"))
                Logger.Log($"Saving LoRA...", false, replace);

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