using StableDiffusionGui.Main;
using System;
using System.Drawing;
using System.Linq;

namespace StableDiffusionGui.Data
{
    public class ImageMetadata
    {
        public string Path { get; set; }
        public string ParsedText { get; set; }
        public string Prompt { get; set; }
        public int Steps { get; set; }
        public int BatchSize { get; set; }
        public Size GeneratedResolution { get; set; }
        public float Scale { get; set; } = -1;
        public string Sampler { get; set; } = "";
        public long Seed { get; set; } = -1;
        public string InitImgName { get; set; }
        public float InitStrength { get; set; }

        public ImageMetadata() { }

        public ImageMetadata(string path, string dreamCli)
        {
            Path = path;
            ParsedText = dreamCli;

            try
            {
                string paramsText = "";


                if (dreamCli.Trim().StartsWith("\""))
                {
                    var split = dreamCli.Split("\"");
                    Prompt = split[1].Remove("\"").Trim();
                    paramsText = split[2];
                }
                else
                {
                    var split = dreamCli.Split(" -");
                    Prompt = split[0];
                    paramsText = string.Join(" -", split.Skip(1));
                }

                GeneratedResolution = new Size();

                bool newFormat = dreamCli.Contains("-W ") && dreamCli.Contains("-H "); // Check if metadata uses new format with spaces
                var parameters = newFormat ? paramsText.Split("-").Select(x => $"-{x.Trim()}").ToList() : paramsText.Split(" ").Select(x => x.Trim()).ToList();

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

                    if (s.StartsWith("-A"))
                        Sampler = s.Remove(0, 2).Trim();

                    if (s.StartsWith("-S"))
                        Seed = s.Remove(0, 2).GetLong();

                    if (s.StartsWith("-f") && !s.Contains("fnformat"))
                        InitStrength = 1f - s.Remove(0, 2).GetFloat();

                    if (s.StartsWith("-IF"))
                        InitImgName = s.Remove(0, 3).Trim();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load image metadata from {path}: {ex.Message}\n{ex.StackTrace}", true);
            }
        }
    }
}
