using StableDiffusionGui.Controls;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Implementations;
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
            ((Action)(() =>
            {
                textboxPrompt.Text = meta.Prompt;
                textboxPromptNeg.Text = meta.NegativePrompt;
                sliderSteps.ActualValue = meta.Steps;
                textboxExtraSteps.Text = "";
                sliderScale.ActualValue = (decimal)meta.Scale;
                textboxExtraScales.Text = "";
                sliderScaleImg.ActualValue = (decimal)meta.ScaleImg;
                comboxResW.Text = meta.GeneratedResolution.Width.ToString();
                comboxResH.Text = meta.GeneratedResolution.Height.ToString();
                SetUpscaleRes(meta.UpscaleResolution, meta.GeneratedResolution, meta.UpscaleMethod);
                checkboxHiresFix.Checked = meta.HiResFix;
                sliderRefinerStart.ActualValue = (decimal)meta.RefineStrength;
                upDownSeed.Value = meta.Seed;
                comboxSampler.SetWithText(meta.Sampler, true, Strings.Samplers);
                // MainUi.CurrentInitImgPaths = new[] { meta.InitImgName }.Where(x => string.IsNullOrWhiteSpace(x)).ToList(); // Does this even work if we only store the temp path?
                MainUi.CurrentInitImgPaths.Clear();
                comboxSeamless.SelectedIndex = meta.SeamlessMode == SeamlessMode.Disabled ? 0 : 1;

                if (comboxModel.Items.Cast<object>().Any(item => item.ToString() == meta.Model))
                    comboxModel.Text = meta.Model;

                if (meta.InitStrength > 0f)
                    sliderInitStrength.ActualValue = (decimal)meta.InitStrength;

                textboxExtraInitStrengths.Text = "";
                SetLoras(new EasyDict<string, List<float>>(meta.Loras.ToDictionary(p => p.Key, p => p.Value.AsList())));

            })).RunWithUiStopped(this, "Error loading metadata into UI:", true);

            TryRefreshUiState();
        }

        public void LoadTtiSettingsIntoUi(string[] prompts, string negPrompt = "")
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, prompts);
            textboxPromptNeg.Text = negPrompt;
        }

        public void LoadTtiSettingsIntoUi(TtiSettings s)
        {
            ((Action)(() =>
            {
                textboxPrompt.Text = string.Join(Environment.NewLine, s.Prompts);
                textboxPromptNeg.Text = s.NegativePrompt;
                upDownIterations.Value = s.Iterations;
                SetSliderValues(s.Steps, sliderSteps, textboxExtraSteps);
                SetSliderValues(s.ScalesTxt, false, sliderScale, textboxExtraScales);
                SetSliderValues(s.ScalesImg, false, sliderScaleImg, textboxExtraScalesImg);
                SetSliderValues(s.RefinerStrengths, false, sliderRefinerStart, textboxExtraRefinerValues);
                MainUi.CurrentInitImgPaths = s.InitImgs.ToList();
                comboxResW.Text = s.Res.Width.ToString();
                comboxResH.Text = s.Res.Height.ToString();
                SetUpscaleRes(s.UpscaleTargetRes, s.Res, s.UpscaleMethod);
                upDownSeed.Value = s.Seed;
                comboxSampler.SetWithText(s.Sampler.ToString(), true, Strings.Samplers);
                SetSliderValues(s.InitStrengths, false, sliderInitStrength, textboxExtraInitStrengths);
                comboxSeamless.SetWithText(s.SeamlessMode.ToString(), true, Strings.SeamlessMode);
                comboxInpaintMode.SetWithText(s.ImgMode.ToString(), true, Strings.InpaintMode);
                checkboxHiresFix.Checked = s.HiresFix;
                checkboxLockSeed.Checked = s.LockSeed;

                if (s.ResizeGravity != (ImageMagick.Gravity)(-1))
                    comboxResizeGravity.SetWithText(s.ResizeGravity.ToString(), true, Strings.ImageGravity);

                SetLoras(s.Loras);
                Controlnets = s.Controlnets.Concat(Enumerable.Repeat(default(ComfyData.ControlnetInfo), 5)).Take(5).ToArray();
                ControlnetSlotChanged(false); // Explicitly call function to load Controlnets into GUI

            })).RunWithUiStopped(this, "Error loading image generation settings:", true);

            TryRefreshUiState();
        }

        private void SetUpscaleRes (Size resUpscale, Size resGenerated, UpscaleMethod method)
        {
            if (resUpscale != resGenerated && resUpscale.Width > resGenerated.Width && resUpscale.Height > resGenerated.Height)
            {
                string factorW = (resUpscale.Width / (float)resGenerated.Width).ToStringDot("0.###");
                string factorH = (resUpscale.Height / (float)resGenerated.Height).ToStringDot("0.###");

                if (factorW == factorH && factorW.Length <= 4) // Check if factor is the same on both axes & max 4 decimal places
                {
                    comboxUpscaleMode.SetWithEnum((method == UpscaleMethod.Latent ? UpscaleMode.LatentsFactor : UpscaleMode.UltimeUpsFactor), stringMap: Strings.UpscaleModes);
                    updownUpscaleFactor.Value = (decimal)factorW.GetFloat();
                }
                else
                {
                    comboxUpscaleMode.SetWithEnum(UpscaleMode.LatentsTargetRes, stringMap: Strings.UpscaleModes);
                    updownUpscaleResultW.Value = resUpscale.Width;
                    updownUpscaleResultH.Value = resUpscale.Height;
                }
            }
            else
            {
                comboxUpscaleMode.SetWithEnum(UpscaleMode.Disabled, stringMap: Strings.UpscaleModes);
            }
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
                var v = toInt ? values.Select(x => (float)(int)x) : values;
                extraValuesTextbox.Text = MainUi.GetValuesStr(v.ToList());
            }
        }

        /// <summary> Set values that have a single slider value and optionally an advanced syntax entry textbox </summary>
        private static void SetSliderValues(IEnumerable<int> values, CustomSlider slider, TextBox extraValuesTextbox = null)
        {
            SetSliderValues(values.Select(n => (float)n), true, slider, extraValuesTextbox);
        }

        public TtiSettings GetCurrentTtiSettings()
        {
            Config.Instance.ModelAux = comboxModel2.Text;

            TtiSettings settings = new TtiSettings
            {
                Implementation = Config.Instance.Implementation,
                Prompts = textboxPrompt.TextNoPlaceholder.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(),
                NegativePrompt = textboxPromptNeg.Visible ? textboxPromptNeg.TextNoPlaceholder.Trim().Replace(Environment.NewLine, " ") : "",
                Iterations = (int)upDownIterations.Value,
                Steps = MainUi.GetExtraValues(textboxExtraSteps.Text, sliderSteps.ActualValueFloat).Select(x => (int)x).ToArray(),
                RefinerStrengths = panelRefineStart.Visible ? MainUi.GetExtraValues(textboxExtraRefinerValues.Text, sliderRefinerStart.ActualValueFloat).ToArray() : new float[1],
                InitImgs = MainUi.CurrentInitImgPaths.ToArray(),
                ScalesTxt = MainUi.GetExtraValues(textboxExtraScales.Text, sliderScale.ActualValueFloat).ToArray(),
                GuidanceVals = panelGuidance.Visible ? MainUi.GetExtraValues(textboxExtraGuidances.Text, sliderGuidance.ActualValueFloat).ToArray() : new float[1],
                InitStrengths = panelInitImgStrength.Visible ? MainUi.GetExtraValues(textboxExtraInitStrengths.Text, sliderInitStrength.ActualValueFloat).ToArray() : new float[1],
                Seed = (upDownSeed.Value < 0 ? new Random().Next(0, int.MaxValue) : ((long)upDownSeed.Value)),
                Sampler = ParseUtils.GetEnum<Sampler>(comboxSampler.Text, true, Strings.Samplers),
                Res = new Size(comboxResW.GetInt(), comboxResH.GetInt()),
                UpscaleTargetRes = GetUpscaleTargetSize(),
                Model = Config.Instance.Model,
                ModelAux = Config.Instance.ModelAux,
                Vae = Config.Instance.ModelVae,
                Controlnets = comboxControlnetSlot.Visible ? Controlnets.Where(c => c != null && c.Model.IsNotEmpty() && c.Model != Constants.NoneMdl).ToList() : new List<ComfyData.ControlnetInfo>(),
                LockSeed = checkboxLockSeed.Checked,
                ClipSegMask = textboxClipsegMask.Text.Trim(),
                ResizeGravity = comboxResizeGravity.Visible ? ParseUtils.GetEnum<ImageMagick.Gravity>(comboxResizeGravity.Text, true, Strings.ImageGravity) : (ImageMagick.Gravity)(-1),
                ModelArch = comboxModelArch.Visible ? ParseUtils.GetEnum<ModelArch>(comboxModelArch.Text, true, Strings.ModelArch) : ModelArch.Automatic,
                SeamlessMode = comboxSeamless.Visible ? ((SeamlessMode)comboxSeamless.SelectedIndex) : SeamlessMode.Disabled,
                SymmetryMode = comboxSymmetry.Visible ? ((SymmetryMode)comboxSymmetry.SelectedIndex) : SymmetryMode.Disabled,
                HiresFix = checkboxHiresFix.Visible && checkboxHiresFix.Checked,
                ImgMode = comboxInpaintMode.Visible ? ((ImgMode)comboxInpaintMode.SelectedIndex) : (ImgMode)(-1),
                ScalesImg = MainUi.GetExtraValues(textboxExtraScalesImg.Text, sliderScaleImg.ActualValueFloat).ToArray(),
                Loras = GetLoras(),
            };

            if(settings.UpscaleTargetRes != settings.Res)
                settings.UpscaleMethod = ParseUtils.GetEnum<UpscaleMode>(comboxUpscaleMode.Text, stringMap: Strings.UpscaleModes) == UpscaleMode.UltimeUpsFactor ? UpscaleMethod.UltimateSd : UpscaleMethod.Latent;

            return settings;
        }

        /// <summary> Applies LoRA list to UI list. If <paramref name="checkLoras"/> is false, enable/disable checkboxes are unaffected and only weights are applied </summary>
        public void SetLoras(EasyDict<string, List<float>> loras, bool checkLoras = true)
        {
            foreach (var row in gridLoras.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow))
            {
                var matches = loras.Where(l => l.Key == row.Cells[1].Value.ToString()).ToList();

                if (checkLoras) // If this is false, also enable/disable the entry if it's in the list. If true, only weight will be set
                    row.Cells[0].Value = matches.Any();

                if (!matches.Any())
                    continue;

                if (row.Cells[1].Value.ToString() == matches[0].Key)
                {
                    if (checkLoras)
                        row.Cells[0].Value = true;

                    row.Cells[2].Value = MainUi.GetValuesStr(matches[0].Value, false);
                }
            }

            if (checkLoras)
            {
                SortLoras();
            }
        }

        private EasyDict<string, List<float>> GetLoras(bool onlyEnabled = true)
        {
            var loras = new EasyDict<string, List<float>>();

            foreach (DataGridViewRow row in gridLoras.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (onlyEnabled && (bool)row.Cells[0].Value != true) // Skip disabled LoRAs if onlyEnabled == True
                    continue;

                string name = row.Cells[1].Value.ToString();

                if (name.IsNotEmpty())
                    loras[name] = MainUi.GetExtraValues(row.Cells[2].Value.ToString(), 1.0f);
            }

            return loras;
        }
    }
}
