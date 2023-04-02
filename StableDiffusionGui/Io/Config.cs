using StableDiffusionGui.Forms;
using System;
using System.IO;
using StableDiffusionGui.Main;
using System.Threading.Tasks;

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
                string path = Path.Combine(Paths.GetDataPath(), "conf.json");
                Instance = File.ReadAllText(path).FromJson<ConfigInstance>();
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }

            if(Instance == null)
                Instance = new ConfigInstance();
        }

        public static void Save ()
        {
            try
            {
                string path = Path.Combine(Paths.GetDataPath(), "conf.json");
                Instance.Clean();
                File.WriteAllText(path, Instance.ToJson(true, true));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
