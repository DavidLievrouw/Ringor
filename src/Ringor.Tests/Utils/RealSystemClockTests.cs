using System;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public class RealSystemClockTests {
        private readonly ILocalTimeConverter _localTimeConverter;
        private readonly RealSystemClock _sut;

        public RealSystemClockTests() {
            _localTimeConverter = _localTimeConverter.Fake();
            _sut = new RealSystemClock(_localTimeConverter);
        }


        public class UtcNow : RealSystemClockTests {
            // Not testable
        }

        public class LocalNow : RealSystemClockTests {
            [Fact]
            public void ReturnsConvertedUtcNow() {
                var localDateTime = new DateTime(2017, 8, 11, 13, 46, 24, DateTimeKind.Local);
                A.CallTo(() => _localTimeConverter.ToLocalDateTime(A<DateTimeOffset>.That.Matches(_ => _.Offset == TimeSpan.Zero))).Returns(localDateTime);
                var actual = _sut.LocalNow;
                actual.Should().Be(localDateTime);
            }
        }
    }
}