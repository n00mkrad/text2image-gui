using StableDiffusionGui.Main;
using System;

namespace StableDiffusionGui.Data
{
    internal class Gpu
    {
        public int CudaDeviceId { get; set; } = -1;
        public string FullName { get; set; } = "";
        public float VramGb { get; set; } = 0f;

        public Gpu() { }

        public Gpu(string fromCheckGpusOutput)
        {
            try
            {
                var split = fromCheckGpusOutput.Split(" - ");
                CudaDeviceId = split[0].GetInt();
                FullName = split[1].Trim();
                VramGb = split[2].Trim().Split(' ')[0].GetFloat();
                VramGb = (float)Math.Round(VramGb * 10f) / 10f;
            }
            catch(Exception ex)
            {
                Logger.Log($"Failed to parse GPU: {ex.Message}\n{ex.StackTrace}", true);
            }
        }
    }
}
