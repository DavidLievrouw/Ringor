using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class DefaultIf : ExtensionsTests {
            [Fact]
            public void GivenValueIsDefault_ReturnsReplacement() {
                string value = null;
                var actual = value.DefaultIf();
                actual.Should().BeNull();
            }

            [Fact]
            public void GivenValueIsCustomDefault_ReturnsReplacement() {
                const string customDefault = "myCustomDefault";
                const string value = "myCustomDefault";
                var actual = value.DefaultIf(customDefault);
                actual.Should().BeNull();
            }

            [Fact]
            public void GivenValueIsCustomDefault_AndCustomReplacementIsGiven_ReturnsCustomReplacement() {
                const string customDefault = "myCustomDefault";
                const string customReplacement = "theReplacement";
                const string value = "myCustomDefault";
                var actual = value.DefaultIf(customDefault, customReplacement);
                actual.Should().Be(customReplacement);
            }

            [Fact]
            public void GivenValueIsNotDefault_ReturnsGivenValue() {
                const string value = "myValue";
                var actual = value.DefaultIf();
                actual.Should().Be(value);
            }

            [Fact]
            public void GivenValueIsNotDefault_AndCustomDefaultIsGiven_ReturnsGivenValue() {
                const string customDefault = "myCustomDefault";
                const string customReplacement = "theReplacement";
                const string value = "myValue";
                var actual = value.DefaultIf(customDefault, customReplacement);
                actual.Should().Be(value);
            }
        }
    }
}