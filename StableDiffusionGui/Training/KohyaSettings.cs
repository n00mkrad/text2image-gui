using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Training
{
    public class KohyaSettings
    {
        public enum NetworkType { LoCon, LoHa };
        public NetworkType LoraType = NetworkType.LoHa;
        public int Steps = 400;
        public float LearningRate;
        public int BatchSize = 4;
        public int Resolution = 512;
        public int NetworkDim;
        public int ConvDim;
        public float NetworkAlpha = 1.0f;
        public float ConvAlpha = 1.0f;
        public int Seed = 42;
        public bool UseAspectBuckets = true;
        public int ClipSkip;
        public int Dropout;

        public string Algo
        {
            get
            {
                if (LoraType == NetworkType.LoCon) return "locon";
                if (LoraType == NetworkType.LoHa) return "loha";
                return "";
            }
        }

        public KohyaSettings()
        {

        }

        public KohyaSettings(NetworkType preset)
        {
            if (preset == NetworkType.LoHa)
            {
                NetworkDim = 8;
                ConvDim = 4;
            }
            if (preset == NetworkType.LoCon)
            {
                throw new Exception($"LoCon LoRA preset not defined yet!");
            }
        }
    }
}
