using System;
using System.Collections.Generic;
using System.Linq;

namespace StableDiffusionGui.Data
{
    public class EasyDict<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <returns> Value for requested key. Returns <paramref name="fallback"/> if the dictionary did not contain <paramref name="key"/>. </returns>
        public TValue Get(TKey key, TValue fallback)
        {
            if (base.ContainsKey(key))
                return base[key];
            else
                return fallback;
        }

        /// <returns> Value for requested key. Returns <paramref name="fallback"/> if the dictionary did not contain <paramref name="key"/> or if the value was null. </returns>
        public TValue GetNoNull(TKey key, TValue fallback)
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
    }
}
