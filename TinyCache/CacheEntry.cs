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
        /// The value for the cache entry.
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// Expired flag
        /// </summary>
        public bool Expired { get; internal set; } = false;

        public bool CheckExpired(in DateTimeOffset now)
        {
            if (!Expired && !AbsoluteExpiration.HasValue)
                return false;

            return FullCheck(now);

            bool FullCheck(in DateTimeOffset offset)
            {
                if (Expired)
                {
                    return true;
                }
                else if (AbsoluteExpiration.HasValue && AbsoluteExpiration.Value <= offset)
                {
                    Expired = true;
                    return true;
                }
                return false;
            }
        }
    }
}
