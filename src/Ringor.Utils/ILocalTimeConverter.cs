using System;

namespace Dalion.Ringor.Utils {
    public interface ILocalTimeConverter {
        DateTime ToLocalDateTime(DateTimeOffset input);
    }
}