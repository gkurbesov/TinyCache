using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public interface IMemoryCache<T> where T : class
    {
        /// <summary>
        /// Create or overwrite a cache entry.
        /// </summary>
        /// <param name="key">Key of cache entry</param>
        /// <returns></returns>
        ICacheEntry<T> CreateEntry(object key);
        /// <summary>
        /// Returns the item associated with this key, if any.
        /// </summary>
        /// <param name="key">Key of cache entry</param>
        /// <param name="value">value of cache</param>
        /// <returns></returns>
        bool TryGetValue(object key, out T value);
        /// <summary>
        /// Removes the object associated with the given key.
        /// </summary>
        /// <param name="key">key of cache entry</param>
        void Remove(object key);
    }
}
