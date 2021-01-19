using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public enum CacheItemPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        NeverRemove = 3
    }
}
