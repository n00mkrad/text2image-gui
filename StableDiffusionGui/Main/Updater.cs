using StableDiffusionGui.Data;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
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
            string tempDir = IoUtils.GetAvailableFilePath(Paths.GetExeDir().TrimEnd('\\') + "-upd");

            try
            {
                IoUtils.TryDeleteIfExists(tempDir);
                Directory.CreateDirectory(tempDir);

                await Setup.Clone(_basefilesGitUrl, tempDir, release.HashBasefiles);
                string exePath = Path.Combine(tempDir, "StableDiffusionGui.exe");
                await Download($"https://github.com/n00mkrad/text2image-gui/raw/main/builds/{release.Channel}/{release.Version}.exe", exePath);
                string gitPath = Path.Combine(tempDir, ".git");
                IoUtils.SetAttributes(gitPath);
                IoUtils.TryDeleteIfExists(gitPath);

                bool moveFilesSuccess = await Task.Run(() => Move(tempDir, copyImages, copyModels, copyConfig));
                Logger.Log($"Move success: {moveFilesSuccess}");

                bool onnx = InstallationStatus.HasOnnx();
                bool up = InstallationStatus.HasSdUpscalers();
                Process.Start(exePath, $"-{Constants.Args.Install}={true} -{Constants.Args.InstallOnnx}={onnx} -{Constants.Args.InstallUpscalers}={up}");
                Program.MainForm.Close();
            }
            catch(Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
                IoUtils.TryDeleteIfExists(tempDir);
            }
        }

        private static bool Move (string newInstallPath, bool images, bool models, bool config)
        {
            string targetDataDir = Directory.CreateDirectory(Path.Combine(newInstallPath, Constants.Dirs.Data)).FullName;

            try
            {
                string cachePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root);

                if(Directory.Exists(cachePath))
                    Directory.Move(cachePath, Path.Combine(targetDataDir, Constants.Dirs.Cache.Root));
            }
            catch (Exception ex)
            {
                Logger.Log($"Error moving cache: {ex.Message}", true);
                Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
            }

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
                    Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
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
                        MoveDirIfInsideInstallFolder(dirInfo.FullName, Path.Combine(targetDataDir, Constants.Dirs.Models));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error moving models: {ex.Message}");
                    Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
                    return false;
                }
            }

            if (config)
            {
                try
                {
                    string targetPath = Path.Combine(targetDataDir, Constants.Files.Config);
                    File.Copy(Path.Combine(Paths.GetDataPath(), Constants.Files.Config), targetPath);
                }
                catch(Exception ex)
                {
                    Logger.Log($"Error moving config: {ex.Message}");
                    Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
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

            Directory.Move(source, target);
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
