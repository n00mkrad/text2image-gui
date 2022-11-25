using Force.Crc32;
using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZetaLongPaths;

namespace StableDiffusionGui.Io
{
    internal class IoUtils
    {
        public static Image GetImage(string path)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    return Image.FromStream(stream);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load image from {path}: {ex.Message}", true);
                return Resources.imgNotFound;
            }
        }

        public static string[] ReadLines(string path)
        {
            List<string> lines = new List<string>();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);
            }

            return lines.ToArray();
        }

        public static bool IsPathDirectory(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            path = path.Trim();

            if (Directory.Exists(path))
                return true;

            if (File.Exists(path))
                return false;

            if (new string[2] { "\\", "/" }.Any((string x) => path.EndsWith(x)))
                return true;

            return string.IsNullOrWhiteSpace(Path.GetExtension(path));
        }

        public static bool IsFileValid(string path)
        {
            if (path == null)
                return false;

            if (!File.Exists(path))
                return false;

            return true;
        }

        public static bool IsDirValid(string path)
        {
            if (path == null)
                return false;

            if (!Directory.Exists(path))
                return false;

            return true;
        }

        public static bool IsPathValid(string path)
        {
            if (path == null)
                return false;

            if (IsPathDirectory(path))
                return IsDirValid(path);
            else
                return IsFileValid(path);
        }

        public static void CopyDir(string sourceDirectory, string targetDirectory, bool move = false)
        {
            Directory.CreateDirectory(targetDirectory);
            ZlpDirectoryInfo source = new ZlpDirectoryInfo(sourceDirectory);
            ZlpDirectoryInfo target = new ZlpDirectoryInfo(targetDirectory);
            CopyWork(source, target, move);
        }

        private static void CopyWork(ZlpDirectoryInfo source, ZlpDirectoryInfo target, bool move)
        {
            ZlpDirectoryInfo[] directories = source.GetDirectories();
            foreach (ZlpDirectoryInfo directoryInfo in directories)
            {
                CopyWork(directoryInfo, target.CreateSubdirectory(directoryInfo.Name), move);
            }
            ZlpFileInfo[] files = source.GetFiles();
            foreach (ZlpFileInfo fileInfo in files)
            {
                if (move)
                    fileInfo.MoveTo(Path.Combine(target.FullName, fileInfo.Name));
                else
                    fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
            }
        }

        /// <summary> Delete everything inside a directory except the dir itself. </summary>
        public static bool DeleteContentsOfDir(string path)
        {
            try
            {
                DeleteIfExists(path);
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("DeleteContentsOfDir Error: " + e.Message, true);
                return false;
            }
        }

        public static void ReplaceInFilenamesDir(string dir, string textToFind, string textToReplace, bool recursive = true, string wildcard = "*")
        {
            int counter = 1;
            var dirInfo = new ZlpDirectoryInfo(dir);
            ZlpFileInfo[] files;

            if (recursive)
                files = dirInfo.GetFiles(wildcard, SearchOption.AllDirectories);
            else
                files = dirInfo.GetFiles(wildcard, SearchOption.TopDirectoryOnly);

            foreach (ZlpFileInfo file in files)
            {
                ReplaceInFilename(file.FullName, textToFind, textToReplace);
                counter++;
            }
        }

        public static void ReplaceInFilename(string path, string textToFind, string textToReplace)
        {
            string ext = Path.GetExtension(path);
            string newFilename = Path.GetFileNameWithoutExtension(path).Replace(textToFind, textToReplace);
            string targetPath = Path.Combine(Path.GetDirectoryName(path), newFilename + ext);
            if (File.Exists(targetPath))
            {
                //Program.Print("Skipped " + path + " because a file with the target name already exists.");
                return;
            }
            File.Move(path, targetPath);
        }

        public static int GetAmountOfFiles(string path, bool recursive, string wildcard = "*")
        {
            try
            {
                var dirInfo = new ZlpDirectoryInfo(path);
                return dirInfo.GetFiles(wildcard, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Length;
            }
            catch
            {
                return 0;
            }
        }

        static bool TryCopy(string source, string target, bool overwrite = true, bool showLog = false)
        {
            try
            {
                File.Copy(source, target, overwrite);
            }
            catch (Exception e)
            {
                if (showLog)
                    Logger.Log($"Failed to move '{source}' to '{target}' (Overwrite: {overwrite}): {e.Message}, !showLog");

                return false;
            }

            return true;
        }

        public static bool TryMove(string source, string target, bool overwrite = true, bool showLog = false)
        {
            try
            {
                if (overwrite && File.Exists(target))
                    File.Delete(target);

                File.Move(source, target);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to move '{source}' to '{target}' (Overwrite: {overwrite}): {e.Message}", !showLog);
                return false;
            }

            return true;
        }

        /// <summary>
		/// Async (background thread) version of TryDeleteIfExists. Safe to run without awaiting.
		/// </summary>
		public static async Task<bool> TryDeleteIfExistsAsync(string path, int retries = 10)
        {
            string renamedPath = path;

            try
            {
                if (IsPathDirectory(path))
                {
                    while (Directory.Exists(renamedPath))
                        renamedPath += "_";

                    if (path != renamedPath)
                        Directory.Move(path, renamedPath);
                }
                else
                {
                    while (File.Exists(renamedPath))
                        renamedPath += "_";

                    if (path != renamedPath)
                        File.Move(path, renamedPath);
                }

                path = renamedPath;

                bool returnVal = await Task.Run(async () => { return TryDeleteIfExists(path); });
                return returnVal;
            }
            catch (Exception e)
            {
                Logger.Log($"TryDeleteIfExistsAsync Move Exception: {e.Message} [{retries} retries left]", true);

                if (retries > 0)
                {
                    await Task.Delay(2000);
                    retries -= 1;
                    return await TryDeleteIfExistsAsync(path, retries);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete a path if it exists. Works for files and directories. Returns success status.
        /// </summary>
        public static bool TryDeleteIfExists(string path)      // Returns true if no exception occurs
        {
            try
            {
                if ((IsPathDirectory(path) && !Directory.Exists(path)) || (!IsPathDirectory(path) && !File.Exists(path)))
                    return true;

                DeleteIfExists(path);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"TryDeleteIfExists: Error trying to delete {path}: {e.Message}", true);
                return false;
            }
        }

        public static bool DeleteIfExists(string path, bool log = false)		// Returns true if the file/dir exists
        {
            if (log)
                Logger.Log($"DeleteIfExists({path})", true);

            if (!IsPathDirectory(path) && File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            if (IsPathDirectory(path) && Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add ".old" suffix to an existing file to avoid it getting overwritten. If one already exists, it will be ".old.old" etc.
        /// </summary>
        public static void RenameExistingFile(string path)
        {
            if (!File.Exists(path))
                return;

            try
            {
                string ext = Path.GetExtension(path);
                string renamedPath = path;

                while (File.Exists(renamedPath))
                    renamedPath = Path.ChangeExtension(renamedPath, null) + ".old" + ext;

                File.Move(path, renamedPath);
            }
            catch (Exception e)
            {
                Logger.Log($"RenameExistingFile: Failed to rename '{path}': {e.Message}", true);
            }
        }

        /// <summary>
        /// Add ".old" suffix to an existing folder to avoid it getting overwritten. If one already exists, it will be ".old.old" etc.
        /// </summary>
        public static void RenameExistingFolder(string path)
        {
            if (!Directory.Exists(path))
                return;

            try
            {
                string renamedPath = path;

                while (Directory.Exists(renamedPath))
                    renamedPath += ".old";

                Directory.Move(path, renamedPath);
            }
            catch (Exception e)
            {
                Logger.Log($"RenameExistingFolder: Failed to rename '{path}': {e.Message}", true);
            }
        }

        /// <summary>
        /// Easily rename a file without needing to specify the full move path
        /// </summary>
        public static bool RenameFile(string path, string newName, bool alsoRenameExtension = false)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                string ext = Path.GetExtension(path);
                string movePath = Path.Combine(dir, newName);

                if (!alsoRenameExtension)
                    movePath += ext;

                File.Move(path, movePath);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to rename '{path}' to '{newName}': {e.Message}", true);
                return false;
            }
        }

        public static string FilenameSuffix(string path, string suffix)
        {
            try
            {
                string ext = Path.GetExtension(path);
                return Path.Combine(path.GetParentDirOfFile(), $"{Path.GetFileNameWithoutExtension(path)}{suffix}{ext}");
            }
            catch
            {
                return path;
            }
        }

        // public enum ErrorMode { HiddenLog, VisibleLog, Messagebox }
        // public static bool CanWriteToDir(string dir, ErrorMode errMode)
        // {
        //     string tempFile = Path.Combine(dir, "flowframes-testfile.tmp");
        //     try
        //     {
        //         File.Create(tempFile);
        //         File.Delete(tempFile);
        //         return true;
        //     }
        //     catch (Exception e)
        //     {
        //         // Logger.Log($"Can't write to {dir}! {e.Message}", errMode == ErrorMode.HiddenLog);
        //         if (errMode == ErrorMode.Messagebox && !BatchProcessing.busy)
        //             UiUtils.ShowMessageBox($"Can't write to {dir}!\n\n{e.Message}", UiUtils.MessageType.Error);
        //         return false;
        //     }
        // }

        public static bool CopyTo(string file, string targetFolder, bool overwrite = true)
        {
            string targetPath = Path.Combine(targetFolder, Path.GetFileName(file));
            try
            {
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);
                File.Copy(file, targetPath, overwrite);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to copy {file} to {targetFolder}: {e.Message}");
                return false;
            }
        }

        public static bool MoveTo(string file, string targetFolder, bool overwrite = true)
        {
            string targetPath = Path.Combine(targetFolder, Path.GetFileName(file));
            try
            {
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);
                if (overwrite)
                    DeleteIfExists(targetPath);
                File.Move(file, targetPath);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to move {file} to {targetFolder}: {e.Message}");
                return false;
            }
        }

        public static bool CreateDir(string path)		// Returns whether the dir already existed
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }

            return true;
        }

        public static string[] GetFilesSorted(string path, bool recursive = false, string pattern = "*")
        {
            try
            {
                if (path == null || !Directory.Exists(path))
                    return new string[0];

                SearchOption opt = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                return Directory.GetFiles(path, pattern, opt).OrderBy(x => Path.GetFileName(x)).ToArray();
            }
            catch (Exception ex)
            {
                Logger.Log($"GetFilesSorted error: {ex.Message}", true);
                return new string[0];
            }
        }

        public static string[] GetFilesSorted(string path, string pattern = "*")
        {
            return GetFilesSorted(path, false, pattern);
        }

        public static string[] GetFilesSorted(string path)
        {
            return GetFilesSorted(path, false, "*");
        }

        public static ZlpFileInfo[] GetFileInfosSorted(string path, bool recursive = false, string pattern = "*")
        {
            try
            {
                if (path == null || !Directory.Exists(path))
                    return new ZlpFileInfo[0];


                SearchOption opt = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var dir = new ZlpDirectoryInfo(path);
                return dir.GetFiles(pattern, opt).OrderBy(x => x.Name).ToArray();
            }
            catch (Exception ex)
            {
                Logger.Log($"GetFileInfosSorted error: {ex.Message}");
                return new ZlpFileInfo[0];
            }
        }

        public static bool CreateFileIfNotExists(string path)
        {
            if (File.Exists(path))
                return false;

            try
            {
                File.Create(path).Close();
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to create file at '{path}': {e.Message}", true);
                return false;
            }
        }

        public static long GetDirSize(string path, bool recursive, string[] extensionWhitelist = null, bool includeFilesWithoutExt = false)
        {
            long size = 0;

            try
            {
                string[] files = Directory.GetFiles(path, includeFilesWithoutExt ? "*" : "*.*");

                if (extensionWhitelist != null)
                    files = files.Where(file => extensionWhitelist.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase))).ToArray();

                foreach (string file in files)
                {
                    try { size += new ZlpFileInfo(file).Length; } catch { size += 0; }
                }

                if (!recursive)
                    return size;

                foreach (ZlpDirectoryInfo dir in new ZlpDirectoryInfo(path).GetDirectories()) // Add subdirectory sizes
                    size += GetDirSize(dir.FullName, true, extensionWhitelist, includeFilesWithoutExt);
            }
            catch (Exception e)
            {
                Logger.Log($"GetDirSize Error: {e.Message}\n{e.StackTrace}", true);
            }

            return size;
        }

        public static long GetFilesize(string path)
        {
            try
            {
                return new ZlpFileInfo(path).Length;
            }
            catch
            {
                return -1;
            }
        }

        public static string GetFilesizeStr(string path)
        {
            try
            {
                return FormatUtils.Bytes(GetFilesize(path));
            }
            catch
            {
                return "?";
            }
        }

        public static void OverwriteFileWithText(string path, string text = "THIS IS A DUMMY FILE - DO NOT DELETE ME")
        {
            try
            {
                File.WriteAllText(path, text);
            }
            catch (Exception e)
            {
                Logger.Log($"OverwriteWithText failed for '{path}': {e.Message}", true);
            }
        }

        public static long GetDiskSpace(string path, bool mbytes = true)
        {
            try
            {
                string driveLetter = path.Substring(0, 2);      // Make 'C:/some/random/path' => 'C:' etc
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady && d.Name.StartsWith(driveLetter))
                    {
                        if (mbytes)
                            return (long)(d.AvailableFreeSpace / 1024f / 1000f);
                        else
                            return d.AvailableFreeSpace;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("Error trying to get disk space: " + e.Message, true);
            }

            return 0;
        }

        public static string[] GetUniqueExtensions(string path, bool recursive = false)
        {
            ZlpFileInfo[] fileInfos = GetFileInfosSorted(path, recursive);
            List<string> exts = fileInfos.Select(x => x.Extension).ToList();
            return exts.Select(x => x).Distinct().ToArray();
        }

        public static ImageMetadata GetImageMetadata(string path)
        {
            return new ImageMetadata(path);
        }

        public static void SetImageMetadata(string imgPath, string text, string keyName = "")
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            text = text.Replace("\"", "\\\""); // Escape quotation marks
            Process p = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            p.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && call activate.bat {Constants.Dirs.Conda}/envs/{Constants.Dirs.SdEnv} && " +
                $"python {Constants.Dirs.SdRepo}/addmetadata.py -i {imgPath.Wrap()} -t {text.Wrap()} {(string.IsNullOrWhiteSpace(keyName) ? "" : $"-k {keyName}")}";
            p.Start();
            p.WaitForExit();
        }

        public static bool SetAttributes(string rootDir, ZetaLongPaths.Native.FileAttributes newAttributes, bool recursive = true)
        {
            try
            {
                GetFileInfosSorted(rootDir, recursive).ToList().ForEach(x => x.Attributes = newAttributes);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsFileLocked(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            return IsFileLocked(new FileInfo(filePath));
        }

        public static bool IsFileLocked(ZlpFileInfo file)
        {
            if (file.FullName.Length <= 256)
            {
                return IsFileLocked(new FileInfo(file.FullName));
            }
            else
            {
                ShowPathLengthErr(file.FullName);
                return false;
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            try
            {
                if (file == null)
                    return false;

                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        public static Stream StringToStream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public enum Hash { MD5, CRC32 }
        public static string GetHash(string pathOrString, Hash hashType, bool log = true)
        {
            string hashStr = "";
            NmkdStopwatch sw = new NmkdStopwatch();

            if (Directory.Exists(pathOrString))
            {
                Logger.Log($"Path '{pathOrString}' is directory! Returning empty hash.", true);
                return hashStr;
            }
            try
            {
                var stream = File.Exists(pathOrString) ? File.OpenRead(pathOrString) : StringToStream(pathOrString);

                if (hashType == Hash.MD5)
                {
                    MD5 md5 = MD5.Create();
                    var hash = md5.ComputeHash(stream);
                    hashStr = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }

                if (hashType == Hash.CRC32)
                {
                    var crc = new Crc32Algorithm();
                    var crc32bytes = crc.ComputeHash(stream);
                    hashStr = BitConverter.ToUInt32(crc32bytes, 0).ToString();
                }

                stream.Close();
            }
            catch (Exception e)
            {
                Logger.Log($"Error getting file hash for {Path.GetFileName(pathOrString)}: {e.Message}", true);
                return "";
            }

            if (log)
                Logger.Log($"Computed {hashType} for '{Path.GetFileNameWithoutExtension(pathOrString).Trunc(40) + Path.GetExtension(pathOrString)}' ({GetFilesizeStr(pathOrString)}): {hashStr} ({sw})", true);

            return hashStr;
        }

        public static string GetPseudoHash(string path)
        {
            return GetPseudoHash(new ZlpFileInfo(path));
        }

        public static string GetPseudoHash(ZlpFileInfo f, bool crc32 = true)
        {
            if (crc32)
                return GetHash($"{f.FullName}{f.Length}", Hash.CRC32, false);
            else
                return $"{f.FullName}{f.Length}";
        }

        public static void ShowPathLengthErr(string path, bool cancel = true)
        {
            Task.Run(() => UiUtils.ShowMessageBox($"The path of the following file is too long!\n\n{path} ({path.Length} characters)\n\n" +
                $"For compatibility reasons, please ensure all file paths are no longer than 256 characters!"));

            if (cancel)
                TextToImage.Cancel("File with invalid path detected.", false);
        }
    }
}
