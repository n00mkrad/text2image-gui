using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class DreamboothForm : Form
    {
        private Dictionary<string, string> _uiStrings = new Dictionary<string, string>()
        {
            { Enums.Dreambooth.TrainPreset.HigherQuality.ToString(), "Higher Quality" },
            { Enums.Dreambooth.TrainPreset.FasterSpeed.ToString(), "Faster Training Speed" },
        };

        public DreamboothForm()
        {
            InitializeComponent();
        }

        private void DreamboothForm_Load(object sender, EventArgs e)
        {
            comboxTrainPreset.FillFromEnum<Enums.Dreambooth.TrainPreset>(_uiStrings, 0);
            LoadModels();
        }

        private void DreamboothForm_Shown(object sender, EventArgs e)
        {
            Task.Run(() => CheckGpu());
        }

        private async Task CheckGpu()
        {
            bool valid = true;
            var gpus = await GpuUtils.GetCudaGpusCached();

            if (valid && gpus.Count < 1)
            {
                UiUtils.ShowMessageBox("No compatible GPU detected.", UiUtils.MessageType.Error);
                valid = false;
            }

            int cudaDeviceOpt = Config.GetInt("comboxCudaDevice");
            Data.Gpu gpu = gpus[cudaDeviceOpt - 2];

            if (valid && gpu.VramGb < 23f)
            {
                UiUtils.ShowMessageBox($"Your GPU has {gpu.VramGb} GB VRAM, but 24 GB are currently required for DreamBooth training.", UiUtils.MessageType.Error);
                valid = false;
            }
            else if (valid && gpu.VramGb < 25f)
            {
                UiUtils.ShowMessageBox($"Your GPU has {gpu.VramGb} GB VRAM.\nThis is enough to train DreamBooth, but you should make sure that no other VRAM-consuming applications are running (Games, Browsers, Game Launchers, AI/ML).", UiUtils.MessageType.Message);
            }

            if (valid)
                btnStart.Enabled = true;
            else
                Close();
        }

        private void btnReloadModels_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnOpenModelFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm().ShowDialog();
            LoadModels();
        }

        private void LoadModels()
        {
            comboxBaseModel.Items.Clear();
            Paths.GetModels().ForEach(x => comboxBaseModel.Items.Add(x.Name));

            if (comboxBaseModel.SelectedIndex < 0 && comboxBaseModel.Items.Count > 0)
                comboxBaseModel.SelectedIndex = 0;
        }

        private async Task<string> RunTraining()
        {
            try
            {
                FileInfo model1 = Paths.GetModel(comboxBaseModel.Text);
                FileInfo model2 = Paths.GetModel(comboxTrainPreset.Text);

                Logger.ClearLogBox();
                // Logger.Log($"Merging models '{Path.GetFileNameWithoutExtension(model1.Name)}' ({PercentModel1}%) and '{Path.GetFileNameWithoutExtension(model2.Name)}' ({PercentModel2}%)...");

                // string filename = $"{Path.GetFileNameWithoutExtension(model1.Name)}-{PercentModel1}-with-{Path.GetFileNameWithoutExtension(model2.Name)}-{PercentModel2}{model1.Extension}";
                // string outPath = Path.Combine(model1.Directory.FullName, filename);
                // 
                // Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                // p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat mb/envs/ldo && " +
                //     $"python {Constants.Dirs.RepoSd}/scripts/merge_models.py -1 {model1.FullName.Wrap()} -2 {model2.FullName.Wrap()} -w {(PercentModel2 / 100f).ToStringDot("0.0000")} -o {outPath.Wrap(true)}";
                // 
                // if (!OsUtils.ShowHiddenCmd())
                // {
                //     p.OutputDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Merge); };
                //     p.ErrorDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Merge); };
                // }
                // 
                // Logger.Log($"cmd {p.StartInfo.Arguments}", true);
                // p.Start();
                // 
                // if (!OsUtils.ShowHiddenCmd())
                // {
                //     p.BeginOutputReadLine();
                //     p.BeginErrorReadLine();
                // }
                // 
                // while (!p.HasExited) await Task.Delay(1);

                Logger.ClearLogBox();
                return ""; // outPath;
            }
            catch (Exception ex)
            {
                UiUtils.ShowMessageBox($"Merging Error: {ex.Message}");
                Logger.Log(ex.StackTrace);
                return "";
            }
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            if (string.IsNullOrWhiteSpace(comboxBaseModel.Text) || string.IsNullOrWhiteSpace(comboxTrainPreset.Text) || comboxBaseModel.Text == comboxTrainPreset.Text)
            {
                UiUtils.ShowMessageBox("Invalid model selection.");
                return;
            }

            Program.MainForm.SetWorking(true);
            Enabled = false;
            btnStart.Text = "Merging...";

            string outPath = await RunTraining();

            Program.MainForm.SetWorking(false);
            Enabled = true;
            btnStart.Text = "Merge!";

            if (File.Exists(outPath))
                Logger.Log($"Done. Saved merged model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");
            else
                Logger.Log($"Failed to merge models.");

            //if (File.Exists(outPath))
            //    UiUtils.ShowMessageBox($"Done.\n\nSaved merged model to:\n{outPath}");
            //else
            //    UiUtils.ShowMessageBox($"Failed to merge models.");
        }

        private void btnTrainImgsBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxTrainImgsDir.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && Directory.Exists(dialog.FileName))
                textboxTrainImgsDir.Text = dialog.FileName;
        }
    }
}
