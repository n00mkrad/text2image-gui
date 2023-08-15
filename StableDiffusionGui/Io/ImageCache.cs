using StableDiffusionGui.Main;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StableDiffusionGui.Io
{
    public class ImageCache
    {
        private static readonly ConcurrentDictionary<string, Image> _cache = new ConcurrentDictionary<string, Image>();
        private static string _lastPath = "";
        private static long MaxCacheSizeBytes { get { return Config.Instance.ImageCacheMaxSizeMb * 1024 * 1024; } }

        public static Image GetOrLoad(string path, Func<string, Image> loadFunc)
        {
            _lastPath = path;

            if (_cache.TryGetValue(path, out Image image))
                return image;

            return loadFunc(path); // If it's not in the cache, use the provided loading function to load the image
        }

        public static Image GetOrLoadAndStore (string path, Func<string,Image> addFunc)
        {
            _lastPath = path;
            ClearIfTooBig();
            return _cache.GetOrAdd(path, addFunc);
        }

        public static Image TryGet(string path)
        {
            _lastPath = path;

            if (_cache.TryGetValue(path, out Image image))
                return image;

            return null;
        }

        public static void Add(string path, Func<string, Image> addFunc)
        {
            _lastPath = path;
            ClearIfTooBig();
            _cache.GetOrAdd(path, addFunc);
        }

        public static bool Contains (string path)
        {
            return _cache.Keys.Contains(path) && _cache[path] != null;
        }

        public static bool TryRemoveImage(string path, int maxAttempts = 10)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                if (!_cache.ContainsKey(path))
                    return false;

                if (_cache.TryRemove(path, out Image image))
                {
                    image.Dispose();
                    return true;
                }
            }

            Logger.Log($"Failed to remove cached image: {path}", true);
            return false;
        }

        public static void Remove(string path)
        {
            TryRemoveImage(path);
        }

        public static void Remove(List<string> paths)
        {
            foreach (var path in paths)
                TryRemoveImage(path);
        }

        public static void Clear()
        {
            foreach (var pair in _cache)
                pair.Value.Dispose();

            _cache.Clear();
        }

        public static void ClearIfTooBig ()
        {
            long size = GetCacheSizeInBytes();

            // if (Program.Debug)
            //     Logger.Log($"Image Cache: {((float)size / 1024 / 1024).RoundToInt()}/{((float)MaxCacheSizeBytes / 1024 / 1024).RoundToInt()} MB in RAM", true);

            if (size > MaxCacheSizeBytes)
            {
                foreach (var path in _cache.Keys.Where(k => k != _lastPath))
                    TryRemoveImage(path);

                Logger.Log($"[Image Cache] {Config.Instance.ImageCacheMaxSizeMb} MB Size limit hit, cache cleared.", true);
            }
        }

        public static long GetCacheSizeInBytes()
        {
            long totalSize = 0;

            foreach (var pair in _cache)
            {
                Image image = pair.Value;
                int bitsPerPixel = Image.GetPixelFormatSize(image.PixelFormat);
                long imageSizeInBytes = (long)image.Width * image.Height * bitsPerPixel / 8;
                totalSize += imageSizeInBytes + 54; // Assumes 54 bytes of overhead, this number is based on tests
            }

            return totalSize;
        }
    }
}
