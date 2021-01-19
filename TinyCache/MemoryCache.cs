using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TinyCache
{
    public class MemoryCache<T> : IMemoryCache<T>, IDisposable where T : class
    {
        private readonly MemoryCacheOptions options;
        private readonly ConcurrentDictionary<object, CacheEntry<T>> entries;
        private DateTimeOffset lastExpirationScan;
        private bool _disposed;

        public MemoryCache(MemoryCacheOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            this.options = options;
            this.entries = new ConcurrentDictionary<object, CacheEntry<T>>();
        }

        public ICacheEntry<T> CreateEntry(object key)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var entry = new CacheEntry<T>(key);
            return entries.AddOrUpdate(key, entry, (k, old) => entry);
        }

        public void Remove(object key)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (entries.TryRemove(key, out CacheEntry<T> entry))
            {
                entry.Expired = true;
            }
            ScanForExpiredItems(options.Clock.UtcNow);
        }

        public bool TryGetValue(object key, out T value)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            value = null;
            var utcNow = options.Clock.UtcNow;

            if (entries.TryGetValue(key, out var entry))
            {
                if (!entry.CheckExpired(utcNow))
                {
                    entry.LastAccessed = utcNow;
                    value = entry.Value;
                    return true;
                }
                else
                {
                    Remove(key);
                }
            }
            ScanForExpiredItems(utcNow);
            value = null;
            return false;
        }

        internal void ScanForExpiredItems(DateTimeOffset utcNow)
        {
            if (options.ExpirationScanFrequency < utcNow - lastExpirationScan)
            {
                lastExpirationScan = utcNow;
                _ = Task.Factory.StartNew(() =>
                {
                    foreach (var item in entries.ToArray())
                    {
                        if (item.Value.CheckExpired(utcNow))
                            Remove(item.Key);
                    }
                });
            }
        }

        internal void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
                _disposed = true;
            }
        }

        ~MemoryCache() => Dispose(false);
    }
}
