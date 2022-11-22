using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui.MainForm
{
    internal class FormParsing
    {
        private static StableDiffusionGui.MainForm F { get { return Program.MainForm; } }

        public static void LoadMetadataIntoUi(ImageMetadata meta)
        {
            F.textboxPrompt.Text = meta.Prompt;
            F.textboxPromptNeg.Text = meta.NegativePrompt;
            F.sliderSteps.ActualValue = meta.Steps;
            F.sliderScale.ActualValue = (decimal)meta.Scale;
            F.comboxResW.Text = meta.GeneratedResolution.Width.ToString();
            F.comboxResH.Text = meta.GeneratedResolution.Height.ToString();
            F.upDownSeed.Value = meta.Seed;
            F.comboxSampler.SetIfTextMatches(meta.Sampler, true, Strings.Samplers);
            // MainUi.CurrentInitImgPaths = new[] { meta.InitImgName }.Where(x => string.IsNullOrWhiteSpace(x)).ToList(); // Does this even work if we only store the temp path?
            MainUi.CurrentInitImgPaths = null;
            F.comboxSeamless.SelectedIndex = meta.Seamless ? 1 : 0; // TODO: Extend Metadata class to include seamless mode

            if (meta.InitStrength > 0f)
                F.sliderInitStrength.ActualValue = (decimal)meta.InitStrength;

            FormControls.UpdateInitImgAndEmbeddingUi();
        }

        public static void LoadTtiSettingsIntoUi(string[] prompts, string negPrompt = "")
        {
            F.textboxPrompt.Text = string.Join(Environment.NewLine, prompts);

            if (!string.IsNullOrWhiteSpace(negPrompt))
                F.textboxPromptNeg.Text = negPrompt;
        }

        public static void LoadTtiSettingsIntoUi(TtiSettings s)
        {
            F.textboxPrompt.Text = string.Join(Environment.NewLine, s.Prompts);
            F.textboxPromptNeg.Text = s.NegativePrompt;
            F.upDownIterations.Value = s.Iterations;

            try
            {
                F.sliderSteps.ActualValue = s.Params.Get("steps").FromJson<int>();
                F.sliderScale.ActualValue = (decimal)s.Params.Get("scales").FromJson<List<float>>().FirstOrDefault();
                F.comboxResW.Text = s.Params.Get("res").FromJson<Size>().Width.ToString();
                F.comboxResH.Text = s.Params.Get("res").FromJson<Size>().Height.ToString();
                F.upDownSeed.Value = s.Params.Get("seed").FromJson<long>();
                F.comboxSampler.SetIfTextMatches(s.Params.Get("sampler").FromJson<string>(), true, Strings.Samplers);
                MainUi.CurrentInitImgPaths = s.Params.Get("initImgs").FromJson<List<string>>();
                F.sliderInitStrength.ActualValue = (decimal)s.Params.Get("initStrengths").FromJson<List<float>>().FirstOrDefault();
                MainUi.CurrentEmbeddingPath = s.Params.Get("embedding").FromJson<string>();
                F.comboxSeamless.SetIfTextMatches(s.Params.Get("seamless").FromJson<string>(), true, Strings.SeamlessMode);
                F.comboxInpaintMode.SelectedIndex = (int)s.Params.Get("inpainting").FromJson<InpaintMode>();
                F.checkboxHiresFix.Checked = s.Params.Get("hiresFix").FromJson<bool>();
                F.checkboxLockSeed.Checked = s.Params.Get("lockSeed").FromJson<bool>();
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load generation settings. This can happen when you try to load prompts from an older version. ({ex.Message})");
                Logger.Log(ex.StackTrace, true);
            }


            FormControls.UpdateInitImgAndEmbeddingUi();
        }

        public static TtiSettings GetCurrentTtiSettings()
        {
            TtiSettings settings = new TtiSettings
            {
                Implementation = (Implementation)Config.GetInt("comboxImplementation"),
                Prompts = F.textboxPrompt.TextNoPlaceholder.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(),
                NegativePrompt = F.textboxPromptNeg.TextNoPlaceholder.Trim().Replace(Environment.NewLine, " "),
                Iterations = (int)F.upDownIterations.Value,
                Params = new Dictionary<string, string>
                        {
                            { "steps", F.sliderSteps.ActualValueInt.ToJson() },
                            { "scales", MainUi.GetScales(F.textboxExtraScales.Text).ToJson() },
                            { "res", new Size(F.comboxResW.Text.GetInt(), F.comboxResH.Text.GetInt()).ToJson() },
                            { "seed", (F.upDownSeed.Value < 0 ? new Random().Next(0, int.MaxValue) : ((long)F.upDownSeed.Value)).ToJson() },
                            { "sampler", ((Sampler)F.comboxSampler.SelectedIndex).ToString().Lower().ToJson() },
                            { "initImgs", MainUi.CurrentInitImgPaths.ToJson() },
                            { "initStrengths", MainUi.GetInitStrengths(F.textboxExtraInitStrengths.Text).ToJson() },
                            { "embedding", MainUi.CurrentEmbeddingPath.ToJson() },
                            { "seamless", ((SeamlessMode)F.comboxSeamless.SelectedIndex).ToJson() },
                            { "inpainting", ((InpaintMode)F.comboxInpaintMode.SelectedIndex).ToJson() },
                            { "clipSegMask", F.textboxClipsegMask.Text.Trim().ToJson() },
                            { "model", Config.Get(Config.Key.comboxSdModel).ToJson() },
                            { "hiresFix", F.checkboxHiresFix.Checked.ToJson() },
                            { "lockSeed", F.checkboxLockSeed.Checked.ToJson() },
                            { "vae", Config.Get(Config.Key.comboxSdModelVae).ToJson() },
                            { "perlin", F.textboxPerlin.GetFloat().ToJson() },
                            { "threshold", F.textboxThresh.GetInt().ToJson() },
                        },
            };

            return settings;
        }
    }
}
