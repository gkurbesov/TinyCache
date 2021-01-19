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

        public object Key { get; private set; }
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public T Value { get; set; }
        public bool Expired { get; set; } = false;


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
