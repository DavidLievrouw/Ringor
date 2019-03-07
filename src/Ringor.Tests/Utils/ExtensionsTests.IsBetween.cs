using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class IsBetweenInclusive : ExtensionsTests {
            private readonly int _left;
            private readonly int _right;

            public IsBetweenInclusive() {
                _left = 0;
                _right = 10;
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(10, true)]
            public void OnBoundaries_ReturnsExpectedResult(int boundaryValue, bool expected) {
                boundaryValue.IsBetweenInclusive(_left, _right).Should().Be(expected);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(9)]
            public void InsideBoundaries_ReturnsTrue(int candidate) {
                candidate.IsBetweenInclusive(_left, _right).Should().BeTrue();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(11)]
            public void OutsideBoundaries_ReturnsFalse(int candidate) {
                candidate.IsBetweenInclusive(_left, _right).Should().BeFalse();
            }
        }

        public class IsBetweenLeftInclusive : ExtensionsTests {
            private readonly int _left;
            private readonly int _right;

            public IsBetweenLeftInclusive() {
                _left = 0;
                _right = 10;
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(10, false)]
            public void OnBoundaries_ReturnsExpectedResult(int boundaryValue, bool expected) {
                boundaryValue.IsBetweenLeftInclusive(_left, _right).Should().Be(expected);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(9)]
            public void InsideBoundaries_ReturnsTrue(int candidate) {
                candidate.IsBetweenLeftInclusive(_left, _right).Should().BeTrue();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(11)]
            public void OutsideBoundaries_ReturnsFalse(int candidate) {
                candidate.IsBetweenLeftInclusive(_left, _right).Should().BeFalse();
            }
        }

        public class IsBetweenRightInclusive : ExtensionsTests {
            private readonly int _left;
            private readonly int _right;

            public IsBetweenRightInclusive() {
                _left = 0;
                _right = 10;
            }

            [Theory]
            [InlineData(0, false)]
            [InlineData(10, true)]
            public void OnBoundaries_ReturnsExpectedResult(int boundaryValue, bool expected) {
                boundaryValue.IsBetweenRightInclusive(_left, _right).Should().Be(expected);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(9)]
            public void InsideBoundaries_ReturnsTrue(int candidate) {
                candidate.IsBetweenRightInclusive(_left, _right).Should().BeTrue();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(11)]
            public void OutsideBoundaries_ReturnsFalse(int candidate) {
                candidate.IsBetweenRightInclusive(_left, _right).Should().BeFalse();
            }
        }

        public class IsBetweenExclusive : ExtensionsTests {
            private readonly int _left;
            private readonly int _right;

            public IsBetweenExclusive() {
                _left = 0;
                _right = 10;
            }

            [Theory]
            [InlineData(0, false)]
            [InlineData(10, false)]
            public void OnBoundaries_ReturnsExpectedResult(int boundaryValue, bool expected) {
                boundaryValue.IsBetweenExclusive(_left, _right).Should().Be(expected);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(9)]
            public void InsideBoundaries_ReturnsTrue(int candidate) {
                candidate.IsBetweenExclusive(_left, _right).Should().BeTrue();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(11)]
            public void OutsideBoundaries_ReturnsFalse(int candidate) {
                candidate.IsBetweenExclusive(_left, _right).Should().BeFalse();
            }
        }
    }
}