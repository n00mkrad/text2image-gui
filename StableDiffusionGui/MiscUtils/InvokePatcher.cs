using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZetaLongPaths;
using static System.Net.Mime.MediaTypeNames;

namespace StableDiffusionGui.MiscUtils
{
    internal class InvokePatcher
    {
        public static void Test()
        {
            string path = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo, "invoke");
            PatchCli(path);
            MiscPatches(path);
            // TODO: Patch pyproject.toml?
            Logger.Log("Done.");
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

                if (f.Name == "devices.py")
                    t = Replace(t, "MPS_DEVICE = torch.device(\"mps\")", "MPS_DEVICE = None");

                t = Replace(t, "if torch.backends.mps.is_available():", "if False:", false);

                if (f.Name == "cross_attention_control.py")
                    t = Replace(t, "diffusers.models.attention.CrossAttention, ", "diffusers.models.cross_attention.CrossAttention, ");

                if (f.Name == "textual_inversion_manager.py")
                {
                    t = Replace(t, "bin_file = self.hf_concepts_library.get_concept_model_path(concept_name)", "print(f\">> Embedding not found: {concept_name}\", flush=True); return");
                    t = Replace(t, "print(\">> Invalid embedding format\")", "print(f\">> Invalid embedding format: {os.path.basename(embedding_file)}\", flush=True)");
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

        private static void PatchCli (string rootPath)
        {
            string cliPath = Path.Combine(rootPath, "invokeai", "frontend", "CLI", "CLI.py");
            var lines = IoUtils.ReadLines(cliPath);

            bool indent = false;

            for(int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];

                if (l == "    while not done:")
                {
                    lines[i] = "    while not done:\n        try:";
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

        private static string Replace(string text, string searchFor, string replaceWith, bool warn = true)
        {
            string newText = text.Replace(searchFor, replaceWith);

            if (newText == text && warn)
                Logger.Log($"Invoke Patcher WARNING: Text has not changed after replacing '{searchFor}' with '{replaceWith}'.");

            return newText;
        }

        private static void ReplaceInFile(ZlpFileInfo file, string searchFor, string replaceWith)
        {
            if (searchFor.IsEmpty())
                return;

            string path = file.FullName;
            string text = File.ReadAllText(path);
            string newText = text.Replace(searchFor, replaceWith);

            if (newText != text)
                File.WriteAllText(path, newText);
        }

        public static void AddFlushParameters(string rootPath)
        {
            var pyFiles = IoUtils.GetFileInfosSorted(rootPath, true, "*.py");
            pyFiles.ToList().ForEach(f => AddFlushParameter(f.FullName));
        }

        public static void AddFlushParameterMultiLine(string filePath)
        {
            string pythonFileContent = File.ReadAllText(filePath);
            string pattern = @"( *)(print\()((?:.|\n)*?)(\))";

            string modifiedFileContent = Regex.Replace(pythonFileContent, pattern, match =>
            {
                string indentation = match.Groups[1].Value;
                string printStatement = match.Groups[2].Value;
                string content = match.Groups[3].Value;
                string closingParenthesis = match.Groups[4].Value;

                if (!content.Trim().EndsWith(","))
                {
                    content = content.TrimEnd() + ",";
                }

                return $"{indentation}{printStatement}{content} flush=True{closingParenthesis}";
            });

            modifiedFileContent = modifiedFileContent.Replace("print(, flush=True)", "print()");
            File.WriteAllText(filePath, modifiedFileContent);
        }

        public static void AddFlushParameter(string inputFilePath)
        {
            string[] lines = File.ReadAllLines(inputFilePath);
            using (StreamWriter outputFile = new StreamWriter(inputFilePath))
            {
                bool inPrint = false;
                bool hasParameters = false;
                bool inMultilineString = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.Contains("print("))
                    {
                        inPrint = true;
                        if (line.Contains(")"))
                        {
                            line = Regex.Replace(line, @"\)$", ", flush=True)");
                            inPrint = false;
                        }
                        else
                        {
                            hasParameters = line.Contains(",");
                            inMultilineString = line.Contains("\"\"\"") || line.Contains("'''");
                        }
                    }
                    else if (inPrint && !inMultilineString && line.Contains(")"))
                    {
                        inPrint = false;
                        if (hasParameters)
                        {
                            line = Regex.Replace(line, @"\)$", ", flush=True)");
                        }
                        else
                        {
                            line = Regex.Replace(line, @"\)$", "flush=True)");
                        }
                    }
                    else if (inPrint && inMultilineString)
                    {
                        if (line.Contains("\"\"\"") || line.Contains("'''"))
                        {
                            inMultilineString = false;
                        }
                        if (line.Contains(")"))
                        {
                            inPrint = false;
                            if (hasParameters)
                            {
                                line = Regex.Replace(line, @"\)$", ", flush=True)");
                            }
                            else
                            {
                                line = Regex.Replace(line, @"\)$", "flush=True)");
                            }
                        }
                    }
                    outputFile.WriteLine(line);
                }
            }
        }
    }
}
