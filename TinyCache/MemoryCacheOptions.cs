using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    /// <summary>
    /// Cache options
    /// </summary>
    public class MemoryCacheOptions
    {
        /// <summary>
        /// Time provider
        /// </summary>
        public ISystemClock Clock { get; set; } = new SystemClock();
        /// <summary>
        /// Gets or sets the minimum length of time between successive scans for expired items.
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1);

        private int? sizeLimit;
        /// <summary>
        /// Gets or sets the maximum size of the cache items.
        /// </summary>
        public int? EntriesSizeLimit
        {
            get => sizeLimit;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be non-negative.");
                }
                sizeLimit = value;
            }
        }
    }
}
