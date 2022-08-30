using LibGit2Sharp;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                Logger.Log("Finished.");
            }
            catch(Exception ex)
            {
                Logger.Log($"Install error: {ex.Message}\n{ex.StackTrace}");
            }   
        }

        private static void Clone (string url, string dir, string commit = "5e5c021b54afff73209f14deb8a09dde0205dcad")
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
