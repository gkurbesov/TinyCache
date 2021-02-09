using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TinyCache.Extensions
{
    public static class MemoryCacheExtension
    {
        public static object GetOrCreate(this IMemoryCache cache, object key, Func<object> factory)
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

        public static object GetOrCreate(this IMemoryCache cache, object key, Action<ICacheEntry> factory)
        {
            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                var entry = cache.CreateEntry(key);
                factory(entry);
                return entry.Value;
            }
        }

        public static async Task<object> GetOrCreateAsync(this IMemoryCache cache, object key, Func<ICacheEntry, Task> factory)
        {
            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                var entry = cache.CreateEntry(key);
                await factory(entry);
                return entry.Value;
            }
        }

        public static object FirstOrDefault(this IMemoryCache cache, Func<object, bool> predicate)
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

        public static IEnumerable<object> FindAll(this IMemoryCache cache, Func<object, bool> predicate)
        {
            var collection = cache.GetCacheCollection();
            List<object> bag = new List<object>(collection.Count());
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
