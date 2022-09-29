using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class MergeModelsForm : Form
    {
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

        private void LoadModels ()
        {
            var ckptFiles = IoUtils.GetFileInfosSorted(Paths.GetModelsPath(), true, "*.ckpt").ToList();

            comboxModel1.Items.Clear();
            comboxModel2.Items.Clear();
            ckptFiles.ForEach(x => comboxModel1.Items.Add(x.Name));
            ckptFiles.ForEach(x => comboxModel2.Items.Add(x.Name));

            if (comboxModel1.SelectedIndex < 0 && comboxModel1.Items.Count > 0)
                comboxModel1.SelectedIndex = 0;

            if (comboxModel2.SelectedIndex < 0 && comboxModel2.Items.Count > 0)
                comboxModel2.SelectedIndex = 0;
        }
    }
}
