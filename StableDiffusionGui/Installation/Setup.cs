using LibGit2Sharp;
using Microsoft.VisualBasic.Logging;
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
                Clone($"https://github.com/{GitFile}", repoPath);
                Logger.Log("Done.");

                string[] subDirs = new string[] { "mc", "git/bin" };

                List<string> l = new List<string>();

                l.Add("@echo off");
                l.Add($"");
                l.Add($"SET PATH={string.Join(";", subDirs.Select(x => GetDataSubPath(x)))};%PATH%");
                l.Add($"");
                l.Add($"cd /D {Paths.GetDataPath().Wrap()}");
                l.Add($"");
                l.Add($"CALL mc/condabin/conda.bat activate base");
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

                Patch();

                RemoveGitFiles(repoPath);

                await DownloadModelFile();

                if (InstallationStatus.IsInstalled)
                {
                    Logger.Log("Finished. Everything is installed.");
                }
                else
                {
                    Logger.Log("Finished - Not all packages could be installed. Installation log was copied to clipboard.");
                    Clipboard.SetText(Logger.GetSessionLog("installer"));
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

            Logger.Log($"[{(stderr ? "E" : "O")}] {log.Remove("PRINTME ")}", !log.Contains("PRINTME "), false, "installation");
        }

        public static void Patch ()
        {
            try
            {
                string modulesPyPath = Path.Combine(Paths.GetDataPath(), "repo", "ldm", "modules", "encoders", "modules.py");

                string patchedText = File.ReadAllText(modulesPyPath).Replace("local_files_only=True", "local_files_only=False");
                File.WriteAllText(modulesPyPath, patchedText);

                Logger.Log("Successfully patched modules.py.");
            }
            catch(Exception ex)
            {
                Logger.Log($"Failed to patch modules.py: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }

        }

        public static async Task DownloadModelFile (bool force = false)
        {
            string mdlPath = Path.Combine(Paths.GetDataPath(), "model.ckpt");
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

        private static void Clone (string url, string dir, string commit = "" /* f77e0a545e28a11206b19f47af0af5c971491fa0 */)
        {
            try
            {
                string path = Repository.Clone(url, dir, new CloneOptions() { BranchName = "main" });

                if (!string.IsNullOrWhiteSpace(commit))
                {
                    using (var localRepo = new Repository(dir))
                    {
                        var localCommit = localRepo.Lookup<Commit>(commit);
                        Commands.Checkout(localRepo, localCommit);
                    }
                }

                Logger.Log($"Done clonging repository.");
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

        public static async Task Cleanup ()
        {
            IoUtils.SetAttributes(GetDataSubPath("repo"), FileAttributes.Normal);
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath("repo"));
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath("ldo"));
        }

        public static async Task RemoveEnv()
        {
            await IoUtils.TryDeleteIfExistsAsync(Path.Combine(Paths.GetDataPath(), "mc", "envs", "ldo"));
        }

        private static string GetDataSubPath (string dir)
        {
            return Path.Combine(Paths.GetDataPath(), dir);
        }
    }
}
