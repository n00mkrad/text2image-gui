using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{
    internal class GetWebInfo
    {
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
                Logger.Log("Parsing Patrons from CSV...", true);
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

                Logger.Log($"Found {goldPatrons.Count} Gold Patrons, {silverPatrons.Count} Silver Patrons", true);

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
