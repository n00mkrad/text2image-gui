using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    internal class ComfyLogHandler
    {
        private static Comfy _instance;

        private static bool _hasErrored = false;
        public static TtiSettings LastSettings = null;

        private static int _progressTargetIters = 1;
        private static int _progressIter = 0;

        public static void HandleOutput(Comfy instance, string line)
        {
            _instance = instance;

            if (TextToImage.CurrentTaskSettings == null || line == null)
                return;

            if (line.Contains("Setting up MemoryEfficientCrossAttention"))
                return;

            if (line.Trim().StartsWith("left over keys:"))
                line = "left over keys: dict_keys([...])";

            if (line.StartsWith("PREVIEW_JPEG:b'")) // Decode base64 encoded JPEG preview
            {
                byte[] imageBytes = Convert.FromBase64String(line.Split('\'')[1]);
                Console.WriteLine($"Received preview {(imageBytes.Length / 1024f).RoundToInt()}k");

                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Image image = Image.FromStream(ms, true);
                    Program.MainForm.pictBoxPreview.Image = image;
                }

                return;
            }

            if (line.StartsWith("PREVIEW_WEBP:b'")) // Decode base64 encoded WEBP preview
            {
                Console.WriteLine($"Received WEBP preview {((line.Length * sizeof(Char)) / 1024f).RoundToInt()}k");
                var magick = ImgUtils.GetMagickImage(line.Split('\'')[1]);

                if (magick != null)
                    Program.MainForm.pictBoxImgViewer.Image = ImgUtils.ToBitmap(magick);

                return;
            }

            Logger.Log(line, true, false, Constants.Lognames.Sd);
            TtiProcessOutputHandler.LastMessages.Insert(0, line);

            if (TextToImage.Canceled)
                return;

            bool ellipsis = Program.MainForm.LogText.EndsWith("...");
            string errMsg = "";
            // bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");
            bool lastLineGeneratedText = Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");
            TextToImage.CancelMode cancelMode = TextToImage.CancelMode.SoftKill;

            if (!TextToImage.Canceled && line.Trim() == "got prompt")
            {
                _progressIter = -1;
                ImageExport.TimeSinceLastImage.Restart();
            }

            if (!TextToImage.Canceled && line.Trim().StartsWith("Prompt executed in "))
            {
                _progressIter = -1;
            }

            // Info: Running sampler
            if (!TextToImage.Canceled && line.Trim().StartsWith("Sampling:"))
            {
                _progressIter = -1;
                _progressTargetIters = 1;
            }

            // Info: Running USDU - Set target iterations to tile count
            if (!TextToImage.Canceled && line.Trim().StartsWith("Tiles amount: "))
            {
                int tiles = line.Split("Tiles amount: ").Last().GetInt();
                _progressIter = -1;
                _progressTargetIters = tiles;
            }

            // Info: Start of new progress iteration
            if (!TextToImage.Canceled && line.Replace(" ", "").StartsWith("0%||0/"))
            {
                _progressIter += 1;
            }

            if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("B/s"))
            {
                if (!lastLineGeneratedText)
                    Logger.LogIfLastLineDoesNotContainMsg($"Generating...", replaceLastLine: ellipsis);

                int percent = line.Split("%|")[0].GetInt();

                Console.WriteLine($"Percent: {percent}");

                percent = ((float)percent / _progressTargetIters).RoundToInt(); // Divide by prog iteration count
                Console.WriteLine($"Scaled percent: {percent}");
                percent += ((100f / _progressTargetIters) * _progressIter).RoundToInt(); // Add already finished iterations prog
                Console.WriteLine($"Scaled percent plus previous: {percent}");

                if (percent >= 0 && percent <= 100)
                    Program.MainForm.SetProgressImg(percent);
            }

            // Info: Model type
            if (!TextToImage.Canceled && line.Trim().StartsWith("MODEL INFO:"))
            {
                var split = line.Split('|');
                string filename = split[0].Split("MODEL INFO:")[1].Trim();
                string modelType = split[1].Trim();
                string unetConfigJson = split[2].Trim();
                var mdlArch = Models.DetectModelType(modelType, unetConfigJson);
                Config.Instance.ModelSettings.GetPopulate(filename, new Models.ModelSettings()).Arch = mdlArch;
                Logger.Log($"Loaded '{filename.Trunc(100)}' - {Strings.ModelArch.Get(mdlArch.ToString(), true)}", false, Logger.LastUiLine.Contains(filename));

                string controlnetError = ComfyUtils.ControlnetCompatCheck(LastSettings.Controlnets, mdlArch);

                if (controlnetError.IsNotEmpty())
                {
                    errMsg = controlnetError;
                    _hasErrored = true;
                    cancelMode = TextToImage.CancelMode.ForceKill;
                }

                if (Program.MainForm.comboxModel.GetTextSafe() == filename && mdlArch != ModelArch.SdXlRefine)
                {
                    Program.MainForm.comboxModelArch.SetWithText(Config.Instance.ModelSettings[filename].Arch.ToString(), false, Strings.ModelArch);
                    Size res = Models.GetDefaultRes(mdlArch);

                    if (LastSettings.Res.Width < res.Width && LastSettings.Res.Height < res.Height)
                        Logger.Log($"Warning: The resolution {LastSettings.Res.Width}x{LastSettings.Res.Height} might lead to low quality results, the native resolution of this model is {res.AsString()}.");
                }
            }

            // Info: Image saved
            if (!TextToImage.Canceled && line.Trim().StartsWith("Saved image"))
            {
                string pfx = line.Split('\'')[1];
                string path = string.Join(":", line.Split(':').Skip(1)).Trim();

                if (pfx == "upscale")
                {
                    ImageViewer.AppendImage(path, ImageViewer.ImgShowMode.ShowLast, false);
                    Program.SetState(Program.BusyState.Standby);
                }
            }

            // Warning: Missing embedding
            if (!TextToImage.Canceled && line.Trim().StartsWith("warning, embedding:") && line.Trim().EndsWith("does not exist, ignoring"))
            {
                string embName = line.Split("embedding:")[1].Split(' ')[0];
                Logger.Log($"Warning: Embedding '{embName}' not found!");
            }

            // Warning: Incompatible embedding
            if (!TextToImage.Canceled && line.Trim().StartsWith("WARNING: shape mismatch when trying to apply embedding"))
            {
                Logger.Log($"Warning: One or more embeddings were ignored because they are not compatible with the selected model.");
            }

            // Warning: Upscaling failure
            if (!TextToImage.Canceled && line.Contains("Upscaling failed!"))
            {
                Logger.Log($"Warning: Image upscaling failed. Maybe your model is incompatible?");
            }

            // Error: Port in use
            if (!_hasErrored && !TextToImage.Canceled && line.Trim().StartsWith("OSError: [Errno 10048]"))
            {
                errMsg = $"Port is already in use. Are you running ComfyUI on port {Comfy.ComfyPort}?";
                _hasErrored = true;
                cancelMode = TextToImage.CancelMode.ForceKill;
            }

            // Error: Incompatible model
            if (!_hasErrored && !TextToImage.Canceled && line.StartsWith("Failed to load model:"))
            {
                errMsg = $"{line}\n\nIt might be incompatible.";
                _hasErrored = true;
            }

            // Error: Shapes
            if (!_hasErrored && !TextToImage.Canceled && line.Contains("shapes cannot be multiplied"))
            {
                errMsg = $"{line}\n\nThis most likely means that certain models (e.g. LoRAs) are not compatible with the selected Stable Diffusion model.";
                _hasErrored = true;
            }

            TtiProcessOutputHandler.HandleLogGeneric(_instance, line, _hasErrored, cancelMode, errMsg, true);
        }

        public static void ResetLogger()
        {
            _hasErrored = false;
            _instance?.LastMessages.Clear();
        }
    }
}
