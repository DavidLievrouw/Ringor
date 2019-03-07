using System;
using System.Collections.Generic;

namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        public static TValue GetItemOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            dict.TryGetValue(key, out var val);
            return val;
        }
    }
}