using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Installation
{
    internal class EnabledFeatures
    {
        public static bool InvokeAiModelCaching { get { return false || Program.Debug; } }
        public static bool WildcardAutocomplete { get { return false; } }
        public static bool RunwayMlInpainting { get { return false || Program.Debug; } }
        public static bool MaskPasting { get { return false || Program.Debug; } }
        public static bool MaskInversion { get { return false || Program.Debug; } }
        public static bool AutoSetSizeForInitImg { get { return false || Program.Debug; } }

        public static List<Implementation> DisabledImplementations { get { return new List<Implementation>() { Implementation.DiffusersOnnx }; } }
        public static List<InpaintMode> DisabledInpaintModes { get { return new List<InpaintMode>() { InpaintMode.TextMask }; } }
    }
}
