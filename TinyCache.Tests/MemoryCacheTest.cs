using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace TinyCache.Tests
{
    public class MemoryCacheTest
    {
        [Fact]
        public void CreateEntryTest1()
        {
            IMemoryCache<object> cache = new MemoryCache<object>(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);

            Assert.Equal(1, entry.Key);
        }

        [Fact]
        public void CreateEntryTest2()
        {
            IMemoryCache<object> cache = new MemoryCache<object>(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);

            var tmp = new object();

            entry.Value = tmp;

            var result = cache.TryGetValue(1, out var value);

            Assert.True(result);
            Assert.NotNull(value);
            Assert.Equal(tmp, value);
        }

        [Fact]
        public void CreateEntryTest3()
        {
            IMemoryCache<object> cache = new MemoryCache<object>(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);

            entry.Value = new object();

            cache.CreateEntry(1);

            var result = cache.TryGetValue(1, out var value);

            Assert.True(result);
            Assert.Null(value);
        }

        [Fact]
        public void RemoveTest()
        {
            IMemoryCache<object> cache = new MemoryCache<object>(new MemoryCacheOptions());

            var entry = cache.CreateEntry(1);
            entry.Value = new object();

            cache.CreateEntry(1);
            cache.Remove(1);
            var result = cache.TryGetValue(1, out var value);

            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void ExpiredTest1()
        {
            IMemoryCache<object> cache = new MemoryCache<object>(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromSeconds(1)});

            var entry = cache.CreateEntry(1);
            entry.Value = new object();

            Assert.True(cache.TryGetValue(1, out var tmp1));
            Thread.Sleep(1500);
            Assert.True(cache.TryGetValue(1, out var tmp2));
        }

        [Fact]
        public void ExpiredTest2()
        {
            IMemoryCache<object> cache = new MemoryCache<object>(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromSeconds(1) });

            var entry = cache.CreateEntry(1);
            entry.Value = new object();
            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(2);

            Assert.True(cache.TryGetValue(1, out var tmp1));
            Thread.Sleep(3500);
            Assert.False(cache.TryGetValue(1, out var tmp2));
        }
    }
}
