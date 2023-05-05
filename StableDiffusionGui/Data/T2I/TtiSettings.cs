using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using Newtonsoft.Json;
using System.Drawing;
using StableDiffusionGui.Ui;

namespace StableDiffusionGui.Data
{
    public class TtiSettings : IEquatable<TtiSettings>
    {
        public Implementation Implementation { get; set; } = Implementation.InvokeAi;
        public string[] Prompts { get; set; } = new string[] { "" };
        public string NegativePrompt { get; set; } = "";
        public int Iterations { get; set; } = 1;
        public int[] Steps { get; set; } = new int[0];
        public string[] InitImgs { get; set; } = new string[] { "" };
        public float[] InitStrengths { get; set; } = new float[0];
        public float[] ScalesTxt { get; set; } = new float[0];
        public float[] ScalesImg { get; set; } = new float[0];
        public long Seed { get; set; } = 0;
        public Sampler Sampler { get; set; } = (Sampler)(-1);
        public Size Res { get; set; } = new Size();
        public string Model { get; set; } = "";
        public string Vae { get; set; } = "";
        public bool LockSeed { get; set; } = false;
        public string AppendArgs { get; set; } = "";

        #region InvokeAI-specific
        public string ClipSegMask { get; set; } = "";
        public ImageMagick.Gravity ResizeGravity { get; set; } = (ImageMagick.Gravity)(-1);
        public Enums.Models.SdArch ModelArch { get; set; } = Enums.Models.SdArch.Automatic;
        public SeamlessMode SeamlessMode { get; set; } = SeamlessMode.Disabled;
        public SymmetryMode SymmetryMode { get; set; } = SymmetryMode.Disabled;
        public bool HiresFix { get; set; } = false;
        public float Perlin { get; set; } = 0f;
        public int Threshold { get; set; } = 0;
        public ImgMode ImgMode { get; set; } = ImgMode.InitializationImage;
        public EasyDict<string, float> Loras { get; set; } = new EasyDict<string, float>();
        #endregion

        public EasyDict<string, string> ExtraParams { get; set; } = new EasyDict<string, string>();
        [JsonIgnore]
        public EasyDict<string, string> ProcessedAndRawPrompts { get; set; } = new EasyDict<string, string>();
        [JsonIgnore]
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
                        foreach (float scale in ScalesTxt)
                        {
                            foreach (int stepCount in Steps)
                            {
                                if (InitImgs == null || InitImgs.Length < 1) // No init image(s)
                                {
                                    count++;
                                }
                                else // With init image(s)
                                {
                                    foreach (string initImg in InitImgs)
                                    {
                                        foreach (float strength in InitStrengths)
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
                string init = "";

                if (InitImgs != null && InitImgs.Length > 0)
                {
                    if (InitImgs.Length == 1)
                        init = " - With Image";
                    else
                        init = $" - {InitImgs.Length} Images";
                }

                string extraPrompts = Prompts.Length > 1 ? $" (+{Prompts.Length - 1})" : "";
                return $"\"{Prompts.FirstOrDefault().Trunc(85)}\"{extraPrompts} - {Iterations} Images - {Steps.FirstOrDefault()} Steps - Seed {Seed} - {Res.AsString()} - {Strings.Samplers[Sampler.ToString()]}{init}";
            }
            catch
            {
                // try // Old format
                // {
                //     string init = !string.IsNullOrWhiteSpace(Params.Get("initImg")) ? $" - With Image" : "";
                //     string extraPrompts = Prompts.Length > 1 ? $" (+{Prompts.Length - 1})" : "";
                //     return $"\"{Prompts.FirstOrDefault().Trunc(85)}\"{extraPrompts} - {Iterations} Images - {Params.Get("steps")} Steps - Seed {Params.Get("seed")} - {Params.Get("res")} - {Params.Get("sampler")}{init}";
                // }
                // catch
                // {
                //     return "";
                // }
            }

            return "";
        }

        public bool Equals(TtiSettings other)
        {
            if (other == null)
                return false;

            return Implementation == other.Implementation &&
                   Enumerable.SequenceEqual(Prompts, other.Prompts) &&
                   NegativePrompt == other.NegativePrompt &&
                   Iterations == other.Iterations &&
                   Enumerable.SequenceEqual(Steps, other.Steps) &&
                   Enumerable.SequenceEqual(InitImgs, other.InitImgs) &&
                   Enumerable.SequenceEqual(InitStrengths, other.InitStrengths) &&
                   Enumerable.SequenceEqual(ScalesTxt, other.ScalesTxt) &&
                   Enumerable.SequenceEqual(ScalesImg, other.ScalesImg) &&
                   Seed == other.Seed &&
                   Sampler == other.Sampler &&
                   Res == other.Res &&
                   Model == other.Model &&
                   Vae == other.Vae &&
                   LockSeed == other.LockSeed &&
                   AppendArgs == other.AppendArgs &&
                   ClipSegMask == other.ClipSegMask &&
                   ResizeGravity == other.ResizeGravity &&
                   ModelArch == other.ModelArch &&
                   SeamlessMode == other.SeamlessMode &&
                   SymmetryMode == other.SymmetryMode &&
                   HiresFix == other.HiresFix &&
                   Perlin == other.Perlin &&
                   Threshold == other.Threshold &&
                   ImgMode == other.ImgMode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            TtiSettings other = obj as TtiSettings;

            if (other == null)
                return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Implementation.GetHashCode();
                hash = hash * 31 + Prompts.GetHashCode();
                hash = hash * 31 + NegativePrompt.GetHashCode();
                hash = hash * 31 + Iterations.GetHashCode();
                hash = hash * 31 + Steps.GetHashCode();
                hash = hash * 31 + InitImgs.GetHashCode();
                hash = hash * 31 + InitStrengths.GetHashCode();
                hash = hash * 31 + ScalesTxt.GetHashCode();
                hash = hash * 31 + ScalesImg.GetHashCode();
                hash = hash * 31 + Seed.GetHashCode();
                hash = hash * 31 + Sampler.GetHashCode();
                hash = hash * 31 + Res.GetHashCode();
                hash = hash * 31 + Model.GetHashCode();
                hash = hash * 31 + Vae.GetHashCode();
                hash = hash * 31 + LockSeed.GetHashCode();
                hash = hash * 31 + AppendArgs.GetHashCode();
                hash = hash * 31 + ClipSegMask.GetHashCode();
                hash = hash * 31 + ResizeGravity.GetHashCode();
                hash = hash * 31 + ModelArch.GetHashCode();
                hash = hash * 31 + SeamlessMode.GetHashCode();
                hash = hash * 31 + SymmetryMode.GetHashCode();
                hash = hash * 31 + HiresFix.GetHashCode();
                hash = hash * 31 + Perlin.GetHashCode();
                hash = hash * 31 + Threshold.GetHashCode();
                hash = hash * 31 + ImgMode.GetHashCode();

                return hash;
            }
        }

        public static bool operator ==(TtiSettings left, TtiSettings right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(TtiSettings left, TtiSettings right)
        {
            return !(left == right);
        }

        public bool EqualsWithoutPrompts(TtiSettings other)
        {
            if (other == null)
                return false;

            return Implementation == other.Implementation &&
                   Iterations == other.Iterations &&
                   Enumerable.SequenceEqual(Steps, other.Steps) &&
                   Enumerable.SequenceEqual(InitImgs, other.InitImgs) &&
                   Enumerable.SequenceEqual(InitStrengths, other.InitStrengths) &&
                   Enumerable.SequenceEqual(ScalesTxt, other.ScalesTxt) &&
                   Enumerable.SequenceEqual(ScalesImg, other.ScalesImg) &&
                   Seed == other.Seed &&
                   Sampler == other.Sampler &&
                   Res == other.Res &&
                   Model == other.Model &&
                   Vae == other.Vae &&
                   LockSeed == other.LockSeed &&
                   AppendArgs == other.AppendArgs &&
                   ClipSegMask == other.ClipSegMask &&
                   ResizeGravity == other.ResizeGravity &&
                   ModelArch == other.ModelArch &&
                   SeamlessMode == other.SeamlessMode &&
                   SymmetryMode == other.SymmetryMode &&
                   HiresFix == other.HiresFix &&
                   Perlin == other.Perlin &&
                   Threshold == other.Threshold &&
                   ImgMode == other.ImgMode;
        }
    }
}