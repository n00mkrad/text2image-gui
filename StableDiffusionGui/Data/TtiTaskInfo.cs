using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Data
{
    public class TtiTaskInfo
    {
        public int ImgCount { get; set; }
        public int TargetImgCount { get; set; }
        public string OutDir { get; set; } = "";
        public bool SubfoldersPerPrompt { get { return Config.FolderPerPrompt; } }
        public bool IgnoreWildcardsForFilenames { get { return Config.FilenameIgnoreWildcards; } }
        public DateTime StartTime { get; set; } = new DateTime();
        public List<Process> Processes { get; set; } = new List<Process>();
        public ConfigInstance Config { get; set; } = null;
    }
}
