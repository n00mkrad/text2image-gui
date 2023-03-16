using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;

namespace StableDiffusionGui.Ui
{
    internal class Fonts
    {
        // public static PrivateFontCollection _privFontCollection = new PrivateFontCollection();
        public static List<FontFamily> LoadedFonts = new List<FontFamily>();

        /// <summary> Returns a font if found, otherwise try loading it from <paramref name="loadPath"/>, optionally keep it loaded with <paramref name="storeAfterLoading"/> </summary>
        public static FontFamily GetFontOnDemand(string name, string loadPath, bool storeAfterLoading, bool matchCase = false, bool matchFullName = false)
        {
            FontFamily font = GetFont(name, matchCase, matchFullName);

            if (font != null)
            {
                Logger.LogIf($"GetFontOnDemand: Font found ({font.Name}), no need to load it.", Logger.Switches.LogFontLoader);
                return font;
            }

            PrivateFontCollection pfc = LoadFont(loadPath);
            font = GetFont(name, matchCase, matchFullName, pfc.Families.ToList());
            Logger.LogIf($"GetFontOnDemand: Font '{name}' not found, fallback from '{loadPath}' {(pfc == null ? "failed" : $"successful ({font.Name})")}", Logger.Switches.LogFontLoader);

            if (storeAfterLoading)
                AddFont(pfc);

            return font;
        }

        /// <summary> Get font by name. Returns null if not found. </summary>
        public static FontFamily GetFont(string name, bool matchCase = false, bool matchFullName = false, List<FontFamily> customList = null)
        {
            var list = customList == null ? LoadedFonts : customList;

            if (!matchCase)
                name = name.Lower();

            foreach (FontFamily loadedFont in list)
            {
                string n = matchCase ? loadedFont.Name : loadedFont.Name.Lower();

                if (matchFullName && n == name)
                    return loadedFont;
                else if (!matchFullName && n.Contains(name))
                    return loadedFont;
            }

            Logger.LogIf($"Font not found: {name} (Match Case: {matchCase}, Match Full Name: {matchFullName})", Logger.Switches.LogFontLoader);
            return null;
        }

        /// <summary> Load font from file <paramref name="fontPath"/> and store in RAM. </summary>
        public static void LoadAndStoreFont(string fontPath)
        {
            var pvc = LoadFont(fontPath);
            AddFont(pvc);
        }

        /// <summary> Load font from file <paramref name="fontPath"/> and return it as FontFamily. </summary>
        public static PrivateFontCollection LoadFont(string fontPath)
        {
            try
            {
                PrivateFontCollection pvc = new PrivateFontCollection();
                pvc.AddFontFile(fontPath);
                return pvc;
            }
            catch
            {
                return null;
            }
        }

        private static void AddFont(PrivateFontCollection pvc)
        {
            if (pvc == null)
                return;

            LoadedFonts.AddRange(pvc.Families.Where(newFont => !LoadedFonts.Select(f => f.Name).ToList().Contains(newFont.Name)));
        }
    }
}
