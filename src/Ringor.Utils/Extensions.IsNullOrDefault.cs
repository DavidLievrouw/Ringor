using System.Collections.Generic;

namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        public static bool IsNullOrDefault<T>(this T reference) {
            return EqualityComparer<T>.Default.Equals(reference, default(T));
        }
    }
}