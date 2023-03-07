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

        public static async Task Install (MdlRelease release, bool copyImages, bool copyModels, bool copyConfig)
        {
            string tempDir = Paths.GetExeDir() + "-update";

            try
            {
                IoUtils.TryDeleteIfExists(tempDir);
                Directory.CreateDirectory(tempDir);

                bool moveFilesSuccess = await Task.Run(() => Copy(tempDir, copyImages, copyModels, copyConfig));
                Logger.Log($"Move success: {moveFilesSuccess}");

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
            catch(Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                IoUtils.TryDeleteIfExists(tempDir);
            }
        }

        private static bool Copy (string newInstallPath, bool images, bool models, bool config)
        {
            if (images)
            {
                try
                {
                    string defaultImgsPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Images);
                    string currentImgsPath = Config.Get<string>(Config.Keys.OutPath);

                    string targetImgsPath = Path.Combine(newInstallPath, Constants.Dirs.Images);

                    MoveDirIfInsideInstallFolder(defaultImgsPath, targetImgsPath);

                    if (new DirectoryInfo(currentImgsPath).FullName != new DirectoryInfo(defaultImgsPath).FullName)
                        MoveDirIfInsideInstallFolder(currentImgsPath, targetImgsPath);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error moving images: {ex.Message}");
                    return false;
                }
            }

            if (models)
            {
                try
                {
                    foreach (string dir in Paths.GetAllModelDirs())
                    {
                        if (!Directory.Exists(dir))
                            continue;

                        var dirInfo = new DirectoryInfo(dir);
                        MoveDirIfInsideInstallFolder(dirInfo.FullName, Path.Combine(newInstallPath, Constants.Dirs.Data, Constants.Dirs.Models));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error moving models: {ex.Message}");
                    return false;
                }
            }

            if (config)
            {
                try
                {
                    string targetPath = Path.Combine(newInstallPath, Constants.Dirs.Data, Constants.Files.Config);
                    File.Copy(Path.Combine(Paths.GetDataPath(), Constants.Files.Config), targetPath);
                }
                catch(Exception ex)
                {
                    Logger.Log($"Error moving config: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        private static void MoveDirIfInsideInstallFolder (string source, string target)
        {
            source = new DirectoryInfo(source).FullName;

            if (!source.Contains(Paths.GetExeDir()))
                return;

            IoUtils.CopyDir(source, target, true);
        }

        private static void MoveFileIfInsideInstallFolder(string source, string target)
        {
            source = new FileInfo(source).FullName;

            if (!source.Contains(Paths.GetExeDir()))
                return;

            File.Move(source, target);
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
