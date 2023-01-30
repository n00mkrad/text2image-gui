using StableDiffusionGui.Controls;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui.MainFormUtils
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
            F.sliderScaleImg.ActualValue = (decimal)meta.ScaleImg;
            F.comboxResW.Text = meta.GeneratedResolution.Width.ToString();
            F.comboxResH.Text = meta.GeneratedResolution.Height.ToString();
            F.upDownSeed.Value = meta.Seed;
            F.comboxSampler.SetIfTextMatches(meta.Sampler, true, Strings.Samplers);
            // MainUi.CurrentInitImgPaths = new[] { meta.InitImgName }.Where(x => string.IsNullOrWhiteSpace(x)).ToList(); // Does this even work if we only store the temp path?
            MainUi.CurrentInitImgPaths = null;
            F.comboxSeamless.SelectedIndex = meta.SeamlessMode == SeamlessMode.Disabled ? 0 : 1;

            if (meta.InitStrength > 0f)
                F.sliderInitStrength.ActualValue = (decimal)meta.InitStrength;

            FormControls.RefreshUiAfterSettingsChanged();
        }

        public static void LoadTtiSettingsIntoUi(string[] prompts, string negPrompt = "")
        {
            F.textboxPrompt.Text = string.Join(Environment.NewLine, prompts);
            F.textboxPromptNeg.Text = negPrompt;
        }

        public static void LoadTtiSettingsIntoUi(TtiSettings s)
        {
            F.textboxPrompt.Text = string.Join(Environment.NewLine, s.Prompts);
            F.textboxPromptNeg.Text = s.NegativePrompt;
            F.upDownIterations.Value = s.Iterations;

            try
            {
                SetSliderValues(s.Params.FromJson<List<float>>("steps"), true, F.sliderSteps, F.textboxExtraSteps);
                SetSliderValues(s.Params.FromJson<List<float>>("scales"), false, F.sliderScale, F.textboxExtraScales);
                SetSliderValues(s.Params.FromJson<List<float>>("scalesImg"), false, F.sliderScaleImg, F.textboxExtraScalesImg);
                F.comboxResW.Text = s.Params.Get("res").FromJson<Size>().Width.ToString();
                F.comboxResH.Text = s.Params.Get("res").FromJson<Size>().Height.ToString();
                F.upDownSeed.Value = s.Params.Get("seed").FromJson<long>();
                F.comboxSampler.SetIfTextMatches(s.Params.Get("sampler").FromJson<string>(), true, Strings.Samplers);
                MainUi.CurrentInitImgPaths = s.Params.Get("initImgs").FromJson<List<string>>();
                SetSliderValues(s.Params.FromJson<List<float>>("initStrengths"), false, F.sliderInitStrength, null);
                MainUi.CurrentEmbeddingPath = s.Params.Get("embedding").FromJson<string>();
                F.comboxSeamless.SetIfTextMatches(s.Params.Get("seamless").FromJson<string>(), true, Strings.SeamlessMode);
                F.comboxInpaintMode.SelectedIndex = (int)s.Params.Get("inpainting").FromJson<InpaintMode>();
                F.checkboxHiresFix.Checked = s.Params.Get("hiresFix").FromJson<bool>();
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load generation settings. This can happen when you try to load prompts from an older version. ({ex.Message})");
                Logger.Log(ex.StackTrace, true);
            }

            FormControls.RefreshUiAfterSettingsChanged();
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

        private static List<string> Prompt
        {
            get
            {
                var Res = F.textboxPrompt.TextNoPlaceholder.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                string Add = "";

                if (Program.MainForm.cbBaW.Checked)
                {
                    Add += ", (black and white image)++";
                }
                
                if (Program.MainForm.cbSepia.Checked)
                {
                    Add += ", (sepia)++";
                }

                if (Program.MainForm.cbDetFace.Checked)
                {
                    Add += ", (detailed eyes)++, (detailed hair)+, (detailed face)+";
                }

                for (int i = 0; i < Res.Count; i++)
                {
                    Res[i] += Add;
                }

                return Res;
            }
        }

        public static TtiSettings GetCurrentTtiSettings()
        {
            TtiSettings settings = new TtiSettings
            {
                Implementation = ParseUtils.GetEnum<Implementation>(Config.Get<string>(Config.Keys.ImplementationName)),

                Prompts = Prompt.ToArray(),
                NegativePrompt = F.textboxPromptNeg.Visible ? F.textboxPromptNeg.TextNoPlaceholder.Trim().Replace(Environment.NewLine, " ") : "",
                Iterations = (int)F.upDownIterations.Value,
                Params = new Dictionary<string, string>
                {
                    { "steps", MainUi.GetExtraValues(F.textboxExtraSteps.Text, F.sliderSteps.ActualValueFloat).Select(x => (int)x).ToArray().ToJson() },
                    { "scales", MainUi.GetExtraValues(F.textboxExtraScales.Text, F.sliderScale.ActualValueFloat).ToJson() },
                    { "scalesImg", MainUi.GetExtraValues(F.textboxExtraScalesImg.Text, F.sliderScaleImg.ActualValueFloat).ToJson() },
                    { "res", new Size(F.comboxResW.Text.GetInt(), F.comboxResH.Text.GetInt()).ToJson() },
                    { "seed", (F.upDownSeed.Value < 0 ? new Random().Next(0, int.MaxValue) : ((long)F.upDownSeed.Value)).ToJson() },
                    { "sampler", ((Sampler)F.comboxSampler.SelectedIndex).ToString().Lower().ToJson() },
                    { "initImgs", MainUi.CurrentInitImgPaths.ToJson() },
                    { "initStrengths", F.panelInitImgStrength.Visible ? MainUi.GetExtraValues("", F.sliderInitStrength.ActualValueFloat).ToJson() : new List<float>() { 0.5f }.ToJson() },
                    { "embedding", MainUi.CurrentEmbeddingPath.ToJson() },
                    { "seamless", ((SeamlessMode)F.comboxSeamless.SelectedIndex).ToJson() },
                    { "inpainting", ((InpaintMode)F.comboxInpaintMode.SelectedIndex).ToJson() },
                    { "clipSegMask", F.textboxClipsegMask.Text.Trim().ToJson() },
                    { "model", Config.Get<string>(Config.Keys.Model).ToJson() },
                    { "scd", Config.Get<string>(Config.Keys.ModelSchd).ToJson() },
                    { "hiresFix", (F.checkboxHiresFix.Visible && F.checkboxHiresFix.Checked).ToJson() },
                    { "vae", Config.Get<string>(Config.Keys.ModelVae).ToJson() },
                    { "perlin", F.textboxPerlin.GetFloat().ToJson() },
                    { "threshold", F.textboxThresh.GetInt().ToJson() },
                },
            };

            return settings;
        }
    }
}
