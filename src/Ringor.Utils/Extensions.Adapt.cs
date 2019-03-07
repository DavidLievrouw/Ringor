using System;
using System.Collections.Generic;
using System.Linq;

namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        public static TOut? Adapt<TIn, TOut>(this IAdapter<TIn, TOut> adapter, TIn? input)
            where TIn : struct
            where TOut : struct {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            return input.HasValue
                ? adapter.Adapt(input.Value)
                : new TOut?();
        }

        public static IEnumerable<TOut> AdaptMany<TIn, TOut>(this IAdapter<TIn, TOut> adapter, IEnumerable<TIn> input) {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            return input?.Select(adapter.Adapt);
        }

        public static IEnumerable<TOut> AdaptManyOrFallback<TIn, TOut>(this IAdapter<TIn, TOut> adapter, IEnumerable<TIn> input, Func<IEnumerable<TOut>> nullFallbackProvider) {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            return adapter.AdaptMany(input) ?? nullFallbackProvider();
        }

        public static IEnumerable<TOut> AdaptManyOrEmpty<TIn, TOut>(this IAdapter<TIn, TOut> adapter, IEnumerable<TIn> input) {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            return adapter.AdaptManyOrFallback(input, Array.Empty<TOut>);
        }

        public static IEnumerable<TOut?> AdaptMany<TIn, TOut>(this IAdapter<TIn, TOut> adapter, IEnumerable<TIn?> input)
            where TIn : struct
            where TOut : struct {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            return input?.Select(adapter.Adapt);
        }

        public static IEnumerable<TOut?> AdaptManyOrFallback<TIn, TOut>(this IAdapter<TIn, TOut> adapter, IEnumerable<TIn?> input, Func<IEnumerable<TOut?>> nullFallbackProvider)
            where TIn : struct
            where TOut : struct {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            return adapter.AdaptMany(input) ?? nullFallbackProvider();
        }

        public static IEnumerable<TOut?> AdaptManyOrEmpty<TIn, TOut>(this IAdapter<TIn, TOut> adapter, IEnumerable<TIn?> input)
            where TIn : struct
            where TOut : struct {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            return adapter.AdaptManyOrFallback(input, Array.Empty<TOut?>);
        }
    }
}