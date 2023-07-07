# LoRA Training GUI
Basic training GUI for LoRA/LyCORIS techniques, allowing you to train characters, concepts or styles that you can apply on top of any model, instead of having to make it a completely separate model (Dreambooth).



## System Requirements

- **GPU:** Nvidia GPU with 8 GB VRAM, Turing Architecture (2018) or newer recommended (6 GB has been reported to work as well)
- **RAM:** 32 GB RAM (16 GB should work as well, but is not tested)



## Training

### Preparing Your Dataset

* Use 5-100 images for training. More images are not necessarily better. **Quality over quantity!** 15 hand-picked, manually tagged images can be better than 300 bulk-downloaded images.
* Size and aspect ratio does not matter much, though it's optimal to crop away unneeded borders of your image, only focusing on your subject.
* Use images that represent your character/object in different (optimally somewhat neutral) poses/scenarios. Variation helps (e.g. if all your images have a blank white background, it will be harder for your model to generate a different background). Avoid low-quality images, messy images where the subject is not clear, and unusual poses. Overall, you need to **find a sweet spot between variety and uniformity**.



### Settings

- **Training Method:** Currently, only LoHa is implemented. It is a more efficient alternative to the conventional LoRA training method.
- **Base Model:** Set the model you want to use as a "template". For best results, use a rather neutral model (e.g. SD 1.5 for realistic LoRAs, or animefull-final for 2D art)
- **LoRA Network Settings:** For simplicity, you don't need to set all values manually, so I made presets. The highest quality is recommended.
  - **Size:** Set the network size, smaller results in a smaller file size and potentially faster generation speed, but might lose details.
  - **CLIP Skip:** This should match your base model (0 for SD 1.5 based models, 2 for NovelAI based models).
- **Project Name:** This is only used for the filename of the LoRA file and does not affect training.
- **Dataset Folder:** Specify the path to your training images (and TXT files if you want to use captions).
- **Captions:** Each image gets trained with a caption, here you can change what should be used as caption
  - **No Caption:** Trains images with an empty caption, this means the LoRA will always affect your image, since you can't prompt it.
  - **Single Phrase:** Use a single word/phrase for all images. Easy, but does not allow fine control over the results.
  - **TXT Captions:** Read captions from TXT files (e.g. image.png needs to have image.txt) in the training folder. Requires tagging, but is the most flexible option.
- **Training Resolution:** Change the resolution to which the images will be resized. Usually 512 or 640 is fine. Higher is not always better.
  - **Aspect Ratio Grouping:** Enabled by default, groups images of similar aspect ratios together, which makes training more efficient.

- **Learning Rate:** Controls how "aggressively" the images are trained. High values allow you to use a lower step count (= faster training) but might result in lower quality or an "overbaked" model that is not very flexible.
  - Generally, you should increase the LR the more "complex" your training images are. Simple line art might require a lower than default value, very messy and detailed images might require a high value. 

- **Training Steps:** Controls for how long the model should be trained. Each step trains one batch of images (4 by default).

- **Developer Options:**
  - **Train Format / Data Format:** Use 16-bit Float, 16-bit BFloat, or 32-bit Float for training/saving the model. Note: "float" might be broken currently!
  - **Gradient Checkpointing:** Reduces VRAM usage a lot, but also slows down training a little bit (~20%). Don't disable it unless you have 16+ GB VRAM.
  - **Seed:** Set initial seed for training. Does not appear to actually work with the current code though, as using the same seed+settings still results in slightly different generated images.
  - **Shuffle Tags:** Randomizes tag order for each trained image, assuming they are comma-separated. Only use this with Booru-style tags.
  - **Flip Augmentation:** Increases dataset diversity by flipping your image. Do not use this if you need to retain asymmetric traits in your images.
  - **Color Augmentation:** Increases dataset diversity by altering the colors. Needs testing, may not actually improve results.

