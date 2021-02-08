using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TinyCache
{
    public class MemoryCache : IMemoryCache, IDisposable
    {
        private readonly MemoryCacheOptions options;
        private readonly ConcurrentDictionary<object, ICacheEntry> entries;
        private DateTimeOffset lastExpirationScan;
        private bool _disposed;

        public MemoryCache(MemoryCacheOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            this.options = options;
            this.entries = new ConcurrentDictionary<object, ICacheEntry>();
        }

        #region Create or set
        public ICacheEntry CreateEntry(object key)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (ScanForRemoveWithItemPriority())
            {
                var entry = new CacheEntry(key);
                entry.LastAccessed = options.Clock.UtcNow;
                return entries.AddOrUpdate(key, entry, (k, old) => entry);
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public bool TrySet(object key, ICacheEntry entry)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            if (entry.Key == null)
                throw new ArgumentNullException(nameof(entry.Key), "Key in entry cannot be null");
            if (!key.Equals(entry.Key))
                throw new ArgumentException("The key argument and key in the entity must match", nameof(key));

            if (ScanForRemoveWithItemPriority() && !entries.ContainsKey(key))
                return entries.TryAdd(key, entry);
            else
                return false;
        }
        #endregion

        #region Get

        public bool ContainsKey(object key)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var utcNow = options.Clock.UtcNow;

            if (entries.TryGetValue(key, out var entry))
            {
                if (!entry.CheckExpired(utcNow))
                    return true;
                else
                    Remove(key);
            }
            return false;

        }

        public bool TryGetValue(object key, out object value)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

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

            StartScanForExpiredItemsIfNeeded(utcNow);

            value = null;
            return false;
        }

        public bool TryGetEntry(object key, out ICacheEntry entry)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var utcNow = options.Clock.UtcNow;

            if (entries.TryGetValue(key, out var cahceEntry))
            {
                if (!cahceEntry.CheckExpired(utcNow))
                {
                    cahceEntry.LastAccessed = utcNow;
                    entry = cahceEntry;
                    return true;
                }
                else
                {
                    Remove(key);
                }
            }

            StartScanForExpiredItemsIfNeeded(utcNow);

            entry = null;
            return false;
        }

        public IEnumerable<ICacheEntry> GetCacheCollection()
        {
            StartScanForExpiredItemsIfNeeded(options.Clock.UtcNow);
            return entries.Values;
        }

        #endregion

        #region Remove

        public void Remove(object key)
        {
            CheckDisposed();
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (entries.TryRemove(key, out ICacheEntry entry))
                entry.Expired = true;

            StartScanForExpiredItemsIfNeeded(options.Clock.UtcNow);
        }

        protected bool ScanForRemoveWithItemPriority()
        {
            if (options.EntriesSizeLimit.HasValue)
            {
                if (entries.Count >= options.EntriesSizeLimit.Value)
                {
                    var tmpList = entries.Select(o => o.Value)
                        .Where(o => o.Priority != CacheItemPriority.NeverRemove)
                        .OrderBy(o => o.LastAccessed)
                        .OrderBy(o => o.Priority)
                        .ToList();
                    int countToRemove = entries.Count - options.EntriesSizeLimit.Value + 1;
                    for (int i = 0; i < tmpList.Count && i < countToRemove; i++)
                        Remove(tmpList[i].Key);
                }
                return entries.Count < options.EntriesSizeLimit.Value;
            }
            else
            {
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void StartScanForExpiredItemsIfNeeded(DateTimeOffset utcNow)
        {
            if (options.ExpirationScanFrequency < utcNow - lastExpirationScan)
            {
                lastExpirationScan = utcNow;
                Task.Factory.StartNew(state => ScanForExpiredItems((Tuple<MemoryCache, DateTimeOffset>)state), new Tuple<MemoryCache, DateTimeOffset>(this, utcNow),
                   CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            }
        }

        private static void ScanForExpiredItems(Tuple<MemoryCache, DateTimeOffset> value)
        {
            var cache = value.Item1;
            var utcNow = value.Item2;
            foreach (var item in value.Item1.entries.ToArray())
            {
                if (item.Value.CheckExpired(utcNow))
                    cache.Remove(item.Key);
            }
        }

        #endregion

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
