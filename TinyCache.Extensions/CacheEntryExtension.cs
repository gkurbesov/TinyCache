using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache.Extensions
{
    public static class CacheEntryExtension
    {
        public static ICacheEntry SetAbsoluteExpiration(this ICacheEntry entry, TimeSpan time)
        {
            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.Add(time);
            return entry;
        }

        public static ICacheEntry SetSlidingExpiration(this ICacheEntry entry, TimeSpan time)
        {
            entry.SlidingExpiration = time;
            return entry;
        }

        public static ICacheEntry SetPriority(this ICacheEntry entry, CacheItemPriority priority)
        {
            entry.Priority = priority;
            return entry;
        }

        public static ICacheEntry SetValue(this ICacheEntry entry, object value)
        {
            entry.Value = value;
            return entry;
        }
    }
}
