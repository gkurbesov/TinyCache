using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache
{
    /// <summary>
    /// Object for getting the current time
    /// </summary>
    public interface ISystemClock
    {
        /// <summary>
        /// Get current time
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
