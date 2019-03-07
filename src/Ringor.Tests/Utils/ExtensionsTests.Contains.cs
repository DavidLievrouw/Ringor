using System;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class Contains : ExtensionsTests {
            private readonly string _source;
            private readonly string _value;

            public Contains() {
                _source = "I am the string containing the text to search!";
                _value = "the text to search";
            }

            [Theory]
            [InlineData(StringComparison.InvariantCulture, true)]
            [InlineData(StringComparison.InvariantCultureIgnoreCase, true)]
            [InlineData(StringComparison.CurrentCulture, true)]
            [InlineData(StringComparison.CurrentCultureIgnoreCase, true)]
            [InlineData(StringComparison.Ordinal, true)]
            [InlineData(StringComparison.OrdinalIgnoreCase, true)]
            public void GivenStringContainsSearchString_WithSameCasing_ReturnsTrue(StringComparison stringComparison, bool expected) {
                _source.Contains(_value, stringComparison).Should().Be(expected);
            }

            [Theory]
            [InlineData(StringComparison.InvariantCulture, false)]
            [InlineData(StringComparison.InvariantCultureIgnoreCase, true)]
            [InlineData(StringComparison.CurrentCulture, false)]
            [InlineData(StringComparison.CurrentCultureIgnoreCase, true)]
            [InlineData(StringComparison.Ordinal, false)]
            [InlineData(StringComparison.OrdinalIgnoreCase, true)]
            public void GivenStringContainsSearchString_WithDifferentCasing_ReturnsExpected(StringComparison stringComparison, bool expected) {
                var differentlyCasedValue = _value.ToUpper();
                _source.Contains(differentlyCasedValue, stringComparison).Should().Be(expected);
            }

            [Theory]
            [InlineData(StringComparison.InvariantCulture, false)]
            [InlineData(StringComparison.InvariantCultureIgnoreCase, false)]
            [InlineData(StringComparison.CurrentCulture, false)]
            [InlineData(StringComparison.CurrentCultureIgnoreCase, false)]
            [InlineData(StringComparison.Ordinal, false)]
            [InlineData(StringComparison.OrdinalIgnoreCase, false)]
            public void GivenStringDoesNotContainSearchString_ReturnsFalse(StringComparison stringComparison, bool expected) {
                var valueThatIsNotContained = "I'm not there";
                _source.Contains(valueThatIsNotContained, stringComparison).Should().Be(expected);
            }
        }
    }
}