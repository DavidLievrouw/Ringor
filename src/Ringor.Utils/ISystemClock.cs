using System;

namespace Dalion.Ringor.Utils {
    public interface ISystemClock {
        DateTimeOffset UtcNow { get; }
        DateTime LocalNow { get; }
    }
}