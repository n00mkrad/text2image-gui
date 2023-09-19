using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static StableDiffusionGui.Implementations.ComfyNodes;
using static StableDiffusionGui.Implementations.ComfyWorkflow;

namespace StableDiffusionGui.Implementations
{
    internal class ComfyPrompts
    {
        public static EasyDict<string, NodeInfo> GetMainWorkflow(ComfyData.GenerationInfo g)
        {
            var nodes = new List<INode>();

            var nodeInfos = new EasyDict<string, NodeInfo>();
            bool refine = g.RefinerStrength > 0.001f;
            bool upscale = !g.TargetResolution.IsEmpty && g.TargetResolution != g.BaseResolution;
            bool inpaint = g.MaskPath.IsNotEmpty();
            bool controlnet = g.Controlnets.Any(c => c.Model.IsNotEmpty() && c.Strength > 0.001f);
            bool img2img = g.InitImg.IsNotEmpty() && !controlnet;
            int baseSteps = (g.Steps * (1f - g.RefinerStrength)).RoundToInt();
            int baseWidth = g.BaseResolution.Width;
            int baseHeight = g.BaseResolution.Height;

            var model1 = AddNode<NmkdCheckpointLoader>(nodes); // Base Model Loader
            model1.ModelPath = g.Model;
            model1.ClipSkip = g.ClipSkip;
            model1.LoadVae = true;
            model1.VaePath = g.Vae ?? "";
            model1.EmbeddingsDir = Paths.ReturnDir(Config.Instance.EmbeddingsDir, true, true);

            INode finalModelNode = model1; // Keep track of the final model node to use

            if (g.Loras.Any(l => l.Value > 0.001f)) // Add LoRAs
            {
                var loraLoader = AddNode<NmkdMultiLoraLoader>(nodes); // LoRA Loader
                loraLoader.Loras = g.Loras;
                loraLoader.Model = new ComfyInput(finalModelNode, OutType.Model);
                loraLoader.Clip = new ComfyInput(model1, OutType.Clip);
                finalModelNode = loraLoader;
            }

            foreach (var hypernetModelStrength in g.Hypernetworks) // Add hypernetworks
            {
                var hypernetLoader = AddNode<NmkdHypernetworkLoader>(nodes); // Hypernetwork Loader
                hypernetLoader.Model = new ComfyInput(finalModelNode, OutType.Model);
                hypernetLoader.ModelPath = hypernetModelStrength.Key;
                hypernetLoader.Strength = hypernetModelStrength.Value;
                finalModelNode = hypernetLoader;
            }

            NmkdCheckpointLoader model2 = null;

            if (g.ModelRefiner.IsNotEmpty())
            {
                model2 = AddNode<NmkdCheckpointLoader>(nodes); // Refiner Model Loader
                model2.ModelPath = g.ModelRefiner == null ? g.Model : g.ModelRefiner;
                model2.ClipSkip = g.ClipSkip;
                model2.LoadVae = false;
                model2.EmbeddingsDir = Io.Paths.ReturnDir(Config.Instance.EmbeddingsDir, true, true);
            }

            var encodePromptBase = AddNode<NmkdDualTextEncode>(nodes); // CLIP Encode Prompt Base
            encodePromptBase.Text1 = g.Prompt;
            encodePromptBase.Text2 = g.NegativePrompt;
            encodePromptBase.Clip = new ComfyInput(finalModelNode, OutType.Clip);

            NmkdDualTextEncode encodePromptRefiner = null;

            if (refine)
            {
                encodePromptRefiner = AddNode<NmkdDualTextEncode>(nodes); // CLIP Encode Prompt Refiner
                encodePromptRefiner.Text1 = g.Prompt;
                encodePromptRefiner.Text2 = g.NegativePrompt;
                encodePromptRefiner.Clip = new ComfyInput(model2, OutType.Clip);
            }

            INode initImageNode = null; // Keep track of the init image to use
            INode finalLatentsNode = null;

            if (g.InitImg.IsNotEmpty())
            {
                var loadInitImg = AddNode<NmkdImageLoader>(nodes); // Load Init Image
                loadInitImg.ImagePath = g.MaskPath.IsNotEmpty() ? g.MaskPath : g.InitImg;
                initImageNode = loadInitImg;
            }

            if (!img2img)
            {
                var emptyLatentImg = AddNode<EmptyLatentImage>(nodes); // Empty Latent Image (T2I)
                emptyLatentImg.Width = baseWidth;
                emptyLatentImg.Height = baseHeight;
                finalLatentsNode = emptyLatentImg;
            }
            else
            {
                var encodeInitImg = AddNode<NmkdVaeEncode>(nodes); // Encode Init Image (I2I)
                encodeInitImg.Image = new ComfyInput(initImageNode, OutType.Image);
                encodeInitImg.Mask = new ComfyInput(initImageNode, OutType.Mask);
                encodeInitImg.Vae = new ComfyInput(model1, OutType.Vae);
                encodeInitImg.UseMask = g.MaskPath.IsNotEmpty();
                finalLatentsNode = encodeInitImg;
            }

            INode finalConditioningNode = encodePromptBase; // Keep track of the final conditioning node for sampling

            for (int i = 0; i < g.Controlnets.Count; i++) // Apply ControlNets
            {
                ComfyData.ControlnetInfo ci = g.Controlnets[i];

                if (ci.Preprocessor != Enums.StableDiffusion.ImagePreprocessor.None)
                {
                    nodes.Add(new GenericImagePreprocessor
                    {
                        Image = new ComfyInput(initImageNode, OutType.Image),
                        Preprocessor = ci.Preprocessor,
                        Id = GetNewId(nodes, "Preproc")
                    });
                }

                nodes.Add(new NmkdControlNet
                {
                    ModelPath = ci.Model,
                    Strength = ci.Strength,
                    Conditioning = new ComfyInput(finalConditioningNode, OutType.Conditioning),
                    Image = new ComfyInput(ci.Preprocessor != Enums.StableDiffusion.ImagePreprocessor.None ? nodes.Last() : initImageNode, OutType.Image),
                    Model = new ComfyInput(model1, OutType.Model),
                    Id = GetNewId(nodes, "ControlNet"),
                });

                finalConditioningNode = nodes.Last();
            }

            INode samplerNode = null;

            if (refine)
            {
                var sampler = AddNode<NmkdHybridSampler>(nodes, "SamplerHybrid"); // Sampler Hybrid
                sampler.AddNoise = true;
                sampler.Seed = g.Seed;
                sampler.Steps = g.Steps;
                sampler.BaseSteps = baseSteps;
                sampler.Cfg = g.Scale;
                sampler.SamplerName = GetComfySampler(g.Sampler);
                sampler.Scheduler = GetComfyScheduler(g);
                sampler.Model = new ComfyInput(finalModelNode, OutType.Model);
                sampler.ModelRefiner = new ComfyInput(model2, OutType.Model);
                sampler.PositiveCond = new ComfyInput(finalConditioningNode, 0);
                sampler.NegativeCond = new ComfyInput(encodePromptBase, 1);
                sampler.RefinerPositiveCond = new ComfyInput(encodePromptRefiner, 0);
                sampler.RefinerNegativeCond = new ComfyInput(encodePromptRefiner, 1);
                sampler.LatentImage = new ComfyInput(finalLatentsNode, OutType.Latents);
                sampler.Denoise = img2img ? g.InitStrength : 1.0f;
                finalLatentsNode = sampler;
                samplerNode = sampler;
            }
            else
            {
                var sampler = AddNode<NmkdKSampler>(nodes, "SamplerBase"); // Sampler Base
                sampler.AddNoise = true;
                sampler.Model = new ComfyInput(finalModelNode, OutType.Model);
                sampler.PositiveCond = new ComfyInput(finalConditioningNode, OutType.Conditioning);
                sampler.NegativeCond = new ComfyInput(encodePromptBase, 1);
                sampler.LatentImage = new ComfyInput(finalLatentsNode, OutType.Latents);
                sampler.SamplerName = GetComfySampler(g.Sampler);
                sampler.Scheduler = GetComfyScheduler(g);
                sampler.Seed = new ComfyInput(g.Seed);
                sampler.StepsTotal = new ComfyInput(g.Steps);
                sampler.Cfg = new ComfyInput(g.Scale);
                sampler.StartStep = new ComfyInput(0);
                sampler.EndStep = new ComfyInput(baseSteps);
                sampler.ReturnLeftoverNoise = refine;
                sampler.Denoise = img2img ? g.InitStrength : 1.0f;
                finalLatentsNode = sampler;
                samplerNode = sampler;
            }

            if (g.UpscaleMethod == Enums.StableDiffusion.UpscaleMethod.Latent) // Latent upscaling
            {
                var latentUpscale = AddNode<LatentUpscale>(nodes); // Latent Upscale
                latentUpscale.Width = g.TargetResolution.Width;
                latentUpscale.Height = g.TargetResolution.Height;
                latentUpscale.Latents = new ComfyInput(samplerNode, OutType.Latents);

                var samplerHires = AddNode<NmkdKSampler>(nodes, "SamplerHires"); // Sampler Hi-Res
                samplerHires.AddNoise = true;
                samplerHires.Model = new ComfyInput(finalModelNode, OutType.Model);
                samplerHires.PositiveCond = new ComfyInput(finalConditioningNode, OutType.Conditioning);
                samplerHires.NegativeCond = new ComfyInput(encodePromptBase, 1);
                samplerHires.LatentImage = new ComfyInput(latentUpscale, OutType.Latents);
                samplerHires.SamplerName = GetComfySampler(g.Sampler);
                samplerHires.Scheduler = "simple"; // GetComfyScheduler(g);
                samplerHires.Seed = new ComfyInput(g.Seed);
                samplerHires.StepsTotal = new ComfyInput((g.Steps * 0.5f).RoundToInt());
                samplerHires.Cfg = new ComfyInput(g.Scale);
                finalLatentsNode = samplerHires;
            }

            var vaeDecode = AddNode<VAEDecode>(nodes); // Final VAE Decode
            vaeDecode.Vae = new ComfyInput(model1, OutType.Vae);
            vaeDecode.Latents = new ComfyInput(finalLatentsNode, OutType.Latents);

            INode finalImageNode = vaeDecode; // Keep track of the final image output node to use

            if (g.UpscaleMethod == Enums.StableDiffusion.UpscaleMethod.UltimateSd) // Ultimate SD Upscaler
            {
                var upscalerModelNode = finalModelNode;
                var conditioningNode = encodePromptBase;
                bool xl = Config.Instance.ModelSettings.Get(Path.GetFileName(g.Model), new Models.ModelSettings()).Arch == Enums.StableDiffusion.ModelArch.SdXlBase;

                if (xl) // Load SD1 model as we can't use XL for USDU yet
                {
                    upscalerModelNode = AddNode<NmkdCheckpointLoader>(nodes, "UpscalingModelSD"); // Base Model Loader
                    ((NmkdCheckpointLoader)upscalerModelNode).ModelPath = g.UltimateSdUpConfig.ModelPathSd;
                    ((NmkdCheckpointLoader)upscalerModelNode).ClipSkip = Config.Instance.ModelSettings.Get(Path.GetFileName(((NmkdCheckpointLoader)upscalerModelNode).ModelPath), new Models.ModelSettings()).ClipSkip;
                    ((NmkdCheckpointLoader)upscalerModelNode).LoadVae = true;
                    ((NmkdCheckpointLoader)upscalerModelNode).EmbeddingsDir = Io.Paths.ReturnDir(Config.Instance.EmbeddingsDir, true, true);

                    conditioningNode = AddNode<NmkdDualTextEncode>(nodes); // CLIP Encode Prompt Base
                    conditioningNode.Text1 = g.Prompt;
                    conditioningNode.Text2 = g.NegativePrompt;
                    conditioningNode.Clip = new ComfyInput(upscalerModelNode, OutType.Clip);
                }

                var upscaleMdl = AddNode<NmkdUpscaleModelLoader>(nodes); // Latent Upscale
                upscaleMdl.UpscaleModelPath = g.UltimateSdUpConfig.ModelPathEsrgan;

                NmkdControlNet tileControlnet = null;

                if (g.UltimateSdUpConfig.UseTileControlnet)
                {
                    tileControlnet = AddNode<NmkdControlNet>(nodes, "TileControlnetUpscale"); // Tile ControlNet
                    tileControlnet.ModelPath = g.UltimateSdUpConfig.ModelPathTileControlnet;
                    tileControlnet.Conditioning = new ComfyInput(conditioningNode, OutType.Conditioning);
                    tileControlnet.Image = new ComfyInput(finalImageNode, OutType.Image);
                    tileControlnet.Model = new ComfyInput(upscalerModelNode, OutType.Model);
                }

                var upscaler = AddNode<UltimateSDUpscale>(nodes); // Ultimate SD Upscale
                upscaler.Image = new ComfyInput(finalImageNode, OutType.Image);
                upscaler.Model = new ComfyInput(upscalerModelNode, OutType.Model);
                upscaler.CondPositive = new ComfyInput(tileControlnet == null ? (INode)conditioningNode : (INode)tileControlnet, OutType.Conditioning);
                upscaler.CondNegative = new ComfyInput(conditioningNode, 1);
                upscaler.Vae = new ComfyInput(xl ? upscalerModelNode : model1, OutType.Vae);
                upscaler.UpscaleModel = new ComfyInput(upscaleMdl, 0);
                upscaler.UpscaleFactor = (float)g.TargetResolution.Width / g.BaseResolution.Width;
                upscaler.TileSize = g.BaseResolution;
                upscaler.Seed = g.Seed;
                upscaler.Steps = (g.Steps * 0.3333f).RoundToInt().Clamp(8, 30);
                upscaler.Scale = g.Scale;
                upscaler.Sampler = GetComfySampler(g.Sampler);
                upscaler.Scheduler = GetComfyScheduler(g);

                finalImageNode = upscaler;
            }

            if (inpaint)
            {
                INode initImgNode = nodes.Where(n => n is NmkdImageLoader).First();
                var compositeImgs = AddNode<NmkdImageMaskComposite>(nodes); // Composite with source image
                compositeImgs.ImageTo = new ComfyInput(vaeDecode, OutType.Image);
                compositeImgs.ImageFrom = new ComfyInput(initImageNode, OutType.Image);
                compositeImgs.Mask = new ComfyInput(initImgNode, OutType.Mask);
                finalImageNode = compositeImgs;
            }

            if (g.Upscaler.IsNotEmpty() && g.SaveOriginalAndUpscale)
            {
                var saveImageUpscaled = AddNode<NmkdSaveImage>(nodes);
                saveImageUpscaled.Prefix = $"nmkd_src";
                saveImageUpscaled.Image = new ComfyInput(finalImageNode, OutType.Image);
            }

            if (g.Upscaler.IsNotEmpty())
            {
                var upscaler = AddNode<NmkdImageUpscale>(nodes); // Upscale Image
                upscaler.UpscaleModelPath = g.Upscaler;
                upscaler.Image = new ComfyInput(finalImageNode, OutType.Image);
                finalImageNode = upscaler;
            }

            var saveImage = AddNode<NmkdSaveImage>(nodes);
            saveImage.Prefix = $"nmkd";
            saveImage.Image = new ComfyInput(finalImageNode, OutType.Image);

            foreach (INode node in nodes.Where(n => n != null))
                nodeInfos.Add(node.Id, node.GetNodeInfo());

            return nodeInfos;
        }

        public static EasyDict<string, NodeInfo> GetUpscaleWorkflow(ComfyData.GenerationInfo g, string savePath = "")
        {
            var nodes = new List<INode>();
            var nodeInfos = new EasyDict<string, NodeInfo>();

            if (savePath.IsEmpty())
            {
                savePath = IoUtils.FilenameSuffix(g.InitImg, ".upscale");
                savePath = IoUtils.GetAvailablePath(savePath, "{0}");
            }

            var loadInitImg = AddNode<NmkdImageLoader>(nodes, FormatUtils.GetUnixTimestamp()); // Load Init Image
            loadInitImg.ImagePath = g.InitImg;
            INode finalImageNode = loadInitImg; // Keep track of the final image output node to use

            // var encodeInitImg = AddNode<NmkdVaeEncode>(nodes); // Encode Init Image (I2I)
            // encodeInitImg.Image = new ComfyInput(loadInitImg, OutType.Image);
            // encodeInitImg.Mask = new ComfyInput(loadInitImg, OutType.Mask);
            // encodeInitImg.Vae = new ComfyInput(model1, OutType.Vae);
            // encodeInitImg.UseMask = g.MaskPath.IsNotEmpty();

            // if (g.UpscaleMethod == Enums.StableDiffusion.UpscaleMethod.Latent) // Latent upscaling
            // {
            //     var latentUpscale = AddNode<LatentUpscale>(nodes); // Latent Upscale
            //     latentUpscale.Width = g.TargetResolution.Width;
            //     latentUpscale.Height = g.TargetResolution.Height;
            //     latentUpscale.Latents = new ComfyInput(samplerNode, OutType.Latents);
            // 
            //     var samplerHires = AddNode<NmkdKSampler>(nodes, "SamplerHires"); // Sampler Hi-Res
            //     samplerHires.AddNoise = true;
            //     samplerHires.Model = new ComfyInput(finalModelNode, OutType.Model);
            //     samplerHires.PositiveCond = new ComfyInput(finalConditioningNode, OutType.Conditioning);
            //     samplerHires.NegativeCond = new ComfyInput(encodePromptBase, 1);
            //     samplerHires.LatentImage = new ComfyInput(latentUpscale, OutType.Latents);
            //     samplerHires.SamplerName = GetComfySampler(g.Sampler);
            //     samplerHires.Scheduler = "simple"; // GetComfyScheduler(g);
            //     samplerHires.Seed = new ComfyInput(g.Seed);
            //     samplerHires.StepsTotal = new ComfyInput((g.Steps * 0.5f).RoundToInt());
            //     samplerHires.Cfg = new ComfyInput(g.Scale);
            //     finalLatentsNode = samplerHires;
            // }

            // var vaeDecode = AddNode<VAEDecode>(nodes); // Final VAE Decode
            // vaeDecode.Vae = new ComfyInput(model1, OutType.Vae);
            // vaeDecode.Latents = new ComfyInput(finalLatentsNode, OutType.Latents);
            // finalImageNode = vaeDecode;

            // if (g.UpscaleMethod == Enums.StableDiffusion.UpscaleMethod.UltimateSd) // Ultimate SD Upscaler
            // {
            //     var upscalerModelNode = finalModelNode;
            //     var conditioningNode = encodePromptBase;
            //     bool xl = Config.Instance.ModelSettings.Get(Path.GetFileName(g.Model), new Models.ModelSettings()).Arch == Enums.StableDiffusion.ModelArch.SdXlBase;
            // 
            //     if (xl) // Load SD1 model as we can't use XL for USDU yet
            //     {
            //         upscalerModelNode = AddNode<NmkdCheckpointLoader>(nodes, "UpscalingModelSD"); // Base Model Loader
            //         ((NmkdCheckpointLoader)upscalerModelNode).ModelPath = "M:\\Weights\\SD\\SafetensorsNew\\dreamshaper_8.safetensors"; // TODO: Don't hardcode
            //         ((NmkdCheckpointLoader)upscalerModelNode).ClipSkip = Config.Instance.ModelSettings.Get(Path.GetFileName(((NmkdCheckpointLoader)upscalerModelNode).ModelPath), new Models.ModelSettings()).ClipSkip;
            //         ((NmkdCheckpointLoader)upscalerModelNode).LoadVae = true;
            //         ((NmkdCheckpointLoader)upscalerModelNode).EmbeddingsDir = Io.Paths.ReturnDir(Config.Instance.EmbeddingsDir, true, true);
            // 
            //         conditioningNode = AddNode<NmkdDualTextEncode>(nodes); // CLIP Encode Prompt Base
            //         conditioningNode.Text1 = g.Prompt;
            //         conditioningNode.Text2 = g.NegativePrompt;
            //         conditioningNode.Clip = new ComfyInput(upscalerModelNode, OutType.Clip);
            //     }
            // 
            //     var upscaleMdl = AddNode<NmkdUpscaleModelLoader>(nodes); // Latent Upscale
            //     upscaleMdl.UpscaleModelPath = "D:\\AI\\ComfyUI\\ComfyUI\\models\\upscale_models\\realesr-general-x4v3.pth"; // TODO: Don't hardcode
            // 
            //     var tileControlnet = AddNode<NmkdControlNet>(nodes, "TileControlnetUpscale"); // Tile ControlNet
            //     tileControlnet.ModelPath = "M:\\Weights\\SD\\ControlNet\\control_v11f1e_sd15_tile_fp16.safetensors"; // TODO: Don't hardcode
            //     tileControlnet.Conditioning = new ComfyInput(conditioningNode, OutType.Conditioning);
            //     tileControlnet.Image = new ComfyInput(finalImageNode, OutType.Image);
            //     tileControlnet.Model = new ComfyInput(upscalerModelNode, OutType.Model);
            // 
            //     var upscaler = AddNode<UltimateSDUpscale>(nodes); // Ultimate SD Upscale
            //     upscaler.Image = new ComfyInput(finalImageNode, OutType.Image);
            //     upscaler.Model = new ComfyInput(upscalerModelNode, OutType.Model);
            //     upscaler.CondPositive = new ComfyInput(tileControlnet, OutType.Conditioning);
            //     upscaler.CondNegative = new ComfyInput(conditioningNode, 1);
            //     upscaler.Vae = new ComfyInput(xl ? upscalerModelNode : model1, OutType.Vae);
            //     upscaler.UpscaleModel = new ComfyInput(upscaleMdl, 0);
            //     upscaler.UpscaleFactor = (float)g.TargetResolution.Width / g.BaseResolution.Width;
            //     upscaler.Seed = g.Seed;
            //     upscaler.Steps = (g.Steps * 0.5f).RoundToInt();
            //     upscaler.Scale = g.Scale;
            //     upscaler.Sampler = GetComfySampler(g.Sampler);
            //     upscaler.Scheduler = GetComfyScheduler(g);
            // 
            //     finalImageNode = upscaler;
            // }

            if (g.Upscaler.IsNotEmpty())
            {
                if (g.SaveOriginalAndUpscale)
                {
                    var saveImageUpscaled = AddNode<NmkdSaveImage>(nodes);
                    saveImageUpscaled.Prefix = "output";
                    saveImageUpscaled.Image = new ComfyInput(finalImageNode, OutType.Image);
                }

                var upscaler = AddNode<NmkdImageUpscale>(nodes); // Upscale Image
                upscaler.UpscaleModelPath = g.Upscaler;
                upscaler.Image = new ComfyInput(finalImageNode, OutType.Image);
                finalImageNode = upscaler;
            }

            var saveImage = AddNode<NmkdSaveImage>(nodes);
            saveImage.Prefix = "upscale";
            saveImage.OverridePath = savePath;
            saveImage.Image = new ComfyInput(finalImageNode, OutType.Image);

            foreach (INode node in nodes.Where(n => n != null))
                nodeInfos.Add(node.Id, node.GetNodeInfo());

            return nodeInfos;
        }

    }
}
