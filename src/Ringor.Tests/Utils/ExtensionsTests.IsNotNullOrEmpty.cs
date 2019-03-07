using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class IsNotNullOrEmpty : ExtensionsTests {
            [Fact]
            public void GivenEnumerableIsNull_ReturnsFalse() {
                IEnumerable<string> nullEnumerable = null;
                nullEnumerable.IsNotNullOrEmpty().Should().BeFalse();
            }

            [Fact]
            public void GivenEnumerableIsEmpty_ReturnsFalse() {
                var emptyEnumerable = new string[0];
                emptyEnumerable.IsNotNullOrEmpty().Should().BeFalse();
            }

            [Fact]
            public void GivenEnumerableIsNotEmpty_ReturnsTrue() {
                var emptyEnumerable = new[] {"A", "B"};
                emptyEnumerable.IsNotNullOrEmpty().Should().BeTrue();
            }
        }
    }
}