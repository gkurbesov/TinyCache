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
    public class MemoryCacheGenericExtensionTest
    {
        [Fact]
        public void GetOrCreateTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = cache.GetOrCreate(3, () => { return "Anna"; });

            Assert.Equal("Anna", value);
        }

        [Fact]
        public void GetOrCreateTest2()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue(new User(1, "Alex", "alex@email.com", 20));
            cache.CreateEntry(2).SetValue(new User(2, "John", "john@email.com", 30));

            var value = cache.GetOrCreate<User>(1, entry =>
            {
                entry.SetValue(new User(1, "Anna", "anna@email.com", 32));
            });

            Assert.NotNull(value);
            Assert.Equal("Alex", value.Name);
        }

        [Fact]
        public void GetOrCreateTest3()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue(new User(1, "Alex", "alex@email.com", 20));
            cache.CreateEntry(2).SetValue(new User(2, "John", "john@email.com", 30));

            var value = cache.GetOrCreate<User>(3, entry =>
            {
                entry.SetValue(new User(3, "Anna", "anna@email.com", 32));
            });

            Assert.NotNull(value);
            Assert.Equal("Anna", value.Name);
        }

        [Fact]
        public async Task GetOrCreateAsyncTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

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
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

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
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = await cache.GetOrCreateAsync(1, async entry =>
            {
                await Task.Delay(1000);
                entry.SetValue("Anna");
            });

            Assert.Equal("Alexander", value);
        }

        [Fact]
        public async Task GetOrCreateAsyncTest4()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue("Alexander");
            cache.CreateEntry(2).SetValue("John");

            var value = await cache.GetOrCreateAsync(3, async entry =>
            {
                await Task.Delay(1000);
                entry.SetValue("Anna");
            });

            Assert.Equal("Anna", value);
        }

        [Fact]
        public void FirstOrDefaultTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue(new User(1, "Alex", "alex@email.com", 20));
            cache.CreateEntry(2).SetValue(new User(2, "John", "john@email.com", 30));

            var value = cache.FirstOrDefault<User>(o => o.Id == 2);

            Assert.Equal("John", value.Name);
        }

        [Fact]
        public void FirstOrDefaultTest2()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue(new User(1, "Alex", "alex@email.com", 20));
            cache.CreateEntry(2).SetValue(new User(2, "John", "john@email.com", 30));

            var value = cache.FirstOrDefault<User>(o => o.Name.Equals("Anna"));

            Assert.Null(value);
        }


        [Fact]
        public void FindAllTest1()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue(new User(1, "Alex", "alex@email.com", 20));
            cache.CreateEntry(2).SetValue("Joe");
            cache.CreateEntry(3).SetValue("Barney");
            cache.CreateEntry(4).SetValue(new User(4, "John", "john@email.com", 30));
            cache.CreateEntry(5).SetValue(new User(5, "Anna", "anna@email.com", 32));

            var collection = cache.FindAll<User>(o => o.Age >= 30);

            Assert.NotEmpty(collection);
            Assert.Equal(2, collection.Count());
        }

        [Fact]
        public void FindAllTest2()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            cache.CreateEntry(1).SetValue(new User(1, "Alex", "alex@email.com", 20));
            cache.CreateEntry(2).SetValue("Joe");
            cache.CreateEntry(3).SetValue("Barney");
            cache.CreateEntry(4).SetValue(new User(4, "John", "john@email.com", 30));
            cache.CreateEntry(5).SetValue(new User(5, "Anna", "anna@email.com", 32));

            var collection = cache.FindAll<User>(o => o != null);

            Assert.NotEmpty(collection);
            Assert.Equal(3, collection.Count());
        }

    }
}
