using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ModelFoldersForm : Form
    {
        public List<string> Folders = new List<string>();

        public ModelFoldersForm()
        {
            InitializeComponent();
        }

        private void ModelFoldersForm_Load(object sender, EventArgs e)
        {

        }

        private void ModelFoldersForm_Shown(object sender, EventArgs e)
        {
            LoadDirs();
            FillList();
        }

        private void ModelFoldersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDirs();
        }

        private void LoadDirs()
        {
            Folders.Clear();
            Folders.Add(Paths.GetModelsPath());

            string serialized = Config.Get(Config.Key.customModelDirs);
            var deserialized = JsonConvert.DeserializeObject<List<string>>(serialized);

            if (deserialized != null)
                Folders.AddRange(deserialized);
        }

        private void SaveDirs()
        {
            string serialized = JsonConvert.SerializeObject(Folders.Where(x => x != Paths.GetModelsPath()));
            Config.Set(Config.Key.customModelDirs, serialized);
        }

        private void FillList()
        {
            folderListView.Items.Clear();

            foreach (string dir in Folders)
            {
                folderListView.Items.Add(GetCleanPath(dir));
                folderListView.Items.Cast<ListViewItem>().Last().Tag = dir;
            }
        }

        private string GetCleanPath(string path)
        {
            path = path.Remove(Paths.GetExeDir());
            path = path.Replace(@"\", "/");
            return path;
        }

        private List<ListViewItem> GetSelectedItems()
        {
            List<ListViewItem> items = new List<ListViewItem>();
            items.AddRange(folderListView.SelectedItems.Cast<ListViewItem>().ToList());
            items.AddRange(folderListView.CheckedItems.Cast<ListViewItem>().ToList());
            return items.Distinct().ToList();
        }

        private void btnOpenSelectedFolder_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedItems();

            if (selected.Count > 0)
                Process.Start((string)selected[0].Tag);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var dirsToRemove = GetSelectedItems().Select(x => (string)x.Tag);
            Folders = Folders.Where(x => x == Paths.GetModelsPath() || !dirsToRemove.Contains(x)).ToList();
            FillList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = Paths.GetModelsPath(), IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if(Directory.Exists(dialog.FileName) && !Folders.Contains(dialog.FileName))
                {
                    Folders.Add(dialog.FileName);
                    FillList();
                }
            }
        }
    }
}
