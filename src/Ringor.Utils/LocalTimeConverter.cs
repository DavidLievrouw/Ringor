using System;

namespace Dalion.Ringor.Utils {
    public class LocalTimeConverter : ILocalTimeConverter {
        private readonly TimeSpan _offsetFromUtc;

        public LocalTimeConverter(TimeSpan offsetFromUtc) {
            _offsetFromUtc = offsetFromUtc;
        }

        public DateTime ToLocalDateTime(DateTimeOffset input) {
            if (input.DateTime.Kind == DateTimeKind.Local) return input.DateTime;
            var localTicks = input.Ticks + _offsetFromUtc.Ticks - input.Offset.Ticks;
            return new DateTime(localTicks, DateTimeKind.Local);
        }
    }
}