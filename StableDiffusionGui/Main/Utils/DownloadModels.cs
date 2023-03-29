using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System.Diagnostics;
using System.IO;

namespace StableDiffusionGui.Main.Utils
{
    internal class DownloadModels
    {
        public static void DownloadModel(string modelId)
        {
            string nameSafe = modelId.Replace(" ", "_").Replace("/", "-");
            string cachePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, $"{nameSafe.Trunc(30, false)}.tmp");
            string savePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Models.Root, nameSafe);

            float diskSpace = IoUtils.GetFreeDiskSpaceGb(savePath);

            if (diskSpace < 5f)
            {
                Logger.Log($"Can't download model: Not enough disk space on {Path.GetPathRoot(savePath)} ({diskSpace.ToString("0.#")} GB).");
                return;
            }

            Logger.Log($"Downloading model to {savePath.Wrap()}.");
            string args = $"/C title Downloading {modelId} && cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && " +
                $"python {Constants.Dirs.SdRepo}\\scripts\\download_model.py -r {modelId.Wrap()} -c {cachePath.Wrap()} -s {savePath.Wrap()} && timeout 60";
            Logger.Log($"cmd {args}", true);
            Process.Start("cmd", args);
        }
    }
}
