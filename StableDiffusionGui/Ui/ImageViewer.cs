using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{

    internal class ImageViewer
    {
        public delegate void ImgChangedEvent();
        public static event ImgChangedEvent OnImageChanged;

        public enum ImgShowMode { DontShow, ShowFirst, ShowLast }

        public static string CurrentImagePath { get { try { return _currentImages.Length > 0 ? _currentImages[_currIndex] : ""; } catch { return ""; } } }
        public static ImageMetadata CurrentImageMetadata { get { return IoUtils.GetImageMetadata(CurrentImagePath); } }

        private static string[] _currentImages = new string[0];
        private static int _currIndex = -1;

        public static DateTime TimeOfLastImageViewerInteraction;
        public static DateTime TimeOfLastBlackImgWarn;

        private const string _strNoPrompt = "No prompt to display.";
        private const string _strNoPromptNeg = "No negative prompt to display.";

        public static void SetImages(List<string> imagePaths, ImgShowMode showMode, bool ignoreTimeout = false)
        {
            if (Enumerable.SequenceEqual(imagePaths, _currentImages))
                return;

            _currentImages = imagePaths.ToArray();

            if (showMode == ImgShowMode.DontShow)
                return;

            if (ignoreTimeout || (DateTime.Now - TimeOfLastImageViewerInteraction).TotalSeconds >= 3)
            {
                if (showMode == ImgShowMode.ShowFirst)
                    _currIndex = 0;

                if (showMode == ImgShowMode.ShowLast)
                    _currIndex = _currentImages.Length - 1;
            }

            Show();
        }

        public static void AppendImage(string imagePath, ImgShowMode showMode, bool ignoreTimeout = false)
        {
            List<string> newImgList = new List<string>(_currentImages) { imagePath };
            SetImages(newImgList, showMode, ignoreTimeout);
        }

        public static void Show()
        {
            if (_currIndex < 0 || _currIndex >= _currentImages.Length)
            {
                Clear();
                return;
            }

            Program.MainForm.pictBoxImgViewer.SetTextSafe("");
            Image img = IoUtils.GetImage(_currentImages[_currIndex]);
            Program.MainForm.pictBoxImgViewer.Image = img;
            OnImageChanged?.Invoke();

            if ((DateTime.Now - TimeOfLastBlackImgWarn).TotalSeconds >= 60 && ImgUtils.IsAllBlack((Bitmap)Program.MainForm.pictBoxImgViewer.GetImageSafe()))
            {
                Logger.Log($"Warning: Your image appears to be completely black. This could be an issue with the model, VAE, sampler, or precision settings.");
                TimeOfLastBlackImgWarn = DateTime.Now;
            }

            ImagePopup.UpdateSlideshow(Program.MainForm.pictBoxImgViewer.Image);
            ImageMetadata meta = CurrentImageMetadata;
            List<string> infos = new List<string>();

            if (meta.Seed >= 0)
                infos.Add($"Seed {meta.Seed}");

            if (meta.Steps >= 0)
                infos.Add($"Steps {meta.Steps}");

            if (meta.RefineStrength > 0)
                infos.Add($"Refine {meta.RefineStrength.ToStringDot("0.###")}");

            if (meta.Scale >= 0)
                infos.Add((meta.Guidance >= 0 ? $"CFG {meta.Scale.ToStringDot("0.###")}/{meta.Guidance.ToStringDot("0.###")}" : $"CFG {meta.Scale.ToStringDot("0.###")}"));

            if (meta.ScaleImg >= 0)
                infos.Add($"Image CFG {meta.ScaleImg.ToStringDot("0.###")}");

            Size res = Program.MainForm.pictBoxImgViewer.GetImageSafe().Size;

            if (meta.GeneratedResolution.IsEmpty)
                infos.Add(res.AsString());
            else
                infos.Add($"{meta.GeneratedResolution.AsString()}{(meta.GeneratedResolution == res ? "" : $" => {res.AsString()}")}");

            if (meta.InitStrength > 0.0001f && meta.InitStrength < 1.0f)
                infos.Add($"Strength {meta.InitStrength.ToStringDot("0.###")}");

            if (!string.IsNullOrWhiteSpace(meta.Sampler))
                infos.Add(Strings.Samplers.Get(meta.Sampler, true, true));

            string prompt = meta.Prompt;

            if (meta.Loras.Any())
                prompt += $" (With {string.Join(", ", meta.Loras.Select(l => $"{l.Key}{(l.Value == 1.0f ? "" : $" at {l.Value.ToStringDot("0.###")}")}"))})";

            Program.MainForm.labelImgInfo.SetTextSafe($"Image {_currIndex + 1}/{_currentImages.Length} {(infos.Count > 0 ? $" - {string.Join(" - ", infos)}" : "")}");
            Program.MainForm.labelImgPrompt.SetTextSafe(prompt.IsNotEmpty() ? prompt : _strNoPrompt);
            Program.MainForm.labelImgPromptNeg.SetTextSafe(meta.NegativePrompt.IsNotEmpty() ? meta.NegativePrompt : _strNoPromptNeg);
            Program.MainForm.toolTip.SetTooltipSafe(Program.MainForm.labelImgPrompt, $"{Program.MainForm.labelImgPrompt.Text}\n\nClick to copy.");
            Program.MainForm.toolTip.SetTooltipSafe(Program.MainForm.labelImgPromptNeg, $"{Program.MainForm.labelImgPromptNeg.Text}\n\nClick to copy.");
            UpdatePromptLabelColors();
        }

        public static void Clear()
        {
            Program.MainForm.pictBoxImgViewer.SetTextSafe("");
            Program.MainForm.pictBoxImgViewer.Image = null;
            OnImageChanged?.Invoke();
            Program.MainForm.labelImgInfo.SetTextSafe("No images to show.");
            Program.MainForm.labelImgPrompt.SetTextSafe(_strNoPrompt);
            Program.MainForm.labelImgPromptNeg.SetTextSafe(_strNoPromptNeg);
            UpdatePromptLabelColors();
        }

        public static void UpdatePromptLabelColors()
        {
            Program.MainForm.labelImgPrompt.ForeColor = Program.MainForm.labelImgPrompt.Text == _strNoPrompt ? Color.Silver : Color.PaleGreen;
            Program.MainForm.labelImgPromptNeg.ForeColor = Program.MainForm.labelImgPromptNeg.Text == _strNoPromptNeg ? Color.Silver : Color.LightCoral;
        }

        public static void Move(bool previous = false)
        {
            if (_currentImages.Length == 0)
                TimeOfLastImageViewerInteraction = new DateTime();
            else
                TimeOfLastImageViewerInteraction = DateTime.Now;

            if (!previous)
            {
                _currIndex += 1;

                if (_currIndex >= _currentImages.Length)
                    _currIndex = 0;
            }
            else
            {
                _currIndex -= 1;

                if (_currIndex < 0)
                    _currIndex = _currentImages.Length - 1;
            }

            Show();
        }

        public static void UpdateInitImgViewer()
        {
            Image initImg = IoUtils.GetImage(MainUi.CurrentInitImgPaths.FirstOrDefault(), false);
            Program.MainForm.pictBoxInitImg.Image = initImg;

            if (!Program.MainForm.checkboxShowInitImg.Visible)
                Program.MainForm.checkboxShowInitImg.Checked = false;
        }

        public static void CopyCurrentToFavs()
        {
            if (File.Exists(CurrentImagePath))
            {
                try
                {
                    string dir = Directory.CreateDirectory(Config.Instance.FavsPath).FullName;
                    string targetPath = Path.Combine(dir, Path.GetFileName(CurrentImagePath));

                    if (File.Exists(targetPath))
                    {
                        OsUtils.ShowNotification($"Image already exists in favorites folder.", false);
                    }
                    else
                    {
                        if (IoUtils.TryCopy(CurrentImagePath, targetPath, true, true))
                            OsUtils.ShowNotification($"Copied image to favorites.", false);
                        else
                            OsUtils.ShowNotification($"Failed to copy image!", false);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to copy image to favorites: {ex.Message}. Check logs for details.");
                    Logger.Log(ex.StackTrace, true);
                }
            }
        }

        public static void OpenCurrent()
        {
            Process.Start(CurrentImagePath);
        }

        public static void OpenFolderOfCurrent()
        {
            Process.Start("explorer", $@"/select, {CurrentImagePath.Replace("/", @"\").Wrap()}");
        }

        public static void DeleteCurrent()
        {
            Program.MainForm.panelSettings.Focus();
            IoUtils.TryDeleteIfExists(CurrentImagePath);
            ImageCache.Remove(CurrentImagePath);
            _currentImages = _currentImages.Where(x => File.Exists(x)).ToArray();
            Move(true);
        }

        public static void DeleteAll(bool askForConfirmation = true)
        {
            if (_currentImages == null || _currentImages.Length < 1)
                return;

            Program.MainForm.panelSettings.Focus();

            if (askForConfirmation)
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox($"Are you sure you want to delete {_currentImages.Length} generated images?", "Are you sure?", MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                    return;
            }

            var parentDirs = _currentImages.Select(x => x.GetParentDirOfFile());

            ImageCache.Remove(_currentImages.ToList());
            _currentImages.ToList().ForEach(x => IoUtils.TryDeleteIfExists(x));
            _currentImages = _currentImages.Where(x => File.Exists(x)).ToArray();

            if (_currentImages.Length > 0)
                Move(true);
            else
                Clear();

            parentDirs.Where(dir => Directory.Exists(dir) && !Directory.EnumerateFileSystemEntries(dir).Any()).ToList().ForEach(dir => IoUtils.TryDeleteIfExists(dir)); // Delete dir if it's now empty
        }

        public static Image GetCurrentImageComparison()
        {
            if (Program.MainForm.pictBoxImgViewer.Image == null)
                return null;

            Image scaledInitImg = IoUtils.GetImage(CurrentImageMetadata.InitImgName);

            if (scaledInitImg == null)
                return null;

            Image outImg = Program.MainForm.pictBoxImgViewer.GetImageSafe();

            Size targetSize = new Size(new[] { scaledInitImg.Width, outImg.Width }.Max(), new[] { scaledInitImg.Height, outImg.Height }.Max());
            Image img = ImgUtils.JuxtaposeSameSize(ImgUtils.ResizeImage(scaledInitImg, targetSize), ImgUtils.ResizeImage(outImg, targetSize));
            return img;
        }
    }
}
