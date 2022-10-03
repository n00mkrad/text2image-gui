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
    public partial class MergeModelsForm : Form
    {
        private int PercentModel1 { get { return 100 - (sliderScale.Value * 5); } }
        private int PercentModel2 { get { return 100 - PercentModel1; } }

        public MergeModelsForm()
        {
            InitializeComponent();
        }

        private void MergeModelsForm_Load(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnReloadModels_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnOpenModelFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Paths.GetModelsPath().Wrap());
        }

        private void LoadModels()
        {
            var ckptFiles = IoUtils.GetFileInfosSorted(Paths.GetModelsPath(), true, "*.ckpt").ToList();

            comboxModel1.Items.Clear();
            comboxModel2.Items.Clear();
            ckptFiles.ForEach(x => comboxModel1.Items.Add(x.Name));
            ckptFiles.ForEach(x => comboxModel2.Items.Add(x.Name));

            if (comboxModel1.SelectedIndex < 0 && comboxModel1.Items.Count > 0)
                comboxModel1.SelectedIndex = 0;

            if (comboxModel2.SelectedIndex < 0)
            {
                if (comboxModel2.Items.Count > 1)
                    comboxModel2.SelectedIndex = 1;
                else if (comboxModel2.Items.Count > 0)
                    comboxModel2.SelectedIndex = 0;
            }
        }

        private void sliderScale_Scroll(object sender, ScrollEventArgs e)
        {
            labelWeight1.Text = $"{PercentModel1}%";
            labelWeight2.Text = $"{PercentModel2}%";
        }

        private async Task<string> Merge ()
        {
            try
            {
                FileInfo model1 = Paths.GetModel(comboxModel1.Text);
                FileInfo model2 = Paths.GetModel(comboxModel2.Text);

                string filename = $"{Path.GetFileNameWithoutExtension(model1.Name)}-{PercentModel1}-with-{Path.GetFileNameWithoutExtension(model2.Name)}-{PercentModel2}{model1.Extension}";
                string outPath = Path.Combine(model1.Directory.FullName, filename);

                List<string> outLines = new List<string>();

                Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat mb/envs/ldo && " +
                    $"python repo/scripts/merge_models.py -1 {model1.FullName.Wrap()} -2 {model2.FullName.Wrap()} -w {(PercentModel2 / 100f).ToStringDot("0.0000")} -o {outPath.Wrap()}";

                // if (!OsUtils.ShowHiddenCmd())
                // {
                //     p.OutputDataReceived += (sender, line) => { if (line != null && line.Data != null) Logger.Log(line.Data); };
                //     p.ErrorDataReceived += (sender, line) => { if (line != null && line.Data != null) Logger.Log(line.Data); };
                // }

                Logger.Log($"cmd {p.StartInfo.Arguments}", true);
                p.Start();

                if (!OsUtils.ShowHiddenCmd())
                {
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                }

                while (!p.HasExited) await Task.Delay(1);

                return outPath;
            }
            catch(Exception ex)
            {
                UiUtils.ShowMessageBox($"Error: {ex.Message}");
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

            if (string.IsNullOrWhiteSpace(comboxModel1.Text) || string.IsNullOrWhiteSpace(comboxModel2.Text) || comboxModel1.Text == comboxModel2.Text)
            {
                UiUtils.ShowMessageBox("Invalid model selection.");
                return;
            }

            Program.MainForm.SetWorking(true);
            Enabled = false;
            btnRun.Text = "Merging...";

            string outPath = await Merge();

            Program.MainForm.SetWorking(false);
            Enabled = true;
            btnRun.Text = "Merge!";

            if (File.Exists(outPath))
                Logger.Log($"Done.\nSaved merged model to:\n{outPath}");
            else
                Logger.Log($"Failed to merge models.");

            //if (File.Exists(outPath))
            //    UiUtils.ShowMessageBox($"Done.\n\nSaved merged model to:\n{outPath}");
            //else
            //    UiUtils.ShowMessageBox($"Failed to merge models.");
        }
    }
}
