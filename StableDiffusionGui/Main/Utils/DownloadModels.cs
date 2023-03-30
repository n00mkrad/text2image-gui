using StableDiffusionGui.Io;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main.Utils
{
    internal class DownloadModels
    {
        public static async Task DownloadModel(string repoId)
        {
            repoId = repoId.Replace("https://huggingface.co/", "").Replace("http://huggingface.co/", "").TrimEnd('/'); // In case some super smart user pasted an entire repo link
            string nameSafe = repoId.Replace(" ", "_").Replace("/", "-");
            string cachePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, $"{nameSafe.Trunc(30, false)}.tmp");
            string savePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Models.Root, nameSafe);
            savePath = IoUtils.GetAvailablePath(savePath);
            Logger.ClearLogBox();

            float diskSpace = IoUtils.GetFreeDiskSpaceGb(savePath);

            if (diskSpace < 5f)
            {
                Logger.Log($"Can't download model: Not enough disk space on {Path.GetPathRoot(savePath)} ({diskSpace.ToString("0.#")} GB).");
                return;
            }

            Logger.Log($"Checking if FP16 variant exists...");
            bool fp16 = await DoesUrlExist($"https://huggingface.co/{repoId}/tree/fp16"); // This URL will return 404 if there is no FP16 branch

            Logger.Log($"Downloading {(fp16 ? "FP16 " : "")}model to {savePath.Wrap()}. Estimated size: {(fp16 ? "2" : "4")} GB.");
            string rev = fp16 ? "--revision fp16" : "";
            string args = $"/C title Downloading {repoId} - Do not close this window! && cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && " +
                $"python {Constants.Dirs.SdRepo}\\scripts\\download_model.py -r {repoId.Wrap()} -c {cachePath.Wrap()} -s {savePath.Wrap()} {rev} && timeout 3";
            Logger.Log($"cmd {args}", true);
            Process.Start("cmd", args);
        }

        public static async Task<bool> DoesUrlExist(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    return response.StatusCode != System.Net.HttpStatusCode.NotFound;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
