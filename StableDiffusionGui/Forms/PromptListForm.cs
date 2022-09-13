using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StableDiffusionGui.Forms
{
    public partial class PromptListForm : Form
    {
        public enum ListMode { History, Queue }
        public ListMode PromptListMode = ListMode.History;

        public PromptListForm()
        {
            InitializeComponent();
        }

        private void PromptListForm_Load(object sender, EventArgs e)
        {
            if (PromptListMode == ListMode.History)
            {
                Text = "Prompt History";
                titleLabel.Text = Text;
            }

            if (PromptListMode == ListMode.Queue)
            {
                Text = "Prompt Queue";
                titleLabel.Text = Text;
            }
        }

        private void PromptListForm_Shown(object sender, EventArgs e)
        {
            if (PromptListMode == ListMode.History)
                LoadPromptHistory();
        }

        private void LoadPromptHistory()
        {
            promptListView.Items.Clear();
            promptListView.Items.AddRange(PromptHistory.Prompts.Select(x => new ListViewItem() { Text = $"{x.Prompts.FirstOrDefault()} ({x.Iterations}x)", Tag = x }).Reverse().ToArray());
        }

        private void btnOpenOutFolder_Click(object sender, EventArgs e)
        {
            menuStripDelete.Show(Cursor.Position);
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var reversed = promptListView.Items.Cast<ListViewItem>().Reverse();
            PromptHistory.Delete(promptListView.CheckedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList());
            PromptHistory.Delete(promptListView.SelectedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList());
            LoadPromptHistory();
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PromptHistory.DeleteAll();
            LoadPromptHistory();
        }

        private void promptListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void promptListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var focusedItem = promptListView.FocusedItem;

                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    if (PromptListMode == ListMode.History)
                        menuStripPromptHistory.Show(Cursor.Position);
                }
            }
        }

        private void loadPromptIntoGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiSettings s = (TtiSettings)promptListView.FocusedItem.Tag;
            Program.MainForm.LoadTtiSettingsIntoUi(s.Prompts);
        }

        private void loadPromptAndSettingsIntoGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiSettings s = (TtiSettings)promptListView.FocusedItem.Tag;
            Program.MainForm.LoadTtiSettingsIntoUi(s);
        }
    }
}
