using System;
using System.Collections.Generic;
using System.Linq;

namespace StableDiffusionGui.Training
{
    public class KohyaSettings
    {
        public string BaseModelPath = "";
        public string DatasetConfigPath = "";
        public string OutDir = "";
        public string OutFilename = "";

        public enum NetworkModule { LoRa, LyCoris };
        public NetworkModule NetModule = NetworkModule.LyCoris;
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
        public bool CacheLatents = true;
        public bool GradientCheckpointing = true;
        public string TrainMixedPrec = "fp16";
        public string SaveMixedPrec = "fp16";
        public bool AugmentFlip = false;
        public bool AgumentColor = false;
        public bool ShuffleCaption = false;

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
            if (preset == NetworkType.LoCon)
            {
                NetModule = NetworkModule.LoRa;
                LoraType = preset;
            }
            if (preset == NetworkType.LoHa)
            {
                NetModule = NetworkModule.LyCoris;
                LoraType = preset;
            }
        }

        public string GetCliArgs()
        {
            var argList = new List<string>();
            argList.Add($"pretrained_model_name_or_path={BaseModelPath.Wrap()}");
            argList.Add($"dataset_config={DatasetConfigPath.Wrap()}");
            argList.Add($"output_dir={OutDir.Wrap()}");
            argList.Add($"output_name={OutFilename}");
            argList.Add($"save_model_as=safetensors");
            argList.Add($"prior_loss_weight=1.0");
            argList.Add($"max_train_steps={Steps}");
            argList.Add($"learning_rate={LearningRate.ToStringDot("0.##########")}");
            argList.Add($"mixed_precision={TrainMixedPrec}");
            argList.Add($"save_precision={TrainMixedPrec}");
            argList.Add($"save_every_n_epochs=100");

            if (CacheLatents)
                argList.Add($"cache_latents");

            if (GradientCheckpointing)
                argList.Add($"gradient_checkpointing");

            if (AugmentFlip)
                argList.Add($"flip_aug");

            if (AgumentColor)
                argList.Add($"color_aug");

            if (ShuffleCaption)
                argList.Add($"shuffle_caption");

            if (NetModule == NetworkModule.LyCoris)
                argList.Add($"network_module=lycoris.kohya");
            else
                argList.Add($"network_module=networks.lora");

            argList.Add($"network_dim={NetworkDim}");
            argList.Add($"network_alpha={NetworkAlpha}");
            argList.Add($"seed={Seed}");
            argList.Add($"clip_skip={ClipSkip}");

            if (NetModule == NetworkModule.LyCoris)
                argList.Add($"network_args \"conv_dim={ConvDim}\" \"conv_alpha={ConvAlpha}\" \"dropout={Dropout}\" \"algo={Algo}\"");

            return string.Join(" ", argList.Select(a => $"--{a}"));
        }
    }
}
