using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StableDiffusionGui.Data
{
    internal class ImageMetadata
    {
        public string Path { get; set; }
        public string Prompt { get; set; }
        public int Steps { get; set; }
        public int BatchSize { get; set; }
        public Size GeneratedResolution { get; set; }
        public float Scale { get; set; }
        public string Sampler { get; set; }
        public int Seed { get; set; }

        public ImageMetadata() { }

        public ImageMetadata(string path, string dreamCli)
        {
            Path = path;

            try
            {
                var split = dreamCli.Split("\"");

                Prompt = split[1].Remove("\"").Trim();

                var parameters = split.Last().Split(" ").Select(x => x.Trim()).ToList();

                GeneratedResolution = new Size();

                foreach (string s in parameters)
                {
                    if (s.StartsWith("-s"))
                        Steps = s.Remove(0, 2).GetInt();

                    if (s.StartsWith("-b"))
                        BatchSize = s.Remove(0, 2).GetInt();

                    if (s.StartsWith("-W"))
                        GeneratedResolution = new Size(s.Remove(0, 2).GetInt(), GeneratedResolution.Height);

                    if (s.StartsWith("-H"))
                        GeneratedResolution = new Size(GeneratedResolution.Width, s.Remove(0, 2).GetInt());

                    if (s.StartsWith("-C"))
                        Scale = s.Remove(0, 2).GetFloat();

                    if (s.StartsWith("-m"))
                        Sampler = s.Remove(0, 2);

                    if (s.StartsWith("-S"))
                        Seed = s.Remove(0, 2).GetInt();
                }
            }
            catch(Exception ex)
            {
                Logger.Log($"Failed to load image metadata from {path}: {ex.Message}", true);
            }
        }
    }
}
