using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public interface IMemoryCache
    {
        /// <summary>
        /// Create or overwrite a cache entry.
        /// </summary>
        /// <param name="key">Key of cache entry</param>
        /// <returns></returns>
        ICacheEntry CreateEntry(object key);
        /// <summary>
        /// Set an entity into the cache with the specified key
        /// </summary>
        /// <param name="key">Key of cache entry</param>
        /// <param name="entry">Entity instance</param>
        /// <returns></returns>
        bool TrySet(object key, ICacheEntry entry);
        /// <summary>
        /// Checks for a key in the cache
        /// </summary>
        /// <param name="key">Key of cache entry</param>
        /// <returns></returns>
        bool ContainsKey(object key);
        /// <summary>
        /// Returns the item associated with this key, if any.
        /// </summary>
        /// <param name="key">Key of cache entry</param>
        /// <param name="value">value of cache</param>
        /// <returns></returns>
        bool TryGetValue(object key, out object value);
        /// <summary>
        /// Returns the cache entry associated with this key, if any.
        /// </summary>
        /// <param name="key">Key of cache entry</param>
        /// <param name="entry">entry of cache</param>
        /// <returns></returns>
        bool TryGetEntry(object key, out ICacheEntry entry);
        /// <summary>
        /// Returns a collection of cache entities
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICacheEntry> GetCacheCollection();
        /// <summary>
        /// Removes the object associated with the given key.
        /// </summary>
        /// <param name="key">key of cache entry</param>
        void Remove(object key);
    }
}
