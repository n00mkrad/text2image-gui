using HTAlt;
using MetadataExtractor.Formats.Photoshop;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TextToImage
    {
        public static TtiTaskInfo CurrentTask { get; set; } = null;

        public static async Task RunTti(TtiSettings s)
        {
            if (s.Implementation == Implementation.StableDiffusion)
                await TtiProcess.RunStableDiffusion(s.Prompts, s.Iterations, s.Params["steps"].GetInt(), s.Params["scales"].Replace(" ", "").Split(",").Select(x => x.GetFloat()).ToArray(), s.Params["seed"].GetLong(), s.Params["sampler"], FormatUtils.ParseSize(s.Params["res"]), s.OutPath);
        }
    }
}
