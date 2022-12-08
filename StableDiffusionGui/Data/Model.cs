using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Constants;

namespace StableDiffusionGui.Data
{
    public class Model
    {
        public Enums.Models.Format Format { get; set; }
        public string Name { get { return _file == null ? _dir.Name : _file.Name; } }
        public string FullName { get { return _file == null ? _dir.FullName : _file.FullName; } }
        public ZlpDirectoryInfo Directory { get { return _file == null ? _dir.Parent : _file.Directory; } }
        public string Extension { get { return _file == null ? "" : _file.Extension; } }
        public long Size { get { return _file == null ? IoUtils.GetDirSize(_dir.FullName, true) : _file.Length; } }
        public bool IsDirectory { get { return _dir != null; } }
        public Enums.StableDiffusion.Implementation[] CompatibleImplementations { get; set; }

        private ZlpFileInfo _file = null;
        private ZlpDirectoryInfo _dir = null;

        public Model() { }

        public Model(string dataPath, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations)
        {
            if (IoUtils.IsPathDirectory(dataPath))
                _dir = new ZlpDirectoryInfo(dataPath);
            else
                _file = new ZlpFileInfo(dataPath);

            CompatibleImplementations = compatibleImplementations.ToArray();
            Format = DetectModelFormat();
        }

        public Model(ZlpFileInfo file, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations)
        {
            _file = file;
            CompatibleImplementations = compatibleImplementations.ToArray();
            Format = DetectModelFormat();
        }

        public Model(ZlpDirectoryInfo dir, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations)
        {
            _dir = dir;
            CompatibleImplementations = compatibleImplementations.ToArray();
            Format = DetectModelFormat();
        }

        private Enums.Models.Format DetectModelFormat()
        {
            try
            {
                if (_file != null)
                {
                    if (_file.Length < 16 * 1024 * 1024) // Assume that a <16 MB file is not a valid model
                        return (Enums.Models.Format)(-1);

                    if (_file.FullName.Lower().EndsWith(".ckpt") || _file.FullName.Lower().EndsWith(".pt"))
                        return Enums.Models.Format.Pytorch;
                }
                else if (_dir != null)
                {
                    List<string> subDirs = _dir.GetDirectories().Select(d => d.Name).ToList();

                    bool diffusersStructureValid = new[] { "text_encoder", "tokenizer", "unet" }.All(d => subDirs.Contains(d));
                    var unetDir = new ZlpDirectoryInfo(Path.Combine(_dir.FullName, "unet"));
                    bool unetValid = unetDir.Exists && IoUtils.GetDirSize(unetDir.FullName, false) < 64 * 1024 * 1024; // Assume that a <64 MB unet file is not valid
                    string indexJsonPath = Path.Combine(_dir.FullName, "model_index.json");

                    if (diffusersStructureValid && unetValid && File.Exists(indexJsonPath))
                    {
                        if (File.ReadAllLines(indexJsonPath).Any(l => l.Contains(@"""_class_name"": ""Onnx")))
                            return Enums.Models.Format.DiffusersOnnx;
                        if (File.ReadAllLines(indexJsonPath).Any(l => l.Contains(@"""_class_name"":")))
                            return Enums.Models.Format.Diffusers;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log($"Failed to detect model format: {ex.Message} ({FullName})");
            }

            return (Enums.Models.Format)(-1);
        }
    }
}
