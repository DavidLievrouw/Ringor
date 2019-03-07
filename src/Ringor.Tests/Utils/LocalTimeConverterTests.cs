using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public class LocalTimeConverterTests {
        public class Construction : LocalTimeConverterTests {
            [Fact]
            public void AcceptsNegativeOffsets() {
                Action act = () => new LocalTimeConverter(TimeSpan.FromHours(-1));
                act.Should().NotThrow();
            }
        }

        public class ToLocalDateTime : LocalTimeConverterTests {
            private LocalTimeConverter Construct(TimeSpan currentTimeZoneUtcOffset) {
                return new LocalTimeConverter(currentTimeZoneUtcOffset);
            }

            [Theory]
            [MemberData(nameof(TestCases.TestData), MemberType = typeof(TestCases))]
            public void ConvertsToExpectedValue(string description, TimeSpan currentTimeZoneUtcOffset, DateTimeOffset input, DateTime expectedOutput) {
                var sut = Construct(currentTimeZoneUtcOffset);

                var actual = sut.ToLocalDateTime(input);
                actual.Should().Be(expectedOutput, description);
                actual.Kind.Should().Be(expectedOutput.Kind);
            }

            public static class TestCases {
                private static readonly List<object[]> _data = new List<object[]> {
                    new object[] {
                        "Return the same as input if we are in UTC zone",
                        TimeSpan.FromHours(0),
                        new DateTimeOffset(2017, 8, 11, 12, 18, 24, TimeSpan.FromHours(0)),
                        new DateTime(2017, 8, 11, 12, 18, 24, DateTimeKind.Local)
                    },
                    new object[] {
                        "Return the same as input if we are in the same non-UTC zone",
                        TimeSpan.FromHours(2),
                        new DateTimeOffset(2017, 8, 11, 12, 18, 24, TimeSpan.FromHours(2)),
                        new DateTime(2017, 8, 11, 12, 18, 24, DateTimeKind.Local)
                    },
                    new object[] {
                        "Return the same as input if we are in the same weird non-UTC zone",
                        TimeSpan.FromMinutes(-45),
                        new DateTimeOffset(2017, 8, 11, 0, 3, 24, TimeSpan.FromMinutes(-45)),
                        new DateTime(2017, 8, 11, 0, 3, 24, DateTimeKind.Local)
                    },
                    new object[] {
                        "Return the expected value if we are in UTC zone, but the input is not",
                        TimeSpan.FromHours(0),
                        new DateTimeOffset(2017, 8, 11, 1, 18, 24, TimeSpan.FromHours(2)),
                        new DateTime(2017, 8, 10, 23, 18, 24, DateTimeKind.Local)
                    },
                    new object[] {
                        "Return the expected value if we are not in UTC zone, but the input is",
                        TimeSpan.FromHours(2),
                        new DateTimeOffset(2017, 8, 10, 23, 18, 24, TimeSpan.FromHours(0)),
                        new DateTime(2017, 8, 11, 1, 18, 24, DateTimeKind.Local)
                    },
                    new object[] {
                        "Return the expected value if we are in UTC zone, but the input is in another weird time zone",
                        TimeSpan.FromMinutes(0),
                        new DateTimeOffset(2017, 8, 11, 0, 3, 24, TimeSpan.FromMinutes(-45)),
                        new DateTime(2017, 8, 11, 0, 48, 24, DateTimeKind.Local)
                    },
                    new object[] {
                        "Return the expected value if we are in a weird non-UTC zone, but the input is a UTC zone",
                        TimeSpan.FromMinutes(-45),
                        new DateTimeOffset(2017, 8, 11, 0, 3, 24, TimeSpan.FromMinutes(0)),
                        new DateTime(2017, 8, 10, 23, 18, 24, DateTimeKind.Local)
                    }
                };

                public static IEnumerable<object[]> TestData => _data;
            }
        }
    }
}