using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Os
{
    internal class GpuUtils
    {
        public static Dictionary<string, int> CachedGpus = new Dictionary<string, int>();

        public static async Task<Dictionary<string, int>> GetCudaGpusCached()
        {
            if (CachedGpus.Count > 0)
                return CachedGpus;
            else
                return await GetCudaGpus();
        }

        public static async Task<Dictionary<string, int>> GetCudaGpus()
        {
            string scriptPath = Path.Combine(Paths.GetDataPath(), "repo", "check_gpus.py");
            List<string> outLines = new List<string>();

            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} python {scriptPath.Wrap()}";

            if (!OsUtils.ShowHiddenCmd())
            {
                p.OutputDataReceived += (sender, line) => { if (line != null && line.Data != null) outLines.Add(line.Data); };
                p.ErrorDataReceived += (sender, line) => { if (line != null && line.Data != null) outLines.Add(line.Data); };
            }

            p.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
            }

            while (!p.HasExited) await Task.Delay(1);

            Dictionary<string, int> gpus = new Dictionary<string, int>();

            foreach(string line in outLines)
            {
                string l = line.Trim();

                if (l.MatchesWildcard("* => *"))
                    gpus.Add(l.Split(" => ")[1], l.Split(" => ")[0].GetInt());
            }

            CachedGpus = gpus;
            return gpus;
        }
    }
}
