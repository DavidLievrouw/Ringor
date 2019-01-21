using System;
using Dalion.Ringor.Api.Services;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationUriResolverTests {
        private readonly IApplicationInfoProvider _applicationInfoProvider;
        private readonly ApplicationUriResolver _sut;

        public ApplicationUriResolverTests() {
            FakeFactory.Create(out _applicationInfoProvider);
            _sut = new ApplicationUriResolver(_applicationInfoProvider);
        }

        public class Resolve : ApplicationUriResolverTests {
            private readonly ApplicationInfo _applicationInfo;

            public Resolve() {
                _applicationInfo = new ApplicationInfo {
                    UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                        SiteUrl = "https://tests.dalion.eu:8080",
                        AppUrl = "/api"
                    }
                };
                A.CallTo(() => _applicationInfoProvider.Provide())
                    .Returns(_applicationInfo);
            }

            [Fact]
            public void ReturnsExpectedApplicationUrl() {
                var actual = _sut.Resolve();

                var expectedUri = new Uri("https://tests.dalion.eu:8080/api/", UriKind.Absolute);
                actual.Should().BeEquivalentTo(expectedUri);
            }

            [Fact]
            public void GivenSiteUrlThatEndsWithSlash_ReturnsExpectedApplicationUrl() {
                _applicationInfo.UrlInfo.SiteUrl = "https://tests.dalion.eu:8080/";

                var actual = _sut.Resolve();

                var expectedUri = new Uri("https://tests.dalion.eu:8080/api/", UriKind.Absolute);
                actual.Should().BeEquivalentTo(expectedUri);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("/")]
            public void GivenNoAppUrl_ReturnsExpectedApplicationUrl(string noAppUrl) {
                _applicationInfo.UrlInfo.AppUrl = noAppUrl;

                var actual = _sut.Resolve();

                var expectedUri = new Uri("https://tests.dalion.eu:8080/", UriKind.Absolute);
                actual.Should().BeEquivalentTo(expectedUri);
            }
        }
    }
}