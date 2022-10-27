using LibGit2Sharp;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Installation
{
    internal class Setup
    {
        public static readonly string GitFile = "n00mkrad/stable-diffusion-cust.git";

        private static bool ReplaceUiLogLine { get { return Logger.LastUiLine.EndsWith("..."); } }

        public static async Task Install(bool force = false)
        {
            Logger.Log($"Installing (Force = {force})", true, false, Constants.Lognames.Installer);

            try
            {
                Program.MainForm.SetWorking(Program.BusyState.Installation);

                if (force || !InstallationStatus.HasSdRepo() || !InstallationStatus.HasSdEnv())
                {
                    if (!force)
                        Logger.Log("Install: Cloning repo and setting up env because either SD Repo or SD Env is missing.", true, false, Constants.Lognames.Installer);

                    await CloneSdRepo();
                }

                if (force || !InstallationStatus.HasSdModel())
                {
                    if (!force)
                        Logger.Log("Install: Downloading model file because there is none.", true, false, Constants.Lognames.Installer);

                    await DownloadSdModelFile();

                }

                if (force || !InstallationStatus.HasSdUpscalers())
                {
                    if (!force)
                        Logger.Log("Install: Downloading upscalers because they are not installed.", true, false, Constants.Lognames.Installer);

                    await InstallUpscalers();
                }

                RemoveGitFiles(GetDataSubPath(Constants.Dirs.RepoSd));

                await Task.Delay(500);

                if (InstallationStatus.IsInstalledBasic)
                {
                    Logger.Log("Finished. Everything is installed.");
                }
                else
                {
                    Logger.Log("Finished - Not everything could be installed. Installation log was copied to clipboard.");
                    OsUtils.SetClipboard(Logger.GetSessionLog("installation"));
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Install error: {ex.Message}\n{ex.StackTrace}");
            }

            Program.MainForm.SetWorking(Program.BusyState.Standby);
        }

        public static async Task SetupPythonEnv()
        {
            string repoPath = GetDataSubPath(Constants.Dirs.RepoSd);
            string batPath = Path.Combine(repoPath, "install.bat");

            List<string> l = new List<string>();

            l.Add($"@echo off");
            l.Add($"");
            l.Add($"cd {repoPath.Wrap()}");
            l.Add($"");
            l.Add($"echo Working dir: %cd%");
            l.Add($"");
            l.Add($"SET CONDA_ROOT_PATH=../{Constants.Dirs.Conda}");
            l.Add($"SET PYTHONHOME=../{Constants.Dirs.Conda}");
            l.Add($"SET CONDA_SCRIPTS_PATH=../{Constants.Dirs.Conda}/Scripts");
            l.Add($"");
            l.Add($"SET PATH={OsUtils.GetTemporaryPathVariable(new string[] { $"../{Constants.Dirs.Conda}", $"../{Constants.Dirs.Conda}/Scripts", $"../{Constants.Dirs.Conda}/condabin", $"../{Constants.Dirs.Conda}/Library/bin" })}");
            l.Add($"");
            l.Add($"_conda env create -f environment.yml -p \"%CONDA_ROOT_PATH%\\envs\\{Constants.Dirs.SdEnv}\"");
            l.Add($"_conda env update --file environment.yml --prune -p \"%CONDA_ROOT_PATH%\\envs\\{Constants.Dirs.SdEnv}\"");
            l.Add($"");
            l.Add($"rmdir /q /s \"%CONDA_ROOT_PATH%\\pkgs\"");
            l.Add($"call \"%CONDA_SCRIPTS_PATH%\\activate.bat\" \"%CONDA_ROOT_PATH%\\envs\\{Constants.Dirs.SdEnv}\"");

            File.WriteAllLines(batPath, l);

            Logger.Log("Running python environment installation script...");

            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd(), batPath);

            if (!OsUtils.ShowHiddenCmd())
            {
                p.OutputDataReceived += (sender, line) => { HandleInstallScriptOutput(line.Data, false); };
                p.ErrorDataReceived += (sender, line) => { HandleInstallScriptOutput(line.Data, true); };
            }

            p.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }

            while (!p.HasExited) await Task.Delay(1);

            RemoveGitFiles(GetDataSubPath(Constants.Dirs.RepoSd));
            Logger.Log("Done.");
        }

        private static void HandleInstallScriptOutput(string log, bool stderr)
        {
            if (string.IsNullOrWhiteSpace(log))
                return;

            log = log.Trim();

            Logger.Log($"{log.Remove("PRINTME ")}", !log.Contains("PRINTME "), false, Constants.Lognames.Installer);

            if (log.EndsWith("%") && log.Contains(" | "))
            {
                var split = log.Split(" | ");
                Logger.Log($"Installing {split.First().Trim()} ({split.Last().Trim()})", false, Logger.LastUiLine.EndsWith("%)"));
            }
        }

        public static async Task DownloadSdModelFile(bool force = false)
        {
            string mdlPath = Path.Combine(Paths.GetModelsPath(), "sd-v1-5-fp16.ckpt");
            bool hasModel = IoUtils.GetFileInfosSorted(Paths.GetModelsPath(), false, "*.ckpt").Where(x => x.Length == 2133058272).Any();

            if (hasModel && !force)
            {
                Logger.Log($"Model file already exists, won't redownload.");
                return;
            }

            IoUtils.TryDeleteIfExists(mdlPath);
            Logger.Log("Downloading model file...");

            Process p = OsUtils.NewProcess(true);
            p.ErrorDataReceived += (sender, line) => { try { Logger.Log($"Downloading... ({line.Data.Trim().Split(' ')[0]}%)", false, Logger.LastUiLine.EndsWith("%)"), Constants.Lognames.Installer); } catch { } };
            p.StartInfo.Arguments = $"/C curl -k \"https://dl.nmkd-hz.de/tti/sd/models/sd-v1-5-fp16.ckpt\" -o {mdlPath.Wrap()}";
            p.Start();
            p.BeginErrorReadLine();

            while (!p.HasExited)
                await Task.Delay(1);

            if (File.Exists(mdlPath))
                Logger.Log($"Model file downloaded ({FormatUtils.Bytes(new FileInfo(mdlPath).Length)}).");
            else
                Logger.Log($"Failed to download model file due to an unknown error. Check the log files.");
        }

        #region Git

        public static async Task CloneSdRepo()
        {
            TtiProcess.ProcessExistWasIntentional = true;
            ProcessManager.FindAndKillOrphans($"*invoke.py*{Paths.SessionTimestamp}*");
            await CloneSdRepo($"https://github.com/{GitFile}", GetDataSubPath(Constants.Dirs.RepoSd));
        }

        public static async Task CloneSdRepo(string url, string dir, string commit = "")
        {
            try
            {
                Logger.Log("Cloning repository...");

                if (Directory.Exists(dir))
                {
                    IoUtils.SetAttributes(dir, FileAttributes.Normal);
                    Directory.Delete(dir, true);
                }

                Task t = Task.Run(() => { Repository.Clone(url, dir, new CloneOptions() { BranchName = "main" }); });

                while (!t.IsCompleted)
                    await Task.Delay(1);

                if (!string.IsNullOrWhiteSpace(commit))
                {
                    using (var localRepo = new Repository(dir))
                    {
                        var localCommit = localRepo.Lookup<Commit>(commit);
                        Commands.Checkout(localRepo, localCommit);
                    }
                }

                Logger.Log($"Done cloning repository.");

                await SetupPythonEnv();
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to clone repository: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        public static void RemoveGitFiles(string rootPath)
        {
            List<string> dirs = new List<string>();

            dirs.AddRange(Directory.GetDirectories(rootPath, ".git", SearchOption.AllDirectories));

            foreach (string dir in dirs)
            {
                new DirectoryInfo(dir).Attributes = FileAttributes.Normal;
                IoUtils.SetAttributes(dir, FileAttributes.Normal);
                IoUtils.TryDeleteIfExists(dir);
            }

            string srcPath = Path.Combine(rootPath, "src");
            IoUtils.SetAttributes(srcPath, FileAttributes.Normal);
            IoUtils.GetFilesSorted(srcPath, true, "*.jpg").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x));
            IoUtils.GetFilesSorted(srcPath, true, "*.png").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x));
            IoUtils.GetFilesSorted(srcPath, true, "*.gif").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x));
            IoUtils.GetFilesSorted(srcPath, true, "*.ipynb").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x));
        }

        #endregion

        #region Upscaling Models

        public static async Task InstallUpscalers(bool print = true)
        {
            try
            {
                if (print) Logger.Log("Installing GFPGAN...", ReplaceUiLogLine);

                string gfpganPath = GetDataSubPath("gfpgan");
                IoUtils.SetAttributes(gfpganPath, FileAttributes.Normal);

                if (Directory.Exists(gfpganPath))
                    Directory.Delete(gfpganPath, true);

                Task t = Task.Run(() => { Repository.Clone(@"https://github.com/TencentARC/GFPGAN.git", gfpganPath, new CloneOptions() { BranchName = "master" }); });

                while (!t.IsCompleted)
                    await Task.Delay(1);

                using (var localRepo = new Repository(gfpganPath))
                {
                    var localCommit = localRepo.Lookup<Commit>("2eac2033893ca7f427f4035d80fe95b92649ac56");
                    Commands.Checkout(localRepo, localCommit);
                }

                if (print) Logger.Log("Downloading GFPGAN model file...", ReplaceUiLogLine);
                string gfpGanMdlPath = Path.Combine(gfpganPath, "gfpgan.pth");
                IoUtils.TryDeleteIfExists(gfpGanMdlPath);
                Process procGfpganDl = OsUtils.NewProcess(true);
                procGfpganDl.ErrorDataReceived += (sender, line) => { try { Logger.Log($"Downloading GFPGAN model ({line.Data.Trim().Split(' ')[0].GetInt()}%)...", false, ReplaceUiLogLine, Constants.Lognames.Installer); } catch { } };
                procGfpganDl.StartInfo.Arguments = $"/C curl -k -L \"https://github.com/TencentARC/GFPGAN/releases/download/v1.3.0/GFPGANv1.4.pth\" -o {gfpGanMdlPath.Wrap()}";
                procGfpganDl.Start();
                procGfpganDl.BeginErrorReadLine();

                while (!procGfpganDl.HasExited) await Task.Delay(1);

                string codeformerPath = GetDataSubPath("codeformer");
                Directory.CreateDirectory(codeformerPath);
                if (print) Logger.Log("Downloading Codeformer model file...", ReplaceUiLogLine);
                string codeformerMdlPath = Path.Combine(codeformerPath, "codeformer.pth");
                IoUtils.TryDeleteIfExists(codeformerMdlPath);
                Process procCodeformerDl = OsUtils.NewProcess(true);
                procCodeformerDl.ErrorDataReceived += (sender, line) => { try { Logger.Log($"Downloading CodeFormer model ({line.Data.Trim().Split(' ')[0].GetInt()}%)...", false, ReplaceUiLogLine, Constants.Lognames.Installer); } catch { } };
                procCodeformerDl.StartInfo.Arguments = $"/C curl -k -L \"https://github.com/sczhou/CodeFormer/releases/download/v0.1.0/codeformer.pth\" -o {codeformerMdlPath.Wrap()}";
                procCodeformerDl.Start();
                procCodeformerDl.BeginErrorReadLine();

                while (!procCodeformerDl.HasExited) await Task.Delay(1);

                await Task.Delay(100);

                Logger.Log($"Downloaded and installed RealESRGAN, CodeFormer, and GFPGAN.", false, ReplaceUiLogLine);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to install upscalers: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        #endregion

        #region Uninstall

        public static async Task RemoveRepo()
        {
            IoUtils.SetAttributes(GetDataSubPath(Constants.Dirs.RepoSd), FileAttributes.Normal);
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath(Constants.Dirs.RepoSd));
        }

        public static async Task RemoveEnv()
        {
            await IoUtils.TryDeleteIfExistsAsync(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv));
        }

        #endregion

        #region Utils

        public static void FixHardcodedPaths()
        {
            try
            {
                Logger.Log($"Fixing hardcoded paths in python files...", true);

                string parentDir = Path.Combine(GetDataSubPath(Constants.Dirs.Conda), "envs", Constants.Dirs.SdEnv, "Lib", "site-packages");
                var eggLinks = IoUtils.GetFileInfosSorted(parentDir, false, "*.egg-link");

                List<string> easyInstallPaths = new List<string>();

                foreach (FileInfo eggLink in eggLinks)
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(eggLink.FullName);

                    if (nameNoExt == "latent-diffusion")
                    {
                        string path = Path.Combine(GetDataSubPath(Constants.Dirs.RepoSd));
                        File.WriteAllText(eggLink.FullName, path + "\n.");
                    }
                    else
                    {
                        string path = Path.Combine(GetDataSubPath(Constants.Dirs.RepoSd), "src", nameNoExt);
                        File.WriteAllText(eggLink.FullName, path + "\n.");
                    }

                    Logger.Log($"Fixed egg-link file {eggLink.FullName}.", true);
                }

                var easyInstallPth = Path.Combine(parentDir, "easy-install.pth");

                if (File.Exists(easyInstallPth))
                {
                    var easyInstallLines = File.ReadAllLines(easyInstallPth);
                    List<string> newLines = new List<string>();

                    string splitText = $@"data\{Constants.Dirs.RepoSd}";
                    string newBasePath = Paths.GetExeDir().Lower().Replace("/", @"\");

                    Logger.Log($"easy-install.pth new lines:", true);

                    foreach (string line in easyInstallLines.Select(x => x.Lower()))
                    {
                        var split = line.Split(splitText);
                        newLines.Add(newBasePath + splitText + split.Last());
                        Logger.Log(newLines.Last(), true);
                    }

                    File.WriteAllLines(easyInstallPth, newLines);
                    Logger.Log($"Fixed easy-install.pth.", true);
                }
            }
            catch(Exception ex)
            {
                Logger.Log($"Error validating installation: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        private static string GetDataSubPath(string dir)
        {
            return Path.Combine(Paths.GetDataPath(), dir);
        }

        #endregion
    }
}
