using System;
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

        public static async Task<T> GetOrCreateAsync<T>(this IMemoryCache<T> cache, object key, Func<Task<T>> factory) where T : class
        {
            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                var newEntryValue = await factory();
                var entry = cache.CreateEntry(key);
                entry.Value = newEntryValue;
                return newEntryValue;
            }
        }

        public static async Task<T> GetOrCreateAsync<T>(this IMemoryCache<T> cache, object key, Func<ICacheEntry<T>, Task<T>> factory) where T : class
        {
            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                var entry = cache.CreateEntry(key);
                var newEntryValue = await factory(entry);
                entry.Value = newEntryValue;
                return newEntryValue;
            }
        }
    }
}
