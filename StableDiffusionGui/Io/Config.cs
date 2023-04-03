using StableDiffusionGui.Forms;
using System;
using System.IO;
using StableDiffusionGui.Main;
using System.Threading.Tasks;
using StableDiffusionGui.MiscUtils;
using Newtonsoft.Json;

namespace StableDiffusionGui.Io
{
    class Config
    {
        public static bool Ready = false;
        public static string ConfigPath = "";
        public static ConfigInstance Instance = null;

        public static void Init()
        {
            ConfigPath = Path.Combine(Paths.GetDataPath(), Constants.Files.Config);
            Load();
            Ready = true;
        }

        public static async Task Reset(int retries = 3, SettingsForm settingsForm = null)
        {
            try
            {
                if (settingsForm != null)
                    settingsForm.Enabled = false;

                File.Delete(ConfigPath);
                await Task.Delay(100);
                Instance = new ConfigInstance();
                await Task.Delay(100);

                if (settingsForm != null)
                    settingsForm.Enabled = true;
            }
            catch (Exception e)
            {
                retries -= 1;
                Logger.Log($"Failed to reset config: {e.Message}. Retrying ({retries} attempts left).", true);
                await Task.Delay(500);
                await Reset(retries, settingsForm);
            }
        }

        public static void Load ()
        {
            try
            {
                Instance = File.ReadAllText(ConfigPath).FromJson<ConfigInstance>(NullValueHandling.Ignore, DefaultValueHandling.Include, true, true);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }

            if(Instance == null)
            {
                if (File.Exists(ConfigPath))
                     IoUtils.TryMove(ConfigPath, ConfigPath.FilenameSuffix($".failedToLoad{FormatUtils.GetUnixTimestamp()}")); // Move out of the way but don't delete, for data restoration purposes

                Logger.Log("Can't load config from file. Creating new config instead.");
                Instance = new ConfigInstance();
            }
        }

        public static void Save ()
        {
            try
            {
                Instance.Clean();
                File.WriteAllText(ConfigPath, Instance.ToJson(true, true));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
