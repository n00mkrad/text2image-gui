using StableDiffusionGui.Data;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class Updater
    {
        private static readonly string _urlBasefilesGit = "https://github.com/n00mkrad/text2image-gui-basefiles";
        private static readonly string _urlBuilds = "https://github.com/n00mkrad/text2image-gui/raw/main/builds";

        public static async Task Install(MdlRelease release)
        {
            Logger.ClearLogBox();
            IoUtils.Cleanup();
            string tempDir = IoUtils.GetAvailablePath(Path.Combine(Paths.GetExeDir() + "upd"), "_{0}");

            if (!InstallationStatus.HasGit)
            {
                Logger.Log($"Error: Can't install update because required files are missing. Try doing a fresh install.\n(Bundled Git installation not found)");
                return;
            }

            try
            {
                IoUtils.TryDeleteIfExists(tempDir);
                Logger.Log("Downloading base dependencies...", false, Logger.LastUiLine.EndsWith("..."));
                await Setup.Clone(_urlBasefilesGit, tempDir, release.HashBasefiles);
                string exePath = Path.Combine(tempDir, "StableDiffusionGui.exe");
                Logger.Log("Downloading executable...", false, Logger.LastUiLine.EndsWith("..."));
                await DownloadRelease(release, exePath);
                Directory.GetDirectories(tempDir, ".git", SearchOption.AllDirectories).ToList().ForEach(d => IoUtils.TryDeleteIfExists(d)); // Delete all .git dirs
                IoUtils.GetFilesSorted(tempDir, ".git*").ToList().ForEach(d => IoUtils.TryDeleteIfExists(d)); // Delete all .gitignore etc files
                Logger.Log("Installing update files...", false, Logger.LastUiLine.EndsWith("..."));
                string moveCmd = Move(tempDir);
                string launchCmd = GetLaunchCmd(release);
                Process p = OsUtils.NewProcess(true);
                p.StartInfo.Arguments = $"/C cd /D C:/ && timeout 2 {moveCmd} && {launchCmd} && rmdir /s/q {tempDir.Wrap().TrimEnd('\\')} ";
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

        private static async Task DownloadRelease(MdlRelease rel, string savePath)
        {
            if (Program.UserArgs.ContainsKey("custUpdExe") && Program.Debug) // Assuming exe dir is ...\text2image-gui\StableDiffusionGui\bin\x64\Debug
            {
                string exePath = Program.UserArgs.Get("custUpdExe").Trim('\"').Trim('\'');

                if (File.Exists(exePath))
                    File.Copy(exePath, savePath, true);
                else
                    Logger.Log($"Specified local exe path is not valid (file does not exist): '{exePath}'");
            }
            else
            {
                string url = Path.Combine(_urlBuilds, rel.Channel, rel.Version);
                await Download(url, savePath);
            }
        }

        private static string GetLaunchCmd(MdlRelease release)
        {
            bool up = InstallationStatus.HasSdUpscalers();
            string info = $"targetHash_{release.HashRepo}_oldHash_{Setup.GitCommit}_installedBasic_{InstallationStatus.IsInstalledBasic}";
            return $"{Paths.GetExe().Wrap()} -{Constants.Args.Install}={true} -{Constants.Args.UpdateDeps}={true} -{Constants.Args.InstallUpscalers}={up} -info={info}";
        }

        private static string Move(string updateDir)
        {
            string newDataDir = Path.Combine(updateDir, "Data");
            string cmd = "";

            // Remove existing data subdirs, overwriting is not a good idea for python etc, but keep dirs not in update
            List<string> rootSubdirs = new DirectoryInfo(updateDir).GetDirectories().Select(d => d.Name).ToList();
            List<string> dataSubdirs = new DirectoryInfo(newDataDir).GetDirectories().Select(d => d.Name).ToList();

            // Move data subdirs (delete existing first)
            foreach (string dataDir in dataSubdirs)
            {
                string oldPath = Path.Combine(Paths.GetDataPath(), dataDir);
                bool success = IoUtils.TryDeleteIfExists(oldPath);

                if (success)
                    IoUtils.TryMove(Path.Combine(newDataDir, dataDir), oldPath);

                if (IoUtils.GetDirSize(newDataDir, true) <= 0)
                    IoUtils.TryDeleteIfExists(newDataDir);
            }

            // Move other subdirs (keep existing, overwrite files)
            foreach (string dir in rootSubdirs)
            {
                string oldPath = Path.Combine(Paths.GetExeDir(), dir);
                IoUtils.TryMove(Path.Combine(updateDir, dir), oldPath);
            }

            var oldRootFiles = IoUtils.GetFilesSorted(Paths.GetExeDir(), false).ToList();
            oldRootFiles.ForEach(f => IoUtils.TryDeleteIfExists(f));

            foreach (string oldRootFile in oldRootFiles)
            {
                bool success = IoUtils.TryDeleteIfExists(oldRootFile);

                if (!success)
                    cmd += $" && del {oldRootFile.Wrap()}";
            }

            var newRootFiles = IoUtils.GetFilesSorted(updateDir, false).ToList();
            var renameUpdFilesPaths = new List<string>();

            foreach (string newRootFile in newRootFiles)
            {
                string targetPath = Path.Combine(Paths.GetExeDir(), Path.GetFileName(newRootFile));

                if (File.Exists(targetPath))
                {
                    string oldTargetPath = targetPath;
                    targetPath += ".upd";
                    cmd += $" && move {targetPath.Wrap()} {oldTargetPath.Wrap()}";
                }

                IoUtils.TryMove(newRootFile, targetPath);
            }

            return cmd;
        }

        private static bool MoveOld(string newInstallPath, bool images, bool models, bool config, bool repoAndVenv)
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
                    string currentImgsPath = Config.Instance.OutPath;

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
                    string targetPathCfg = Path.Combine(targetDataDir, Constants.Files.Config);
                    File.Copy(Path.Combine(Paths.GetDataPath(), Constants.Files.Config), targetPathCfg);

                    string targetPathIni = Path.Combine(newInstallPath, Constants.Files.Ini);
                    File.Copy(Path.Combine(Paths.GetExeDir(), Constants.Files.Ini), targetPathIni);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error moving config: {ex.Message}");
                    Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
                    return false;
                }
            }

            try
            {
                string targetPathPromptHistory = Path.Combine(targetDataDir, Constants.Files.PromptHistory);
                File.Copy(Path.Combine(Paths.GetDataPath(), Constants.Files.PromptHistory), targetPathPromptHistory);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error moving prompt history: {ex.Message}");
                Logger.Log(ex.StackTrace, true, false, Constants.Lognames.Installer);
                return false;
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
