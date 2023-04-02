using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Io
{
    public class ConfigInstance
    {
        public EasyDict<string, Enums.Models.SdArch> ModelArchs = new EasyDict<string, Enums.Models.SdArch>();

        public ConfigInstance ()
        {

        }

        public void Clean ()
        {
            ModelArchs.Keys.Where(path => !File.Exists(path)).ToList().ForEach(path => ModelArchs.Remove(path));
        }
    }
}
