using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TinyCache.Extensions
{
    public static class MemoryCacheExtension
    {
        public static T GetOrCreate<T>(this IMemoryCache<T> cache, object key, Func<T> factory) where T : class
        {
            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                var newEntryValue = factory();
                var entry = cache.CreateEntry(key);
                entry.Value = newEntryValue;
                return newEntryValue;
            }
        }

        public static T GetOrCreate<T>(this IMemoryCache<T> cache, object key, Func<ICacheEntry<T>, T> factory) where T : class
        {
            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                var entry = cache.CreateEntry(key);
                var newEntryValue = factory(entry);
                entry.Value = newEntryValue;
                return newEntryValue;
            }
        }

        public static T FirstOrDefault<T>(this IMemoryCache<T> cache, Func<T, bool> predicate) where T : class
        {
            var collection = cache.GetCacheCollection();
            foreach(var entry in collection)
            {
                if(entry.Value != null)
                {
                    if (predicate(entry.Value))
                        return entry.Value;
                }
            }
            return default;
        }

        public static IEnumerable<T> FindAll<T>(this IMemoryCache<T> cache, Func<T, bool> predicate) where T : class
        {
            List<T> bag = new List<T>();
            var collection = cache.GetCacheCollection();
            foreach (var entry in collection)
            {
                if (entry.Value != null)
                {
                    if (predicate(entry.Value))
                        bag.Add(entry.Value);
                }
            }
            return bag;
        }
    }
}
