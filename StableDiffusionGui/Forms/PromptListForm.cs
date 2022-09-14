using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                btnAddPromptsToQueue.Visible = false;
            }

            if (PromptListMode == ListMode.Queue)
            {
                Text = "Prompt Queue";
                btnAddPromptsToQueue.Visible = true;
            }

            titleLabel.Text = Text;
        }

        private void PromptListForm_Shown(object sender, EventArgs e)
        {
            if (PromptListMode == ListMode.History)
                LoadPromptHistory();
            if (PromptListMode == ListMode.Queue)
                LoadQueue();
        }

        private void LoadPromptHistory()
        {
            promptListView.Items.Clear();
            promptListView.Items.AddRange(PromptHistory.Prompts.Select(x => new ListViewItem() { Text = x.ToString(), Tag = x }).Reverse().ToArray());
        }

        private void LoadQueue()
        {
            promptListView.Items.Clear();
            promptListView.Items.AddRange(MainUi.Queue.Select(x => new ListViewItem() { Text = x.ToString(), Tag = x }).Reverse().ToArray());
        }

        private void btnOpenOutFolder_Click(object sender, EventArgs e)
        {
            menuStripDelete.Show(Cursor.Position);
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PromptListMode == ListMode.History)
            {
                PromptHistory.Delete(promptListView.CheckedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList());
                PromptHistory.Delete(promptListView.SelectedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList());
                LoadPromptHistory();
            }

            if (PromptListMode == ListMode.Queue)
            {
                MainUi.Queue = MainUi.Queue.Except(promptListView.CheckedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList()).ToList();
                MainUi.Queue = MainUi.Queue.Except(promptListView.SelectedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList()).ToList();
                LoadQueue();
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PromptListMode == ListMode.History)
            {
                PromptHistory.DeleteAll();
                LoadPromptHistory();
            }

            if (PromptListMode == ListMode.Queue)
            {
                MainUi.Queue.Clear();
                LoadQueue();
            }
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

        private void btnAddPromptsToQueue_Click(object sender, EventArgs e)
        {
            MainUi.Queue.Add(Program.MainForm.GetCurrentTtiSettings());
            LoadQueue();
        }
    }
}
