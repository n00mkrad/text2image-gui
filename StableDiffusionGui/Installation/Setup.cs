using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Constants;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

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

                RemoveGitFiles();

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

            File.WriteAllText(batPath,
                $"@echo off\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"SET PATH={OsUtils.GetPathVar(new string[] { $@".\{Constants.Dirs.SdVenv}\Scripts", $@".\{Constants.Dirs.Python}\Scripts", $@".\{Constants.Dirs.Python}", $@".\{Constants.Dirs.Git}\cmd" })}\n" +
                $"python -m virtualenv {Constants.Dirs.SdVenv}\n" +
                $"{Constants.Dirs.SdRepo}\\install-venv-deps.bat\n" +
                //$"{Constants.Dirs.SdRepo}\\install-venv-deps-onnx.bat\n" +
                $""
            );

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

            Logger.Log("Cleaning up...");
            IoUtils.TryDeleteIfExists(Path.Combine(Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%"), "pip", "cache"));
            RemoveGitFiles();
            Logger.Log("Done.");
        }

        public static async Task SetupPythonEnv()
        {
            string repoPath = GetDataSubPath(Constants.Dirs.SdRepo);
            string batPath = Path.Combine(repoPath, "install.bat");

            List<string> l = new List<string>();

            l.Add($"@echo off");
            l.Add($"");
            l.Add($"cd /D {repoPath.Wrap()}");
            l.Add($"");
            l.Add($"SET CONDA_ROOT_PATH=..\\{Constants.Dirs.Conda}");
            // l.Add($"SET PYTHONHOME=..\\{Constants.Dirs.Conda}");
            l.Add($"SET CONDA_SCRIPTS_PATH=..\\{Constants.Dirs.Conda}\\Scripts");
            l.Add($"");
            l.Add($"SET PATH={OsUtils.GetPathVar(new string[] { $"..\\{Constants.Dirs.Conda}", $"..\\{Constants.Dirs.Conda}\\Scripts", $"..\\{Constants.Dirs.Conda}\\condabin", $"..\\{Constants.Dirs.Conda}\\Library\\bin" })}");
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
                p.OutputDataReceived += (sender, line) => { HandleInstallScriptOutput(line.Data, true, false); };
                p.ErrorDataReceived += (sender, line) => { HandleInstallScriptOutput(line.Data, true, true); };
            }

            p.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }

            while (!p.HasExited) await Task.Delay(1);

            RemoveGitFiles();
            Logger.Log("Done.");
        }

        private static void HandleInstallScriptOutput(string log, bool conda, bool stderr)
        {
            if (string.IsNullOrWhiteSpace(log))
                return;

            log = log.Trim();

            Logger.Log($"{log.Remove("PRINTME ")}", !log.Contains("PRINTME "), false, Constants.Lognames.Installer);

            if (!conda && log.StartsWith("Collecting "))
            {
                Logger.Log($"Installing {log.Split("Collecting ")[1].Split("=")[0].Split("<")[0].Split(">")[0].Split("!")[0].Trim()}...", false, Logger.LastUiLine.EndsWith("..."));
            }

            if (conda && log.EndsWith("%") && log.Contains(" | "))
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
            await CloneSdRepo($"https://github.com/{GitFile}", GetDataSubPath(Constants.Dirs.SdRepo));
        }

        public static async Task CloneSdRepo(string url, string dir, string branch = "main", string commit = "")
        {
            try
            {
                Logger.Log("Cloning repository...");
                await Clone(url, dir, commit, branch);
                Logger.Log($"Done cloning repository.");

                await SetupVenv();
                // await SetupPythonEnv();
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to clone repository: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        public static async Task Clone (string gitUrl, string dir, string commit = "", string branch = "main")
        {
            if (Directory.Exists(dir))
            {
                IoUtils.SetAttributes(dir, FileAttributes.Normal);
                Directory.Delete(dir, true);
            }

            string gitDir = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "cmd");
            string gitExe = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "cmd", "git.exe");
            Process p = OsUtils.NewProcess(true);
            p.StartInfo.EnvironmentVariables["PATH"] = TtiUtils.GetEnvVarsSd(true, Paths.GetDataPath()).First().Value;
            p.StartInfo.Arguments = $"/C git clone --single-branch --branch {branch} {gitUrl} {dir.Wrap(true)} {(string.IsNullOrWhiteSpace(commit) ? "" : $"&& cd /D {dir.Wrap()} && git checkout {commit}")}";
            Logger.Log($"{p.StartInfo.FileName} {p.StartInfo.Arguments}", true);
            p.OutputDataReceived += (sender, line) => { HandleInstallScriptOutput($"[git] {line.Data}", false, false); };
            p.ErrorDataReceived += (sender, line) => { HandleInstallScriptOutput($"[git] {line.Data}", false, true); };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            while (!p.HasExited) await Task.Delay(1);
        }

        public static void RemoveGitFiles()
        {
            try
            {
                string repoPath = GetDataSubPath(Constants.Dirs.SdRepo);
                string venvSrcPath = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "src");

                List<DirectoryInfo> dirs = new List<DirectoryInfo>();

                dirs.AddRange(Directory.GetDirectories(repoPath, "*", SearchOption.AllDirectories).Select(x => new DirectoryInfo(x)));
                dirs.AddRange(Directory.GetDirectories(venvSrcPath, "*", SearchOption.AllDirectories).Select(x => new DirectoryInfo(x)));

                new DirectoryInfo(repoPath).Attributes = FileAttributes.Normal;
                IoUtils.SetAttributes(repoPath, FileAttributes.Normal);

                new DirectoryInfo(venvSrcPath).Attributes = FileAttributes.Normal;
                IoUtils.SetAttributes(venvSrcPath, FileAttributes.Normal);

                foreach (var dir in dirs)
                {
                    if (dir.Name == ".git")
                        IoUtils.TryDeleteIfExists(dir.FullName);
                }

                var unneededDirs = new List<string> { "docs", "assets" };
                unneededDirs.ForEach(dir => IoUtils.TryDeleteIfExists(Path.Combine(repoPath, dir)));

                var unneededFileTypes = new List<string> { "jpg", "jpeg", "png", "gif", "ipynb", "ttf" };
                unneededFileTypes.ForEach(ext => IoUtils.GetFilesSorted(repoPath, true, $"*.{ext}").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x)));
                unneededFileTypes.ForEach(ext => IoUtils.GetFilesSorted(venvSrcPath, true, $"*.{ext}").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x)));
            }
            catch { }
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

                await Clone("https://github.com/TencentARC/GFPGAN.git", gfpganPath, "2eac2033893ca7f427f4035d80fe95b92649ac56", "master");

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
                if (print) Logger.Log("Downloading CodeFormer model file...", ReplaceUiLogLine);
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
            IoUtils.SetAttributes(GetDataSubPath(Constants.Dirs.SdRepo), FileAttributes.Normal);
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath(Constants.Dirs.SdRepo));
        }

        public static async Task RemoveEnv()
        {
            await IoUtils.TryDeleteIfExistsAsync(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv));
        }

        #endregion

        #region Utils

        public static void FixHardcodedPathsVenv()
        {
            try
            {
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

                File.WriteAllLines(pyvenvCfgPath, pyvenvCfgLines);

                Logger.Log($"Fixed pyvenv.cfg", true);

                string sitePkgsDir = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Lib", "site-packages");
                var eggLinks = IoUtils.GetFileInfosSorted(sitePkgsDir, false, "*.egg-link");

                List<string> easyInstallPaths = new List<string>();

                foreach (FileInfo eggLink in eggLinks)
                {
                    string nameNoExt = Path.GetFileNameWithoutExtension(eggLink.FullName);

                    if (nameNoExt == "invoke-ai")
                    {
                        string path = Path.Combine(GetDataSubPath(Constants.Dirs.SdRepo));
                        File.WriteAllText(eggLink.FullName, path + "\n.");
                    }
                    else
                    {
                        string path = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "src", nameNoExt);
                        File.WriteAllText(eggLink.FullName, path + "\n.");
                    }
                }

                Logger.Log($"Fixed egg-link files", true);

                var easyInstallPth = Path.Combine(sitePkgsDir, "easy-install.pth");

                if (File.Exists(easyInstallPth))
                {
                    var easyInstallLines = File.ReadAllLines(easyInstallPth);

                    string basePath = easyInstallLines.Where(l => l.Trim().EndsWith($@"\{Constants.Dirs.SdRepo}")).FirstOrDefault().Split($@"\{Constants.Dirs.SdRepo}").First();
                    string newBasePath = Paths.GetDataPath().Lower().Replace("/", @"\");

                    List<string> newLines = easyInstallLines.Select(l => l.Replace(basePath, newBasePath).Replace(@"\\", @"\")).ToList();

                    File.WriteAllLines(easyInstallPth, newLines);
                    Logger.Log($"Fixed easy-install.pth.", true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error fixing paths: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        public static void FixHardcodedPathsConda()
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
                        string path = Path.Combine(GetDataSubPath(Constants.Dirs.SdRepo));
                        File.WriteAllText(eggLink.FullName, path + "\n.");
                    }
                    else
                    {
                        string path = Path.Combine(GetDataSubPath(Constants.Dirs.SdRepo), "src", nameNoExt);
                        File.WriteAllText(eggLink.FullName, path + "\n.");
                    }

                    Logger.Log($"Fixed egg-link file {eggLink.FullName}.", true);
                }

                var easyInstallPth = Path.Combine(parentDir, "easy-install.pth");

                if (File.Exists(easyInstallPth))
                {
                    var easyInstallLines = File.ReadAllLines(easyInstallPth);
                    List<string> newLines = new List<string>();

                    string splitText = $@"data\{Constants.Dirs.SdRepo}";
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
            catch (Exception ex)
            {
                Logger.Log($"Error validating installation: {ex.Message}");
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
