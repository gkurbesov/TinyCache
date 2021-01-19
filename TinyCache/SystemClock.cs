using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    /// <summary>
    /// Default time provider
    /// </summary>
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
