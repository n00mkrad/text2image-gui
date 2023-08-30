# SD XL Guide (1.12)
SD GUI 1.12 comes with an experimental Diffusers-based implementation of Stable Diffusion XL.

**Note:** The AMD implementation uses an entirely different codebase from the default implementation and thus lacks certain features. These will be available in 1.13.

## Prerequisites

* SD XL Base Model (Can be downloaded by going to Developer Tools (Wrench Icon) -> Download Stable Diffusion Models)
* Optional: SD XL Refiner Model (Can be downloaded by going to Developer Tools (Wrench Icon) -> Download Stable Diffusion Models)
  * **Important:** If you are not using a Refiner, you have to set Refine Strength to 0.

* Nvidia GPU with at least 8 GB VRAM, 12 GB+ is recommended



## Guide

1. Set the AI Implementation to  `Stable Diffusion XL (Diffusers - CUDA) `.
2. Ensure you have SD XL models in Diffusers format (if not, see above)
3. Select your model and optionally a refiner model
4. Set your prompt and settings and click `Generate!` to generate images.
