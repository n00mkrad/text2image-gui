using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZetaLongPaths;

namespace StableDiffusionGui.Installation
{
    internal class Setup
    {
        private static readonly string _gitFile = "n00mkrad/stable-diffusion-cust.git";
        private static readonly string _gitBranch = "main";
        public static readonly string GitCommit = "beee6c43e6d476a11b5e3e27f3b2c45919d8f729";

        public static async Task Install(bool force = false, bool forceUpdateDeps = false, bool installUpscalers = true)
        {
            Logger.Log($"Installing (Force = {force} - Upscalers: {installUpscalers})", true, false, Constants.Lognames.Installer);
            Logger.ClearLogBox();

            try
            {
                Program.SetState(Program.BusyState.Installation);

                if (force || forceUpdateDeps || !InstallationStatus.HasSdRepo() || !InstallationStatus.HasSdEnv())
                {
                    if (!force)
                        Logger.Log("Install: Cloning repo and setting up env because either SD Repo or SD Env is missing.", true, false, Constants.Lognames.Installer);

                    bool installRepoSuccess = await InstallRepo("", force);

                    if (!installRepoSuccess)
                        throw new Exception("Repo installation failed. Check logs for details.");
                }

                if (force || (installUpscalers && !InstallationStatus.HasSdUpscalers()))
                {
                    if (!force)
                        Logger.Log("Install: Downloading upscalers because they are not installed.", true, false, Constants.Lognames.Installer);

                    await InstallUpscalers();
                }

                RepoCleanup();

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

            Program.SetState(Program.BusyState.Standby);
        }

        public static async Task SetupVenv(bool deleteVenvFirst = false)
        {
            Logger.Log("Checking python environment...");

            if (deleteVenvFirst)
            {
                bool clean = IoUtils.TryDeleteIfExists(GetDataSubPath(Constants.Dirs.SdVenv));

                if (!clean)
                {
                    Logger.Log("Failed to install python environment: Can't delete existing folder.");
                    return;
                }
            }

            var pkgs = await OsUtils.GetPythonPkgList(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv), false);
            var installLines = new List<string>();
            var depsBatLines = IoUtils.ReadLines(Path.Combine(GetDataSubPath(Constants.Dirs.SdRepo), "install-venv-deps-all.bat"));

            for(int i = 0; i < depsBatLines.Count(); i++)
            {
                var matches = Regex.Matches(depsBatLines[i], @"([a-zA-Z0-9_-]+==[a-zA-Z0-9_.+]+)", RegexOptions.Compiled);
                List<string> linePkgs = matches.Cast<Match>().Select(match => match.Value.ToString()).ToList();

                if (linePkgs.Any() && linePkgs.All(linePkg => pkgs.Contains(linePkg))) // Skip this line if we already have this version of this package installed
                    continue;

                installLines.Add(depsBatLines[i]);
            }

            Logger.Log($"Will install {installLines.Count} python packages.", false, Logger.LastUiLine.EndsWith("..."));

            string repoPath = GetDataSubPath(Constants.Dirs.SdRepo);
            string batPath = Path.Combine(repoPath, "install.bat");

            File.WriteAllText(batPath, $"@echo off\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"SET PATH={OsUtils.GetPathVar(new string[] { $@".\{Constants.Dirs.SdVenv}\Scripts", $@".\{Constants.Dirs.Python}\Scripts", $@".\{Constants.Dirs.Python}", $@".\{Constants.Dirs.Git}\cmd" })}\n" +
                $"SET HOME={Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "home")}\n" +
                $"python -m virtualenv {Constants.Dirs.SdVenv}\n" +
                $"\n{string.Join("\n", installLines)}\n\n" +
                $"");

            Process p = OsUtils.NewProcess(true, batPath, logAction: (s) => HandleInstallScriptOutput(s, false));
            OsUtils.StartProcess(p, killWithParent: true);
            await OsUtils.WaitForProcessExit(p);

            Logger.Log("Cleaning up...", false, Logger.LastUiLine.EndsWith("..."));

            if(!Config.Instance.DontClearPipCache)
                IoUtils.TryDeleteIfExists(Path.Combine(Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%"), "pip", "cache"));

            RepoCleanup();
            PatchUtils.PatchAllPkgs();
            Logger.Log("Done.", false, Logger.LastUiLine.EndsWith("..."));
        }

        private static void HandleInstallScriptOutput(string log, bool conda)
        {
            if (string.IsNullOrWhiteSpace(log))
                return;

            log = log.Trim();
            Logger.Log($"{log.Remove("PRINTME ")}", !log.Contains("PRINTME "), false, Constants.Lognames.Installer);

            if (!conda)
            {
                if (log.StartsWith("Downloading "))
                {
                    string filename = log.Split('/').Last().Split('(').FirstOrDefault().Remove("Downloading ").Trim().Trunc(150);
                    string filesize = log.Split(" (").Last().Split(')').FirstOrDefault().Trim();
                    Logger.Log($"Downloading {filename} ({filesize})...", false, Logger.LastUiLine.EndsWith("..."));
                }

                if (log.StartsWith("Collecting "))
                {
                    string name = log.Split("Collecting ")[1].Split("=")[0].Split("<")[0].Split(">")[0].Split("!")[0].Replace("git+", "").Replace("https://github.com/", "").Replace(".git", " (Git)");
                    name = name.Split("@")[0].Trim().Trunc(150, false);
                    Logger.Log($"Installing {name}...", false, Logger.LastUiLine.EndsWith("..."));
                }

                if (log.StartsWith("Processing "))
                {
                    string name = log.Split("Processing ")[1].Split('/').Last().Split('\\').Last();
                    Logger.Log($"Installing {name}...", false, Logger.LastUiLine.EndsWith("..."));
                }

                if (log.StartsWith("Installing collected packages: "))
                {
                    int count = log.Split("Installing collected packages: ").Last().Split(", ").Count(); // Count comma-separated packages
                    Logger.Log($"Installing {count} downloaded package{(count == 1 ? "" : "s")}...", false, Logger.LastUiLine.EndsWith("..."));
                }

                if (log.StartsWith("Building wheel"))
                {
                    Logger.Log($"Building packages...", false, Logger.LastUiLine.EndsWith("..."));
                }
            }
            else
            {
                if (log.EndsWith("%") && log.Contains(" | "))
                {
                    var split = log.Split(" | ");
                    Logger.Log($"Installing {split.First().Trim()} ({split.Last().Trim()})", false, Logger.LastUiLine.EndsWith("%)"));
                }
            }
        }

        #region Git

        public static async Task<bool> InstallRepo(string overrideCommit = "", bool cleanVenvInstall = false)
        {
            try
            {
                string commit = string.IsNullOrWhiteSpace(overrideCommit) ? GitCommit : overrideCommit;
                TtiProcess.ProcessExistWasIntentional = true;
                ProcessManager.FindAndKillOrphans($"*invoke.py*{Paths.SessionTimestamp}*");
                await CloneSdRepo($"https://github.com/{_gitFile}", GetDataSubPath(Constants.Dirs.SdRepo), _gitBranch, commit);
                await SetupVenv(cleanVenvInstall);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error installing repo: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                return false;
            }
        }

        public static async Task CloneSdRepo(string url, string dir, string branch = "main", string commit = "")
        {
            try
            {
                Logger.Log("Downloading repository...");
                await Clone(url, dir, commit, branch);
                Logger.Log($"Downloaded repository.", false, Logger.LastUiLine.EndsWith("..."));
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to download repository: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        public static async Task Clone(string gitUrl, string dir, string commit = "", string branch = "main")
        {
            if (Directory.Exists(dir))
            {
                IoUtils.SetAttributes(dir, ZetaLongPaths.Native.FileAttributes.Normal);
                Directory.Delete(dir, true);
            }

            string gitExe = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "cmd", "git.exe");
            Process p = OsUtils.NewProcess(true, logAction: (s) => HandleInstallScriptOutput($"[git] {s}", false));
            p.StartInfo.EnvironmentVariables["PATH"] = TtiUtils.GetEnvVarsSd(true, Paths.GetDataPath()).First().Value;
            p.StartInfo.Arguments = $"/C git clone --depth=1 --single-branch --branch {branch} {gitUrl} {dir.Wrap(true)} {(string.IsNullOrWhiteSpace(commit) ? "" : $"&& cd /D {dir.Wrap()} && git checkout {commit}")}";
            Logger.Log($"{p.StartInfo.FileName} {p.StartInfo.Arguments}", true);
            OsUtils.StartProcess(p, killWithParent: true);
            await OsUtils.WaitForProcessExit(p);
        }

        public static void RepoCleanup()
        {
            try
            {
                string repoPath = GetDataSubPath(Constants.Dirs.SdRepo);
                string sitePkgsPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Lib", "site-packages");

                var dirs = new List<ZlpDirectoryInfo>();

                dirs.AddRange(Directory.GetDirectories(repoPath, "*", SearchOption.AllDirectories).Select(x => new ZlpDirectoryInfo(x)));

                foreach (var dir in dirs)
                {
                    if (dir.Name == ".git")
                        IoUtils.TryDeleteIfExists(dir.FullName);
                }

                foreach (var dir in Directory.GetDirectories(sitePkgsPath, "*", SearchOption.TopDirectoryOnly).Select(x => new ZlpDirectoryInfo(x)))
                {
                    if (dir.Name.StartsWith("~"))
                        dir.Delete(true);
                }

                var unneededDirs = new List<string> { "docs", "assets" };
                unneededDirs.ForEach(dir => IoUtils.TryDeleteIfExists(Path.Combine(repoPath, dir)));

                var unneededFileTypes = new List<string> { "jpg", "jpeg", "png", "gif", "ipynb", "ttf" };
                unneededFileTypes.ForEach(ext => IoUtils.GetFilesSorted(repoPath, true, $"*.{ext}").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x)));

                IoUtils.TryDeleteIfExists(Path.Combine(sitePkgsPath, "pandas", "tests"));
                IoUtils.TryDeleteIfExists(Path.Combine(sitePkgsPath, "imageio", "resources", "images"));
                IoUtils.TryDeleteIfExists(GetDataSubPath("0.7.5"));
                IoUtils.GetFilesSorted(Path.Combine(sitePkgsPath, "cv2"), false, "opencv_videoio_*.dll").ToList().ForEach(f => IoUtils.TryDeleteIfExists(f));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, true, "RepoCleanup Exception: ");
            }
        }

        #endregion

        #region Upscaling Models

        public static async Task InstallUpscalers(bool forceReinstall = true)
        {
            try
            {
                Logger.ClearLogBox();
                Logger.Log($"Installing enhancement models...");

                if (forceReinstall)
                {
                    var dirs = new[] { "realesrgan", "gfpgan", "codeformer" }.ToList();
                    dirs.ForEach(dir => IoUtils.DeleteIfExists(Path.Combine(Paths.GetDataPath(), "invoke", "models", dir)));
                }

                void HandleOutput (string s)
                {
                    if (s.Contains("%|") && s.Trim().EndsWith("B/s]"))
                        Logger.Log($"Downloading {s.Trim().Split("%|")[0].Replace("  ", " ")}%", false, Logger.LastUiLine.EndsWith("%"), Constants.Lognames.Installer);
                }

                Process p = OsUtils.NewProcess(true, logAction: HandleOutput);

                p.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && {Constants.Files.VenvActivate} && " +
                    $"python {Constants.Dirs.SdRepo}/scripts/nmkd_install_upscalers.py";

                Logger.Log($"cmd {p.StartInfo.Arguments}", true);
                OsUtils.StartProcess(p, killWithParent: true);

                await OsUtils.WaitForProcessExit(p);

                if (InstallationStatus.HasSdUpscalers(Constants.Lognames.Installer))
                    Logger.Log($"All enhancement models downloaded.", false, true);
                else
                    Logger.Log($"Some model files could not be downloaded. Check the '{Constants.Lognames.Installer}' log for details.", false, true);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error downloading enhancement models: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        #endregion

        #region Uninstall

        public static async Task RemoveRepo()
        {
            IoUtils.SetAttributes(GetDataSubPath(Constants.Dirs.SdRepo), ZetaLongPaths.Native.FileAttributes.Normal);
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath(Constants.Dirs.SdRepo));
        }

        public static async Task RemoveEnv()
        {
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath(Constants.Dirs.SdVenv));
        }

        #endregion

        #region Utils

        public static void PatchFiles()
        {
            try
            {
                if (IoUtils.GetAmountOfFiles(Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv)), false) < 1)
                    return;

                Logger.Log($"Validating paths...", true);
                string pyvenvCfgPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "pyvenv.cfg");
                var pyvenvCfgLines = File.ReadAllLines(pyvenvCfgPath);

                for (int i = 0; i < pyvenvCfgLines.Length; i++)
                {
                    if (pyvenvCfgLines[i].StartsWith("home = ")) pyvenvCfgLines[i] = $"home = {GetDataSubPath(Constants.Dirs.Python)}"; // Python installation
                    if (pyvenvCfgLines[i].StartsWith("base-prefix = ")) pyvenvCfgLines[i] = $"base-prefix = {GetDataSubPath(Constants.Dirs.Python)}"; // Python installation
                    if (pyvenvCfgLines[i].StartsWith("base-exec-prefix = ")) pyvenvCfgLines[i] = $"base-exec-prefix = {GetDataSubPath(Constants.Dirs.Python)}"; // Python installation
                    if (pyvenvCfgLines[i].StartsWith("base-executable = ")) pyvenvCfgLines[i] = $"base-executable = {Path.Combine(GetDataSubPath(Constants.Dirs.Python), "python.exe")}"; // Python executable
                }

                if (IoUtils.WriteAllLinesIfDifferent(pyvenvCfgPath, pyvenvCfgLines))
                    Logger.Log($"Fixed {Path.GetFileName(pyvenvCfgPath)}", true);

                string activateNuPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Scripts", "activate.nu");
                var activateNuLines = File.ReadAllLines(activateNuPath);

                for (int i = 0; i < activateNuLines.Length; i++)
                {
                    string searchStr = "let virtual_env = '";
                    if (activateNuLines[i].Trim().StartsWith(searchStr))
                    {
                        var split = activateNuLines[i].Split(searchStr);
                        activateNuLines[i] = $"{split.First()}{searchStr}{GetDataSubPath(Constants.Dirs.SdVenv).TrimEnd('\\')}'";
                    }

                    string searchStr2 = "alias deactivate = source '";
                    if (activateNuLines[i].Trim().StartsWith(searchStr2))
                    {
                        var split = activateNuLines[i].Split(searchStr2);
                        activateNuLines[i] = $"{split.First()}{searchStr2}{GetDataSubPath(Constants.Dirs.SdVenv).TrimEnd('\\')}'";
                    }
                }

                if (IoUtils.WriteAllLinesIfDifferent(activateNuPath, activateNuLines))
                    Logger.Log($"Fixed {Path.GetFileName(activateNuPath)}", true);


                string activateBatPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Scripts", "activate.bat");
                var activateBatLines = File.ReadAllLines(activateBatPath);

                for (int i = 0; i < activateBatLines.Length; i++)
                {
                    string searchStr = "@set \"VIRTUAL_ENV=";
                    if (activateBatLines[i].Trim().StartsWith(searchStr))
                    {
                        var split = activateBatLines[i].Split(searchStr);
                        activateBatLines[i] = $"{split.First()}{searchStr}{GetDataSubPath(Constants.Dirs.SdVenv).TrimEnd('\\')}\"";
                    }
                }

                if (IoUtils.WriteAllLinesIfDifferent(activateBatPath, activateBatLines))
                    Logger.Log($"Fixed {Path.GetFileName(activateBatPath)}", true);


                string activatePath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Scripts", "activate");
                var activateLines = File.ReadAllLines(activatePath);

                for (int i = 0; i < activateLines.Length; i++)
                {
                    string searchStr = "VIRTUAL_ENV='";
                    if (activateLines[i].Trim().StartsWith(searchStr))
                    {
                        var split = activateLines[i].Split(searchStr);
                        activateLines[i] = $"{split.First()}{searchStr}{GetDataSubPath(Constants.Dirs.SdVenv).TrimEnd('\\')}'";
                    }
                }

                if (IoUtils.WriteAllLinesIfDifferent(activatePath, activateLines))
                    Logger.Log($"Fixed {Path.GetFileName(activatePath)}", true);


                string activateFishPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Scripts", "activate.fish");
                var activateFishLines = File.ReadAllLines(activateFishPath);

                for (int i = 0; i < activateFishLines.Length; i++)
                {
                    string searchStr = "set -gx VIRTUAL_ENV '";
                    if (activateFishLines[i].Trim().StartsWith(searchStr))
                    {
                        var split = activateFishLines[i].Split(searchStr);
                        activateFishLines[i] = $"{split.First()}{searchStr}{GetDataSubPath(Constants.Dirs.SdVenv).TrimEnd('\\')}'";
                    }
                }

                if (IoUtils.WriteAllLinesIfDifferent(activateFishPath, activateFishLines))
                    Logger.Log($"Fixed {Path.GetFileName(activateFishPath)}", true);


                var scriptsFiles = IoUtils.GetFileInfosSorted(Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Scripts"), true, "*");

                foreach (var file in scriptsFiles)
                {
                    try
                    {
                        if (file.Extension.Lower() != ".exe")
                        {
                            var lines = File.ReadAllLines(file.FullName);

                            if (!lines[0].StartsWith("#!") || !lines[0].Lower().EndsWith("python.exe"))
                                continue;

                            lines[0] = $"#!{Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Scripts", "python.exe")}";

                            if (IoUtils.WriteAllLinesIfDifferent(file.FullName, lines))
                                Logger.Log($"Fixed shebang in script {file.Name}", true);
                        }
                    }
                    catch(Exception ex)
                    {
                        Logger.Log($"Failed to fix shebang in {file.Name}: {ex.Message}\n{ex.StackTrace}", true);
                    }
                }

                Logger.Log($"Validation complete.", true);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error patching files: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        public static string GetDataSubPath(string dir)
        {
            return Path.Combine(Paths.GetDataPath(), dir);
        }

        #endregion
    }
}
