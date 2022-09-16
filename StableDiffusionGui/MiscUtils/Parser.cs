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
                var split = s.Split(delimiter);
                int w = split[0].GetInt();
                int h = split[1].GetInt();
                return new Size(w, h);
            }
            catch
            {
                return new Size();
            }
        }
    }
}
