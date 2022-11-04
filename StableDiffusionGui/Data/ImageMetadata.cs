using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StableDiffusionGui.Data
{
    public class ImageMetadata
    {
        public enum MetadataType { InvokeAi, Auto1111 }
        public MetadataType Type { get; set; }
        public string Path { get; set; } = "";
        public string ParsedText { get; set; } = ""; 
        public string Prompt { get; set; } = "";
        public string NegativePrompt { get; set; } = "";
        public string CombinedPrompt { get { return FormatUtils.GetCombinedPrompt(Prompt, NegativePrompt); } }
        public int Steps { get; set; } = -1;
        public int BatchSize { get; set; } = 1;
        public Size GeneratedResolution { get; set; }
        public float Scale { get; set; } = -1;
        public string Sampler { get; set; } = "";
        public long Seed { get; set; } = -1;
        public string InitImgName { get; set; } = "";
        public float InitStrength { get; set; } = 0f;
        public bool Seamless { get; set; } = false;

        private readonly Dictionary<MetadataType, string> _tags = new Dictionary<MetadataType, string>() {
            { MetadataType.InvokeAi, "Dream: " },
            { MetadataType.Auto1111, "parameters:" }
        };

        public ImageMetadata() { }

        public ImageMetadata(string path)
        {
            Path = path;

            try
            {
                IEnumerable<MetadataExtractor.Directory> directories = MetadataExtractor.ImageMetadataReader.ReadMetadata(path);

                MetadataExtractor.Directory pngTextDir = directories.Where(x => x.Name.Lower() == "png-text").FirstOrDefault();

                if (pngTextDir != null)
                {
                    foreach(var tag in pngTextDir.Tags)
                    {
                        if (tag.Description.Contains(_tags[MetadataType.InvokeAi]))
                        {
                            LoadInfoInvokeAi(tag.Description.Split(_tags[MetadataType.InvokeAi]).Last());
                            return;
                        }

                        if (tag.Description.Contains(_tags[MetadataType.Auto1111]))
                        {
                            LoadInfoAuto1111(tag.Description.Split(_tags[MetadataType.Auto1111]).Last());
                            return;
                        }
                    }
                }
            }
            catch
            {

            }
        }


        public void LoadInfoInvokeAi(string info)
        {
            Type = MetadataType.InvokeAi;
            ParsedText = info;

            try
            {
                string paramsText = "";


                if (info.Trim().StartsWith("\""))
                {
                    var split = info.Split("\"");
                    Prompt = split[1].Remove("\"").Trim();
                    paramsText = split[2];
                }
                else
                {
                    var split = info.Split(" -");
                    Prompt = split[0];
                    paramsText = string.Join(" -", split.Skip(1));
                }

                if(Prompt.EndsWith("]") && Prompt.Contains(" [") && Prompt.Count(x => x =='[') == 1 && Prompt.Count(x => x == ']') == 1)
                {
                    NegativePrompt = Prompt.Split(" [").Last().Split(']')[0];
                    var split = Prompt.Split(" [");
                    Prompt = string.Join(" [", split.Reverse().Skip(1).Reverse());
                }

                GeneratedResolution = new Size();

                bool newFormat = info.Contains("-W ") && info.Contains("-H "); // Check if metadata uses new format with spaces
                var parameters = newFormat ? paramsText.Split("-").Select(x => $"-{x.Trim()}").ToList() : paramsText.Split(" ").Select(x => x.Trim()).ToList();

                foreach (string s in parameters)
                {
                    if (s.StartsWith("-s") && !s.Contains("seamless"))
                        Steps = s.Remove(0, 2).GetInt();

                    else if (s.StartsWith("-b"))
                        BatchSize = s.Remove(0, 2).GetInt();

                    else if (s.StartsWith("-W"))
                        GeneratedResolution = new Size(s.Remove(0, 2).GetInt(), GeneratedResolution.Height);

                    else if (s.StartsWith("-H"))
                        GeneratedResolution = new Size(GeneratedResolution.Width, s.Remove(0, 2).GetInt());

                    else if (s.StartsWith("-C"))
                        Scale = s.Remove(0, 2).GetFloat();

                    else if (s.StartsWith("-A"))
                        Sampler = s.Remove(0, 2).Trim();

                    else if (s.StartsWith("-S"))
                        Seed = s.Remove(0, 2).GetLong();

                    else if (s.StartsWith("-f") && !s.Contains("fnformat"))
                        InitStrength = 1f - s.Remove(0, 2).GetFloat();

                    else if (s.StartsWith("-IF"))
                        InitImgName = s.Remove(0, 3).Trim();

                    else if (s == "-seamless")
                        Seamless = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load InvokeAI image metadata from: {ex.Message}\n{ex.StackTrace}", true);
            }
        }

        public void LoadInfoAuto1111(string info)
        {
            Type = MetadataType.Auto1111;
            ParsedText = info;

            try
            {
                var lines = info.SplitIntoLines();

                Prompt = lines[0].Trim();
                NegativePrompt = lines[1].Split("Negative prompt: ")[1];
                Steps = lines[2].Split("Steps: ")[1].Split(',')[0].GetInt();
                GeneratedResolution = Parser.GetSize(lines[2].Split("Size: ")[1].Split(',')[0]);
                Scale = lines[2].Split("CFG scale: ")[1].Split(',')[0].GetFloat();
                Sampler = lines[2].Split("Sampler: ")[1].Split(',')[0].Replace(" ", "_");
                Seed = lines[2].Split("Seed: ")[1].Split(',')[0].GetLong();
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load Automatic1111 image metadata from: {ex.Message}\n{ex.StackTrace}", true);
            }
        }
    }
}
