using System;

namespace Dalion.Ringor.Utils {
    public class RealSystemClock : ISystemClock {
        private readonly ILocalTimeConverter _localTimeConverter;

        public RealSystemClock(ILocalTimeConverter localTimeConverter) {
            if (localTimeConverter == null) throw new ArgumentNullException(paramName: nameof(localTimeConverter));
            _localTimeConverter = localTimeConverter;
        }

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
        public DateTime LocalNow => _localTimeConverter.ToLocalDateTime(UtcNow);
    }
}