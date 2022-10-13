# NMKD Stable Diffusion GUI
Somewhat modular text2image GUI, initially just for Stable Diffusion.

Relies on a slightly customized fork of the InvokeAI Stable Diffusion code (formerly lstein): [Code Repo](https://github.com/n00mkrad/stable-diffusion-cust/commits/main)



## System Requirements

### Minimum:

**GPU:** Nvidia GPU with 4 GB VRAM, Maxwell Architecture (2014) or newer
**RAM:** 8 GB RAM (Note: Pagefile must be enabled as swapping will occur with only 8 GB!)
**Disk:** 12 GB (another free 2 GB for temporary files recommended)

### Recommended:

**GPU:** Nvidia GPU with 8 GB VRAM, Pascal Architecture (2016) or newer
**RAM:** 16 GB RAM
**Disk:** 12 GB (another free 2 GB for temporary files recommended)

### Professional/DreamBooth-capable:

**GPU:** Nvidia GPU with 24GB VRAM, Turing Architecture (2018) or newer
**RAM:** 32 GB RAM
**Disk:** 12 GB (another free 30 GB for temporary files recommended)

## Features and How to Use Them

### Prompt Input

- **Multiple prompts at once:** Enter each prompt on a new line (newline-separated). Word wrapping does not count towards this.
- **Exclusion Words:** Put words or phrases into [square brackets] to tell the AI to exclude those things when generating images.
- **Emphasis:** Use (parentheses) to make a word/phrase more impactful, or {curly brackets} to do the opposite. You can also use ((multiple)).

### Additional Inputs

* **Load Image:** Load an initialization image that will be used together with your text prompt ("img2img")
* **Load Concept:** Load a Textual Inversion concept to apply a style or use a specific character

### Stable Diffusion Settings

* **Steps:** More steps can increase detail, but only to a certain extend. Depending on the sampler, 20-60 is a good range.
  * Has a linear performance impact: Doubling the step count means each image takes twice as long to generate.
* **Prompt Guidance (CFG Scale):** Lower values are closer to the raw output of the AI, higher values try to respect your prompt more accurately.
  * Use low values if you are happy with the AI's representation of your prompt. Use higher values if not - but going too high will degrade quality.
  * No performance impact, no matter the value.
* **Seed:** Starting value for the image generation. Allows you to create the exact same image again by using the same seed.
  * When using the same seed, the image will only be identical if you also use the same sampler and resolution (and other settings).
* **Resolution:** Adjust image size. Only values that are divisible by 64 are possible. Sizes above 512x512 can lead to repeated patterns.
* **Sampler:** Changes the way images are sampled. Euler Ancestral is default because it's fast and tends to look good even with few steps.
* **Generate Seamless Images:** Generates seamless/tileable images, very useful for making game textures or repeating backgrounds.

### Image Viewer

* **Review current images:** Use the scroll wheel while hovering over the image to go to the previous/next image.
* **Slideshow:** The image viewer always shows the newest generated image if you haven't manually changed it in the last 3 seconds.
* **Context Menu:** Right-click into the image area to show more options
* **Pop-Up Viewer:** Click into the image area to open the current image in a floating window.
  * Use the mouse wheel to change the window's size (zoom), right-click for more options, double-click to toggle fullscreen.

### WORK IN PROGRESS

## Hotkeys (Main Window)

- **CTRL+G:** Run Image Generation (or Cancel if already running)
- **CTRL+M:** Show Model Quick Switcher
- **CTRL+PLUS:** Toggle Prompt Textbox Size
- **CTLR+DEL:** Delete currently viewed image
- **CTRL+SHIFT+DEL:** Delete all generated images (of the current batch)
- **CTRL+O:** Open currently viewed image
- **CTRL+SHIFT+O:** Show current image in its folder
- **CTRL+V:** Paste Image (If clipboard contains a bitmap)
- **CTRL+Q:** Quit
- **CTRL+Scroll:** Change font size (only while mouse is over prompt text field)
- **F1:** Open Help (Currently links to GitHub Readme)
- **F12:** Open Settings
