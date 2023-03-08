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
using ZetaLongPaths;

namespace StableDiffusionGui.Installation
{
    internal class Setup
    {
        private static readonly string _gitFile = "n00mkrad/stable-diffusion-cust.git";
        private static readonly string _gitBranch = "main";
        public static readonly string GitCommit = "5d4b95c10b59fdaf307efa6f201984c3f5f62542";

        private static readonly bool _allowModelDownload = false;

        private static bool ReplaceUiLogLine { get { return Logger.LastUiLine.EndsWith("..."); } }

        public static async Task Install(bool force = false, bool installOnnxDml = false, bool installUpscalers = true)
        {
            Logger.Log($"Installing (Force = {force} - ONNX/DML: {installOnnxDml} - Upscalers: {installUpscalers})", true, false, Constants.Lognames.Installer);

            try
            {
                Program.SetState(Program.BusyState.Installation);

                if (force || !InstallationStatus.HasSdRepo() || !InstallationStatus.HasSdEnv())
                {
                    if (!force)
                        Logger.Log("Install: Cloning repo and setting up env because either SD Repo or SD Env is missing.", true, false, Constants.Lognames.Installer);

                    await InstallRepo(installOnnxDml);
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

        public static async Task SetupVenv(bool installOnnx)
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
                $"python -m virtualenv {Constants.Dirs.SdVenv}\n" +
                $"{Constants.Dirs.SdRepo}\\install-venv-deps.bat {(installOnnx ? $"&& {Constants.Dirs.SdRepo}\\install-venv-deps-onnx.bat" : "")}\n" +
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
            p.ErrorDataReceived += (sender, line) => { try { Logger.Log($"Downloading... ({line.Data.Trim().Split(' ')[0]}%)", false, Logger.LastUiLine.EndsWith("%)"), Constants.Lognames.Installer); } catch { } };
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

        public static async Task InstallRepo(bool installOnnx, string overrideCommit = "", bool setupVenvAfterwards = true)
        {
            string commit = string.IsNullOrWhiteSpace(overrideCommit) ? GitCommit : overrideCommit;
            TtiProcess.ProcessExistWasIntentional = true;
            ProcessManager.FindAndKillOrphans($"*invoke.py*{Paths.SessionTimestamp}*");
            await CloneSdRepo($"https://github.com/{_gitFile}", GetDataSubPath(Constants.Dirs.SdRepo), _gitBranch, commit);
            await SetupVenv(installOnnx);
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

        public static async Task Clone (string gitUrl, string dir, string commit = "", string branch = "main")
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

                foreach(var dir in Directory.GetDirectories(sitePkgsPath, "*", SearchOption.TopDirectoryOnly).Select(x => new ZlpDirectoryInfo(x)))
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
                IoUtils.SetAttributes(gfpganPath, ZetaLongPaths.Native.FileAttributes.Normal);

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

                #region virtualenv (pyvenv.cfg)

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

                #endregion
                #region virtualenv (egg-link files)

                string sitePkgsDir = Path.Combine(GetDataSubPath(Constants.Dirs.SdVenv), "Lib", "site-packages");
                var eggLinks = IoUtils.GetFileInfosSorted(sitePkgsDir, false, "*.egg-link");

                List<string> easyInstallPaths = new List<string>();

                foreach (ZlpFileInfo eggLink in eggLinks)
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

                #endregion
                #region virtualenv (easy-install.pth)

                var easyInstallPth = Path.Combine(sitePkgsDir, "easy-install.pth");

                if (File.Exists(easyInstallPth))
                {
                    var easyInstallLines = File.ReadAllLines(easyInstallPth);

                    string basePath = String.Join($@"\{Constants.Dirs.SdRepo}", easyInstallLines.Where(l => l.Trim().EndsWith($@"\{Constants.Dirs.SdRepo}")).FirstOrDefault().Split($@"\{Constants.Dirs.SdRepo}").Reverse().Skip(1).Reverse());
                    string newBasePath = Paths.GetDataPath().Lower().Replace("/", @"\");

                    List<string> newLines = easyInstallLines.Select(l => l.Replace(basePath, newBasePath).Replace(@"\\", @"\")).ToList();

                    File.WriteAllLines(easyInstallPth, newLines);
                    Logger.Log($"Fixed easy-install.pth.", true);
                }

                #endregion
                #region Finders (k-diffusion)
                var finderFiles = IoUtils.GetFileInfosSorted(sitePkgsDir, false, "*_finder.py");

                foreach (var file in finderFiles)
                {
                    var lines = File.ReadAllLines(file.FullName);
                    
                    for(int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("MAPPING = {"))
                        {
                            string name = lines[i].Split('\'')[1].Split('\'')[0];
                            string relPath = lines[i].Split('\'')[3].Split(@"\\Data").Reverse().Take(1).Reverse().First().TrimStart('\\');
                            lines[i] = $"import os; import sys; MAPPING = {{'{name}': os.path.join(sys.path[0], \"..\", \"..\", \"{relPath}\")}}";
                        }
                    }

                    File.WriteAllLines(file.FullName, lines);
                }
                #endregion
                #region CLIP Cache

                string marker = "# PATCHED BY NMKD SD GUI"; // String to mark files as patched
                string clipPyPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "src", "clip", "clip", "clip.py"); // Path to the main clip script to modify
                var clipPyLines = File.ReadAllLines(clipPyPath).Where(l => !l.Contains(marker)).ToList(); // All lines, but exclude the marked one to avoid double-patching it
                string dlLine = clipPyLines.Where(l => l.Trim().StartsWith("model_path = _download(_MODELS[name], download_root or")).FirstOrDefault(); // Find the target line
                int indentSpaces = dlLine.TakeWhile(char.IsWhiteSpace).Count(); // Count how many spaces there are (python indentation matters!!)
                string clipCacheDir = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, Constants.Dirs.Cache.Clip); // Our custom cache directory
                clipPyLines.Insert(clipPyLines.IndexOf(dlLine), $"{new string(' ', indentSpaces)}download_root = {clipCacheDir.Wrap(true)} {marker}"); // Insert a line overwriting the download dir
                File.WriteAllLines(clipPyPath, clipPyLines); // Save patched file.

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
