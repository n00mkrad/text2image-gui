using StableDiffusionGui.Io;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            
        }

        private void ModelQuickSelectForm_Shown(object sender, EventArgs e)
        {
            LoadModels(true);
            comboxSdModel.DroppedDown = true;
        }

        private void LoadModels(bool loadCombox)
        {
            comboxSdModel.Items.Clear();
            IoUtils.GetFileInfosSorted(Paths.GetModelsPath(), true, "*.ckpt").ToList().ForEach(x => comboxSdModel.Items.Add(x.Name));

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
