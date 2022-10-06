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

        private async Task<string> Prune()
        {
            try
            {
                bool halfPrec = checkboxHalfPrec.Checked;
                FileInfo model = Paths.GetModel(comboxModel.Text);

                Logger.ClearLogBox();
                Logger.Log($"Pruning model '{Path.GetFileNameWithoutExtension(model.Name)}' and saving as fp{(halfPrec ? "16" : "32")}...");

                string filename = $"{Path.GetFileNameWithoutExtension(model.Name)}-pruned-{(halfPrec ? "fp16" : "fp32")}{model.Extension}";
                string outPath = Path.Combine(model.Directory.FullName, filename);

                List<string> outLines = new List<string>();

                Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                p.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat mb/envs/ldo && " +
                    $"python {Constants.Dirs.RepoSd}/scripts/prune_model.py -i {model.FullName.Wrap()} -o {outPath.Wrap()} {(halfPrec ? "-half" : "")}";

                if (!OsUtils.ShowHiddenCmd())
                {
                    p.OutputDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Prune); };
                    p.ErrorDataReceived += (sender, line) => { Logger.Log(line?.Data, true, false, Constants.Lognames.Prune); };
                }

                Logger.Log($"cmd {p.StartInfo.Arguments}", true);
                p.Start();

                if (!OsUtils.ShowHiddenCmd())
                {
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                }

                while (!p.HasExited) await Task.Delay(1);

                Logger.ClearLogBox();
                return outPath;
            }
            catch (Exception ex)
            {
                Logger.Log($"Pruning Error: {ex.Message}");
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
                Logger.Log($"Done. Saved pruned model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");
            else
                Logger.Log($"Failed to prune model.");

            // if (File.Exists(outPath))
            //     UiUtils.ShowMessageBox($"Done.\n\nSaved pruned model to:\n{outPath}");
            // else
            //     UiUtils.ShowMessageBox($"Failed to prune model.");
        }
    }
}
