using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache.Extensions
{
    public static class CacheEntryExtension
    {
        public static ICacheEntry<T> SetAbsoluteExpiration<T>(this ICacheEntry<T> entry, TimeSpan time) where T: class
        {
            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.Add(time);
            return entry;
        }

        public static ICacheEntry<T> SetSlidingExpiration<T>(this ICacheEntry<T> entry, TimeSpan time) where T : class
        {
            entry.SlidingExpiration = time;
            return entry;
        }

        public static ICacheEntry<T> SetPriority<T>(this ICacheEntry<T> entry, CacheItemPriority priority) where T : class
        {
            entry.Priority = priority;
            return entry;
        }

        public static ICacheEntry<T> SetValue<T>(this ICacheEntry<T> entry, T value) where T : class
        {
            entry.Value = value;
            return entry;
        }
    }
}
