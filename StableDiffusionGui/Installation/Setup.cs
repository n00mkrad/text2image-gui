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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Installation
{
    internal class Setup
    {
        public static readonly string GitFile = "n00mkrad/stable-diffusion-cust.git";

        public static async Task Install (string repoCommit = "")
        {
            try
            {
                Program.MainForm.SetWorking(true);

                Logger.Log("Removing existing SD files...");
                await Cleanup();
                Logger.Log("Done.");

                string batPath = GetDataSubPath("install.bat");
                string repoPath = GetDataSubPath("repo");

                Logger.Log("Cloning repository...");
                CloneSdRepo();
                Logger.Log("Done.");

                string[] subDirs = new string[] { "mb", "git/bin" };

                List<string> l = new List<string>();

                l.Add("@echo off");
                l.Add($"");
                l.Add($"SET PATH={string.Join(";", subDirs.Select(x => GetDataSubPath(x)))};%PATH%");
                l.Add($"");
                l.Add($"cd /D {Paths.GetDataPath().Wrap()}");
                l.Add($"");
                l.Add($"cd repo");
                l.Add($"");
                l.Add($"install.cmd");
                l.Add($"");
                l.Add($"pause");

                File.WriteAllLines(batPath, l);

                Logger.Log("Running installation script...");

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

                await InstallUpscalers();
                RemoveGitFiles(repoPath);
                await DownloadSdModelFile();

                if (InstallationStatus.IsInstalled)
                {
                    Logger.Log("Finished. Everything is installed.");
                }
                else
                {
                    Logger.Log("Finished - Not all packages could be installed. Installation log was copied to clipboard.");
                    OsUtils.SetClipboard(Logger.GetSessionLog("installation"));
                }
            }
            catch(Exception ex)
            {
                Logger.Log($"Install error: {ex.Message}\n{ex.StackTrace}");
            }

            Program.MainForm.SetWorking(false);
        }

        private static void HandleInstallScriptOutput (string log, bool stderr)
        {
            if (string.IsNullOrWhiteSpace(log))
                return;

            Logger.Log($"[{(stderr ? "E" : "O")}] {log.Remove("PRINTME ").Remove("[O]").Remove("[E]")}", !log.Contains("PRINTME "), false, "installation");
        }

        public static async Task DownloadSdModelFile (bool force = false)
        {
            string mdlPath = Path.Combine(Paths.GetModelsPath(), "stable-diffusion-1.4.ckpt");
            var filesize = File.Exists(mdlPath) ? new FileInfo(mdlPath).Length : 0;

            if(filesize == 4265380512 && !force)
            {
                Logger.Log($"Model file already exists ({FormatUtils.Bytes(filesize)}), won't redownload.");
                return;
            }

            IoUtils.TryDeleteIfExists(mdlPath);
            Logger.Log("Downloading model file...");

            Process p = OsUtils.NewProcess(false);
            p.StartInfo.Arguments = $"/C curl \"https://www.googleapis.com/storage/v1/b/aai-blog-files/o/sd-v1-4.ckpt?alt=media\" -o {mdlPath.Wrap()}";
            p.Start();

            while (!p.HasExited)
                await Task.Delay(100);

            Logger.Log($"Model file downloaded ({FormatUtils.Bytes(new FileInfo(mdlPath).Length)}).");
        }

        #region Git

        public static void CloneSdRepo ()
        {
            CloneSdRepo($"https://github.com/{GitFile}", GetDataSubPath("repo"));
        }

        public static void CloneSdRepo (string url, string dir, string commit = "" /* 1404d8e98cd9038e2cd3e33b177e9b995b42bc3a */)
        {
            try
            {
                Directory.Delete(dir, true);

                string path = Repository.Clone(url, dir, new CloneOptions() { BranchName = "main" });

                if (!string.IsNullOrWhiteSpace(commit))
                {
                    using (var localRepo = new Repository(dir))
                    {
                        var localCommit = localRepo.Lookup<Commit>(commit);
                        Commands.Checkout(localRepo, localCommit);
                    }
                }

                Logger.Log($"Done cloning repository.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to clone repository: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        public static void RemoveGitFiles (string rootPath)
        {
            foreach(string dir in Directory.GetDirectories(rootPath, ".git", SearchOption.AllDirectories))
            {
                new DirectoryInfo(dir).Attributes = FileAttributes.Normal;
                IoUtils.SetAttributes(dir, FileAttributes.Normal);
                IoUtils.TryDeleteIfExists(dir);
            }

            string tamingPath = Path.Combine(rootPath, "src", "taming-transformers");
            IoUtils.SetAttributes(tamingPath, FileAttributes.Normal);
            IoUtils.GetFilesSorted(tamingPath, true, "*.jpg").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x));
            IoUtils.GetFilesSorted(tamingPath, true, "*.png").ToList().ForEach(x => IoUtils.TryDeleteIfExists(x));
        }

        #endregion

        #region Upscaling Models

        public static async Task InstallUpscalers (bool force = false, bool print = true)
        {
            try
            {
                if (print)
                    Logger.Log("Installing RealESRGAN...");

                string batPath = Path.Combine(GetDataSubPath("repo"), "install-upscalers.cmd");
                Process installBat = OsUtils.NewProcess(false, batPath);
                installBat.Start();

                while (!installBat.HasExited)
                    await Task.Delay(100);

                if (print)
                    Logger.Log("Installing GFPGAN...");

                string gfpganPath = Path.Combine(GetDataSubPath("repo"), "GFPGAN");
                IoUtils.SetAttributes(gfpganPath, FileAttributes.Normal);
                Directory.Delete(gfpganPath, true);
                Repository.Clone(@"https://github.com/TencentARC/GFPGAN.git", gfpganPath, new CloneOptions() { BranchName = "master" });

                using (var localRepo = new Repository(gfpganPath))
                {
                    var localCommit = localRepo.Lookup<Commit>("3e27784b1b4eb008d06c04dbbaf6bdde34c4da84");
                    Commands.Checkout(localRepo, localCommit);
                }

                if (print)
                    Logger.Log("Downloading GFPGAN model file...");

                string gfpGanMdlPath = Path.Combine(gfpganPath, "model.pth");

                IoUtils.TryDeleteIfExists(gfpGanMdlPath);

                Process gfpMdlDl = OsUtils.NewProcess(false);
                gfpMdlDl.StartInfo.Arguments = $"/C curl -L \"https://github.com/TencentARC/GFPGAN/releases/download/v1.3.0/GFPGANv1.3.pth\" -o {gfpGanMdlPath.Wrap()}";
                gfpMdlDl.Start();

                while (!gfpMdlDl.HasExited)
                    await Task.Delay(100);

                Logger.Log($"Downloaded and installed RealESRGAN and GFPGAN.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to install upscalers: {ex.Message}");
                Logger.Log($"{ex.StackTrace}", true);
            }
        }

        #endregion

        #region Uninstall

        public static async Task Cleanup ()
        {
            IoUtils.SetAttributes(GetDataSubPath("repo"), FileAttributes.Normal);
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath("repo"));
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath("ldo"));
        }

        public static async Task RemoveEnv()
        {
            await IoUtils.TryDeleteIfExistsAsync(Path.Combine(Paths.GetDataPath(), "mb", "envs", "ldo"));
        }

        #endregion

        private static string GetDataSubPath (string dir)
        {
            return Path.Combine(Paths.GetDataPath(), dir);
        }
    }
}
