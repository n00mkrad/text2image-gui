using StableDiffusionGui.Io;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ModelQuickSelectForm : Form
    {
        public ModelQuickSelectForm()
        {
            InitializeComponent();
        }

        private void ModelQuickSelectForm_Load(object sender, EventArgs e)
        {
            Location = Cursor.Position;
        }

        private void ModelQuickSelectForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            LoadModels(true);
            comboxSdModel.DroppedDown = true;
        }

        private void LoadModels(bool loadCombox)
        {
            comboxSdModel.Visible = true;
            comboxSdModel.Focus();
            comboxSdModel.Items.Clear();
            Paths.GetModels().ForEach(x => comboxSdModel.Items.Add(x.Name));

            if (loadCombox)
                ConfigParser.LoadGuiElement(comboxSdModel);
        }

        private void ModelQuickSelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (IsModelValid())
                    ConfigParser.SaveGuiElement(comboxSdModel);

                Close();
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private bool IsModelValid()
        {
            return Paths.GetModel(comboxSdModel.Text.Trim()) != null;
        }
    }
}
