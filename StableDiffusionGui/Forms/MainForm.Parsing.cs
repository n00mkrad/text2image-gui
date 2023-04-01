using StableDiffusionGui.Controls;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm
    {
        public void LoadMetadataIntoUi(ImageMetadata meta)
        {
            textboxPrompt.Text = meta.Prompt;
            textboxPromptNeg.Text = meta.NegativePrompt;
            sliderSteps.ActualValue = meta.Steps;
            sliderScale.ActualValue = (decimal)meta.Scale;
            sliderScaleImg.ActualValue = (decimal)meta.ScaleImg;
            comboxResW.Text = meta.GeneratedResolution.Width.ToString();
            comboxResH.Text = meta.GeneratedResolution.Height.ToString();
            upDownSeed.Value = meta.Seed;
            comboxSampler.SetIfTextMatches(meta.Sampler, true, Strings.Samplers);
            // MainUi.CurrentInitImgPaths = new[] { meta.InitImgName }.Where(x => string.IsNullOrWhiteSpace(x)).ToList(); // Does this even work if we only store the temp path?
            MainUi.CurrentInitImgPaths.Clear();
            comboxSeamless.SelectedIndex = meta.SeamlessMode == SeamlessMode.Disabled ? 0 : 1;

            if (meta.InitStrength > 0f)
                sliderInitStrength.ActualValue = (decimal)meta.InitStrength;

            TryRefreshUiState();
        }

        public void LoadTtiSettingsIntoUi(string[] prompts, string negPrompt = "")
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, prompts);
            textboxPromptNeg.Text = negPrompt;
        }

        public void LoadTtiSettingsIntoUi(TtiSettings s)
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, s.Prompts);
            textboxPromptNeg.Text = s.NegativePrompt;
            upDownIterations.Value = s.Iterations;

            ((Action)(() =>
            {
                SetSliderValues(s.Params.FromJson<List<float>>("steps"), true, sliderSteps, textboxExtraSteps);
                SetSliderValues(s.Params.FromJson<List<float>>("scales"), false, sliderScale, textboxExtraScales);
                SetSliderValues(s.Params.FromJson<List<float>>("scalesImg"), false, sliderScaleImg, textboxExtraScalesImg);
                MainUi.CurrentInitImgPaths = s.Params.Get("initImgs").FromJson<List<string>>();
                Size res = s.Params.Get("res").FromJson<Size>();
                comboxResW.Text = res.Width.ToString();
                comboxResH.Text = res.Height.ToString();
                upDownSeed.Value = s.Params.Get("seed").FromJson<long>();
                comboxSampler.SetIfTextMatches(s.Params.Get("sampler").FromJson<string>(), true, Strings.Samplers);
                SetSliderValues(s.Params.FromJson<List<float>>("initStrengths"), false, sliderInitStrength, textboxExtraInitStrengths);
                comboxSeamless.SetIfTextMatches(s.Params.Get("seamless").FromJson<string>(), true, Strings.SeamlessMode);
                comboxInpaintMode.SelectedIndex = (int)s.Params.Get("inpainting").FromJson<InpaintMode>();
                checkboxHiresFix.Checked = s.Params.Get("hiresFix").FromJson<bool>();
                checkboxLockSeed.Checked = s.Params.Get("lockSeed").FromJson<bool>();

                if(s.Params.Get("resizeGravity").IsNotEmpty())
                    comboxResizeGravity.SetIfTextMatches(s.Params.Get("resizeGravity").FromJson<string>(), true, Strings.ImageGravity);

            })).RunWithUiStoppedShowErrors(this, "Error loading image generation settings:");

            TryRefreshUiState();
        }

        /// <summary> Set values that have a single slider value and optionally an advanced syntax entry textbox </summary>
        private static void SetSliderValues(IEnumerable<float> values, bool toInt, CustomSlider slider, TextBox extraValuesTextbox = null)
        {
            if (values != null && values.Count() == 1)
            {
                slider.ActualValue = toInt ? (int)values.First() : (decimal)values.First();
            }
            else
            {
                var v = toInt ? values.Select(x => ((int)x).ToString()) : values.Select(x => x.ToStringDot());

                if (v.Count() > 1)
                    extraValuesTextbox.Text = string.Join(",", v);
                else
                    extraValuesTextbox.Text = "";
            }
        }

        public TtiSettings GetCurrentTtiSettings()
        {
            TtiSettings settings = new TtiSettings
            {
                Implementation = ParseUtils.GetEnum<Implementation>(Config.Get<string>(Config.Keys.ImplementationName)),

                Prompts = textboxPrompt.TextNoPlaceholder.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(),
                NegativePrompt = textboxPromptNeg.Visible ? textboxPromptNeg.TextNoPlaceholder.Trim().Replace(Environment.NewLine, " ") : "",
                Iterations = (int)upDownIterations.Value,
                Params = new EasyDict<string, string>
                {
                    { "steps", MainUi.GetExtraValues(textboxExtraSteps.Text, sliderSteps.ActualValueFloat).Select(x => (int)x).ToArray().ToJson() },
                    { "scales", MainUi.GetExtraValues(textboxExtraScales.Text, sliderScale.ActualValueFloat).ToJson() },
                    { "scalesImg", MainUi.GetExtraValues(textboxExtraScalesImg.Text, sliderScaleImg.ActualValueFloat).ToJson() },
                    { "res", new Size(comboxResW.Text.GetInt(), comboxResH.Text.GetInt()).ToJson() },
                    { "seed", (upDownSeed.Value < 0 ? new Random().Next(0, int.MaxValue) : ((long)upDownSeed.Value)).ToJson() },
                    { "sampler", ((Sampler)comboxSampler.SelectedIndex).ToString().Lower().ToJson() },
                    { "initImgs", MainUi.CurrentInitImgPaths.ToJson() },
                    { "initStrengths", panelInitImgStrength.Visible ? MainUi.GetExtraValues(textboxExtraInitStrengths.Text, sliderInitStrength.ActualValueFloat).ToJson() : new List<float>() { 0.5f }.ToJson() },
                    { "seamless", (comboxSeamless.Visible ? ((SeamlessMode)comboxSeamless.SelectedIndex) : SeamlessMode.Disabled).ToJson() },
                    { "symmetry", (comboxSymmetry.Visible ? ((SymmetryMode)comboxSymmetry.SelectedIndex) : SymmetryMode.Disabled).ToJson() },
                    { "inpainting", (comboxInpaintMode.Visible ? ((InpaintMode)comboxInpaintMode.SelectedIndex) : InpaintMode.Disabled).ToJson() },
                    { "clipSegMask", textboxClipsegMask.Text.Trim().ToJson() },
                    { "model", Config.Get<string>(Config.Keys.Model).ToJson() },
                    { "hiresFix", (checkboxHiresFix.Visible && checkboxHiresFix.Checked).ToJson() },
                    { "lockSeed", checkboxLockSeed.Checked.ToJson() },
                    { "vae", Config.Get<string>(Config.Keys.ModelVae).ToJson() },
                    { "perlin", textboxPerlin.GetFloat().ToJson() },
                    { "threshold", textboxThresh.GetInt().ToJson() },
                    { "appendArgs", textboxDebugAppendArgs.Text.ToJson() },
                    { "resizeGravity", comboxResizeGravity.Visible ? ParseUtils.GetEnum<ImageMagick.Gravity>(comboxResizeGravity.Text, true, Strings.ImageGravity).ToJson() : "" },
                    { "modelArch", comboxModelArch.Visible ? ParseUtils.GetEnum<Enums.Models.SdArch>(comboxModelArch.Text, true, Strings.SdModelArch).ToJson() : "" },
                },
            };

            return settings;
        }
    }
}
