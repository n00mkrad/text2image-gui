using StableDiffusionGui.Io;
using System.Linq;
using StableDiffusionGui.Main;
using System.IO;
using StableDiffusionGui.Forms;
using System.Windows.Forms;

namespace StableDiffusionGui.Implementations
{
    internal class InvokePatcher235 : IInvokePatcher
    {
        public void Run()
        {
            var form = new PromptForm("Enter Path", "Enter path to repo root", "", 1.5f, 1f);
            form.ShowDialog();

            if (form.DialogResult != DialogResult.OK)
                return;

            string path = form.EnteredText.Trim();
            IoUtils.DeleteIfExists(Path.Combine(path, "build")); // Delete build folder
            PatchTiMgr(path);
            PatchCli(path);
            MiscPatches(path);
            Logger.Log("Done patching InvokeAI code.");
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

                if (f.Name == "globals.py")
                {
                    t = Replace(t, "    '''\n    home: ", "    '''\n    return Path(osp.abspath(osp.join(os.environ.get('INVOKEAI_ROOT'), '..', 'cache', 'hf')),\"\") \n    home: ");
                }

                if (f.Name == "args.py")
                    t = Replace(t, "rfc_dict[\"orig_hash\"] = calculate_init_img_hash(opt.init_img)", "rfc_dict[\"orig_hash\"] = 0", false);

                if (f.Name == "textual_inversion_manager.py")
                {
                    t = Replace(t, "bin_file = self.hf_concepts_library.get_concept_model_path(concept_name)", "print(f\">> Embedding not found: {concept_name}\"); return");
                    t = Replace(t, "print(\">> Invalid embedding format\")", "print(f\">> Invalid embedding format: {os.path.basename(embedding_file)}\")");
                    t = Replace(t, "return [ti.trigger_string for ti in self.textual_inversions]", "return [f\"{ti.trigger_string} from {self.trigger_to_sourcefile[ti.trigger_string]}\" for ti in self.textual_inversions]");
                }

                if (f.Name == "model_manager.py")
                {
                    t = Replace(t, "print(\"   | Calculating sha256 hash of model files\")", "return 0");
                    t = Replace(t, "legacy_layout = False", "return"); // Disable legacy cache layout check as it seems completely unnecessary
                }

                if (f.Name == "generate.py")
                {
                    t = Replace(t, "print(\"** trying to reload previous model\")", "pass");
                    t = Replace(t, "model_data = cache.get_model(previous_model_name)", "model_data = None");
                    t = Replace(t, "assert cfg_scale > 1.0, \"CFG_Scale (-C) must be >1.0\"", "pass # disabled cfg assert");
                    t = Replace(t, "x % 64", "x % 8");
                    t = Replace(t, "image, seed, _ = r", "image, seed, _ = r if len(r) == 3 else (r[0], r[1], None)");
                }

                if (f.Name == "CLI.py")
                {
                    t = Replace(t, "print(f'** An error occurred while attempting to initialize the model: \"{str(e)}\"')", "print(f'** An error occurred while attempting to initialize the model: \"{e}\"'); return");
                    t = Replace(t, "completer.add_history(command)", "pass");
                }

                if (f.Name == "util.py")
                    t = Replace(t, "x % 64", "x % 8");

                if (f.Name == "concepts_lib.py")
                {
                    t = Replace(t, "f\"{concept_name} is not a local embedding trigger, nor is it a HuggingFace concept. Generation will continue without the concept.\"", "f\"Not a valid embedding trigger: {concept_name}\"");
                }

                if (t != originalText)
                {
                    Logger.Log($"Patched {f.Name}.");
                    File.WriteAllText(f.FullName, t);
                }
            }
        }

        private static void PatchCli(string rootPath)
        {
            string cliPath = IoUtils.GetFileInfosSorted(rootPath, true, "CLI.py").First().FullName;
            var lines = IoUtils.ReadLines(cliPath);

            if (lines.Any(l => l.Contains("nmkd patched")))
                return;

            bool indent = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];

                if (l == "    while not done:")
                {
                    lines[i] = "    while not done:\n        try: # nmkd patched";
                    indent = true;
                    continue;
                }

                if (l == "        print()")
                {
                    lines[i] = "            print()\n        except KeyboardInterrupt:\n            pass";
                    indent = false;
                    continue;
                }

                if (indent)
                    lines[i] = "    " + l;

                if (l.StartsWith("def do_command"))
                    break;
            }

            File.WriteAllLines(cliPath, lines);
        }

        private static void PatchTiMgr(string rootPath)
        {
            string cliPath = IoUtils.GetFileInfosSorted(rootPath, true, "textual_inversion_manager.py").First().FullName;

            var lines = IoUtils.ReadLines(cliPath);

            if (lines.Any(l => l.Contains("nmkd patched")))
                return;

            bool v4Passed = false;
            bool indent = false;
            string currentIndent = "";


            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];

                if (l.Trim() == "print(f'   | Loading v4 embedding file: {short_path}')")
                {
                    currentIndent = new string(l.TakeWhile(c => c == ' ').ToArray());
                    lines[i] = $"{l} # nmkd patched\n{currentIndent}try:";
                    indent = true;
                    v4Passed = true;
                    continue;
                }

                if (v4Passed && l.Trim() == "return embeddings")
                {
                    lines[i] = $"    {currentIndent}return embeddings\n{currentIndent}except:\n{currentIndent}    print(f\"   ** Invalid embeddings file: {{short_path}}\")\n{currentIndent}    return list()";
                    break;
                }

                if (indent)
                    lines[i] = "    " + l;
            }

            File.WriteAllLines(cliPath, lines);
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
