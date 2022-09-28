using StableDiffusionGui.Forms;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Forms.ImagePopupForm;

namespace StableDiffusionGui.Ui
{
    internal class ImagePopup
    {
        public static bool IsOpen { get { return Application.OpenForms.OfType<ImagePopupForm>().Any(); } }
        public static ImagePopupForm Form;

        public static void Show (Image img, SizeMode initSizeMode = SizeMode.Percent100)
        {
            if(!IsOpen)
            {
                Form = new ImagePopupForm(img, initSizeMode);
                Form.Show();
            }
            else
            {
                if(Form == null)
                    return;

                Form.CurrentImage = img;
                Form.SetSize(initSizeMode);
                Form.BringToFront();
            }
        }

        public static void UpdateImageIfOpen (Image img)
        {
            if(Form != null)
                Form.CurrentImage = img;
        }
    }
}
