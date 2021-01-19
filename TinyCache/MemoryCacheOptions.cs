using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public class MemoryCacheOptions
    {
        public ISystemClock Clock { get; set; } = new SystemClock();
        /// <summary>
        /// Gets or sets the minimum length of time between successive scans for expired items.
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1);

        private int? _sizeLimit;
        /// <summary>
        /// Gets or sets the maximum size of the cache.
        /// </summary>
        public int? SizeLimit
        {
            get => _sizeLimit;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be non-negative.");
                }
                _sizeLimit = value;
            }
        }
    }
}
