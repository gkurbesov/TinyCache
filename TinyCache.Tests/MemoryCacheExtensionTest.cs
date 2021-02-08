using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TinyCache.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace TinyCache.Tests
{
    public class MemoryCacheExtensionTest
    {
        [Fact]
        public void FindTest1()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");
            cache.CreateEntry(3).SetValue("Anna");

            Assert.NotNull(cache.FirstOrDefault(o => o.Equals("Anna")));
        }

        [Fact]
        public void FindTest2()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");
            cache.CreateEntry(3).SetValue("Anna");

            var collection = cache.FindAll(o => o.StartsWith("A"));

            Assert.NotEmpty(collection);
            Assert.Equal(2, collection.Count());
        }

        [Fact]
        public void GetOrCreateTest1()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(1, () => { return "Anna"; });

            Assert.Equal("Alexander", value);
        }

        [Fact]
        public void GetOrCreateTest2()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(3, () => { return "Anna"; });

            Assert.Equal("Anna", value);
        }

        [Fact]
        public void GetOrCreateTest3()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(1, entry =>
            {
                entry.SetValue("Anna");
            });

            Assert.Equal("Alexander", value);
        }

        [Fact]
        public void GetOrCreateTest4()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(3, entry =>
            {
                entry.SetValue("Anna");
            });

            Assert.Equal("Anna", value);
        }

        [Fact]
        public async Task GetOrCreateAsyncTest1()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = await cache.GetOrCreateAsync(1, async () =>
            {
                await Task.Delay(1000);
                return "Anna";
            });

            Assert.Equal("Alexander", value);
        }

        [Fact]
        public async Task GetOrCreateAsyncTest2()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = await cache.GetOrCreateAsync(3, async () =>
            {
                await Task.Delay(1000);
                return "Anna";
            });

            Assert.Equal("Anna", value);
        }

        [Fact]
        public async Task GetOrCreateAsyncTest3()
        {
            IMemoryCache<string> cache = new MemoryCache<string>(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = await cache.GetOrCreateAsync(3, async entry =>
            {
                await Task.Delay(1000);
                entry.SetValue("Anna");
            });

            Assert.Equal("Anna", value);
        }
    }
}
