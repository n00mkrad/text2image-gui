using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Controls
{
    public class CustomUpDown : NumericUpDown
    {
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;

            if (hme != null)
                hme.Handled = true;

            if (e.Delta > 0 && (this.Value + this.Increment) <= this.Maximum)
                this.Value += this.Increment;
            else if (e.Delta < 0 && (this.Value - this.Increment) >= this.Minimum)
                this.Value -= this.Increment;
        }
    }
}