using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Implementations
{
    public class ImplementationBase
    {
        public virtual string GetEmbeddingStringFormat ()
        {
            return "<{0}>";
        }

        public static async Task CancelNmkdiffusers()
        {
            await TtiProcess.WriteStdIn("stop", 0, true);
            await Logger.WaitForMessageAsync("Stopped.", true, false);
            await Task.Delay(500);
        }
    }
}
