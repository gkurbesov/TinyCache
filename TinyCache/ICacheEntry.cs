﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public interface ICacheEntry
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
        ///  Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        ///  This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        TimeSpan? SlidingExpiration { get; set; }
        /// <summary>
        /// Gets or sets the value for the cache entry.
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// Expired flag
        /// </summary>
        bool Expired { get; set; }
        /// <summary>
        /// Last access time
        /// </summary>
        DateTimeOffset LastAccessed { get; set; }
        /// <summary>
        /// Expiration check
        /// </summary>
        /// <param name="now">current date and time</param>
        /// <returns></returns>
        bool CheckExpired(in DateTimeOffset now);
    }
}
