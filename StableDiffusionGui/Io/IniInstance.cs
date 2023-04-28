using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Io
{
    public class IniInstance
    {
        public class Keys
        {
            public const string StepsMax = "StepsMax";
            public const string ScaleMax = "ScaleMax";
            public const string ResolutionMin = "ResolutionMin";
            public const string ResolutionMax = "ResolutionMax";
            public const string IterationsMax = "IterationsMax";
        }

        public int StepsMax = 100;
        public float ScaleMax = 25;
        public int ResolutionMin = 512;
        public int ResolutionMax = 2048;
        public int IterationsMax = 1000;

        public IniInstance () { }

        public IniInstance (string text)
        {
            string[] lines = text.SplitIntoLines();

            foreach(string line in lines.Where(l => l.Contains("=")))
            {
                var split = line.Split('/').First().Split('=');
                string key = split[0].Trim();
                string value = split[1].Trim();

                if (key == Keys.StepsMax) StepsMax = value.GetInt();
                if (key == Keys.ScaleMax) ScaleMax = value.GetFloat();
                if (key == Keys.ResolutionMin) ResolutionMin = value.GetInt();
                if (key == Keys.ResolutionMax) ResolutionMax = value.GetInt();
                if (key == Keys.IterationsMax) IterationsMax = value.GetInt();
            }
        }
    }
}
