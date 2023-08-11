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
        public Enums.StableDiffusion.ModelArch LoadArchitecture { get; set; } = Enums.StableDiffusion.ModelArch.Automatic;
        public string Name { get { return _file == null ? (_dir == null ? Constants.NoneMdl : _dir.Name) : _file.Name; } }
        public string FullName { get { return _file == null ? _dir.FullName : _file.FullName; } }
        public string FormatIndependentName { get { return Path.ChangeExtension(Name, null); } }
        public ZlpDirectoryInfo Directory { get { return _file == null ? _dir.Parent : _file.Directory; } }
        public string Extension { get { return _file == null ? "" : _file.Extension; } }
        public long Size { get { return _file == null ? IoUtils.GetDirSize(_dir.FullName, true) : _file.Length; } }
        public bool IsDirectory { get { return _dir != null; } }

        private ZlpFileInfo _file = null;
        private ZlpDirectoryInfo _dir = null;

        public Model() { }

        public Model(string dataPath, Enums.Models.Format format = (Enums.Models.Format)(-1), Enums.Models.Type type = (Enums.Models.Type)(-1))
        {
            if (IoUtils.IsPathDirectory(dataPath) == true)
                _dir = new ZlpDirectoryInfo(dataPath);
            else
                _file = new ZlpFileInfo(dataPath);

            Format = format == (Enums.Models.Format)(-1) ? Models.DetectModelFormat(FullName) : format;
            Type = type == (Enums.Models.Type)(-1) ? Models.GetModelType(FullName) : type;
        }

        public Model(ZlpFileInfo file, Enums.Models.Format format = (Enums.Models.Format)(-1), Enums.Models.Type type = (Enums.Models.Type)(-1))
        {
            _file = file;
            Format = format == (Enums.Models.Format)(-1) ? Models.DetectModelFormat(FullName) : format;
            Type = type == (Enums.Models.Type)(-1) ? Models.GetModelType(FullName) : type;
        }

        public Model(ZlpDirectoryInfo dir, Enums.Models.Format format = (Enums.Models.Format)(-1), Enums.Models.Type type = (Enums.Models.Type)(-1))
        {
            _dir = dir;
            Format = format == (Enums.Models.Format)(-1) ? Models.DetectModelFormat(FullName) : format;
            Type = type == (Enums.Models.Type)(-1) ? Models.GetModelType(FullName) : type;
        }

        public void SetArch(Enums.StableDiffusion.ModelArch arch, int res = 0)
        {
            LoadArchitecture = arch;

            // if (res > 0)
            //     BaseResolution = res;
            // else
            //     BaseResolution = arch == Enums.Models.SdArch.V2V ? 768 : 512;
        }

        public override string ToString()
        {
            return Name; // $"{Name} {(Format != (Enums.Models.Format)(-1) ? $"({Format} {Type})" : "")}";
        }
    }
}
