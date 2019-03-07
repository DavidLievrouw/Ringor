using System;

namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        public static bool Contains(this string source, string value, StringComparison comparisonType) {
            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}