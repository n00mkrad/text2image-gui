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

### Run Multiple Prompts:

Enter each prompt on a new line (newline-separated). Word wrapping does not count towards this.

### WORK IN PROGRESS

## Hotkeys (Main Window)

**CTRL+G:** Run Image Generation (or Cancel if already running)
**CTRL+M:** Show Model Quick Switcher
**CTRL+PLUS:** Toggle Prompt Textbox Size
**CTLR+DEL:** Delete currently viewed image
**CTRL+SHIFT+DEL:** Delete all generated images (of the current batch)
**CTRL+O:** Open currently viewed image
**CTRL+SHIFT+O:** Show current image in its folder
**F12:** Open Settings
**CTRL+V:** Paste Image (If clipboard contains a bitmap)
**CTRL+Q:** Quit
