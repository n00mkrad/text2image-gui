using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
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
            int readConfigRetries = 0;

            // This function might run at the same time as the config file gets first created, so we retry up to 10 times with 100ms delay if it's locked
            while (!Config.Ready)
            {
                readConfigRetries++;
                await Task.Delay(200);
            }

            List<string> outLines = new List<string>();

            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd(true)} && call activate.bat mb/envs/ldo && python {Constants.Dirs.RepoSd}/scripts/check_gpus.py";

            if (!OsUtils.ShowHiddenCmd())
            {
                p.OutputDataReceived += (sender, line) => { if (line != null && line.Data != null) outLines.Add(line.Data); };
                p.ErrorDataReceived += (sender, line) => { if (line != null && line.Data != null) outLines.Add(line.Data); };
            }

            Logger.Log("cmd.exe " + p.StartInfo.Arguments, true);
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
