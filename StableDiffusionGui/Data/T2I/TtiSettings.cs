using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using Newtonsoft.Json;
using System.Drawing;
using StableDiffusionGui.Ui;
using static StableDiffusionGui.Serialization.JsonUtils;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.MiscUtils;

namespace StableDiffusionGui.Data
{
    public class TtiSettings : IEquatable<TtiSettings>
    {
        public Implementation Implementation { get; set; } = Implementation.InvokeAi;
        public string[] Prompts { get; set; } = new string[] { "" };
        public string NegativePrompt { get; set; } = "";
        public int Iterations { get; set; } = 1;
        public int[] Steps { get; set; } = new int[0];
        public float[] RefinerStrengths { get; set; } = new float[0];
        public string[] InitImgs { get; set; } = new string[] { "" };
        public float[] InitStrengths { get; set; } = new float[0];
        [JsonIgnore]
        public float[] InitStrengthsReverse { get { return InitStrengths.Select(n => 1f - n).ToArray(); } }
        public float[] ScalesTxt { get; set; } = new float[0];
        public float[] ScalesImg { get; set; } = new float[0];
        public float[] GuidanceVals { get; set; } = new float[0];
        public long Seed { get; set; } = 0;
        public Sampler Sampler { get; set; } = (Sampler)(-1);
        public Size Res { get; set; } = new Size();
        public Size UpscaleTargetRes { get; set; } = new Size();
        public UpscaleMethod UpscaleMethod { get; set; } = (UpscaleMethod)(-1);
        public string Model { get; set; } = "";
        public string ModelAux { get; set; } = "";
        public string Vae { get; set; } = "";
        public List<ComfyData.ControlnetInfo> Controlnets { get; set; } = new List<ComfyData.ControlnetInfo>();
        public bool LockSeed { get; set; } = false;
        public string AppendArgs { get; set; } = "";

        #region InvokeAI-specific
        public string ClipSegMask { get; set; } = "";
        public ImageMagick.Gravity ResizeGravity { get; set; } = (ImageMagick.Gravity)(-1);
        public ModelArch ModelArch { get; set; } = ModelArch.Automatic;
        public SeamlessMode SeamlessMode { get; set; } = SeamlessMode.Disabled;
        public SymmetryMode SymmetryMode { get; set; } = SymmetryMode.Disabled;
        public bool HiresFix { get; set; } = false;
        public ImgMode ImgMode { get; set; } = ImgMode.InitializationImage;
        [JsonConverter(typeof(EasyDictValueToListConverter<string, float>))]
        public EasyDict<string, List<float>> Loras { get; set; } = new EasyDict<string, List<float>>();
        #endregion

        public EasyDict<string, string> ExtraParams { get; set; } = new EasyDict<string, string>();
        [JsonIgnore]
        public EasyDict<string, string> ProcessedAndRawPrompts { get; set; } = new EasyDict<string, string>();
        [JsonIgnore]
        public EasyDict<string, string> RawAndProcessedPrompts { get { return ProcessedAndRawPrompts.SwapKeysValues(); } } // Same as above but Key/Value swapped


        public int GetTargetImgCount(ConfigInstance config = null)
        {
            int count;

            try
            {
                // For empty lists, we need to multiply the image count by 1 instead of 0, so clamp it
                int iniImgMult = (InitImgs == null || InitImgs.Length < 1) ? 1 : InitImgs.Length * InitStrengths.Length.Clamp(1, int.MaxValue);
                int scalesMult = ScalesTxt.Length.Clamp(1, int.MaxValue) * ScalesImg.Length.Clamp(1, int.MaxValue);
                int lorasMult = Loras != null && Loras.Count == 1 ? Loras.First().Value.Count : 1;
                int refineMult = RefinerStrengths.Length.Clamp(1, int.MaxValue);
                int guidanceMult = GuidanceVals.Length.Clamp(1, int.MaxValue);

                count = Prompts.Length * Iterations * scalesMult * Steps.Length * iniImgMult * lorasMult * refineMult * guidanceMult;

                if (ConfigParser.UpscaleAndSaveOriginals(config))
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
                string initImgInfo = "";

                if (InitImgs != null && InitImgs.Length > 0)
                {
                    initImgInfo = InitImgs.Length == 1 ? "With Image" : $"{InitImgs.Length} Images";
                }

                //string extraPrompts = Prompts.Length > 1 ? $" (+{Prompts.Length - 1})" : "";

                // Revised version, using lists of values where empty values are ignored at the end when joining into a single colon-separated string:
                var infos = new List<object>
                {
                    $"{Prompts.First().Trunc(85).Wrap()}{(Prompts.Length > 1 ? $" (+{Prompts.Length - 1}" : "")}",
                    Iterations > 1 ? $"{Iterations} Images" : "",
                    FormatUtils.PrintValues(Steps, "{0} Steps"),
                    FormatUtils.PrintValues(ScalesTxt, "CFG {0}"),
                    FormatUtils.PrintValues(GuidanceVals, "G {0}"),
                    $"Seed {Seed}",
                    Res.AsString(),
                    Strings.Samplers[$"{Sampler}"],
                    initImgInfo,
                };

                return string.Join(" - ", infos.Select(o => $"{o}").Where(s => s.IsNotEmpty()));

                //return $"\"{Prompts.FirstOrDefault().Trunc(85)}\"{extraPrompts} - {Iterations} Images - {Steps.FirstOrDefault()} Steps - Seed {Seed} - {Res.AsString()} - {Strings.Samplers[$"{Sampler}"]}{initImgInfo}";
            }
            catch
            {
                return "";
            }
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
                   ImgMode == other.ImgMode;
        }
    }
}