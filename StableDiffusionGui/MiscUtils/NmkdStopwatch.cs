using System;
using System.Diagnostics;

namespace StableDiffusionGui.MiscUtils
{
    public class NmkdStopwatch : Stopwatch
    {
        public long ElapsedMs { get { return ElapsedMilliseconds; } }
        public string ElapsedString { get { return FormatUtils.Time(this); } }

        public NmkdStopwatch(bool startOnCreation = true)
        {
            if (startOnCreation)
                Restart();
        }
    }
}
