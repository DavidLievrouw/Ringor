using System.Reflection;
using Dalion.Ringor.Api.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Dalion.Ringor.Api.Services {
    public class ApplicationInfoProviderTests {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Assembly _entryAssembly;
        private readonly ApplicationInfoProvider _sut;

        public ApplicationInfoProviderTests() {
            _entryAssembly = typeof(Program).Assembly;
            FakeFactory.Create(out _httpContextAccessor);
            _sut = new ApplicationInfoProvider(_httpContextAccessor, _entryAssembly);
        }

        public class Provide : ApplicationInfoProviderTests {
            private readonly HttpRequest _httpRequest;

            public Provide() {
                var httpContext = new DefaultHttpContext();
                A.CallTo(() => _httpContextAccessor.HttpContext)
                    .Returns(httpContext);
                _httpRequest = httpContext.Request;
            }

            [Fact]
            public void ReportsExpectedVersion() {
                var actual = _sut.Provide();
                var expectedVersion = typeof(Program).Assembly.GetName().Version.ToString(3);
                actual.Version.Should().Be(expectedVersion);
            }

            [Fact]
            public void ReportsExpectedCompany() {
                var actual = _sut.Provide();
                actual.Company.Should().Be("Dalion");
            }

            [Fact]
            public void ReportsExpectedProduct() {
                var actual = _sut.Provide();
                actual.Product.Should().Be("Ringor");
            }

            [Fact]
            public void ReportsExpectedUrlInfo() {
                _httpRequest.Scheme = "https";
                _httpRequest.Host = new HostString("recomatics.com:12345");
                _httpRequest.PathBase = new PathString("/api");
                var actual = _sut.Provide();
                var expectedUrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                    AppUrl = "/api",
                    SiteUrl = "https://recomatics.com:12345"
                };
                actual.UrlInfo.Should().BeEquivalentTo(expectedUrlInfo);
            }

            [Fact]
            public void GivenNoPathBase_ReportsExpectedUrlInfo() {
                _httpRequest.Scheme = "https";
                _httpRequest.Host = new HostString("recomatics.com:12345");
                _httpRequest.PathBase = null;
                var actual = _sut.Provide();
                var expectedUrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                    AppUrl = "",
                    SiteUrl = "https://recomatics.com:12345"
                };
                actual.UrlInfo.Should().BeEquivalentTo(expectedUrlInfo);
            }
        }
    }
}