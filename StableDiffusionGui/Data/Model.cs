using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Main.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZetaLongPaths;

namespace StableDiffusionGui.Data
{
    public class Model
    {
        public Enums.Models.Format Format { get; set; } = (Enums.Models.Format)(-1);
        public Enums.Models.Type Type { get; set; } = (Enums.Models.Type)(-1);
        public string Name { get { return _file == null ? _dir.Name : _file.Name; } }
        public string FullName { get { return _file == null ? _dir.FullName : _file.FullName; } }
        public string FormatIndependentName { get { return Path.ChangeExtension(Name, null); } }
        public ZlpDirectoryInfo Directory { get { return _file == null ? _dir.Parent : _file.Directory; } }
        public string Extension { get { return _file == null ? "" : _file.Extension; } }
        public long Size { get { return _file == null ? IoUtils.GetDirSize(_dir.FullName, true) : _file.Length; } }
        public bool IsDirectory { get { return _dir != null; } }
        public Enums.StableDiffusion.Implementation[] CompatibleImplementations { get; set; }

        private ZlpFileInfo _file = null;
        private ZlpDirectoryInfo _dir = null;

        public Model() { }

        public Model(string dataPath, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations = null)
        {
            if (IoUtils.IsPathDirectory(dataPath))
                _dir = new ZlpDirectoryInfo(dataPath);
            else
                _file = new ZlpFileInfo(dataPath);

            CompatibleImplementations = compatibleImplementations == null ? new List<Enums.StableDiffusion.Implementation>().ToArray() : compatibleImplementations.ToArray();
            Format = Models.DetectModelFormat(FullName);
            Type = Models.GetModelType(FullName);
        }

        public Model(ZlpFileInfo file, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations = null)
        {
            _file = file;
            CompatibleImplementations = compatibleImplementations == null ? new List<Enums.StableDiffusion.Implementation>().ToArray() : compatibleImplementations.ToArray();
            Format = Models.DetectModelFormat(FullName);
            Type = Models.GetModelType(FullName);
        }

        public Model(ZlpDirectoryInfo dir, IEnumerable<Enums.StableDiffusion.Implementation> compatibleImplementations = null)
        {
            _dir = dir;
            CompatibleImplementations = compatibleImplementations == null ? new List<Enums.StableDiffusion.Implementation>().ToArray() : compatibleImplementations.ToArray();
            Format = Models.DetectModelFormat(FullName);
            Type = Models.GetModelType(FullName);
        }

        public override string ToString()
        {
            return $"{Name} {(Format != (Enums.Models.Format)(-1) ? $"({Format} {Type})" : "")}";
        }
    }
}
