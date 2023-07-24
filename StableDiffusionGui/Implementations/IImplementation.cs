using StableDiffusionGui.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StableDiffusionGui.Implementations
{
    public interface IImplementation
    {
        List<string> LastMessages { get; }
        Task Run(TtiSettings s, string outPath);
        void HandleOutput(string line);
        Task Cancel ();
    }
}