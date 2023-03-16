using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class ModelQuickSelectForm : Form
    {
        private Enums.Models.Type _modelType;
        private Implementation _implementation;
        private string ModelConfigKey { get { return _modelType == Enums.Models.Type.Normal ? Config.Keys.Model : Config.Keys.ModelVae; } }

        public ModelQuickSelectForm(Enums.Models.Type modelType)
        {
            InitializeComponent();
            _modelType = modelType;
        }

        private void ModelQuickSelectForm_Load(object sender, EventArgs e)
        {
            Location = Cursor.Position;
        }

        private void ModelQuickSelectForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            _implementation = ParseUtils.GetEnum<Implementation>(Config.Get<string>(Config.Keys.ImplementationName));
            LoadModels(true);

            if (comboxModel.Items.Count > 0)
            {
                comboxModel.DroppedDown = true;
            }
            else
            {
                comboxModel.Visible = false;
                statusLabel.Text = "No compatible models found for the current implementation.";
            }
        }

        private void LoadModels(bool loadCombox)
        {
            comboxModel.Visible = true;
            comboxModel.Focus();
            comboxModel.Items.Clear();

            if (_modelType == Enums.Models.Type.Vae)
                comboxModel.Items.Add("None");

            Models.GetModels(_modelType, _implementation).ForEach(x => comboxModel.Items.Add(x.Name));

            if (loadCombox)
                ConfigParser.LoadGuiElement(comboxModel, ModelConfigKey);
        }

        private void ModelQuickSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (IsModelValid())
                    ConfigParser.SaveGuiElement(comboxModel, ModelConfigKey);

                Program.MainForm.TryRefreshUiState();
                Close();
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private bool IsModelValid()
        {
            if (comboxModel.Text == "None")
                return true;

            return Models.GetModel(comboxModel.Text.Trim(), false, _modelType, _implementation) != null;
        }
    }
}
