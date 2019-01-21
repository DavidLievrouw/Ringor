using System;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api {
    public partial class ExtensionsTests {
        public class WithStringRelativePath : ExtensionsTests {
            private Uri _baseUri;
            private string _relativePath;

            public WithStringRelativePath() {
                _baseUri = new Uri("http://www.recomatics.com/api", UriKind.Absolute);
                _relativePath = "documents/getall";
            }

            [Fact]
            public void GivenBaseUriIsNull_Throws() {
                Uri nullUri = null;
                Assert.Throws<ArgumentNullException>(() => nullUri.WithRelativePath(_relativePath));
            }

            [Theory]
            [InlineData(UriKind.Relative)]
            [InlineData(UriKind.RelativeOrAbsolute)]
            public void GivenBaseUriIsNotAbsolute_Throws(UriKind uriKind) {
                var relativeBaseUri = new Uri("api", uriKind);
                Assert.Throws<InvalidOperationException>(() => relativeBaseUri.WithRelativePath(_relativePath));
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            public void GivenRelativePathIsNullOrEmpty_ReturnsBaseUri(string nullOrEmptyRelativePath) {
                var actual = _baseUri.WithRelativePath(nullOrEmptyRelativePath);
                var expected = new Uri(_baseUri.OriginalString + "/", UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenBaseUriHasMissingSlash_AddsSlashBeforeRelativePath() {
                var actual = _baseUri.WithRelativePath(_relativePath);
                var expected = new Uri(_baseUri.OriginalString + "/" + _relativePath, UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenBaseUriEndsWithSlash_JustAppendsRelativePath() {
                _baseUri = new Uri(_baseUri.OriginalString + "/", UriKind.Absolute);

                var actual = _baseUri.WithRelativePath(_relativePath);

                var expected = new Uri(_baseUri.OriginalString + _relativePath, UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenRelativePathStartsWithSlash_RemovesThatSlash() {
                var expectedRelativePathString = _relativePath;
                _relativePath = "/" + _relativePath;
                var actual = _baseUri.WithRelativePath(_relativePath);
                var expected = new Uri(_baseUri.OriginalString + "/" + expectedRelativePathString, UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        public class WithUriRelativePath : ExtensionsTests {
            private Uri _baseUri;
            private Uri _relativePath;

            public WithUriRelativePath() {
                _baseUri = new Uri("http://www.recomatics.com/api", UriKind.Absolute);
                _relativePath = new Uri("documents/getall", UriKind.Relative);
            }

            [Fact]
            public void GivenBaseUriIsNull_Throws() {
                Uri nullUri = null;
                Assert.Throws<ArgumentNullException>(() => nullUri.WithRelativePath(_relativePath));
            }

            [Theory]
            [InlineData(UriKind.Relative)]
            [InlineData(UriKind.RelativeOrAbsolute)]
            public void GivenBaseUriIsNotAbsolute_Throws(UriKind uriKind) {
                var relativeBaseUri = new Uri("api", uriKind);
                Assert.Throws<InvalidOperationException>(() => relativeBaseUri.WithRelativePath(_relativePath));
            }

            [Fact]
            public void GivenRelativePathIsAbsolute_Throws() {
                var absoluteUri = new Uri("http://www.google.com/", UriKind.Absolute);
                Assert.Throws<InvalidOperationException>(() => _baseUri.WithRelativePath(absoluteUri));
            }

            [Fact]
            public void GivenRelativePathIsNull_ReturnsBaseUri() {
                var actual = _baseUri.WithRelativePath((Uri) null);
                var expected = new Uri(_baseUri.OriginalString + "/", UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenBaseUriHasMissingSlash_AddsSlashBeforeRelativePath() {
                var actual = _baseUri.WithRelativePath(_relativePath);
                var expected = new Uri(_baseUri.OriginalString + "/" + _relativePath, UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenBaseUriEndsWithSlash_JustAppendsRelativePath() {
                _baseUri = new Uri(_baseUri.OriginalString + "/", UriKind.Absolute);

                var actual = _baseUri.WithRelativePath(_relativePath);

                var expected = new Uri(_baseUri.OriginalString + _relativePath.OriginalString, UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GivenRelativePathStartsWithSlash_RemovesThatSlash() {
                var expectedRelativePathString = _relativePath.OriginalString;
                _relativePath = new Uri("/" + _relativePath.OriginalString, UriKind.Relative);
                var actual = _baseUri.WithRelativePath(_relativePath);
                var expected = new Uri(_baseUri.OriginalString + "/" + expectedRelativePathString, UriKind.Absolute);
                actual.Should().BeEquivalentTo(expected);
            }
        }
    }
}