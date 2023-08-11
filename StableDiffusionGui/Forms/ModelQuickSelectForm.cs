using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class ModelQuickSelectForm : Form
    {
        private Enums.Models.Type _modelType;
        private Implementation _implementation;

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
            _implementation = Config.Instance.Implementation;
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
            {
                comboxModel.Items.Add(Constants.NoneMdl);
                Models.GetVaes().ForEach(x => comboxModel.Items.Add(x.Name));
            }
            else
            {
                Models.GetModels(_modelType, _implementation).ForEach(x => comboxModel.Items.Add(x.Name));
            }

            if (loadCombox)
                ConfigParser.LoadGuiElement(comboxModel, ref _modelType == Enums.Models.Type.Normal ? ref Config.Instance.Model : ref Config.Instance.ModelVae);
        }

        private void ModelQuickSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (IsModelValid())
                    ConfigParser.SaveGuiElement(comboxModel, ref _modelType == Enums.Models.Type.Normal ? ref Config.Instance.Model : ref Config.Instance.ModelVae);

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
            if (comboxModel.Text == Constants.NoneMdl)
                return true;

            if (_modelType == Enums.Models.Type.Vae)
                return Models.GetVaes().Select(v => v.Name).Contains(comboxModel.Text);
            else
                return Models.GetModel(comboxModel.Text, _modelType, _implementation) != null;

        }
    }
}
