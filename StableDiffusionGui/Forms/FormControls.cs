using StableDiffusionGui.MiscUtils;
using System.Drawing;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class DrawForm
    {
        public void SetPictureBoxPadding()
        {
            Size frameSize = tableLayoutPanelImg.Size;
            Size imageSize = BackgroundImg.Size;

            Size targetImgBoxSize = ImgMaths.FitIntoFrame(imageSize, frameSize);

            int padTopBot = ((tableLayoutPanelImg.Size.Height - targetImgBoxSize.Height) / 2f).RoundToInt();
            int padSides = ((tableLayoutPanelImg.Size.Width - targetImgBoxSize.Width) / 2f).RoundToInt();

            tableLayoutPanelImg.ColumnStyles[0].Width = padSides;
            tableLayoutPanelImg.ColumnStyles[1].Width = targetImgBoxSize.Width;
            tableLayoutPanelImg.ColumnStyles[2].Width = padSides;

            tableLayoutPanelImg.RowStyles[0].Height = padTopBot;
            tableLayoutPanelImg.RowStyles[1].Height = targetImgBoxSize.Height;
            tableLayoutPanelImg.RowStyles[2].Height = padTopBot;
        }

        public void ShowContextMenu ()
        {
            undoToolStripMenuItem.Visible = History != null && History.Count > 1;
            menuStripOptions.Show(Cursor.Position);
        }
    }
}
