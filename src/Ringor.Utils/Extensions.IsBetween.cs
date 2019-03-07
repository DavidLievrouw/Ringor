using System;

namespace Dalion.Ringor.Utils {
    public partial class Extensions {
        public static bool IsBetweenInclusive(this IComparable candidate, IComparable left, IComparable right) {
            if (candidate == null) throw new ArgumentNullException(nameof(candidate));
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));
            return candidate.CompareTo(left) >= 0 && candidate.CompareTo(right) <= 0;
        }

        public static bool IsBetweenExclusive(this IComparable candidate, IComparable left, IComparable right) {
            if (candidate == null) throw new ArgumentNullException(nameof(candidate));
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));
            return candidate.CompareTo(left) > 0 && candidate.CompareTo(right) < 0;
        }

        public static bool IsBetweenLeftInclusive(this IComparable candidate, IComparable left, IComparable right) {
            if (candidate == null) throw new ArgumentNullException(nameof(candidate));
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));
            return candidate.CompareTo(left) >= 0 && candidate.CompareTo(right) < 0;
        }

        public static bool IsBetweenRightInclusive(this IComparable candidate, IComparable left, IComparable right) {
            if (candidate == null) throw new ArgumentNullException(nameof(candidate));
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));
            return candidate.CompareTo(left) > 0 && candidate.CompareTo(right) <= 0;
        }
    }
}