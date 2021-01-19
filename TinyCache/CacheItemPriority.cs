using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    /// <summary>
    /// Priority for keeping the cache entry in the cache during a triggered cleanup.
    /// </summary>
    public enum CacheItemPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        NeverRemove = 3
    }
}
