using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class GetItemOrDefault : ExtensionsTests {
            private readonly Dictionary<string, int> _dic;

            public GetItemOrDefault() {
                _dic = new Dictionary<string, int> {{"A", 1}, {"B", 2}};
            }

            [Fact]
            public void GivenNullDictionary_Throws() {
                Dictionary<string, int> dic = null;
                Action act = () => dic.GetItemOrDefault("A");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenKeyIsNotFound_ReturnsDefault() {
                var actual = _dic.GetItemOrDefault("C");
                actual.Should().Be(default(int));
            }

            [Fact]
            public void WhenKeyIsFound_ReturnsCorrespondingValue() {
                var actual = _dic.GetItemOrDefault("B");
                actual.Should().Be(2);
            }
        }
    }
}