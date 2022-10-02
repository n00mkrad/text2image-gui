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
    public partial class PruneModelsForm : Form
    {

        public PruneModelsForm()
        {
            InitializeComponent();
        }

        private void PruneModelsForm_Load(object sender, EventArgs e)
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

            comboxModel.Items.Clear();
            ckptFiles.ForEach(x => comboxModel.Items.Add(x.Name));

            if (comboxModel.SelectedIndex < 0 && comboxModel.Items.Count > 0)
                comboxModel.SelectedIndex = 0;
        }

        private async Task<string> Prune ()
        {
            try
            {
                FileInfo model1 = Paths.GetModel(comboxModel.Text);

                bool halfPrec = checkboxHalfPrec.Checked;

                string filename = $"{Path.GetFileNameWithoutExtension(model1.Name)}-pruned-{(halfPrec ? "fp16" : "fp32")}{model1.Extension}";
                string outPath = Path.Combine(model1.Directory.FullName, filename);

                List<string> outLines = new List<string>();

                Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat mb/envs/ldo && " +
                    $"python repo/scripts/prune_model.py -i {model1.FullName.Wrap()} -o {outPath.Wrap()} {(halfPrec ? "-half" : "")}";

                // if (!OsUtils.ShowHiddenCmd())
                // {
                //     p.OutputDataReceived += (sender, line) => { if (line != null && line.Data != null) Logger.Log(line.Data.Trunc(120)); };
                //     p.ErrorDataReceived += (sender, line) => { if (line != null && line.Data != null) Logger.Log(line.Data.Trunc(120)); };
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
                Logger.Log($"Error: {ex.Message}");
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

            if (string.IsNullOrWhiteSpace(comboxModel.Text))
            {
                UiUtils.ShowMessageBox("Invalid model selection.");
                return;
            }

            Program.MainForm.SetWorking(true);
            Enabled = false;
            btnRun.Text = "Pruning...";

            string outPath = await Prune();

            Program.MainForm.SetWorking(false);
            Enabled = true;
            btnRun.Text = "Prune!";

            if (File.Exists(outPath))
                UiUtils.ShowMessageBox($"Done.\n\nSaved pruned model to:\n{outPath}");
            else
                UiUtils.ShowMessageBox($"Failed to prune model.");
        }
    }
}
