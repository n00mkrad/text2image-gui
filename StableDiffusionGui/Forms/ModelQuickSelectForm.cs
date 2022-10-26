using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ModelQuickSelectForm : Form
    {
        private Enums.StableDiffusion.ModelType _modelType;

        public ModelQuickSelectForm(Enums.StableDiffusion.ModelType modelType)
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

            if(_modelType == Enums.StableDiffusion.ModelType.Normal)
                comboxModel.Name = Config.Key.comboxSdModel.ToString();
            else if (_modelType == Enums.StableDiffusion.ModelType.Vae)
                comboxModel.Name = Config.Key.comboxSdModelVae.ToString();

            LoadModels(true);
            comboxModel.DroppedDown = true;
        }

        private void LoadModels(bool loadCombox)
        {
            comboxModel.Visible = true;
            comboxModel.Focus();
            comboxModel.Items.Clear();

            if (_modelType == Enums.StableDiffusion.ModelType.Vae)
                comboxModel.Items.Add("None");

            Paths.GetModels(_modelType).ForEach(x => comboxModel.Items.Add(x.Name));

            if (loadCombox)
                ConfigParser.LoadGuiElement(comboxModel);
        }

        private void ModelQuickSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (IsModelValid())
                    ConfigParser.SaveGuiElement(comboxModel);

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

            return Paths.GetModel(comboxModel.Text.Trim(), false, _modelType) != null;
        }
    }
}
