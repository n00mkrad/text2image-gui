using StableDiffusionGui.Data;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{
    internal class GetWebInfo
    {
        public static async Task<string> LoadVersion()
        {
            try
            {
                string url = $"https://raw.githubusercontent.com/n00mkrad/text2image-gui/main/meta/version.json";
                string text = await new WebClient().DownloadStringTaskAsync(new Uri(url));
                var dict = text.FromJson<EasyDict<string, string>>();
                return dict.Get($"latest{Program.ReleaseChannel}Rel", "0.0.0");
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to load version info: {e.Message}", true);
                return "0.0.0";
            }
        }

        public static async Task<List<MdlRelease>> LoadReleases(bool useLocalFileIfDebug = true)
        {
            try
            {
                string text = "";

                if (Program.UserArgs.ContainsKey("custUpdJson") && Program.Debug) // Assuming exe dir is ...\text2image-gui\StableDiffusionGui\bin\x64\Debug
                {
                    string txtPath = Program.UserArgs.Get("custUpdJson").Trim('\"').Trim('\'');
                    text = File.ReadAllText(txtPath);
                }
                else
                {
                    string url = $"https://raw.githubusercontent.com/n00mkrad/text2image-gui/main/meta/versions.json";
                    text = await new WebClient().DownloadStringTaskAsync(new Uri(url));
                }

                List<EasyDict<string, string>> data = text.FromJson<List<EasyDict<string, string>>>();
                return data.ToList().Select(dict => new MdlRelease(dict)).ToList();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to load versions: {e.Message}", true);
                return new List<MdlRelease>();
            }
        }

        public static async Task LoadNews(Label newsLabel)
        {
            string text = "";

            try
            {
                string url = $"https://raw.githubusercontent.com/n00mkrad/text2image-gui/main/changelog-motd.txt";
                text = await new WebClient().DownloadStringTaskAsync(new Uri(url));
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to load news: {e.Message}", true);
            }

            newsLabel.Text = string.IsNullOrWhiteSpace(text) ? "Failed to load." : text;
        }

        public static async Task LoadPatronListCsv(Label patronsLabel)
        {
            string text = "";

            try
            {
                string url = $"https://raw.githubusercontent.com/n00mkrad/flowframes/main/patrons.csv";
                string csvData = await new WebClient().DownloadStringTaskAsync(new Uri(url));
                text = ParsePatreonCsv(csvData);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to load patreon CSV: {e.Message}", true);
            }

            patronsLabel.Text = string.IsNullOrWhiteSpace(text) ? "Failed to load." : text;
        }

        public static string ParsePatreonCsv(string csvData)
        {
            try
            {
                List<string> goldPatrons = new List<string>();
                List<string> silverPatrons = new List<string>();
                string str = "Gold:\n";
                string[] lines = csvData.SplitIntoLines().Select(x => x.Replace(";", ",")).ToArray();

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] values = line.Split(',');
                    if (i == 0 || line.Length < 10 || values.Length < 5) continue;
                    string name = values[0].Trim();
                    string status = values[4].Trim();
                    string tier = values[9].Trim();

                    if (status.Contains("Active"))
                    {
                        if (tier.Contains("Gold"))
                            goldPatrons.Add(name.Trunc(30));

                        if (tier.Contains("Silver"))
                            silverPatrons.Add(name.Trunc(30));
                    }
                }

                foreach (string pat in goldPatrons)
                    str += pat + "\n";

                str += "\nSilver:\n";

                foreach (string pat in silverPatrons)
                    str += pat + "\n";

                return str;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to parse Patreon CSV: {e.Message}\n{e.StackTrace}", true);
                return "Failed to load patron list.";
            }
        }
    }
}
