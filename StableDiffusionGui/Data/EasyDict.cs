using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StableDiffusionGui.Data
{
    public class EasyDict<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public EasyDict()
        {

        }

        public EasyDict(Dictionary<TKey, TValue> dictionary)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
                Add(kvp.Key, kvp.Value);
        }

        /// <returns> Value for requested key. Returns <paramref name="fallback"/> if the dictionary did not contain <paramref name="key"/>. </returns>
        public TValue Get(TKey key, TValue fallback = default)
        {
            if (base.ContainsKey(key))
                return base[key];
            else
                return fallback;
        }

        /// <returns> Value for requested key. Returns <paramref name="fallback"/> if the dictionary did not contain <paramref name="key"/> or if the value was null. </returns>
        public TValue GetNoNull(TKey key, TValue fallback = default)
        {
            if (!base.ContainsKey(key) || base[key] == null)
                return fallback;
            else
                return base[key];
        }

        /// <returns> Value for requested key. If the dictionary did not contain <paramref name="key"/> or if the value was null, the entry will get populated with <paramref name="populateObj"/> . </returns>
        public TValue GetPopulate(TKey key, TValue populateObj)
        {
            if (!base.ContainsKey(key) || base[key] == null)
                base[key] = populateObj;

            return base[key];
        }

        public void Set(TKey key, TValue value)
        {
            base[key] = value;
        }

        public void CloneTo(ref EasyDict<TKey, TValue> dict)
        {
            dict = new EasyDict<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> kvp in this)
                dict.Add(kvp.Key, kvp.Value);
        }

        public EasyDict<TKey, TValue> Clone()
        {
            var dict = new EasyDict<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> kvp in this)
                dict.Add(kvp.Key, kvp.Value);

            return dict;
        }

        public void Log ()
        {
            Console.WriteLine($"Dictionary Contents {this.Count}:");

            foreach (var kvp in this)
                Console.WriteLine($"{kvp.Key} | {kvp.Value}");
        }
    }
}
