using System;
using System.Net.Http;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class HyperlinkFactoryTests {
        private readonly IApplicationUriResolver _applicationUriResolver;
        private readonly HyperlinkFactory _sut;

        public HyperlinkFactoryTests() {
            FakeFactory.Create(out _applicationUriResolver);
            _sut = new HyperlinkFactory(_applicationUriResolver);
        }


        public class Create : HyperlinkFactoryTests {
            private readonly Uri _applicationUri;

            public Create() {
                _applicationUri = new Uri("http://www.unittest.com/application/", UriKind.Absolute);
                A.CallTo(() => _applicationUriResolver.Resolve()).Returns(_applicationUri);
            }

            [Fact]
            public void GivenRelTypeNotAnEnum_Throws() {
                Assert.Throws<ArgumentException>(() => _sut.Create(HttpMethod.Get, "somelink", 2));
            }

            [Fact]
            public void GivenRelValueUndefined_Throws() {
                Assert.Throws<ArgumentOutOfRangeException>(() => _sut.Create(HttpMethod.Get, "somelink", (FakeRel) (-1)));
            }

            [Fact]
            public void GivenEmptyRelativeUri_ReturnsBaseUriAsHref() {
                var actual = _sut.Create(HttpMethod.Get, string.Empty, FakeRel.Rel2);
                var expected = _applicationUri.ToString();
                actual.Href.Should().Be(expected);
            }

            [Fact]
            public void PrependsApplicationUrlToHref() {
                var actual = _sut.Create(HttpMethod.Get, "somelink/id", FakeRel.Rel2);
                var expected = $"{_applicationUri}somelink/id";
                actual.Href.Should().Be(expected);
            }

            [Fact]
            public void StripsLastSlashFromUrl() {
                var actual = _sut.Create(HttpMethod.Get, "somelink/id/", FakeRel.Rel2);
                var expected = $"{_applicationUri}somelink/id";
                actual.Href.Should().Be(expected);
            }

            [Fact]
            public void CreatesHyperlinkWithSpecifiedMethod() {
                var actual = _sut.Create(HttpMethod.Delete, "somelink/id", FakeRel.Rel2);
                actual.Method.Should().Be(HttpMethod.Delete.Method);
            }

            [Fact]
            public void CreatesHyperlinkWithSpecifiedRel() {
                var actual = _sut.Create(HttpMethod.Delete, "somelink/id", FakeRel.Rel2);
                actual.Rel.Should().Be(FakeRel.Rel2);
            }

            private enum FakeRel {
                Rel1,
                Rel2
            }
        }
    }
}