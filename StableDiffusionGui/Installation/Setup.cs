using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace StableDiffusionGui.Installation
{
    internal class Setup
    {
        public static readonly string GitFile = "n00mkrad/stable-diffusion-cust.git";

        public static async Task Install (string repoCommit = "")
        {
            await Cleanup();

            string batPath = GetDataSubPath("install.bat");
            string repoPath = GetDataSubPath("repo");

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
            l.Add($"git clone https://github.com/{GitFile} repo");
            l.Add($"cd repo");
            if(!string.IsNullOrWhiteSpace(repoCommit))
                l.Add($"git checkout {repoCommit}");
            l.Add($"");
            l.Add($"install.cmd");
            l.Add($"");
            l.Add($"cd ..");
            l.Add($"");
            l.Add($"pause");

            File.WriteAllLines(batPath, l);

            Process.Start(batPath);
        }

        public static async Task Cleanup ()
        {
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath("repo"));
            await IoUtils.TryDeleteIfExistsAsync(GetDataSubPath("ldo"));
        }

        private static string GetDataSubPath (string dir)
        {
            return Path.Combine(Paths.GetDataPath(), dir);
        }
    }
}
