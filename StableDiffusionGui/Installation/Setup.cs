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
                l.Add($"cd ..");
                l.Add($"");
                l.Add($"pause");

                File.WriteAllLines(batPath, l);

                Logger.Log("Running installation script...");

                Process p = Process.Start(batPath);

                while (!p.HasExited)
                    await Task.Delay(100);

                Patch();

                await DownloadModelFile();

                Logger.Log("Finished.");
            }
            catch(Exception ex)
            {
                Logger.Log($"Install error: {ex.Message}\n{ex.StackTrace}");
            }   
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
            var filesize = new FileInfo(mdlPath).Length;

            if(filesize == 4265380512 && !force)
            {
                Logger.Log($"Model file already exists ({FormatUtils.Bytes(filesize)}), won't redownload.");
                return;
            }

            Logger.Log("Downloading model file...");

            Process p = OsUtils.NewProcess(false);
            p.StartInfo.Arguments = $"/C curl \"https://dl.nmkd-hz.de/tti/sd/models/1.4/model.ckpt\" -o {mdlPath.Wrap()}";
            p.Start();

            while (!p.HasExited)
                await Task.Delay(100);

            Logger.Log($"Model file downloaded ({FormatUtils.Bytes(new FileInfo(mdlPath).Length)}).");
        }

        private static void Clone (string url, string dir, string commit = "014e60d0f221794a365eca672d1e086ace8bfdee" /* f77e0a545e28a11206b19f47af0af5c971491fa0 */)
        {
            string path = Repository.Clone(url, dir, new CloneOptions () { BranchName = "main" });

            if (!string.IsNullOrWhiteSpace(commit))
            {
                using (var localRepo = new Repository(dir))
                {
                    var localCommit = localRepo.Lookup<Commit>(commit);
                    Commands.Checkout(localRepo, localCommit);
                }
            }

            Logger.Log($"Done.");
        }

        public static async Task Cleanup ()
        {
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
