using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Dalion.Ringor.Api {
    public static partial class Extensions {
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action) {
            if (source == null) return;
            if (action == null) return;
            foreach (var element in source) {
                await action(element);
            }
        }
        
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, int, Task> action) {
            if (source == null) return;
            if (action == null) return;
            var index = 0;
            foreach (var element in source) {
                await action(element, index++);
            }
        }

        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T, Task> action) {
            if (source == null) return;
            if (action == null) return;
            foreach (var element in source) {
                await action(element);
            }
        }

        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T, int, Task> action) {
            if (source == null) return;
            if (action == null) return;
            var index = 0;
            foreach (var element in source) {
                await action(element, index++);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            if (source == null) return;
            if (action == null) return;
            foreach (var element in source) {
                action(element);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action) {
            if (source == null) return;
            if (action == null) return;
            var index = 0;
            foreach (var element in source) {
                action(element, index++);
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> DoActionIf<T>(this IEnumerable<T> source, Predicate<T> predicate, Action<T> action) {
            if (source == null) return source;
            if (predicate == null) return source;
            if (action == null) return source;
            source.Where(_ => predicate(_)).ForEach(action);
            return source;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> DoActionIf<T>(this IEnumerable<T> source, Predicate<T> predicate, Action<T, int> action) {
            if (source == null) return source;
            if (predicate == null) return source;
            if (action == null) return source;
            source.Where(_ => predicate(_)).ForEach(action);
            return source;
        }
    }
}