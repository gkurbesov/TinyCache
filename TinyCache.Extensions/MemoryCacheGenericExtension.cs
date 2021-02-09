using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCache.Extensions
{
    public static class MemoryCacheGenericExtension
    {
        public static T GetOrCreate<T>(this IMemoryCache cache, object key, Func<T> factory)
        {
            if (cache.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            else
            {
                var newEntryValue = factory();
                var entry = cache.CreateEntry(key);
                entry.Value = newEntryValue;
                return newEntryValue;
            }
        }

        public static T GetOrCreate<T>(this IMemoryCache cache, object key, Action<ICacheEntry> factory)
        {
            if (cache.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            else
            {
                var entry = cache.CreateEntry(key);
                factory(entry);
                return (T)entry.Value;
            }
        }

        public static async Task<T> GetOrCreateAsync<T>(this IMemoryCache cache, object key, Func<Task<T>> factory)
        {
            if (cache.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            else
            {
                var newEntryValue = await factory();
                var entry = cache.CreateEntry(key);
                entry.Value = newEntryValue;
                return newEntryValue;
            }
        }

        public static async Task<T> GetOrCreateAsync<T>(this IMemoryCache cache, object key, Func<ICacheEntry, Task> factory)
        {
            if (cache.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            else
            {
                var entry = cache.CreateEntry(key);
                await factory(entry);
                return (T)entry.Value;
            }
        }

        public static T FirstOrDefault<T>(this IMemoryCache cache, Func<T, bool> predicate)
        {
            var collection = cache.GetCacheCollection();
            foreach (var entry in collection)
            {
                if (entry.Value is T value)
                {
                    if (predicate(value))
                        return value;
                }
            }
            return default;
        }

        public static IEnumerable<T> FindAll<T>(this IMemoryCache cache, Func<T, bool> predicate)
        {
            var collection = cache.GetCacheCollection();
            List<T> bag = new List<T>(collection.Count());
            foreach (var entry in collection)
            {
                if (entry.Value is T value)
                {
                    if (predicate(value))
                        bag.Add(value);
                }
            }
            return bag;
        }
    }
}
