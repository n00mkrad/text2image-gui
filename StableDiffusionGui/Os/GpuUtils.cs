using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            string scriptPath = Path.Combine(Paths.GetDataPath(), "repo", "check_gpus.py");
            List<string> outLines = new List<string>();

            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"mb\\Scripts\\activate.bat\" \"mb/envs/ldo\" && python {scriptPath.Wrap()}";

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
