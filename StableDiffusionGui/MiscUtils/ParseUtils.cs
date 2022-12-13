using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.MiscUtils
{
    internal class ParseUtils
    {
        public static Size GetSize (string s, string delimiter = "x")
        {
            try
            {
                string[] values = s.Split(delimiter);
                return new Size(values[0].GetInt(), values[1].GetInt());
            }
            catch
            {
                return new Size();
            }
        }

        public static TEnum GetEnum<TEnum>(string str, bool ignoreCase = true, Dictionary<string, string> stringMap = null) where TEnum : Enum
        {
            if (stringMap == null)
                stringMap = new Dictionary<string, string>();

            str = stringMap.Get(str, true, true);
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            foreach (var entry in values)
            {
                string entryString = stringMap.Get(entry.ToString(), true);

                if (ignoreCase)
                {
                    if (entryString.Lower() == str.Lower())
                        return entry;
                }
                else
                {
                    if (entryString == str)
                        return entry;
                }
            }

            return (TEnum)(object)(-1);
        }
    }
}
