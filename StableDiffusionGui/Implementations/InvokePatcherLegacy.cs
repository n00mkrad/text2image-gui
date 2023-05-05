using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System.IO;
using System.Linq;
using ZetaLongPaths;

namespace StableDiffusionGui.Implementations
{
    internal class InvokePatcherLegacy : IInvokePatcher
    {
        public void Run()
        {
            string path = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "invoke");
            PatchTiMgr(path);
            PatchCli(path);
            MiscPatches(path);
            // TODO: Patch pyproject.toml?
            Logger.Log("Done patching InvokeAI code.");
        }

        private static void MiscPatches(string rootPath)
        {
            foreach (var f in IoUtils.GetFileInfosSorted(rootPath, true, "*.py"))
            {
                string originalText = File.ReadAllText(f.FullName);
                string t = originalText;

                string printPatch = "import functools; print = functools.partial(print)";

                if (!t.StartsWith("print = ") && t.Contains("print") && !t.Contains(printPatch))
                {
                    if (t.Contains("from __future__ import annotations"))
                        t = Replace(t, "from __future__ import annotations", $"from __future__ import annotations\n{printPatch}");
                    else
                        t = $"{printPatch}\n{t}";
                }

                if (f.Name == "devices.py")
                    t = Replace(t, "MPS_DEVICE = torch.device(\"mps\")", "MPS_DEVICE = None");

                t = Replace(t, "if torch.backends.mps.is_available():", "if False:", false);

                if (f.Name == "cross_attention_control.py")
                    t = Replace(t, "diffusers.models.attention.CrossAttention, ", "diffusers.models.cross_attention.CrossAttention, ");

                if (f.Name == "args.py")
                    t = Replace(t, "rfc_dict[\"orig_hash\"] = calculate_init_img_hash(opt.init_img)", "rfc_dict[\"orig_hash\"] = 0", false);

                if (f.Name == "textual_inversion_manager.py")
                {
                    t = Replace(t, "bin_file = self.hf_concepts_library.get_concept_model_path(concept_name)", "print(f\">> Embedding not found: {concept_name}\"); return");
                    t = Replace(t, "print(\">> Invalid embedding format\")", "print(f\">> Invalid embedding format: {os.path.basename(embedding_file)}\")");
                    t = Replace(t, "return [ti.trigger_string for ti in self.textual_inversions]", "return [f\"{ti.trigger_string} from {self.trigger_to_sourcefile[ti.trigger_string]}\" for ti in self.textual_inversions]");
                }

                if (f.Name == "globals.py")
                {
                    t = Replace(t, "if os.environ.get(\"INVOKEAI_ROOT\"):", "if True:\n    import sys; Globals.root = osp.abspath(osp.join(sys.path[0], \"..\"));");
                    t = Replace(t, "Globals.root = osp.abspath(os.environ.get(\"INVOKEAI_ROOT\"))", "");
                }

                if (f.Name == "model_manager.py")
                {
                    // t = Replace(t, "def _cached_sha256(self, path, data) -> Union[str, bytes]:", "def _cached_sha256(self, path, data) -> Union[str, bytes]:\n        return 0");
                    t = Replace(t, "print(\"   | Calculating sha256 hash of model files\")", "return 0");
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

        private static void PatchSchedulers(string rootPath)
        {
            string cliPath = IoUtils.GetFileInfosSorted(rootPath, true, "generate.py").Where(f => f.Directory.Name == "backend").First().FullName;
            var lines = IoUtils.ReadLines(cliPath);

            if (lines.Any(l => l.Contains("nmkd patched")))
                return;

            string r = "        scheduler_map = dict(\n" +
                       "            ddim=(diffusers.DDIMScheduler, dict()),\n" +
                       "            plms=(diffusers.PNDMScheduler, dict()),\n" +
                       "            lms=(diffusers.LMSDiscreteScheduler, dict()),\n" +
                       "            heun=(diffusers.HeunDiscreteScheduler, dict()),\n" +
                       "            euler=(diffusers.EulerDiscreteScheduler, dict(use_karras_sigmas=False)),\n" +
                       "            k_euler=(diffusers.EulerDiscreteScheduler, dict(use_karras_sigmas=True)),\n" +
                       "            euler_a=(diffusers.EulerAncestralDiscreteScheduler, dict()),\n" +
                       "            dpm_2=(diffusers.KDPM2DiscreteScheduler, dict()),\n" +
                       "            dpm_2_a=(diffusers.KDPM2AncestralDiscreteScheduler, dict()),\n" +
                       "            dpmpp_2s=(diffusers.DPMSolverSinglestepScheduler, dict()),\n" +
                       "            dpmpp_2m=(diffusers.DPMSolverMultistepScheduler, dict(use_karras_sigmas=False)),\n" +
                       "            k_dpmpp_2m=(diffusers.DPMSolverMultistepScheduler, dict(use_karras_sigmas=True)),\n" +
                       "        )\n\n" +
                       "        scheduler_convert_map = {\n" +
                       "            \"k_lms\": \"lms\",\n" +
                       "            \"k_heun\": \"heun\",\n" +
                       "            \"k_euler_a\": \"euler_a\",\n" +
                       "            \"k_dpm_2\": \"dpm_2\",\n" +
                       "            \"k_dpm_2_a\": \"dpm_2_a\",\n" +
                       "            \"dpmpp_2\": \"dpmpp_2m\",\n" +
                       "            \"k_dpmpp_2\": \"k_dpmpp_2m\",\n" +
                       "        }\n\n" +
                       "        if self.sampler_name in scheduler_convert_map:\n" +
                       "            self.sampler_name = scheduler_convert_map[self.sampler_name]";

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
