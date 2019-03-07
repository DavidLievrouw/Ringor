using System;
using System.Collections.Concurrent;

namespace Dalion.Ringor.Utils {
    public abstract class AmbientContext<T> : IDisposable {
        private static readonly ConcurrentStack<AmbientContext<T>> ScopeStack = new ConcurrentStack<AmbientContext<T>>();
        private readonly T _value;

        protected AmbientContext(T value) {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            _value = value;
            ScopeStack.Push(this);
        }

        public static AmbientContext<T> CurrentContext => !ScopeStack.TryPeek(out var ctx)
            ? null
            : ctx;

        public static T Current {
            get {
                var ctx = CurrentContext;
                return ctx == null
                    ? throw new InvalidOperationException($"There is no current {typeof(T).Name} scope.")
                    : ctx._value;
            }
        }

        public void Dispose() {
            ScopeStack.TryPop(out var dummy);
        }
    }
}