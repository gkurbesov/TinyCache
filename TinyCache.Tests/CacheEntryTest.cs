using System;
using Xunit;

namespace TinyCache.Tests
{
    public class CacheEntryTest
    {
        [Fact]
        public void Test1()
        {
            CacheEntry<object> entry = new CacheEntry<object>(1);

            Assert.False(entry.CheckExpired(DateTimeOffset.UtcNow));
        }

        [Fact]
        public void Test2()
        {
            CacheEntry<object> entry = new CacheEntry<object>(1)
            {
                Expired = true
            };

            Assert.True(entry.CheckExpired(DateTimeOffset.UtcNow));
        }

        [Fact]
        public void Test3()
        {
            CacheEntry<object> entry = new CacheEntry<object>(1)
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(-1)
            };

            Assert.True(entry.CheckExpired(DateTimeOffset.UtcNow));
            Assert.True(entry.Expired);
        }
    }
}
