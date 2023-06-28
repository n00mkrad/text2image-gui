using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZetaLongPaths;

namespace StableDiffusionGui.Installation
{
    internal class Setup
    {
        private static readonly string _gitFile = "n00mkrad/stable-diffusion-cust.git";
        private static readonly string _gitBranch = "main";
        public static readonly string GitCommit = "eda6e02496ef43aeb8a56c8ba1d2df7ecb18e1a7";

        private static readonly bool _allowModelDownload = false;

        public static async Task Install(bool force = false, bool installUpscalers = true)
        {
            Logger.Log($"Installing (Force = {force} - Upscalers: {installUpscalers})", true, false, Constants.Lognames.Installer);

            try
            {
                Program.SetState(Program.BusyState.Installation);

                if (force || !InstallationStatus.HasSdRepo() || !InstallationStatus.HasSdEnv())
                {
                    if (!force)
                        Logger.Log("Install: Cloning repo and setting up env because either SD Repo or SD Env is missing.", true, false, Constants.Lognames.Installer);

                    await InstallRepo();
                }

                if (_allowModelDownload && (force || !InstallationStatus.HasSdModel()))
                {
                    if (!force)
                        Logger.Log("Install: Downloading model file because there is none.", true, false, Constants.Lognames.Installer);

                    await DownloadSdModelFile();
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

        public static async Task SetupVenv()
        {
            bool clean = IoUtils.TryDeleteIfExists(GetDataSubPath(Constants.Dirs.SdVenv));

            if (!clean)
            {
                Logger.Log("Failed to install python environment: Can't delete existing folder.");
                return;
            }

            string repoPath = GetDataSubPath(Constants.Dirs.SdRepo);
            string batPath = Path.Combine(repoPath, "install.bat");

            File.WriteAllText(batPath, $"@echo off\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"SET PATH={OsUtils.GetPathVar(new string[] { $@".\{Constants.Dirs.SdVenv}\Scripts", $@".\{Constants.Dirs.Python}\Scripts", $@".\{Constants.Dirs.Python}", $@".\{Constants.Dirs.Git}\cmd" })}\n" +
                $"SET HOME={Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "home")}\n" +
                $"python -m virtualenv {Constants.Dirs.SdVenv}\n" +
                $"{Constants.Dirs.SdRepo}\\install-venv-deps-all.bat\n" +
                $"");

            Logger.Log("Running python environment installation script...");

            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd(), batPath);

            if (!OsUtils.ShowHiddenCmd())
            {
                p.OutputDataReceived += (sender, line) => { HandleInstallScriptOutput(line.Data, false, false); };
                p.ErrorDataReceived += (sender, line) => { HandleInstallScriptOutput(line.Data, false, true); };
            }

            p.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }

            while (!p.HasExited) await Task.Delay(1);

            Logger.Log("Cleaning up...", false, Logger.LastUiLine.EndsWith("..."));
            IoUtils.TryDeleteIfExists(Path.Combine(Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%"), "pip", "cache"));
            RepoCleanup();
            PatchUtils.PatchAllPkgs();
            Logger.Log("Done.");
        }

        private static void HandleInstallScriptOutput(string log, bool conda, bool stderr)
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
                    string name = log.Split("Collecting ")[1].Split("=")[0].Split("<")[0].Split(">")[0].Split("!")[0].Trim().Replace(" git+", " ").Trunc(150, false);
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
            p.ErrorDataReceived += (sender, line) => { try { Logger.Log($"Downloading... ({line.Data?.Trim().Split(' ')[0]}%)", false, Logger.LastUiLine.EndsWith("%)"), Constants.Lognames.Installer); } catch { } };
            p.StartInfo.Arguments = $"/C curl -k \"https://dl.nmkd-hz.de/tti/sd/models/sd-v1-5-fp16.ckpt\" -o {mdlPath.Wrap()}";
            p.Start();
            p.BeginErrorReadLine();

            while (!p.HasExited)
                await Task.Delay(1);

            if (File.Exists(mdlPath))
                Logger.Log($"Model file downloaded ({FormatUtils.Bytes(new ZlpFileInfo(mdlPath).Length)}).");
            else
                Logger.Log($"Failed to download model file due to an unknown error. Check the log files.");
        }

        #region Git

        public static async Task InstallRepo(string overrideCommit = "", bool setupVenvAfterwards = true)
        {
            string commit = string.IsNullOrWhiteSpace(overrideCommit) ? GitCommit : overrideCommit;
            TtiProcess.ProcessExistWasIntentional = true;
            ProcessManager.FindAndKillOrphans($"*invoke.py*{Paths.SessionTimestamp}*");
            await CloneSdRepo($"https://github.com/{_gitFile}", GetDataSubPath(Constants.Dirs.SdRepo), _gitBranch, commit);
            await SetupVenv();
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

            string gitDir = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "cmd");
            string gitExe = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "cmd", "git.exe");
            Process p = OsUtils.NewProcess(true);
            p.StartInfo.EnvironmentVariables["PATH"] = TtiUtils.GetEnvVarsSd(true, Paths.GetDataPath()).First().Value;
            p.StartInfo.Arguments = $"/C git clone --depth=1 --single-branch --branch {branch} {gitUrl} {dir.Wrap(true)} {(string.IsNullOrWhiteSpace(commit) ? "" : $"&& cd /D {dir.Wrap()} && git checkout {commit}")}";
            Logger.Log($"{p.StartInfo.FileName} {p.StartInfo.Arguments}", true);
            p.OutputDataReceived += (sender, line) => { HandleInstallScriptOutput($"[git] {line.Data}", false, false); };
            p.ErrorDataReceived += (sender, line) => { HandleInstallScriptOutput($"[git] {line.Data}", false, true); };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            while (!p.HasExited) await Task.Delay(1);
        }

        public static void RepoCleanup()
        {
            try
            {
                string repoPath = GetDataSubPath(Constants.Dirs.SdRepo);
                string venvSrcPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "src");
                string sitePkgsPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Lib", "site-packages");

                var dirs = new List<ZlpDirectoryInfo>();

                dirs.AddRange(Directory.GetDirectories(repoPath, "*", SearchOption.AllDirectories).Select(x => new ZlpDirectoryInfo(x)));
                dirs.AddRange(Directory.GetDirectories(venvSrcPath, "*", SearchOption.AllDirectories).Select(x => new ZlpDirectoryInfo(x)));

                new ZlpDirectoryInfo(repoPath).Attributes = ZetaLongPaths.Native.FileAttributes.Normal;
                IoUtils.SetAttributes(repoPath, ZetaLongPaths.Native.FileAttributes.Normal);

                new ZlpDirectoryInfo(venvSrcPath).Attributes = ZetaLongPaths.Native.FileAttributes.Normal;
                IoUtils.SetAttributes(venvSrcPath, ZetaLongPaths.Native.FileAttributes.Normal);

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
                unneededFileTypes.ForEach(ext => IoUtils.GetFilesSorted(venvSrcPath, true, $"*.{ext}").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x)));

                IoUtils.TryDeleteIfExists(Path.Combine(sitePkgsPath, "pandas", "tests"));
                IoUtils.TryDeleteIfExists(Path.Combine(sitePkgsPath, "imageio", "resources", "images"));
                IoUtils.TryDeleteIfExists(GetDataSubPath("0.7.5"));
                IoUtils.GetFilesSorted(Path.Combine(sitePkgsPath, "cv2"), false, "opencv_videoio_*.dll").ToList().ForEach(f => IoUtils.TryDeleteIfExists(f));
            }
            catch { }
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
                    dirs.ForEach(dir => IoUtils.DeleteIfExists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "invoke", "models", dir)));
                }

                Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && {Constants.Files.VenvActivate} && " +
                    $"python {Constants.Dirs.SdRepo}/invoke/scripts/nmkd_install_upscalers.py";
                p.ErrorDataReceived += (sender, line) =>
                {
                    if (line.Data != null && line.Data.Contains("%|") && line.Data.Trim().EndsWith("B/s]"))
                        Logger.Log($"Downloading {line.Data.Trim().Split("%|")[0].Replace("  ", " ")}%", false, Logger.LastUiLine.EndsWith("%"), Constants.Lognames.Installer);
                };
                Logger.Log($"cmd {p.StartInfo.Arguments}", true);
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                while (!p.HasExited) await Task.Delay(1);

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

        private static readonly Regex _exeShebangRegex = new Regex("(#!\")(.+?)(\")", RegexOptions.Compiled);

        public static void PatchFiles()
        {
            try
            {
                if (IoUtils.GetAmountOfFiles(Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv)), false) < 1)
                    return;

                #region virtualenv (pyvenv.cfg, Scripts)

                Logger.Log($"Fixing pyenv paths...", true);
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
                        if (file.Extension.Lower() == ".exe")
                        {
                            byte[] exeBytes = File.ReadAllBytes(file.FullName);
                            Encoding encoding = Encoding.Default;
                            string exeText = encoding.GetString(exeBytes);
                            string shebang = _exeShebangRegex.Match(exeText).Value.NullToEmpty();
                            string pyPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Scripts", "python.exe");

                            if (shebang.IsEmpty() || shebang.Contains(pyPath, StringComparison.OrdinalIgnoreCase))
                                continue;

                            Logger.Log($"Detected invalid shebang in executable {file.Name}: {shebang}", true);
                            exeText = _exeShebangRegex.Replace(exeText, $"$1{pyPath}$3");
                            byte[] newExeBytes = encoding.GetBytes(exeText);
                            
                            if (newExeBytes.SequenceEqual(exeBytes))
                                continue;
                            
                            File.WriteAllBytes(file.FullName, newExeBytes);
                            Logger.Log($"Fixed shebang in executable {file.Name}", true);
                        }
                        else
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

                #endregion
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
