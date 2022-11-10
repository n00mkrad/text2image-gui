using Microsoft.VisualBasic.Devices;
using System;

namespace StableDiffusionGui.Os
{
    internal class HwInfo
    {
        public static ulong GetTotalRam { get { return new ComputerInfo().TotalPhysicalMemory; } }
        public static float GetTotalRamGb { get { return GetTotalRam / 1024f / 1024f / 1024f; } }
        public static ulong GetFreeRam { get { return new ComputerInfo().AvailablePhysicalMemory; } }
        public static float GetFreeRamGb { get { return GetFreeRam / 1024f / 1024f / 1024f; } }
    }
}
