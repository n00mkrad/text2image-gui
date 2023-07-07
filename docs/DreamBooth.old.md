# DreamBooth Training GUI
Basic training GUI for [DreamBooth](https://dreambooth.github.io/).



## System Requirements

- **GPU:** Nvidia GPU with 24 GB VRAM, Turing Architecture (2018) or newer
- **RAM:** 32 GB RAM
- **Disk:** 12 GB on NVME SSD (another free 30 GB for temporary files recommended)

**Performance Reference:** RTX 3090 (no OC) should achieve a speed of around 1.1 seconds per training iteration/step.

**Important:** The current implementation is rather fast but also VRAM-hungry. Close all GPU-accelerated programs during training, this includes Web Browsers, web-based applications (Discord, Game Launchers, GitHub Desktop, Visual Studio, etc.), and obviously games. **Without the full 24 GB available, training will slow down a lot!**



## Training

### Preparing Your Dataset

* Use 4-20 images for training. More images are not necessarily better. **Quality over quantity!** 5 hand-picked, manually cropped images can be better than 200 bulk-downloaded images that were automatically cropped/padded. 
* Resize images to 512px on the longer side, then pad them to fill 512x512.
* Use images that represent your character/object in different (optimally somewhat neutral) poses/scenarios. Variation helps (e.g. if all your images have a blank white background, it will be harder for your model to generate a different background). Avoid low-quality images, messy images where the subject is not clear, and unusual poses. Overall, you need to **find a sweet spot between variety and uniformity**.



### Settings

- **Base Model:** Set the model you want to use as a "template". Your new object/character will be added into it.
  - You do **not** need a "full-ema" model for this. It works, but there is no benefit over a 4 GB model. You can also use 2 GB (fp16) models as base, but this might decrease quality slightly.
  - The resulting model should, apart from minor "bleeding", still work the same as the base model, it is not limited to your trained concept.

- **Training Preset:** For simplicity, you don't need to set all values manually, so I made presets. The highest quality is recommended.
  - Very High Quality: 4000 Steps (Reference)
  - High Quality: 2000 Steps, 2x Learning Rate to compensate
  - Medium Quality: 1000 Steps, 4x Learning Rate to compensate
  - Low Quality: 250 Steps, 16x Learning Rate to compensate

- **Training Images Folder:** Specify the path to your training images. **They need to be 512x512** - If they aren't, you should resize and pad them.
  - An automatic resizing/padding tool is planned for a future update.

- **Class Token:** This is the word that we will use to put our new character/object in. Once done, you can type this to generate it.
  - Currently, the class is fully replaced, instead of using regularization like in the original implementation. This means that you shouldn't use a generic class like "man" or "person" because otherwise every man/person will look like the training data.
  - Try to use existing classes, especially for characters. For example, if your base model was trained on Gelbooru, and you want to train a model on Zelda, use `princess_zelda` as that's already an existing tag/token in the model.

- **Learning Rate:** Usually "Normal" is fine. However, if your trained model does not seem to properly learn the data, you can increase this.
  - A learning rate that's too high can result in overfitting (model is not flexible and does not react to prompt changes) or decreased visual quality.

