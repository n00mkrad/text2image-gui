using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.MiscUtils
{
    internal class Parser
    {
        public static Size GetSize (string s, string delimiter = "x")
        {
            try
            {
                string[] values = s.Split(delimiter);
                return new Size(values[0].GetInt(), values[1].GetInt());
            }
            catch
            {
                return new Size();
            }
        }
    }
}
