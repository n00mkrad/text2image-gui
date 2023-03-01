using StableDiffusionGui.Data;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class Updater
    {
        private static readonly string _basefilesGitUrl = "https://github.com/n00mkrad/text2image-gui-basefiles";

        public static async Task Install (MdlRelease release)
        {
            //string tempDir = Path.Combine(Path.GetTempPath(), $"sdgui{FormatUtils.GetUnixTimestamp()}");
            string tempDir = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Update);
            IoUtils.TryDeleteIfExists(tempDir);
            Directory.CreateDirectory(tempDir);

            await Setup.Clone(_basefilesGitUrl, tempDir, release.HashBasefiles);
            string exePath = Path.Combine(tempDir, "StableDiffusionGui.exe");
            await Download($"https://github.com/n00mkrad/text2image-gui/raw/main/builds/{release.Channel}/{release.Version}.exe", exePath);
            string gitPath = Path.Combine(tempDir, ".git");
            IoUtils.SetAttributes(gitPath);
            IoUtils.TryDeleteIfExists(gitPath);

            bool onnx = InstallationStatus.HasOnnx();
            bool up = InstallationStatus.HasSdUpscalers();
            Process.Start(exePath, $"-{Constants.Args.Install}={true} -{Constants.Args.InstallOnnx}={onnx} -{Constants.Args.InstallUpscalers}={up}");
            Program.MainForm.Close();
        }

        public static async Task Download (string url, string savePath)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = File.Create(savePath))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}
