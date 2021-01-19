using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
