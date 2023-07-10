using System.Collections.Generic;
using System.Threading.Tasks;

namespace StableDiffusionGui.Implementations
{
    public interface IImplementation
    {
        Task Run(string[] prompts, string negPrompt, int iterations, Dictionary<string, string> parameters, string outPath);
    }
}
