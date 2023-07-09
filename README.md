# NMKD Stable Diffusion GUI
Somewhat modular text2image GUI, initially just for Stable Diffusion.

Relies on a slightly customized fork of the InvokeAI Stable Diffusion code: [Code Repo](https://github.com/n00mkrad/stable-diffusion-cust/commits/main)

**Main Guide:**  
[**System Requirements**](#system-requirements)  
[**Features and How to Use Them**](#features-and-how-to-use-them)  
[**Hotkeys (Main Window)**](#hotkeys-main-window)

**Additional Guides:**  
[**AMD GPU Support**](https://github.com/n00mkrad/text2image-gui/blob/main/docs/Amd.md)  
[**Inpainting**](https://github.com/n00mkrad/text2image-gui/blob/main/docs/Inpainting.md)  



## System Requirements

**OS:** Windows 10/11 64-bit

#### Minimum:

- **GPU:** Nvidia GPU with 4 GB VRAM, Maxwell Architecture (2014) or newer
  - Alternatively, with limited feature support: Any DirectML-capable GPU with 8 GB of VRAM

- **RAM:** 8 GB RAM (Note: Pagefile must be enabled as swapping will occur with only 8 GB!)
- **Disk:** 10 GB (another free 5 GB for temporary files recommended)

#### Recommended:

- **GPU:** Nvidia GPU with 8 GB VRAM, Pascal Architecture (2016) or newer
- **RAM:** 16 GB RAM
- **Disk:** 12 GB on SSD (another free 5 GB for temporary files recommended)



## Features and How to Use Them

### Prompt Input

- **Multiple prompts at once:** Enter each prompt on a new line (newline-separated). Word wrapping does not count towards this.
- **Negative Prompt:** Put words or phrases into this box to tell the AI to exclude those things when generating images.
  - Alternatively, you can also put the negative prompt into the regular prompt box by wrapping it in [brackets].

- **Emphasis:** Use `+` after a word/phrase to make it more impactful, or `-` to do the opposite. You can also use to increase the effect. Wrap your phrase in parentheses if you want to apply it to more than one word.
  - Each plus/minus applies a multiplier of 1.1. So two `+++` would be 1.1^3 = 1.331, and so on.
  - You can also type the strength manually after parentheses, e.g. `a (huge)1.33 dog` instead of `a huge+++ dog`
  - Syntax Examples: `a green++ tree`, `a (big green)+ tree with orange- leaves (in the woods)++`

- **Wildcards:** Fill in words or phrases from a list into the prompt.
  - Inline: `photo of a ~car,tree,dog~`.
  - From File: `photo of a ~objects` for loading texts from `objects.txt` in your `Wildcards` folder in the SD GUI root folder.
  - Order: Use `~` for random/shuffled, `~~` for unchanged order, or `~~~` for sorted (A-Z) mode.




### Additional Inputs

* **Textual Inversion Embeddings:** Select a prompt embedding and add it to your prompt (Path can be set in Settings).
* **LoRA Files:** (Hidden if no files are in the folder) Select LoRA models and set the weight.
* **Base Image:** Load an initialization image that will be used together with your text prompt ("img2img"), or for inpainting
  * Loading multiple images means that each image will be processed separately.



### Stable Diffusion Settings

* **Steps:** More steps can increase detail, but only to a certain extent. Depending on the sampler, 15-50 is a good range.
  * Has a linear performance impact: Doubling the step count means each image takes twice as long to generate.
* **Prompt Guidance (CFG Scale):** Lower values are closer to the raw output of the AI, higher values try to respect your prompt more accurately.
  * Use low values if you are happy with the AI's representation of your prompt. Use higher values if not - but going too high will degrade quality.
  * No performance impact, no matter the value.
* **Seed:** Starting value for the image generation. Allows you to create the exact same image again by using the same seed.
  * When using the same seed, the image will only be identical if you also use the same sampler and resolution (and other settings).
  * Lock Seed Option: Disable incrementing the seed by 1 for each image. Only useful in combination with wildcards.
* **Resolution:** Adjust image size. Only values that are divisible by 64 are possible. Sizes above 512x512 can lead to repeated patterns.
  * Higher resolution images require more VRAM and are slower to generate.
  * High-Resolution Fix: Enable this to avoid getting repeated patterns at high resolutions (~768px+).
  
* **Sampler:** Changes the way images are sampled. DPM++ 2M Karras is the default because it's fast and tends to look good even with 10-20 steps.
* **Generate Seamless Images:** Generates seamless/tileable images, very useful for making game textures or repeating backgrounds.
* **Generate Symmetric Images:** Generates images that are mirrored on one axis.



### Image Viewer

* **Review current images:** Use the scroll wheel while hovering over the image to go to the previous/next image.
* **Slideshow:** The image viewer always shows the newest generated image if you haven't manually changed it in the last 3 seconds.
* **Context Menu:** Right-click into the image area to show more options.
* **Pop-Up Viewer:** Click into the image area to open the current image in a floating window.
  * Use the mouse wheel to change the window's size (zoom), right-click for more options, double-click to toggle fullscreen.



### Settings Button (Top Bar)

*Note: Some options might be hidden depending on the selected implementation.*

* **Image Generation Implementation:** Choose the AI implementation that's used for image generation.
  * Stable Diffusion - [InvokeAI](https://github.com/invoke-ai/InvokeAI/): Supports the most features, but struggles with 4 GB or less VRAM, requires an Nvidia GPU
  * Stable Diffusion - [ONNX](https://github.com/huggingface/diffusers): Lacks some features and is relatively slow, but can utilize AMD GPUs (any DirectML capable card)
  * InstructPix2Pix - For instruction-based image editing. Requires an Nvidia GPU
  
* **Use Full Precision:** Use FP32 instead of FP16 math, which requires more VRAM but can fix certain compatibility issues.*

* **Unload Model After Each Generation:** Completely unload Stable Diffusion after images are generated.*

* **Stable Diffusion Model File:** Select the model file to use for image generation.
  
  * Included models are located in `Models/Checkpoints`. You can add external folder paths by clicking on "Folders...".
  
* **Stable Diffusion VAE:** Select external VAE (Variational Autoencoder) model. VAEs can improve image quality.*

  * Default path is `Models/VAEs`. You can add external folder paths by clicking on "Folders...".

* **Textual Inversion Embeddings Folder:** Select folder where embeddings (usually `.pt` files) are loaded from.

* **LoRA Models Folder:** Select folder where LoRA models (`.safetensors` files) are loaded from.

* **Cache Models in RAM:** When enabled, models are offloaded into RAM when switching to a new one. This makes it very fast to switch back, but takes up 2GB+ per cached model.

* **Skip Final CLIP Layers (CLIP Skip):** Can improve quality on certain models.

* **CUDA Device:** Allows your to specify the GPU to run the AI on, or set it to run on the CPU (very slow).*

  

* **Image Output Folder:** Set the folder where your generated images will be saved.

* **Output Subfolder Options:**

  * Subfolder Per Prompt: Save images in a subfolder for each prompt. Negative prompt is excluded from the folder name.
  * Ignore Wildcards: Use wildcard name (as in prompt input) instead of the replaced text in file/folder names.
  * Subfolder Per Session: Save images in a subfolder for each session (every time the program is started).

* **Information to Include in Filename:** Specify which information should be included in the filename.

* **Favorites Folder:** Specify your favorites folder, where your favorite images will be copied to (right-click image viewer or use Ctrl+D)

* **Image Save Mode:** Choose whether you want to delete or keep generated images by default.

* **When Running Multiple Prompts, Use Same Starting Seed for All of Them:** If enabled, the seed resets to the starting value for every new prompt. If disabled, the seed will be incremented by 1 after each iteration, being sequential until all prompts/iterations have been generated.

* **When Post-Processing Is Enabled, Also Save Un-Processed Image:** When enabled, both the "raw" and the post-processed image will be saved.

  

* **Automatically Set Generation Resolution After Loading an Initialization Image:** Automatically sets the image generation to match your image.

* **Retain Aspect Ratio of Initialization Image (If It Needs Resizing):** Use padding (black borders) instead of stretching, in case the init image resolution does not match the image generation resolution.

* **Advanced Mode:** Increases the limits of the sliders in the main window. Not very useful most of the time unless you really need those high values.

* **Notify When Image Generation Has Finished:** Play a sound, show a notification, or do both if image generation finishes in background.



### Logs Button (Top Bar)

* **Open Logs Folder:** Opens the log folder of the current session. The application deletes older logs on every startup.
* **\<logname\>.txt**: Open the log file or copy the text.



### Installation Manager & Updater Button (Top Bar)

* **Manage Installation:** Allows you to check if your installation is valid and can repair/reset it.
  * **Installation Status:** Shows which modules are installed (checkboxes are not interactive and only indicate if a module is installed correctly!).
  * **Re-Install Python Dependencies:** Re-installs the Stable Diffusion code from its repository and re-installs all required python packages.
  * **Re-Install Upscalers:** (Re-)Installs upscaling files (RealESRGAN, GFPGAN, CodeFormer, including model files).
  * **(Re-)Install:** Installs everything. Skips already installed components.
  * **Uninstall:** Removes everything except for Conda which is included and needed for a re-installation.


- **Install Updates:** Allows you to update to a new version or re-install the current one.



### Developer Tools Button (Top Bar)

* **Open Stable Diffusion CLI:** Use Stable Diffusion in command-line interface.
* **Open CMD in Python Environment:** Opens a CMD window with the built-in python environment activated.
* **Merge Models:** Allows you to merge/blend two models. The percentage numbers represent their respective weight.
* **Prune Models:** Allows you to reduce the size of models by removing data that's not needed for image generation.
* **Convert Models:** Allows you to convert model weights between Pytorch (ckpt/pt), Diffusers, Diffusers ONNX, and SafeTensors formats.
* **View Log In Realtime:** Opens a separate window that shows all log output, including messages that are not shown in the normal log box.



### Post-Processing Button (Top Bar)

* **Upscaling:** Set RealESRGAN upscaling factor.
* **Face Restoration:** Enable GFPGAN or CodeFormer for face restoration.



### Training Button (Top Bar)

* Opens LoRA training window ([Guide here](https://github.com/n00mkrad/text2image-gui/blob/main/DreamBooth.md))



### Bottom Bar Buttons

* **Generate:** Start AI image generation (or cancel if it's already running).
* **Prompt Queue Button:** Right-click to add the current settings to the queue, or left-click to manage the queued entries.
* **Prompt History Button:** View recent prompts, load them into the main window, search or clear history, or disable it.
* **Image Deletion Button:** Delete either the image that is being viewed currently, or all images from the current batch.
* **Open Folder Button:** Opens the (root) image output folder.
* **Left/Right Buttons:** Show the previous or next image from the current batch.



## Hotkeys (Main Window)

- **CTRL+G:** Run Image Generation (or Cancel if already running)
- **CTRL+M:** Show Model Quick Switcher (Once it's open, use ESC to Cancel or Enter to confirm)
- **CTRL+Shift+M:** Show VAE Quick Switcher
- **CTRL+PLUS:** Toggle Prompt Textbox Size
- **CTRL+Shift+PLUS:** Toggle Negative Prompt Textbox Size
- **CTLR+DEL:** Delete currently viewed image
- **CTRL+SHIFT+DEL:** Delete all generated images (of the current batch)
- **CTRL+O:** Open currently viewed image
- **CTRL+SHIFT+O:** Show current image in its folder
- **CTRL+C:** Copy currently viewed image to clipboard
- **CTRL+D:** Copy currently viewed image to favorites
- **CTRL+V:** Paste image (If clipboard contains a bitmap)
- **CTRL+Q:** Quit
- **CTRL+Scroll:** Change textbox font size (only works while the textbox is being used)
- **F1:** Open Help (Currently links to GitHub Readme)
- **F12:** Open Settings
- **ESC:** Remove focus from currently focused GUI element (e.g. get out of the prompt textbox)
