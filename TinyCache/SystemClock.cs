using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
