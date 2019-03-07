using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static async Task<IEnumerable<T>> ForEach<T>(this IEnumerable<T> source, Func<T, Task> action) {
            if (source == null) return source;
            if (action == null) return source;
            foreach (var element in source) {
                await action(element).ConfigureAwait(false);
            }
            return source;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static async Task<IEnumerable<T>> ForEach<T>(this IEnumerable<T> source, Func<T, int, Task> action) {
            if (source == null) return source;
            if (action == null) return source;
            var index = 0;
            foreach (var element in source) {
                await action(element, index++).ConfigureAwait(false);
            }
            return source;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            if (source == null) return source;
            if (action == null) return source;
            foreach (var element in source) {
                action(element);
            }
            return source;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action) {
            if (source == null) return source;
            if (action == null) return source;
            var index = 0;
            foreach (var element in source) {
                action(element, index++);
            }
            return source;
        }
    }
}