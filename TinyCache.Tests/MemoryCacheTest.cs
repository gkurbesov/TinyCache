using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using Xunit;
using TinyCache.Extensions;

namespace TinyCache.Tests
{
    public class MemoryCacheTest
    {
        [Fact]
        public void CreateEntryTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);

            Assert.Equal(1, entry.Key);
        }

        [Fact]
        public void TryGetValueTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);
            var tmp = new object();
            entry.Value = tmp;

            var result = cache.TryGetValue(1, out var value);

            Assert.True(result);
            Assert.NotNull(value);
            Assert.Equal(tmp, value);
        }

        [Fact]
        public void TryGetValueTest2()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);
            entry.Value = new object();
            cache.CreateEntry(1);
            var result = cache.TryGetValue(1, out var value);

            Assert.True(result);
            Assert.Null(value);
        }

        [Fact]
        public void TryGetEntryTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);
            var tmp = new object();
            entry.Value = tmp;

            var result = cache.TryGetEntry(1, out var value);

            Assert.True(result);
            Assert.NotNull(value);
            Assert.Equal(entry, value);
        }

        [Fact]
        public void TryGetEntryTest2()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);
            entry.Value = new object();
            cache.CreateEntry(1);
            var result = cache.TryGetEntry(1, out var value);

            Assert.True(result);
            Assert.NotEqual(entry, value);
        }

        [Fact]
        public void GetCollectionTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1);
            cache.CreateEntry(2);
            cache.CreateEntry(3);

            var collection = cache.GetCacheCollection();
            cache.Remove(1);

            Assert.NotEmpty(collection);
            Assert.Equal(3, collection.Count());
            Assert.False(cache.ContainsKey(1));
        }

        [Fact]
        public void GetCollectionTest2()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromSeconds(1) });

            cache.CreateEntry(1).SetAbsoluteExpiration(TimeSpan.FromSeconds(1));
            var beforeCollection = cache.GetCacheCollection();
            Thread.Sleep(1500);

            // HACK: deletion of entities happens in the background so the first result can be TRUE
            if (cache.ContainsKey(1))
                Thread.Sleep(500);

            var afterCollection = cache.GetCacheCollection();

            Assert.NotNull(beforeCollection.FirstOrDefault(o => o.Key.Equals(1)));
            Assert.Null(afterCollection.FirstOrDefault(o => o.Key.Equals(1)));
        }

        [Fact]
        public void RemoveTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);
            entry.Value = new object();
            cache.Remove(1);

            var result = cache.TryGetValue(1, out var value);

            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void ExpiredTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromSeconds(1) });

            cache.CreateEntry(1)
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(1))
                .SetValue(new object());

            Assert.True(cache.TryGetValue(1, out var tmp1));
            Thread.Sleep(1500);
            Assert.False(cache.TryGetValue(1, out var tmp2));
        }

        [Fact]
        public void ExpiredTest2()
        {
            IMemoryCache cache = new MemoryCache(
                new MemoryCacheOptions()
                {
                    ExpirationScanFrequency = TimeSpan.FromSeconds(1)
                });

            var entry = cache.CreateEntry(1);
            entry.Value = new object();
            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(2);

            Assert.True(cache.TryGetValue(1, out var tmp1));
            Thread.Sleep(3500);
            Assert.False(cache.TryGetValue(1, out var tmp2));
        }

        [Fact]
        public void ExpiredTest3()
        {
            IMemoryCache cache = new MemoryCache(
                new MemoryCacheOptions()
                {
                    ExpirationScanFrequency = TimeSpan.FromSeconds(30)
                });

            var entry = cache.CreateEntry(1)
                .SetSlidingExpiration(TimeSpan.FromSeconds(1))
                .SetValue(new object());

            Assert.True(cache.TryGetValue(1, out var tmp1));
            Thread.Sleep(3500);
            Assert.False(cache.TryGetValue(1, out var tmp2));
        }

        [Fact]
        public void LimitTest1()
        {
            IMemoryCache cache = new MemoryCache(
                new MemoryCacheOptions()
                {
                    EntriesSizeLimit = 3,
                });

            cache.CreateEntry(1).SetPriority(CacheItemPriority.NeverRemove);
            cache.CreateEntry(2).SetPriority(CacheItemPriority.Low);
            cache.CreateEntry(3).SetPriority(CacheItemPriority.Normal);
            cache.CreateEntry(4).SetPriority(CacheItemPriority.Normal);

            Assert.False(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(3));
            Assert.True(cache.ContainsKey(4));
            Assert.True(cache.TrySet(5, new CacheEntry(5)));

        }

        [Fact]
        public void LimitTest2()
        {
            IMemoryCache cache = new MemoryCache(
                new MemoryCacheOptions()
                {
                    EntriesSizeLimit = 3,
                });

            cache.CreateEntry(1).SetPriority(CacheItemPriority.NeverRemove);
            cache.CreateEntry(2).SetPriority(CacheItemPriority.NeverRemove);
            cache.CreateEntry(3).SetPriority(CacheItemPriority.NeverRemove);

            Assert.Throws<IndexOutOfRangeException>(() => { cache.CreateEntry(4); });
            Assert.False(cache.TrySet(4, new CacheEntry(4)));
        }       
    }
}
