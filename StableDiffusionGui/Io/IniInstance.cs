using System.Linq;

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
            public const string LoraMaxSteps = "LoraMaxSteps";
            public const string HiresFixMinimumDimensionMultiplier = "HiresFixMinimumDimensionMultiplier";
        }

        public int StepsMax = 100;
        public float ScaleMax = 25;
        public int ResolutionMin = 512;
        public int ResolutionMax = 2048;
        public int IterationsMax = 1000;
        public int LoraMaxSteps = 1000;
        public float HiresFixMinimumDimensionMultiplier = 1.0f;

        public IniInstance () { }

        public IniInstance (string text)
        {
            string[] lines = text.SplitIntoLines();

            foreach(string line in lines.Where(l => l.Contains("=")))
            {
                if (line.Trim().StartsWith("#") || line.Trim().StartsWith("//")) // Commented-out lines
                    continue;

                var split = line.Split('/').First().Split('=');
                string key = split[0].Trim();
                string value = split[1].Trim();

                if (key == Keys.StepsMax) StepsMax = value.GetInt();
                if (key == Keys.ScaleMax) ScaleMax = value.GetFloat();
                if (key == Keys.ResolutionMin) ResolutionMin = value.GetInt();
                if (key == Keys.ResolutionMax) ResolutionMax = value.GetInt();
                if (key == Keys.IterationsMax) IterationsMax = value.GetInt();
                if (key == Keys.LoraMaxSteps) LoraMaxSteps = value.GetInt();
                if (key == Keys.HiresFixMinimumDimensionMultiplier) HiresFixMinimumDimensionMultiplier = value.GetFloat();
            }
        }
    }
}
