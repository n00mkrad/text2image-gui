using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Forms.ModelFoldersForm;

namespace StableDiffusionGui.Forms
{
    public partial class ModelFoldersForm : Form
    {
        public enum Folder { Models, Vaes }
        public List<string> Folders = new List<string>();
        private readonly Folder _folder;

        private string DefaultPath { get { return _folder == Folder.Models ? Paths.GetModelsPath() : Paths.GetVaesPath(); } }

        public ModelFoldersForm(Folder folderType)
        {
            InitializeComponent();
            _folder = folderType;
        }

        private void ModelFoldersForm_Load(object sender, EventArgs e)
        {

        }

        private void ModelFoldersForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            LoadDirs();
            FillList();
        }

        private void ModelFoldersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDirs();
        }

        private void LoadDirs()
        {
            Folders = new List<string>() { DefaultPath };

            if (_folder == Folder.Models)
            {
                Folders = Folders.Concat(Config.Instance.CustomModelDirs).ToList();
            }
            else if (_folder == Folder.Vaes)
            {
                Folders = Folders.Concat(Config.Instance.CustomVaeDirs).ToList();
            }
        }

        private void SaveDirs()
        {
            List<string> folders = Folders.Where(dir => dir != DefaultPath && Directory.Exists(dir)).ToList();
            
            if (_folder == Folder.Models)
                Config.Instance.CustomModelDirs = folders;
            else if (_folder == Folder.Vaes)
                Config.Instance.CustomVaeDirs = folders;
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
            Folders = Folders.Where(x => x == DefaultPath || !dirsToRemove.Contains(x)).ToList();
            FillList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = DefaultPath, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (Directory.Exists(dialog.FileName) && !Folders.Contains(dialog.FileName))
                {
                    Folders.Add(dialog.FileName);
                    FillList();
                }
            }
        }
    }
}
