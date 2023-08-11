using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class PruneModelsForm : Form
    {
        private Dictionary<string, string> _uiStrings = new Dictionary<string, string>()
        {
            { Enums.Models.Precision.Fp16.ToString(), "Half Precision (FP16 - 2 GB)" },
            { Enums.Models.Precision.Fp32.ToString(), "Full Precision (FP32 - 4 GB)" },
        };

        public PruneModelsForm()
        {
            InitializeComponent();
        }

        private void PruneModelsForm_Load(object sender, EventArgs e)
        {
            LoadModels();
            comboxPrunePrecision.FillFromEnum<Enums.Models.Precision>(_uiStrings);

            // ConfigParser.LoadComboxIndex(comboxPrunePrecision, ref Config.Instance.PrunePrecisionIdx);
            // ConfigParser.LoadGuiElement(checkboxPruneDeleteInput, ref Config.Instance.PruneDeleteInput);
        }

        private void btnReloadModels_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnOpenModelFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(ModelFoldersForm.Folder.Models).ShowDialogForm();
            LoadModels();
        }

        private void LoadModels()
        {
            comboxModel.Items.Clear();
            Models.GetModels().ForEach(x => comboxModel.Items.Add(x.Name));

            if (comboxModel.SelectedIndex < 0 && comboxModel.Items.Count > 0)
                comboxModel.SelectedIndex = 0;
        }

        private async Task<string> Prune()
        {
            try
            {
                bool fp16 = (Enums.Models.Precision)comboxPrunePrecision.SelectedIndex == Enums.Models.Precision.Fp16;
                Model model = Models.GetModel(comboxModel.Text);

                Logger.ClearLogBox();
                Logger.Log($"Pruning model '{Path.GetFileNameWithoutExtension(model.Name)}' and saving as fp{(fp16 ? "16" : "32")} checkpoint...");

                string filename = $"{Path.GetFileNameWithoutExtension(model.Name)}-pruned-{(fp16 ? "fp16" : "fp32")}{model.Extension}";
                string outPath = Path.Combine(model.Directory.FullName, filename);

                List<string> outLines = new List<string>();

                Process py = OsUtils.NewProcess(true, logAction: (s) => Logger.Log(s, true, false, Constants.Lognames.Prune));
                py.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && {Constants.Files.VenvActivate} && " +
                    $"python {Constants.Dirs.SdRepo}/scripts/prune_model.py -i {model.FullName.Wrap()} -o {outPath.Wrap(true)} {(fp16 ? "-half" : "")}";

                Logger.Log($"cmd {py.StartInfo.Arguments}", true);
                py.Start();
                await OsUtils.WaitForProcessExit(py);

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

            SaveConfig();

            Program.SetState(Program.BusyState.Script);
            Enabled = false;
            btnRun.Text = "Pruning...";

            string outPath = await Prune();

            Program.SetState(Program.BusyState.Standby);
            Enabled = true;
            btnRun.Text = "Prune!";

            if (File.Exists(outPath))
            {
                Logger.Log($"Done. Saved pruned model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");

                if (checkboxPruneDeleteInput.Checked)
                {
                    var inputFile = Models.GetModel(comboxModel.Text);
                    bool deleteSuccess = IoUtils.TryDeleteIfExists(inputFile.FullName);
                    Logger.Log($"{(deleteSuccess ? "Deleted" : "Failed to delete")} input file '{inputFile.Name}'.");

                    LoadModels();
                }
            }
            else
            {
                Logger.Log($"Failed to prune model.");
            }

            // if (File.Exists(outPath))
            //     UiUtils.ShowMessageBox($"Done.\n\nSaved pruned model to:\n{outPath}");
            // else
            //     UiUtils.ShowMessageBox($"Failed to prune model.");
        }

        private void PruneModelsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig ()
        {
            // ConfigParser.SaveComboxIndex(comboxPrunePrecision, ref Config.Instance.PrunePrecisionIdx);
            // ConfigParser.SaveGuiElement(checkboxPruneDeleteInput, ref Config.Instance.PruneDeleteInput);
        }
    }
}
