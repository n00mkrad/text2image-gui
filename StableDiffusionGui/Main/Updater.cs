using StableDiffusionGui.Data;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class Updater
    {
        private static readonly string _basefilesGitUrl = "https://github.com/n00mkrad/text2image-gui-basefiles";

        public static async Task Install(MdlRelease release, bool copyImages, bool copyModels, bool copyConfig)
        {
            Logger.ClearLogBox();
            string tempDir = IoUtils.GetAvailableFilePath(Paths.GetExeDir().TrimEnd('\\') + "-upd");

            if (!InstallationStatus.HasBins())
            {
                Logger.Log($"Error: Can't install update because required files are missing.");
                return;
            }

            try
            {
                IoUtils.TryDeleteIfExists(tempDir);
                Logger.Log("Downloading base dependencies...", false, Logger.LastUiLine.EndsWith("..."));
                await Setup.Clone(_basefilesGitUrl, tempDir, release.HashBasefiles);
                string exePath = Path.Combine(tempDir, "StableDiffusionGui.exe");
                Logger.Log("Downloading executable...", false, Logger.LastUiLine.EndsWith("..."));
                await Download($"https://github.com/n00mkrad/text2image-gui/raw/main/builds/{release.Channel}/{release.Version}", exePath);
                string gitPath = Path.Combine(tempDir, ".git");
                IoUtils.SetAttributes(gitPath);
                IoUtils.TryDeleteIfExists(gitPath);
                Logger.Log("Moving files...", false, Logger.LastUiLine.EndsWith("..."));
                bool moveFilesSuccess = await Task.Run(() => Move(tempDir, copyImages, copyModels, copyConfig, release.HashRepo == Setup.GitCommit));
                Logger.Log($"Move success: {moveFilesSuccess}", true);
                Logger.Log("Preparing to start new version and delete old files...", false, Logger.LastUiLine.EndsWith("..."));
                string launchCmd = GetLaunchCmd(release);
                Process p = OsUtils.NewProcess(true);
                p.StartInfo.Arguments = $"/C cd /D C:/ && timeout 2 && rmdir /s/q {Paths.GetExeDir().Wrap().TrimEnd('\\')} && move {tempDir.TrimEnd('\\').Wrap()} {Paths.GetExeDir().TrimEnd('\\').Wrap()} && {launchCmd}";
                p.Start();
                Program.SetState(Program.BusyState.Standby);
                Program.MainForm.Close();
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
                IoUtils.TryDeleteIfExists(tempDir);
            }
        }

        private static string GetLaunchCmd (MdlRelease release)
        {
            if(release.HashRepo == Setup.GitCommit && InstallationStatus.IsInstalledBasic) // Do not re-install dependencies if they are installed and up-to-date
                return $"{Paths.GetExe().Wrap()} -info=no_reinstall_necessary";

            bool onnx = InstallationStatus.HasOnnx();
            bool up = InstallationStatus.HasSdUpscalers();
            string info = $"targetHash_{release.HashRepo}_oldHash_{Setup.GitCommit}_installedBasic_{InstallationStatus.IsInstalledBasic}";
            return $"{Paths.GetExe().Wrap()} -{Constants.Args.Install}={true} -{Constants.Args.InstallOnnx}={onnx} -{Constants.Args.InstallUpscalers}={up} -info={info}";
        }

        private static bool Move(string newInstallPath, bool images, bool models, bool config, bool repoAndVenv)
        {
            string targetDataDir = Directory.CreateDirectory(Path.Combine(newInstallPath, Constants.Dirs.Data)).FullName;

            try
            {
                string cachePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root);

                if (Directory.Exists(cachePath))
                    Directory.Move(cachePath, Path.Combine(targetDataDir, Constants.Dirs.Cache.Root));
            }
            catch (Exception ex)
            {
                Logger.Log($"Error moving cache: {ex.Message}", true);
                Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
            }

            try
            {
                string wildcardsPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Wildcards);

                if (Directory.Exists(wildcardsPath))
                {
                    foreach (var wc in IoUtils.GetFileInfosSorted(wildcardsPath, false, "*.*"))
                    {
                        string targetPath = Path.Combine(newInstallPath, Constants.Dirs.Wildcards, wc.Name);

                        if (!File.Exists(targetPath)) // Only copy additional files, otherwise it would be impossible to update existing wc files
                            wc.MoveTo(targetPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error copying wildcards: {ex.Message}", true);
                Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
            }

            if (images)
            {
                try
                {
                    string defaultImgsPath = Path.Combine(Paths.GetExeDir(), Constants.Dirs.Images);
                    string currentImgsPath = Config.Get<string>(Config.Keys.OutPath);

                    string targetImgsPath = Path.Combine(newInstallPath, Constants.Dirs.Images);

                    if (Directory.Exists(defaultImgsPath))
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
                    foreach (string dir in Models.GetAllModelDirs())
                    {
                        if (!Directory.Exists(dir))
                            continue;

                        var dirInfo = new DirectoryInfo(dir);
                        MoveDirIfInsideInstallFolder(dirInfo.FullName, Path.Combine(targetDataDir, Constants.Dirs.Models.Root));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error moving models: {ex.Message}");
                    Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
                    return false;
                }
            }

            if (repoAndVenv)
            {
                IoUtils.MoveDir(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo), Path.Combine(targetDataDir, Constants.Dirs.SdRepo));
                IoUtils.MoveDir(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv), Path.Combine(targetDataDir, Constants.Dirs.SdVenv));
            }

            if (config)
            {
                try
                {
                    string targetPath = Path.Combine(targetDataDir, Constants.Files.Config);
                    File.Copy(Path.Combine(Paths.GetDataPath(), Constants.Files.Config), targetPath);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error moving config: {ex.Message}");
                    Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
                    return false;
                }
            }

            return true;
        }

        private static void MoveDirIfInsideInstallFolder(string source, string target)
        {
            source = new DirectoryInfo(source).FullName;

            if (!source.Contains(Paths.GetExeDir()))
                return;

            Logger.Log($"Directory Move: {source} => {target}", true, false, Constants.Lognames.Installer);
            IoUtils.MoveDir(source, target);
        }

        private static void MoveFileIfInsideInstallFolder(string source, string target)
        {
            source = new FileInfo(source).FullName;

            if (!source.Contains(Paths.GetExeDir()))
                return;

            Logger.Log($"File Move: {source} => {target}", true, false, Constants.Lognames.Installer);
            File.Move(source, target);
        }

        public static async Task Download(string url, string savePath)
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
