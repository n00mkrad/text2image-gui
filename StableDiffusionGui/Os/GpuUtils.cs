using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Os
{
    internal class GpuUtils
    {
        public static List<Gpu> CachedGpus = new List<Gpu>();

        public static async Task<List<Gpu>> GetCudaGpusCached()
        {
            if (CachedGpus.Count > 0)
                return CachedGpus;
            else
                return await GetCudaGpus();
        }

        public static async Task<List<Gpu>> GetCudaGpus()
        {
            List<string> outLines = new List<string>();

            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd(true)} && call activate.bat mb/envs/ldo && python repo/scripts/check_gpus.py";

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

            CachedGpus = outLines.Where(x => x.MatchesWildcard("* - * - *")).Select(x => new Gpu(x)).ToList();
            return CachedGpus;
        }
    }
}
