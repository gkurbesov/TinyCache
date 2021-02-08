using System;
using Xunit;

namespace TinyCache.Tests
{
    public class CacheEntryTest
    {
        [Fact]
        public void Test1()
        {
            CacheEntry entry = new CacheEntry(1);

            Assert.False(entry.CheckExpired(DateTimeOffset.UtcNow));
        }

        [Fact]
        public void Test2()
        {
            CacheEntry entry = new CacheEntry(1)
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(-1)
            };

            Assert.True(entry.CheckExpired(DateTimeOffset.UtcNow));
            Assert.True(entry.Expired);
        }
    }
}
