using System.Collections.Generic;
using System.Linq;

namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        /// <returns>Like IEnumerable.Any() that can handle null.</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable) {
            return enumerable != null && enumerable.Any();
        }
    }
}