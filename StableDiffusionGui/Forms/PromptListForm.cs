using LibGit2Sharp;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
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
        private ListMode _promptListMode = ListMode.History;

        public PromptListForm(ListMode mode)
        {
            _promptListMode = mode;
            InitializeComponent();
        }

        private void PromptListForm_Load(object sender, EventArgs e)
        {
                if (_promptListMode == ListMode.History)
            {
                Text = "Prompt History";
                btnAddPromptsToQueue.Visible = false;
                panelEnableHistory.Visible = true;
                panelFilter.Location = new System.Drawing.Point(570, panelFilter.Location.Y);
            }

            if (_promptListMode == ListMode.Queue)
            {
                Text = "Prompt Queue";
                btnAddPromptsToQueue.Visible = true;
                panelEnableHistory.Visible = false;
                panelFilter.Location = new System.Drawing.Point(680, panelFilter.Location.Y);
            }

            titleLabel.Text = Text;

            ConfigParser.LoadGuiElement(checkboxEnableHistory);
        }

        private void PromptListForm_Shown(object sender, EventArgs e)
        {
            if (_promptListMode == ListMode.History)
                LoadPromptHistory();
            if (_promptListMode == ListMode.Queue)
                LoadQueue();
        }

        private void LoadPromptHistory(string filter = "")
        {
            promptListView.Items.Clear();
            var items = Filter(PromptHistory.History, filter).Select(x => new ListViewItem() { Text = x.ToString(), Tag = x }).Reverse();
            promptListView.Items.AddRange(items.ToArray());
        }

        private void LoadQueue(string filter = "")
        {
            promptListView.Items.Clear();
            var items = Filter(MainUi.Queue, filter).Select(x => new ListViewItem() { Text = x.ToString(), Tag = x }).Reverse();
            promptListView.Items.AddRange(items.ToArray());
        }

        private IEnumerable<TtiSettings> Filter (IEnumerable<TtiSettings> ttiSettings, string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                return ttiSettings.Where(x => x.Prompts.FirstOrDefault().Lower().Contains(text.Lower()));
            else
                return ttiSettings;
        }

        private void btnOpenOutFolder_Click(object sender, EventArgs e)
        {
            menuStripDelete.Show(Cursor.Position);
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_promptListMode == ListMode.History)
            {
                PromptHistory.Delete(promptListView.CheckedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList());
                PromptHistory.Delete(promptListView.SelectedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList());
                LoadPromptHistory();
            }

            if (_promptListMode == ListMode.Queue)
            {
                MainUi.Queue = MainUi.Queue.Except(promptListView.CheckedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList()).ToList();
                MainUi.Queue = MainUi.Queue.Except(promptListView.SelectedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag).ToList()).ToList();
                LoadQueue();
            }
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_promptListMode == ListMode.History)
            {
                PromptHistory.DeleteAll();
                LoadPromptHistory();
            }

            if (_promptListMode == ListMode.Queue)
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
                    if (_promptListMode == ListMode.History)
                        menuStripPromptHistory.Show(Cursor.Position);
                }
            }
        }

        private void loadPromptIntoGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiSettings s = (TtiSettings)promptListView.FocusedItem.Tag;
            Program.MainForm.LoadTtiSettingsIntoUi(s.Prompts);
            Close();
        }

        private void loadPromptAndSettingsIntoGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiSettings s = (TtiSettings)promptListView.FocusedItem.Tag;
            Program.MainForm.LoadTtiSettingsIntoUi(s);
            Close();
        }

        private void btnAddPromptsToQueue_Click(object sender, EventArgs e)
        {
            var settings = Program.MainForm.GetCurrentTtiSettings();

            if (!settings.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).Any())
                return;

            MainUi.Queue.Add(settings);
            LoadQueue();
        }

        private void PromptListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigParser.SaveGuiElement(checkboxEnableHistory);
        }

        string _previousFilterText = "";

        private void textboxFilter_TextChanged(object sender, EventArgs e)
        {
            string t = textboxFilter.Text.Trim();

            if (t == _previousFilterText)
                return;

            _previousFilterText = t;

            if (_promptListMode == ListMode.History)
                LoadPromptHistory(t);
            else if (_promptListMode == ListMode.Queue)
                LoadQueue(t);
        }

        private void copyPromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiSettings s = (TtiSettings)promptListView.FocusedItem.Tag;
            OsUtils.SetClipboard(s.Prompts.FirstOrDefault());
        }
    }
}
