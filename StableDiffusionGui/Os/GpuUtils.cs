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
            int readConfigRetries = 0;

            // This function might run at the same time as the config file gets first created, so we retry up to 10 times with a delay
            while (!Config.Ready)
            {
                readConfigRetries++;
                await Task.Delay(200);
            }

            List<string> outLines = new List<string>();

            Process py = OsUtils.NewProcess(true, logAction: (s) => outLines.Add(s));
            py.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true)} && {Constants.Files.VenvActivate} && python {Constants.Dirs.SdRepo}/scripts/check_gpus.py";
            Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

            OsUtils.StartProcess(py, killWithParent: true);
            await OsUtils.WaitForProcessExit(py);

            CachedGpus = new List<string>(outLines).Where(x => x.MatchesWildcard("* - * - *")).Select(x => new Gpu(x)).ToList();
            return CachedGpus;
        }
    }
}
