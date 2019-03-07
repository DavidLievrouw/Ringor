using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class AdaptNullable : ExtensionsTests {
            private readonly DateTime? _sampleInput;
            private IAdapter<DateTime, bool> _testAdapter;

            public AdaptNullable() {
                _testAdapter = _testAdapter.Fake();
                _sampleInput = new DateTime(2016, 12, 22);
            }

            [Fact]
            public void GivenNullAdapter_Throws() {
                _testAdapter = null;
                Action act = () => _testAdapter.Adapt(_sampleInput);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenNullInput_ReturnsNull() {
                var nullInput = new DateTime?();
                _testAdapter.Adapt(nullInput).Should().NotHaveValue();
            }

            [Fact]
            public void GivenNonNullInput_ReturnsResultFromAdapter() {
                var expected = true;
                A.CallTo(() => _testAdapter.Adapt(A<DateTime>._)).Returns(expected);
                _testAdapter.Adapt(_sampleInput).Should().Be(expected);
            }
        }

        public class AdaptMany : ExtensionsTests {
            private string[] _sampleInput;
            private IAdapter<string, Foo> _testAdapter;

            public AdaptMany() {
                _testAdapter = _testAdapter.Fake();
                _sampleInput = new[] {"One", "Two"};
            }

            [Fact]
            public void GivenNullAdapter_Throws() {
                _testAdapter = null;
                Action act = () => _testAdapter.AdaptMany(_sampleInput);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenNullEnumerable_ReturnsNull() {
                _testAdapter.AdaptMany(null).Should().BeNull();
            }

            [Fact]
            public void GivenZeroItems_DoesNotCallAdapter() {
                _sampleInput = Array.Empty<string>();
                _testAdapter.AdaptMany(_sampleInput).Should().NotBeNull().And.BeEmpty();
                A.CallTo(() => _testAdapter.Adapt(A<string>._)).MustNotHaveHappened();
            }

            [Fact]
            public void ReturnsResultFromAdapterForEveryInputItem() {
                A.CallTo(() => _testAdapter.Adapt(A<string>._))
                    .ReturnsLazily(fakeCall => new Foo {Value = fakeCall.Arguments.Get<string>(0)});
                var expected = _sampleInput.Select(str => new Foo {Value = str});
                _testAdapter.AdaptMany(_sampleInput).Should().BeEquivalentTo(expected);
            }
        }

        public class AdaptNullableMany : ExtensionsTests {
            private DateTime?[] _sampleInput;
            private IAdapter<DateTime, bool> _testAdapter;

            public AdaptNullableMany() {
                _testAdapter = _testAdapter.Fake();
                _sampleInput = new DateTime?[] {new DateTime(2016, 12, 22), new DateTime(2017, 1, 1)};
            }

            [Fact]
            public void GivenNullAdapter_Throws() {
                _testAdapter = null;
                Action act = () => _testAdapter.AdaptMany(_sampleInput);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenAdaptManyDoesNotReturnNull_ReturnsResultFromAdaptMany() {
                A.CallTo(() => _testAdapter.Adapt(A<DateTime>._)).Returns(true);
                var expected = _sampleInput.Select(str => true);
                _testAdapter.AdaptMany(_sampleInput).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenInputForSingleAdaptIsNull_ReturnsNullPerInputItem() {
                A.CallTo(() => _testAdapter.Adapt(A<DateTime>._)).Returns(true);
                _sampleInput = new DateTime?[] {null, null};
                var expected = _sampleInput.Select(str => (bool?) null);
                _testAdapter.AdaptMany(_sampleInput).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenAdaptManyReturnsNull_ReturnsNull() {
                _sampleInput = null;
                _testAdapter.AdaptMany(_sampleInput).Should().BeNull();
            }
        }

        public class AdaptManyOrFallback : ExtensionsTests {
            private readonly Func<IEnumerable<Foo>> _fallBack;
            private string[] _sampleInput;
            private IAdapter<string, Foo> _testAdapter;

            public AdaptManyOrFallback() {
                _testAdapter = _testAdapter.Fake();
                _sampleInput = new[] {"One", "Two"};
                _fallBack = () => new[] {new Foo {Value = "FallbackResult"}};
            }

            [Fact]
            public void GivenNullAdapter_Throws() {
                _testAdapter = null;
                Action act = () => _testAdapter.AdaptManyOrFallback(_sampleInput, _fallBack);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenAdaptManyDoesNotReturnNull_ReturnsResultFromAdaptMany() {
                A.CallTo(() => _testAdapter.Adapt(A<string>._))
                    .ReturnsLazily(fakeCall => new Foo {Value = fakeCall.Arguments.Get<string>(0)});
                var expected = _sampleInput.Select(str => new Foo {Value = str});
                _testAdapter.AdaptManyOrFallback(_sampleInput, _fallBack).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenAdaptManyReturnsNull_ReturnsResultFromCallback() {
                _sampleInput = null;
                var expected = _fallBack();
                _testAdapter.AdaptManyOrFallback(_sampleInput, _fallBack).Should().BeEquivalentTo(expected);
            }
        }

        public class AdaptNullableManyOrFallback : ExtensionsTests {
            private readonly Func<IEnumerable<bool?>> _fallBack;
            private DateTime?[] _sampleInput;
            private IAdapter<DateTime, bool> _testAdapter;

            public AdaptNullableManyOrFallback() {
                _testAdapter = _testAdapter.Fake();
                _sampleInput = new DateTime?[] {new DateTime(2016, 12, 22), new DateTime(2017, 1, 1)};
                _fallBack = () => new bool?[] {true, false};
            }

            [Fact]
            public void GivenNullAdapter_Throws() {
                _testAdapter = null;
                Action act = () => _testAdapter.AdaptManyOrFallback(_sampleInput, _fallBack);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenAdaptManyDoesNotReturnNull_ReturnsResultFromAdaptMany() {
                A.CallTo(() => _testAdapter.Adapt(A<DateTime>._)).Returns(true);
                var expected = _sampleInput.Select(str => true);
                _testAdapter.AdaptManyOrFallback(_sampleInput, _fallBack).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenInputForSingleAdaptIsNull_ReturnsNullPerInputItem() {
                A.CallTo(() => _testAdapter.Adapt(A<DateTime>._)).Returns(true);
                _sampleInput = new DateTime?[] {null, null};
                var expected = _sampleInput.Select(str => (bool?) null);
                _testAdapter.AdaptManyOrFallback(_sampleInput, _fallBack).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenAdaptManyReturnsNull_ReturnsResultFromCallback() {
                _sampleInput = null;
                var expected = _fallBack();
                _testAdapter.AdaptManyOrFallback(_sampleInput, _fallBack).Should().BeEquivalentTo(expected);
            }
        }

        public class AdaptManyOrEmpty : ExtensionsTests {
            private string[] _sampleInput;
            private IAdapter<string, Foo> _testAdapter;

            public AdaptManyOrEmpty() {
                _testAdapter = _testAdapter.Fake();
                _sampleInput = new[] {"One", "Two"};
            }

            [Fact]
            public void GivenNullAdapter_Throws() {
                _testAdapter = null;
                Action act = () => _testAdapter.AdaptManyOrEmpty(_sampleInput);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenAdaptManyDoesNotReturnNull_ReturnsResultFromAdaptMany() {
                A.CallTo(() => _testAdapter.Adapt(A<string>._))
                    .ReturnsLazily(fakeCall => new Foo {Value = fakeCall.Arguments.Get<string>(0)});
                var expected = _sampleInput.Select(str => new Foo {Value = str});
                _testAdapter.AdaptManyOrEmpty(_sampleInput).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenAdaptManyReturnsNull_ReturnsEmptyResult() {
                _sampleInput = null;
                _testAdapter.AdaptManyOrEmpty(_sampleInput).Should().BeEquivalentTo(Enumerable.Empty<Foo>());
            }
        }

        public class AdaptNullableManyOrEmpty : ExtensionsTests {
            private DateTime?[] _sampleInput;
            private IAdapter<DateTime, bool> _testAdapter;

            public AdaptNullableManyOrEmpty() {
                _testAdapter = _testAdapter.Fake();
                _sampleInput = new DateTime?[] {new DateTime(2016, 12, 22), new DateTime(2017, 1, 1)};
            }

            [Fact]
            public void GivenNullAdapter_Throws() {
                _testAdapter = null;
                Action act = () => _testAdapter.AdaptManyOrEmpty(_sampleInput);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenAdaptManyDoesNotReturnNull_ReturnsEmptyEnumerable() {
                A.CallTo(() => _testAdapter.Adapt(A<DateTime>._)).Returns(true);
                var expected = _sampleInput.Select(str => true);
                _testAdapter.AdaptManyOrEmpty(_sampleInput).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenInputForSingleAdaptIsNull_ReturnsNullPerInputItem() {
                A.CallTo(() => _testAdapter.Adapt(A<DateTime>._)).Returns(true);
                _sampleInput = new DateTime?[] {null, null};
                var expected = _sampleInput.Select(str => (bool?) null);
                _testAdapter.AdaptManyOrEmpty(_sampleInput).Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void WhenAdaptManyReturnsNull_ReturnsResultFromCallback() {
                _sampleInput = null;
                _testAdapter.AdaptManyOrEmpty(_sampleInput).Should().BeEquivalentTo(Enumerable.Empty<bool?>());
            }
        }

        public class Foo {
            public string Value { get; set; }
        }
    }
}