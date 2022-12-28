# Inpainting
Inpainting means replacing parts of an image with AI-generated content, optimally in way that makes it look like it was never edited.



## Inpainting Methods

#### Masked Overlay Inpainting

Generates an entire image based on your prompt and the original image, they overlays the original image minus the masked areas.

* **Pros:** Works with any implementation, and all regular features
* **Cons:** Not truly context-aware, prompt needs to describe the entire scene instead of just the inpainting regions

#### RunwayML Inpainting

Generates an entire image based on your prompt and the original image, they overlays the original image minus the masked areas.

* **Pros:** Excellent quality, fully context-aware even without a prompt, reproduces environment well (lighting, style, quality, etc.)
* **Cons:** Requires specific models (`sd-v1-5-inpainting`), negative prompts do not work currently, prompt has less impact

**Important:** Inpainting-capable models must be marked as such by using the filename suffix `-inpainting`. If the filename does not end with that, it will not be recognized as an inpainting model.  
Also, do not enable any mask blurring/smoothing when using this method - it requires a clean cutoff. When making a mask in an image editor, set your brush to full strength and disable anti-aliasing (avoid semi-transparent pixels).



## Masking Methods

#### Image Mask

Will open a basic painting tool once you press *Generate* that allows you to paint the regions which should be replaced by AI-generated content.  
The mask editor also supports loading a mask bitmap from a file or from the clipboard, where black is the masked area and white is the area that should remain untouched.

#### Text Mask

Allows you to describe the objects you want to replace, using CLIP (clipseg). Not very accurate usually, but worth experimenting with.



## Tutorial

#### Masked Overlay Inpainting

1. Load an image into SD GUI by dragging and dropping it, or by pressing "Load Image(s)"
2. Select a masking mode next to *Inpainting* (Image Mask or Text)
3. Press Generate, wait for the Mask Editor window to pop up, and create your mask
4. Once your mask is ready, click OK and image generation will start!
5. If you are not happy with the results, try creating a different mask and changing the `Initialization Image Strength (Influence)` value.

#### RunwayML Inpainting


Make sure you have an inpainting model. With the default InvokeAI implementation, download [sd-v1-5-inpainting](https://huggingface.co/runwayml/stable-diffusion-inpainting/blob/main/sd-v1-5-inpainting.ckpt) to `Data/models`. If you use ONNX/DirectML, download the ONNX model instead ([stable-diffusion-inpainting](https://icedrive.net/s/hfhiGB4uC4XyCBTzN4x9F3fYQA7A)) and extract it to `Data/models` so that your folder structure is `Data/models/stable-diffusion-inpainting/model_index.json` etc.

1. Select your inpainting model (in settings or with Ctrl+M)
2. Load an image into SD GUI by dragging and dropping it, or by pressing "Load Image(s)"
3. Select a masking mode next to *Inpainting* (Image Mask or Text)
4. Press Generate, wait for the Mask Editor window to pop up, and create your mask (**Important: Do not use a blurred mask with this method!**)
5. Once your mask is ready, click OK and image generation will start!
6. If you are not happy with the results, try creating a different mask. Usually it's better if it's bigger, to cover edges or shadows of your object.



## Examples

#### Masked Overlay Inpainting

![](https://raw.githubusercontent.com/n00mkrad/text2image-gui/main/docs/assets/inpdemo-wing-basic.png)

*Original vs Mask - Result using `photo of a landscape, cloudy sky` using Stable Diffusion 1.5*  
Note that the wing does not really disappear, it just gets replaced by a weird cloud formation.

#### RunwayML Inpainting

![](https://raw.githubusercontent.com/n00mkrad/text2image-gui/main/docs/assets/inpdemo-wing-rwml.png)

*Original - Mask - Result using `photo of a landscape, cloudy sky` using Stable Diffusion 1.5 Inpainting (RunwayML)*  
Note that with the RunwayML inpainting model, the wing gets fully removed and replaced with landscape, making it almost impossible to tell that the area was filled with AI-generated content.
