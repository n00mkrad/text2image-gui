using Microsoft.VisualBasic.Devices;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace StableDiffusionGui.Os
{
    internal class HwInfo
    {
        public static ulong GetTotalRam { get { return new ComputerInfo().TotalPhysicalMemory; } }
        public static float GetTotalRamGb { get { return GetTotalRam / 1024f / 1024f / 1024f; } }
        public static ulong GetFreeRam { get { return new ComputerInfo().AvailablePhysicalMemory; } }
        public static float GetFreeRamGb { get { return GetFreeRam / 1024f / 1024f / 1024f; } }
        public static List<GpuInfo> KnownGpus { get { return GetGpus().Where(g => g.Vendor != GpuInfo.GpuVendor.Unknown).ToList(); } }
        public static List<GpuInfo> KnownGpusNoIntel { get { return GetGpus().Where(g => g.Vendor != GpuInfo.GpuVendor.Intel && g.Vendor != GpuInfo.GpuVendor.Unknown).ToList(); } }

        public class GpuInfo
        {
            public enum GpuVendor { Nvidia, Amd, Intel, Unknown }

            private GpuVendor _vendor = GpuVendor.Unknown;
            public GpuVendor Vendor { get { return _vendor; } }

            private string _name;
            public string Name { get { return _name; } }

            public GpuInfo () { }

            public GpuInfo (string name, GpuVendor vendor)
            {
                _name = name;
                _vendor = vendor;
            }

            public override string ToString ()
            {
                return $"{Name} ({Vendor})";
            }
        }

        public static List<GpuInfo> GetGpus()
        {
            var list = new List<GpuInfo>();
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");

            foreach (var obj in searcher.Get())
            {
                var properties = obj.Properties;

                foreach (var prop in properties)
                {
                    if (prop.Name != "Name")
                        continue;

                    string name = prop.Value.ToString();

                    if (name.StartsWith("NVIDIA "))
                        list.Add(new GpuInfo(name, GpuInfo.GpuVendor.Nvidia));
                    else if (name.StartsWith("AMD "))
                        list.Add(new GpuInfo(name, GpuInfo.GpuVendor.Amd));
                    else if (name.StartsWith("Intel"))
                        list.Add(new GpuInfo(name, GpuInfo.GpuVendor.Intel));
                    else
                        list.Add(new GpuInfo(name, GpuInfo.GpuVendor.Unknown));
                }
            }

            return list;
        }
    }
}
