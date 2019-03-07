using System;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class IsNullOrDefault : ExtensionsTests {
            [Fact]
            public void ReturnsTrueForNull() {
                object nullObj = null;
                var actual = nullObj.IsNullOrDefault();
                actual.Should().BeTrue();
            }

            [Fact]
            public void ReturnsTrueForNullClasses() {
                Exception nullClass = null;
                var actual = nullClass.IsNullOrDefault();
                actual.Should().BeTrue();
            }

            [Fact]
            public void ReturnsTrueForNullNullableTypes() {
                DateTime? nullableType = null;
                var actual = nullableType.IsNullOrDefault();
                actual.Should().BeTrue();
            }

            [Fact]
            public void ReturnsTrueForNullStrings() {
                string nullString = null;
                var actual = nullString.IsNullOrDefault();
                actual.Should().BeTrue();
            }

            [Fact]
            public void ReturnsTrueForDefaultBoolean() {
                const bool defaultBool = false;
                var actual = defaultBool.IsNullOrDefault();
                actual.Should().BeTrue();
            }

            [Theory]
            [InlineData(0.0)]
            public void ReturnsTrueForDefaultPrimitiveNumber(double primitive) {
                var actual = primitive.IsNullOrDefault();
                actual.Should().BeTrue();
            }

            [Fact]
            public void ReturnsTrueForDefaultStructs() {
                var defaultStruct = default(DateTimeOffset);
                var actual = defaultStruct.IsNullOrDefault();
                actual.Should().BeTrue();
            }

            [Fact]
            public void ReturnsFalseForNotNull() {
                var actual = new object().IsNullOrDefault();
                actual.Should().BeFalse();
            }

            [Fact]
            public void ReturnsFalseForNotNullClasses() {
                var anInstance = new Exception("An error");
                var actual = anInstance.IsNullOrDefault();
                actual.Should().BeFalse();
            }

            [Fact]
            public void ReturnsFalseForNotNullNullableTypes() {
                DateTime? nullableType = new DateTime(2015, 3, 18);
                var actual = nullableType.IsNullOrDefault();
                actual.Should().BeFalse();
            }

            [Fact]
            public void ReturnsFalseForNotNullStrings() {
                const string nullString = "A string";
                var actual = nullString.IsNullOrDefault();
                actual.Should().BeFalse();
            }

            [Fact]
            public void ReturnsFalseForNotDefaultBoolean() {
                const bool nonDefaultBool = true;
                var actual = nonDefaultBool.IsNullOrDefault();
                actual.Should().BeFalse();
            }

            [Theory]
            [InlineData(2.56)]
            public void ReturnsFalseForNotDefaultPrimitiveNumbers(double primitive) {
                var actual = primitive.IsNullOrDefault();
                actual.Should().BeFalse();
            }

            [Fact]
            public void ReturnsFalseForNotDefaultStructs() {
                var defaultStruct = new DateTimeOffset(DateTime.Now);
                var actual = defaultStruct.IsNullOrDefault();
                actual.Should().BeFalse();
            }
        }
    }
}