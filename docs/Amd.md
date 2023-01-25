# AMD GPU Guide
SD GUI comes with an implementation that enables hardware-accelerated AI image generation on all GPUs that support DirectML.

**Note:** The AMD implementation uses an entirely different codebase from the default Pytorch implementation and thus lacks certain features, such as tileable image generation or custom samplers.

## Prerequisites

* At least one Pytorch (ckpt) model file (included with the regular 3 GB SD GUI download)
* A graphics card that supports DirectML (e.g. AMD 6XXX series)



## Guide

1. Open the Settings (F12) and set `Image Generation Implementation ` to `Stable Diffusion (ONNX - DirectML - For AMD GPUs)`.
2. *(Skip to #5 if you already have an ONNX model)* Click the wrench button in the main window and click `Convert Models.`
3. Select your model file to convert (e.g. `sd-v1-5-fp16.ckpt`).
4. Set the `Model Output Format` to `Diffusers ONNX (Folder)`, click `Convert`, and wait for it to finish.
5. Select the ONNX model either in the settings or using the quick-switcher (Ctrl+M).
6. Set your prompt and settings and click `Generate!` to generate images.

Alternatively, download and extract the already converted model from [here](https://icedrive.net/s/VbCSVjRPBPfybgwF5hwhjS1PZaWv).
