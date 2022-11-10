using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Controls
{
    public class CustomComboBox : ComboBox
    {
        private int _previousIndex = -1;
        private int _index = -1;
        public int PreviousIndex { get { return _previousIndex; } }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            _previousIndex = _index;
            _index = SelectedIndex;
            base.OnSelectedIndexChanged(e);
        }
    }
}
