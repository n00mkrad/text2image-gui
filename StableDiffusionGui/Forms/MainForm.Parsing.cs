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
                SetSliderValues(s.Steps, sliderSteps, textboxExtraSteps);
                SetSliderValues(s.ScalesTxt, false, sliderScale, textboxExtraScales);
                SetSliderValues(s.ScalesImg, false, sliderScaleImg, textboxExtraScalesImg);
                MainUi.CurrentInitImgPaths = s.InitImgs.ToList();
                comboxResW.Text = s.Res.Width.ToString();
                comboxResH.Text = s.Res.Height.ToString();
                upDownSeed.Value = s.Seed;
                comboxSampler.SetIfTextMatches(s.Sampler.ToString(), true, Strings.Samplers);
                SetSliderValues(s.InitStrengths, false, sliderInitStrength, textboxExtraInitStrengths);
                comboxSeamless.SetIfTextMatches(s.SeamlessMode.ToString(), true, Strings.SeamlessMode);
                comboxInpaintMode.SetIfTextMatches(s.ImgMode.ToString(), true, Strings.InpaintMode);
                checkboxHiresFix.Checked = s.HiresFix;
                checkboxLockSeed.Checked = s.LockSeed;

                if (s.ResizeGravity != (ImageMagick.Gravity)(-1))
                    comboxResizeGravity.SetIfTextMatches(s.ResizeGravity.ToString(), true, Strings.ImageGravity);

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

        /// <summary> Set values that have a single slider value and optionally an advanced syntax entry textbox </summary>
        private static void SetSliderValues(IEnumerable<int> values, CustomSlider slider, TextBox extraValuesTextbox = null)
        {
            SetSliderValues(values.Select(n => (float)n), true, slider, extraValuesTextbox);
        }

        public TtiSettings GetCurrentTtiSettings()
        {
            TtiSettings settings = new TtiSettings
            {
                Implementation = Config.Instance.Implementation,
                Prompts = textboxPrompt.TextNoPlaceholder.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(),
                NegativePrompt = textboxPromptNeg.Visible ? textboxPromptNeg.TextNoPlaceholder.Trim().Replace(Environment.NewLine, " ") : "",
                Iterations = (int)upDownIterations.Value,
                Steps = MainUi.GetExtraValues(textboxExtraSteps.Text, sliderSteps.ActualValueFloat).Select(x => (int)x).ToArray(),
                InitImgs = MainUi.CurrentInitImgPaths.ToArray(),
                ScalesTxt = MainUi.GetExtraValues(textboxExtraScales.Text, sliderScale.ActualValueFloat).ToArray(),
                InitStrengths = panelInitImgStrength.Visible ? MainUi.GetExtraValues(textboxExtraInitStrengths.Text, sliderInitStrength.ActualValueFloat).ToArray() : new float[] { 0.5f },
                Seed = (upDownSeed.Value < 0 ? new Random().Next(0, int.MaxValue) : ((long)upDownSeed.Value)),
                Sampler = ParseUtils.GetEnum<Sampler>(comboxSampler.Text, true, Strings.Samplers),
                Res = new Size(comboxResW.Text.GetInt(), comboxResH.Text.GetInt()),
                Model = Config.Instance.Model,
                Vae = Config.Instance.ModelVae,
                LockSeed = checkboxLockSeed.Checked,
                ClipSegMask = textboxClipsegMask.Text.Trim(),
                ResizeGravity = comboxResizeGravity.Visible ? ParseUtils.GetEnum<ImageMagick.Gravity>(comboxResizeGravity.Text, true, Strings.ImageGravity) : (ImageMagick.Gravity)(-1),
                ModelArch = comboxModelArch.Visible ? ParseUtils.GetEnum<Enums.Models.SdArch>(comboxModelArch.Text, true, Strings.SdModelArch) : Enums.Models.SdArch.Automatic,
                SeamlessMode = (comboxSeamless.Visible ? ((SeamlessMode)comboxSeamless.SelectedIndex) : SeamlessMode.Disabled),
                SymmetryMode = (comboxSymmetry.Visible ? ((SymmetryMode)comboxSymmetry.SelectedIndex) : SymmetryMode.Disabled),
                HiresFix = checkboxHiresFix.Visible && checkboxHiresFix.Checked,
                Perlin = textboxPerlin.GetFloat(),
                Threshold = textboxThresh.GetInt(),
                ImgMode = (comboxInpaintMode.Visible ? ((ImgMode)comboxInpaintMode.SelectedIndex) : ImgMode.InitializationImage),
                AppendArgs = textboxDebugAppendArgs.Text,
                ScalesImg = MainUi.GetExtraValues(textboxExtraScalesImg.Text, sliderScaleImg.ActualValueFloat).ToArray(),
            };

            return settings;
        }
    }
}
