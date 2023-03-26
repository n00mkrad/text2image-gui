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
        public Implementation Implementation { get; set; } = Implementation.InvokeAi;
        public string[] Prompts { get; set; } = new string[] { "" };
        public string NegativePrompt { get; set; } = "";
        public int Iterations { get; set; } = 1;
        public EasyDict<string, string> Params { get; set; } = new EasyDict<string, string>();
        public EasyDict<string, string> ProcessedAndRawPrompts { get; set; } = new EasyDict<string, string>();
        public EasyDict<string, string> RawAndProcessedPrompts { get { return ProcessedAndRawPrompts.SwapKeysValues(); } } // Same as above but Key/Value swapped


        public int GetTargetImgCount()
        {
            int count = 0;

            try
            {
                foreach (string prompt in Prompts)
                {
                    for (int i = 0; i < Iterations; i++)
                    {
                        foreach (float scale in Params.Get("scales").FromJson<List<float>>())
                        {
                            foreach (int stepCount in Params.Get("steps").FromJson<List<int>>())
                            {
                                List<string> initImages = Params.Get("initImgs").FromJson<List<string>>();

                                if (initImages == null || initImages.Count < 1) // No init image(s)
                                {
                                    count++;
                                }
                                else // With init image(s)
                                {
                                    foreach (string initImg in initImages)
                                    {
                                        foreach (float strength in Params.Get("initStrengths").FromJson<List<float>>())
                                        {
                                            count++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ConfigParser.UpscaleAndSaveOriginals)
                    count *= 2;

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
                Size s = Params.Get("res").FromJson<Size>();
                var initImgs = Params.Get("initImgs").FromJson<List<string>>();
                string init = initImgs != null && initImgs.Count > 0 ? $" - With Image(s)" : "";
                string extraPrompts = Prompts.Length > 1 ? $" (+{Prompts.Length - 1})" : "";
                return $"\"{Prompts.FirstOrDefault().Trunc(85)}\"{extraPrompts} - {Iterations} Images - {Params.Get("steps").FromJson<int[]>().FirstOrDefault()} Steps - Seed {Params.Get("seed").FromJson<long>()} - {s.Width}x{s.Height} - {Params.Get("sampler").FromJson<string>()}{init}";
            }
            catch
            {
                try // Old format
                {
                    string init = !string.IsNullOrWhiteSpace(Params.Get("initImg")) ? $" - With Image" : "";
                    string extraPrompts = Prompts.Length > 1 ? $" (+{Prompts.Length - 1})" : "";
                    return $"\"{Prompts.FirstOrDefault().Trunc(85)}\"{extraPrompts} - {Iterations} Images - {Params.Get("steps")} Steps - Seed {Params.Get("seed")} - {Params.Get("res")} - {Params.Get("sampler")}{init}";
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}
