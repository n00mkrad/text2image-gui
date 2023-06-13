using System;

namespace StableDiffusionGui.MiscUtils
{
    internal class Assert
    {
        public static void Check(bool condition, string message)
        {
            if (!condition)
                throw new Exception(message);
        }
    }
}
