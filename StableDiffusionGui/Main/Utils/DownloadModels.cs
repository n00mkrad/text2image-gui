using StableDiffusionGui.Io;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main.Utils
{
    internal class DownloadModels
    {
        private const string _main = "main"; // Default revision name

        public static async Task DownloadModel(string repoId, string rev = _main)
        {
            repoId = repoId.Replace("https://huggingface.co/", "").Replace("http://huggingface.co/", "").TrimEnd('/'); // Remove domain and trailing slashes
            var split = repoId.Split("/tree/");
            repoId = split.First();
            string revStr = split.Last(); // Check if it's an URL with a revision specified

            if (revStr.IsNotEmpty() && !revStr.Contains("/") && revStr != rev) // If /tree/ was followed by a valid revision, set it
                rev = revStr.Trim();

            Logger.ClearLogBox();

            Logger.Log($"Checking if model is a valid Diffusers model...");
            bool isDiffusersModel = await DoesUrlExist($"https://huggingface.co/{repoId}/tree/{rev}/unet"); // Verify that this repo contains a Diffusers model

            if (!isDiffusersModel)
            {
                Logger.Log("Can't download model: The repository does not seem to contain a Diffusers-format model.", false, Logger.LastUiLine.EndsWith("..."));
                return;
            }

            bool fp16 = rev == "fp16";

            if(!fp16 && rev == _main)
            {
                Logger.Log($"Checking if FP16 variant exists...");
                fp16 = await DoesUrlExist($"https://huggingface.co/{repoId}/tree/fp16"); // This URL will return 404 if there is no FP16 branch
            }

            if (fp16)
                rev = "fp16";

            string nameSafe = repoId.Replace(" ", "_").Replace("/", "-");

            if (rev != _main)
                nameSafe += $"-{rev}";

            string cachePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, $"{nameSafe.Trunc(30, false)}.tmp");
            string savePath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Models.Root, Constants.Dirs.Models.Ckpts, nameSafe);
            savePath = IoUtils.GetAvailablePath(savePath);

            float diskSpace = IoUtils.GetFreeDiskSpaceGb(savePath);

            if (diskSpace < 5f)
            {
                Logger.Log($"Can't download model: Not enough disk space on {Path.GetPathRoot(savePath)} ({diskSpace.ToString("0.#")} GB).");
                return;
            }

            Logger.Log($"Downloading {(fp16 ? "FP16 " : "")}model to {savePath.Wrap()}. Estimated size: {(fp16 ? "2" : "4")} GB.");
            string args = $"/C title Downloading {repoId} - Do not close this window! && cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && {Constants.Files.VenvActivate} && " +
                $"python \"{Constants.Dirs.SdRepo}\\scripts\\download_model.py\" -r {repoId.Wrap()} -c {cachePath.Wrap()} -s {savePath.Wrap()} --revision {rev} && timeout 3";
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
