using StableDiffusionGui.Forms;
using Newtonsoft.Json;
using StableDiffusionGui.Io;
using StableDiffusionGui;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using StableDiffusionGui.Main;

namespace StableDiffusionGui.IO
{
    class Config
    {
        private static string configPath;
        public static Dictionary<string, string> cachedValues = new Dictionary<string, string>();

        public static void Init()
        {
            configPath = Path.Combine(Paths.GetDataPath(), "config.json");
            IoUtils.CreateFileIfNotExists(configPath);
            Reload();
        }

        // public static async Task Reset(int retries = 3, SettingsForm settingsForm = null)
        // {
        //     try
        //     {
        //         if (settingsForm != null)
        //             settingsForm.Enabled = false;
        // 
        //         File.Delete(configPath);
        //         await Task.Delay(100);
        //         cachedValues.Clear();
        //         await Task.Delay(100);
        // 
        //         if (settingsForm != null)
        //             settingsForm.Enabled = true;
        //     }
        //     catch (Exception e)
        //     {
        //         retries -= 1;
        //         Logger.Log($"Failed to reset config: {e.Message}. Retrying ({retries} attempts left).", true);
        //         await Task.Delay(500);
        //         await Reset(retries, settingsForm);
        //     }
        // }

        public static void Set(Key key, string value)
        {
            Set(key.ToString(), value);
        }

        public static void Set(string str, string value)
        {
            Reload();
            cachedValues[str] = value;
            WriteConfig();
        }

        public static void Set(Dictionary<string, string> keyValuePairs)
        {
            Reload();

            foreach (KeyValuePair<string, string> entry in keyValuePairs)
                cachedValues[entry.Key] = entry.Value;

            WriteConfig();
        }

        private static void WriteConfig()
        {
            SortedDictionary<string, string> cachedValuesSorted = new SortedDictionary<string, string>(cachedValues);
            File.WriteAllText(configPath, JsonConvert.SerializeObject(cachedValuesSorted, Newtonsoft.Json.Formatting.Indented));
        }

        private static void Reload()
        {
            try
            {
                Dictionary<string, string> newDict = new Dictionary<string, string>();
                Dictionary<string, string> deserializedConfig = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(configPath));

                if (deserializedConfig == null)
                    deserializedConfig = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> entry in deserializedConfig)
                    newDict.Add(entry.Key, entry.Value);

                cachedValues = newDict; // Use temp dict and only copy it back if no exception was thrown
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to reload config! {e.Message}", true);
            }
        }

        // Get using fixed key
        public static string Get(Key key, string defaultVal)
        {
            WriteIfDoesntExist(key.ToString(), defaultVal);
            return Get(key);
        }

        // Get using string
        public static string Get(string key, string defaultVal)
        {
            WriteIfDoesntExist(key, defaultVal);
            return Get(key);
        }

        public static string Get(Key key, Type type = Type.String)
        {
            return Get(key.ToString(), type);
        }

        public static string Get(string key, Type type = Type.String)
        {
            string keyStr = key.ToString();

            try
            {
                if (cachedValues.ContainsKey(keyStr))
                    return cachedValues[keyStr];

                return WriteDefaultValIfExists(key.ToString(), type);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to get {keyStr.Wrap()} from config! {e.Message}");
            }

            return null;
        }

        #region Get Bool

        public static bool GetBool(Key key)
        {
            return Get(key, Type.Bool).GetBool();
        }

        public static bool GetBool(Key key, bool defaultVal = false)
        {
            WriteIfDoesntExist(key.ToString(), (defaultVal ? "True" : "False"));
            return Get(key, Type.Bool).GetBool();
        }

        public static bool GetBool(string key)
        {
            return Get(key, Type.Bool).GetBool();
        }

        public static bool GetBool(string key, bool defaultVal)
        {
            WriteIfDoesntExist(key.ToString(), (defaultVal ? "True" : "False"));
            return bool.Parse(Get(key, Type.Bool));
        }

        #endregion

        #region Get Int

        public static int GetInt(Key key)
        {
            return Get(key, Type.Int).GetInt();
        }

        public static int GetInt(Key key, int defaultVal)
        {
            WriteIfDoesntExist(key.ToString(), defaultVal.ToString());
            return GetInt(key);
        }

        public static int GetInt(string key)
        {
            return Get(key, Type.Int).GetInt();
        }

        public static int GetInt(string key, int defaultVal)
        {
            WriteIfDoesntExist(key.ToString(), defaultVal.ToString());
            return GetInt(key);
        }

        #endregion

        #region Get Float

        public static float GetFloat(Key key)
        {
            return Get(key, Type.Float).GetFloat();
        }

        public static float GetFloat(Key key, float defaultVal)
        {
            WriteIfDoesntExist(key.ToString(), defaultVal.ToStringDot());
            return Get(key, Type.Float).GetFloat();
        }

        public static float GetFloat(string key)
        {
            return Get(key, Type.Float).GetFloat();
        }

        public static float GetFloat(string key, float defaultVal)
        {
            WriteIfDoesntExist(key.ToString(), defaultVal.ToStringDot());
            return Get(key, Type.Float).GetFloat();
        }

        public static string GetFloatString(Key key)
        {
            return Get(key, Type.Float).Replace(",", ".");
        }

        public static string GetFloatString(string key)
        {
            return Get(key, Type.Float).Replace(",", ".");
        }

        #endregion

        static void WriteIfDoesntExist(string key, string val)
        {
            if (cachedValues.ContainsKey(key.ToString()))
                return;

            Set(key, val);
        }

        public enum Type { String, Int, Float, Bool }
        private static string WriteDefaultValIfExists(string keyStr, Type type)
        {
            Key key;

            try
            {
                key = (Key)Enum.Parse(typeof(Key), keyStr);
            }
            catch
            {
                return WriteDefault(keyStr, "");
            }

            if (key == Key.sliderSteps) return WriteDefault(key, "7");
            if (key == Key.sliderScale) return WriteDefault(key, "16");
            if (key == Key.upDownIterations) return WriteDefault(key, "4");

            return WriteDefault(key, "");
        }

        private static string WriteDefault(Key key, string def)
        {
            Set(key, def);
            return def;
        }

        private static string WriteDefault(string key, string def)
        {
            Set(key, def);
            return def;
        }

        public enum Key
        {
            sliderSteps,
            sliderScale,
            upDownIterations,
        }
    }
}
