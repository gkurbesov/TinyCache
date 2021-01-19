using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public interface ICacheEntry<T> where T : class
    {
        /// <summary>
        /// Key of cache item
        /// </summary>
        object Key { get; }
        /// <summary>
        /// Gets or sets the priority of keeping a cache entry during cleanup
        /// </summary>
        CacheItemPriority Priority { get; set; }
        /// <summary>
        /// Gets or sets the absolute expiration date for the cache entry.
        /// </summary>
        DateTimeOffset? AbsoluteExpiration { get; set; }
        /// <summary>
        /// Gets or sets the value for the cache entry.
        /// </summary>
        T Value { get; set; }
    }
}
