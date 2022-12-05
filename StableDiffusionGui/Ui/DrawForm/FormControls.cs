using ImageMagick;
using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Ui.DrawForm
{
    internal class FormControls
    {
        public static Forms.DrawForm F;

        public static void SetPictureBoxPadding()
        {
            Size frameSize = F.tableLayoutPanelImg.Size;
            Size imageSize = F.BackgroundImg.Size;

            Size targetImgBoxSize = ImgMaths.FitIntoFrame(imageSize, frameSize);

            int padTopBot = ((F.tableLayoutPanelImg.Size.Height - targetImgBoxSize.Height) / 2f).RoundToInt();
            int padSides = ((F.tableLayoutPanelImg.Size.Width - targetImgBoxSize.Width) / 2f).RoundToInt();

            F.tableLayoutPanelImg.ColumnStyles[0].Width = padSides;
            F.tableLayoutPanelImg.ColumnStyles[1].Width = targetImgBoxSize.Width;
            F.tableLayoutPanelImg.ColumnStyles[2].Width = padSides;

            F.tableLayoutPanelImg.RowStyles[0].Height = padTopBot;
            F.tableLayoutPanelImg.RowStyles[1].Height = targetImgBoxSize.Height;
            F.tableLayoutPanelImg.RowStyles[2].Height = padTopBot;
        }

        public static void ShowContextMenu ()
        {
            F.undoToolStripMenuItem.Visible = F.History != null && F.History.Count > 1;
            F.menuStripOptions.Show(Cursor.Position);
        }
    }
}
