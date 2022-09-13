using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StableDiffusionGui.Os
{
    class SdGuiProcess
    {
        public Process Process { get; }
        public enum ProcessType { Ai, Helper }
        public ProcessType Type { get; }

        public SdGuiProcess(Process p, ProcessType type)
        {
            Process = p;
            Type = type;
        }
    }
}
