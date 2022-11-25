using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZetaLongPaths;

namespace StableDiffusionGui.Data
{
    public class Model
    {
        public enum ModelFormat { Pytorch, Onnx }

        public ModelFormat Format { get; set; }
        public string Name { get { return _file == null ? _dir.Name : _file.Name; } }
        public string FullName { get { return _file == null ? _dir.FullName : _file.FullName; } }
        public ZlpDirectoryInfo Directory { get { return _file == null ? _dir.Parent : _file.Directory; } }
        public string Extension { get { return _file == null ? "" : _file.Extension; } }
        public long Size { get { return _file == null ? IoUtils.GetDirSize(_dir.FullName, true) : _file.Length; } }
        public Enums.StableDiffusion.Implementation[] CompatibleImplementations { get; set; }

        private ZlpFileInfo _file = null;
        private ZlpDirectoryInfo _dir = null;

        public Model () { }

        public Model(string dataPath, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations)
        {
            if (IoUtils.IsPathDirectory(dataPath))
                _dir = new ZlpDirectoryInfo(dataPath);
            else
                _file = new ZlpFileInfo(dataPath);

            CompatibleImplementations = compatibleImplementations.ToArray();
        }

        public Model(ZlpFileInfo file, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations)
        {
            _file = file;
            CompatibleImplementations = compatibleImplementations.ToArray();
        }

        public Model(ZlpDirectoryInfo dir, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations)
        {
            _dir = dir;
            CompatibleImplementations = compatibleImplementations.ToArray();
        }
    }
}
