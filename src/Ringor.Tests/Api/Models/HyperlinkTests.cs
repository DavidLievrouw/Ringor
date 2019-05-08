using System;
using System.Net.Http;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models {
    public class HyperlinkTests {
        public class Construction : HyperlinkTests {
            [Fact]
            public void GivenRelTypeNotAnEnum_Throws() {
                Assert.Throws<ArgumentException>(() => new Hyperlink<int>(HttpMethod.Get, "somelink", 1));
            }

            [Fact]
            public void GivenRelValueUndefined_Throws() {
                Assert.Throws<ArgumentOutOfRangeException>(() => new Hyperlink<DummyRel>(HttpMethod.Get, "somelink", (DummyRel) (-1)));
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData(" \t ")]
            public void GivenNullEmptyOrWhiteSpaceHref_SetsHrefToNull(string href) {
                var actual = new Hyperlink<DummyRel>(HttpMethod.Get, href, DummyRel.DummyType1);
                actual.Href.Should().BeNull();
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData(" \t ")]
            public void GivenNullEmptyOrWhiteSpaceHref_SetsRelToDefault(string href) {
                var actual = new Hyperlink<DummyRel>(HttpMethod.Get, href, DummyRel.DummyType1);
                actual.Rel.Should().Be(default(DummyRel));
            }

            [Fact]
            public void CanHandleCreationWithHttpMethod() {
                var actual = new Hyperlink<DummyRel>(HttpMethod.Options, "/api/document", DummyRel.DummyType2);
                actual.Method.Should().Be(HttpMethod.Options.Method);
                actual.Rel.Should().Be(DummyRel.DummyType2);
                actual.Href.Should().Be("/api/document2");
            }

            [Fact]
            public void CanHandleCreationWithUncommonHttpMethod() {
                var actual = new Hyperlink<DummyRel>(new HttpMethod("UNCOMMONMETHOD"), "/api/document", DummyRel.DummyType2);
                actual.Method.Should().Be("UNCOMMONMETHOD");
                actual.Rel.Should().Be(DummyRel.DummyType2);
                actual.Href.Should().Be("/api/document");
            }
        }

        public class Equality : HyperlinkTests {
            private readonly Hyperlink<DummyRel> _sut;

            public Equality() {
                _sut = new Hyperlink<DummyRel>(HttpMethod.Get, "/api/document", DummyRel.DummyType1);
            }

            [Fact]
            public void NotEqualToNull() {
                _sut.Equals(null).Should().BeFalse();
                (_sut != null).Should().BeTrue();
                (_sut == null).Should().BeFalse();
            }

            [Fact]
            public void ObjectOfOtherRelType_NotEqual() {
                var other = new Hyperlink<OtherDummyRel>(HttpMethod.Get, "/api/document", OtherDummyRel.DummyType1);
                _sut.Equals(other).Should().BeFalse();
            }

            [Fact]
            public void ObjectWithSameRelButDifferentHref_IsEqual() {
                var other = new Hyperlink<DummyRel>(HttpMethod.Get, "/api/otherdocument", _sut.Rel);
                _sut.Equals(other).Should().BeTrue();
                _sut.GetHashCode().Should().Be(other.GetHashCode());
                (_sut == other).Should().BeTrue();
                (_sut != other).Should().BeFalse();
            }

            [Fact]
            public void ObjectWithOtherRelButSameHref_IsNotEqual() {
                var other = new Hyperlink<DummyRel>(HttpMethod.Get, _sut.Href, DummyRel.DummyType2);
                _sut.Equals(other).Should().BeFalse();
                (_sut != other).Should().BeTrue();
                (_sut == other).Should().BeFalse();
            }

            [Fact]
            public void ObjectWithOtherRelAndOtherHref_IsNotEqual() {
                var other = new Hyperlink<DummyRel>(HttpMethod.Get, "/api/otherdocument", DummyRel.DummyType2);
                _sut.Equals(other).Should().BeFalse();
                (_sut != other).Should().BeTrue();
                (_sut == other).Should().BeFalse();
            }

            [Fact]
            public void ObjectWithOtherMethodButSameRel_IsEqual() {
                var other = new Hyperlink<DummyRel>(HttpMethod.Delete, "/api/document", DummyRel.DummyType1);
                _sut.Equals(other).Should().BeTrue();
                _sut.GetHashCode().Should().Be(other.GetHashCode());
                (_sut == other).Should().BeTrue();
                (_sut != other).Should().BeFalse();
            }
        }

        private enum DummyRel {
            DummyType1,
            DummyType2
        }

        private enum OtherDummyRel {
            DummyType1
        }
    }
}