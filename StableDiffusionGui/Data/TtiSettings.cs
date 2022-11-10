using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using System.Windows;

namespace StableDiffusionGui.Data
{
    public class TtiSettings
    {
        public Implementation Implementation { get; set; } = Implementation.StableDiffusion;
        public string[] Prompts { get; set; } = new string[] { "" };
        public string NegativePrompt { get; set; } = "";
        public int Iterations { get; set; } = 1;
        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ProcessedAndRawPrompts { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> RawAndProcessedPrompts { get { return ProcessedAndRawPrompts.SwapKeysValues(); } } // Same as above but Key/Value swapped


        public int GetTargetImgCount()
        {
            int count = 0;

            try
            {
                foreach (string prompt in Prompts)
                {
                    for (int i = 0; i < Iterations; i++)
                    {
                        foreach (float scale in Params["scales"].FromJson<List<float>>())
                        {
                            List<string> initImages = Params["initImgs"].FromJson<List<string>>();

                            if (initImages == null || initImages.Count < 1) // No init image(s)
                            {
                                count++;
                            }
                            else // With init image(s)
                            {
                                foreach (string initImg in initImages)
                                {
                                    foreach (float strength in Params["initStrengths"].FromJson<List<float>>())
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public override string ToString()
        {
            try // New format
            {
                Size s = Params["res"].FromJson<Size>();
                var initImgs = Params["initImgs"].FromJson<List<string>>();
                string init = initImgs != null && initImgs.Count > 0 ? $" - With Image(s)" : "";
                string emb = !string.IsNullOrWhiteSpace(Params["embedding"].FromJson<string>()) ? $" - With Concept" : "";
                string extraPrompts = Prompts.Length > 1 ? $" (+{Prompts.Length - 1})" : "";
                return $"\"{Prompts.FirstOrDefault().Trunc(85)}\"{extraPrompts} - {Iterations} Images - {Params["steps"].FromJson<int>()} Steps - Seed {Params["seed"].FromJson<long>()} - {s.Width}x{s.Height} - {Params["sampler"].FromJson<string>()}{init}{emb}";
            }
            catch
            {
                try // Old format
                {
                    string init = !string.IsNullOrWhiteSpace(Params["initImg"]) ? $" - With Image" : "";
                    string emb = !string.IsNullOrWhiteSpace(Params["embedding"]) ? $" - With Concept" : "";
                    string extraPrompts = Prompts.Length > 1 ? $" (+{Prompts.Length - 1})" : "";
                    return $"\"{Prompts.FirstOrDefault().Trunc(85)}\"{extraPrompts} - {Iterations} Images - {Params["steps"]} Steps - Seed {Params["seed"]} - {Params["res"]} - {Params["sampler"]}{init}{emb}";
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}
