using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Controls
{
    public class CustomPanel : Panel
    {
        public bool _allowScrolling = false;
        public bool _ctrlDisablesScrolling = false;
        public bool _onlyAllowScrollIfNeeded = false;

        [Category("Custom")]
        public bool AllowScrolling { get { return _allowScrolling; } set { _allowScrolling = value; } }
        [Category("Custom")]
        public bool CtrlDisablesScrolling { get { return _ctrlDisablesScrolling; } set { _ctrlDisablesScrolling = value; } }
        [Category("Custom")]
        public bool OnlyAllowScrollIfNeeded { get { return _onlyAllowScrollIfNeeded; } set { _onlyAllowScrollIfNeeded = value; } }

        protected override void WndProc(ref Message m)
        {
            if (m.ToString().Contains("SCROLL") || m.ToString().Contains("WHEEL"))
            {
                if (!AllowScrolling)
                    return;

                if (OnlyAllowScrollIfNeeded && PreferredSize.Height <= Size.Height)
                    return;

                if (CtrlDisablesScrolling && InputUtils.IsHoldingCtrl)
                    return;
            }

            base.WndProc(ref m);
        }

    }
}