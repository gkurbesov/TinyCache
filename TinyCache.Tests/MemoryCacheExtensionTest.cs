using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TinyCache.Extensions;
using System.Linq;
using System.Threading.Tasks;
using TinyCache.Tests.Mock;

namespace TinyCache.Tests
{
    public class MemoryCacheExtensionTest
    {
        [Fact]
        public void GetOrCreateTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var obj = new object();

            cache.CreateEntry(1).SetValue(obj);
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(1, () => obj);

            Assert.Equal(obj, value);
        }

        [Fact]
        public void GetOrCreateTest2()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(1, entry =>
            {
                entry.SetValue("Anna");
            });

            Assert.Equal("Alexander", value);
        }

        [Fact]
        public void GetOrCreateTest3()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(3, entry =>
            {
                entry.SetValue("Anna");
            });

            Assert.Equal("Anna", value);
        }

        [Fact]
        public async Task GetOrCreateAsyncTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = await cache.GetOrCreateAsync(3, async entry =>
            {
                await Task.Delay(100);
                entry.SetValue("Anna");
            });

            Assert.Equal("Anna", value);
        }

        [Fact]
        public void FirstOrDefaultTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var obj = new object();

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue(obj);
            cache.CreateEntry(3).SetValue("Anna");

            Assert.NotNull(cache.FirstOrDefault(o => o.Equals(obj)));
        }

        [Fact]
        public void FindAllTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var obj = new object();

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue(obj);
            cache.CreateEntry(3).SetValue(obj);

            var collection = cache.FindAll(o => o.Equals(obj));

            Assert.NotEmpty(collection);
            Assert.Equal(2, collection.Count());
        }

    }
}
