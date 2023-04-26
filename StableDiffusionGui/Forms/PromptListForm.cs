using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class PromptListForm : CustomForm
    {
        public enum ListMode { History, Queue }
        public ListMode PromptListMode = ListMode.History;

        public PromptListForm(ListMode mode)
        {
            PromptListMode = mode;
            InitializeComponent();
        }

        private void PromptListForm_Load(object sender, EventArgs e)
        {
            if (PromptListMode == ListMode.History)
            {
                Text = "Prompt History";
                btnAddPromptsToQueue.Visible = false;
                panelEnableHistory.Visible = true;
                panelFilter.Location = new System.Drawing.Point(570, panelFilter.Location.Y);
            }

            if (PromptListMode == ListMode.Queue)
            {
                Text = "Prompt Queue";
                btnAddPromptsToQueue.Visible = true;
                panelEnableHistory.Visible = false;
                panelFilter.Location = new System.Drawing.Point(680, panelFilter.Location.Y);
            }

            titleLabel.Text = Text;
            ConfigParser.LoadGuiElement(checkboxEnableHistory, ref Config.Instance.EnablePromptHistory);
        }

        private void PromptListForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            TabOrderInit(new List<Control>() { checkboxEnableHistory, promptListView }, 0);

            if (PromptListMode == ListMode.History)
                LoadPromptHistory();
            if (PromptListMode == ListMode.Queue)
                LoadQueue();
        }

        private void LoadPromptHistory()
        {
            string filter = textboxFilter.Text.Trim();
            promptListView.Items.Clear();
            var items = Filter(PromptHistory.History, filter).Select(settings => new ListViewItem() { Text = settings.ToString(), Tag = settings });
            promptListView.Items.AddRange(items.ToArray());
            LoadTooltips();
        }

        public void LoadQueue()
        {
            string filter = textboxFilter.Text.Trim();
            promptListView.Items.Clear();
            var items = Filter(MainUi.Queue, filter).Select(x => new ListViewItem() { Text = x.ToString(), Tag = x });
            promptListView.Items.AddRange(items.ToArray());
            LoadTooltips();
        }

        private void LoadTooltips()
        {
            foreach (ListViewItem item in promptListView.Items)
            {
                TtiSettings s = (TtiSettings)item.Tag;

                if (s.Prompts.FirstOrDefault().Length < 85 && string.IsNullOrWhiteSpace(s.NegativePrompt)) // Do not add tooltips where full prompt is already visible in list
                    continue;

                item.ToolTipText = $"Prompt:\n{s.Prompts.FirstOrDefault()}{(string.IsNullOrWhiteSpace(s.NegativePrompt) ? "" : $"\n\nNegative Prompt:\n{s.NegativePrompt}")}";
            }
        }

        private IEnumerable<TtiSettings> Filter(IEnumerable<TtiSettings> ttiSettings, string text)
        {
            return string.IsNullOrWhiteSpace(text) ? ttiSettings : ttiSettings.Where(x => x.Prompts.FirstOrDefault().Lower().Contains(text.Lower()));
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
                var checkedItems = new HashSet<TtiSettings>(promptListView.CheckedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag));
                var selectedItems = new HashSet<TtiSettings>(promptListView.SelectedItems.Cast<ListViewItem>().Select(x => (TtiSettings)x.Tag));

                TtiSettings item;
                ConcurrentQueue<TtiSettings> newQueue = new ConcurrentQueue<TtiSettings>();

                while (MainUi.Queue.TryDequeue(out item))
                {
                    if (!checkedItems.Contains(item) && !selectedItems.Contains(item))
                        newQueue.Enqueue(item);
                }

                MainUi.Queue = newQueue;
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
                bool multiSelect = promptListView.CheckedItems.Count > 1 || promptListView.SelectedItems.Count > 1;

                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    loadPromptIntoGUIToolStripMenuItem.Visible = !multiSelect;
                    loadPromptAndSettingsIntoGUIToolStripMenuItem.Visible = !multiSelect;
                    copyPromptToolStripMenuItem.Visible = !multiSelect;

                    if (PromptListMode == ListMode.History)
                        menuStripPromptHistory.Show(Cursor.Position);
                }
            }
        }

        private void loadPromptIntoGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiSettings s = (TtiSettings)promptListView.FocusedItem.Tag;
            Program.MainForm.LoadTtiSettingsIntoUi(s.Prompts, s.NegativePrompt);
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

            MainUi.Queue.Enqueue(settings);
            LoadQueue();
        }

        private void PromptListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigParser.SaveGuiElement(checkboxEnableHistory, ref Config.Instance.EnablePromptHistory);
        }

        string _previousFilterText = "";

        private void textboxFilter_TextChanged(object sender, EventArgs e)
        {
            string t = textboxFilter.Text.Trim();

            if (t == _previousFilterText)
                return;

            _previousFilterText = t;

            if (PromptListMode == ListMode.History)
                LoadPromptHistory();
            else if (PromptListMode == ListMode.Queue)
                LoadQueue();
        }

        private void copyPromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiSettings s = (TtiSettings)promptListView.FocusedItem.Tag;
            OsUtils.SetClipboard(s.Prompts.FirstOrDefault());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelectedToolStripMenuItem_Click(null, null);
        }
    }
}
