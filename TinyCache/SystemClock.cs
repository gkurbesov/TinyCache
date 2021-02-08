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
        private int _lastTicks = 0;
        private DateTimeOffset _lastTime = DateTimeOffset.UtcNow;

        public DateTimeOffset UtcNow
        {
            get
            {
                int tickCount = Environment.TickCount;
                if (tickCount != _lastTicks)
                {
                    _lastTime = DateTimeOffset.UtcNow;
                    _lastTicks = tickCount;
                }
                return _lastTime;
            }
        }
    }
}
