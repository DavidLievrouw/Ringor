using System;
using System.Reflection;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public abstract class AmbientContextTestsBase<TAmbientContext, TValue>
        where TAmbientContext : AmbientContext<TValue>
        where TValue : class {
        [Fact]
        public void ConstructorSetsScope() {
            RunTest((ctx, value) => {
                GetCurrentValue().Should().Be(value);
                GetCurrentContext().Should().Be(ctx);
            });
        }

        [Fact]
        public void SupportsNestedScopes() {
            RunTest((ctx, value) => {
                var innerValue = A.Fake<TValue>();
                using (var innerCtx = Construct(innerValue)) {
                    GetCurrentValue().Should().Be(innerValue);
                    GetCurrentContext().Should().Be(innerCtx);
                }
                GetCurrentValue().Should().Be(value);
                GetCurrentContext().Should().Be(ctx);
            });
        }

        [Fact]
        public void DisposalPopsScope() {
            var expectedContext = GetCurrentContext();
            RunTest((ctx, value) => {
                ctx.Dispose();
                GetCurrentContext().Should().Be(expectedContext);
            });
        }

        [Fact]
        public void WhenNoScopeIsDefined_DisposingDoesNotThrow() {
            RunTest((ctx, value) => {
                ClearScopes();
                Action act = ctx.Dispose;
                act.Should().NotThrow();
            });
        }

        [Fact]
        public void WhenNoScopeIsDefined_ReturnsNullForContext() {
            RunTest((ctx, value) => {
                ClearScopes();
                var actual = GetCurrentContext();
                actual.Should().BeNull();
            });
        }

        [Fact]
        public void WhenNoScopeIsDefined_CannotQueryCurrentValue() {
            RunTest((ctx, value) => {
                ClearScopes();
                Action act = () => GetCurrentValue();
                act.Should().Throw<TargetInvocationException>().WithInnerException<InvalidOperationException>();
            });
        }

        private void ClearScopes() {
            while (GetCurrentContext() != null) GetCurrentContext().Dispose();
        }

        private void RunTest(Action<TAmbientContext, TValue> test) {
            var value = A.Fake<TValue>();
            using (var ambientContext = Construct(value)) {
                test(ambientContext, value);
            }
        }

        private static TAmbientContext Construct(TValue value) {
            var ctor = typeof(TAmbientContext).GetConstructor(new[] {typeof(TValue)});
            if (ctor == null) throw new InvalidOperationException($"Could not find a valid constructor for {typeof(TAmbientContext).Name}.");
            return (TAmbientContext) ctor.Invoke(new object[] {value});
        }

        private static TAmbientContext GetCurrentContext() {
            var propName = "CurrentContext";
            var prop = typeof(TAmbientContext).GetProperty(propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (prop == null) throw new InvalidOperationException($"Could not find the '{propName}' property for {typeof(TAmbientContext).Name}.");
            return (TAmbientContext) prop.GetValue(null);
        }

        private static TValue GetCurrentValue() {
            var propName = "Current";
            var prop = typeof(TAmbientContext).GetProperty(propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (prop == null) throw new InvalidOperationException($"Could not find the '{propName}' property for {typeof(TAmbientContext).Name}.");
            return (TValue) prop.GetValue(null);
        }
    }
}