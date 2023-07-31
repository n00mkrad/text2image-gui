using StableDiffusionGui.Io;
using System.Linq;
using StableDiffusionGui.Main;
using System.IO;
using StableDiffusionGui.Forms;
using System.Windows.Forms;

namespace StableDiffusionGui.Implementations
{
    internal class ComfyPatcher
    {
        public void Run()
        {
            var form = new PromptForm("Enter Path", "Enter path to repo root", "", 1.5f, 1f);
            form.ShowDialog();

            if (form.DialogResult != DialogResult.OK)
                return;

            string path = form.EnteredText.Trim();
            MiscPatches(path);
            Logger.Log("Done patching ComfyUI code.");
        }

        private static void MiscPatches(string rootPath)
        {
            foreach (var f in IoUtils.GetFileInfosSorted(rootPath, true, "*.py"))
            {
                string originalText = File.ReadAllText(f.FullName);
                string t = originalText;

                string printPatch = "import functools; print = functools.partial(print, flush=True)";

                if (!t.StartsWith("print = ") && t.Contains("print") && !t.Contains(printPatch))
                {
                    if (t.Contains("from __future__ import annotations"))
                        t = Replace(t, "from __future__ import annotations", $"from __future__ import annotations\n{printPatch}");
                    else
                        t = $"{printPatch}\n{t}";
                }

                if (t != originalText)
                {
                    Logger.Log($"Patched {f.Name}.");
                    File.WriteAllText(f.FullName, t);
                }
            }
        }

        private static string Replace(string text, string searchFor, string replaceWith, bool warn = true)
        {
            string newText = text.Replace(searchFor, replaceWith);

            if (newText == text && warn)
                Logger.Log($"Invoke Patcher WARNING: Text has not changed after replacing '{searchFor}' with '{replaceWith}'.");

            return newText;
        }
    }
}
