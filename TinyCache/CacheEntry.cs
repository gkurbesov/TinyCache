using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public class CacheEntry<T> : ICacheEntry<T> where T : class
    {
        public CacheEntry(object key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }
        /// <summary>
        /// Key of cache item
        /// </summary>
        public object Key { get; private set; }
        /// <summary>
        /// The priority of keeping a cache entry during cleanup
        /// </summary>
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;
        /// <summary>
        /// The absolute expiration date for the cache entry.
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        /// <summary>
        ///  Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        ///  This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public TimeSpan? SlidingExpiration { get; set; }
        /// <summary>
        /// The value for the cache entry.
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// Expired flag
        /// </summary>
        public bool Expired { get; internal set; } = false;
        internal DateTimeOffset LastAccessed { get; set; }

        public bool CheckExpired(in DateTimeOffset now)
        {
            if (!Expired && !AbsoluteExpiration.HasValue && !SlidingExpiration.HasValue)
                return false;

            return FullCheck(now);

            bool FullCheck(in DateTimeOffset offset)
            {
                if (Expired) return true;
                
                if (AbsoluteExpiration.HasValue && AbsoluteExpiration.Value <= offset)
                {
                    Expired = true;
                    return true;
                }
                if (SlidingExpiration.HasValue && (offset - LastAccessed) >= SlidingExpiration)
                {
                    Expired = true;
                    return true;
                }
                return false;
            }
        }
    }
}
