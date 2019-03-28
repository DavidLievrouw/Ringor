using System;
using System.Reflection;
using Dalion.Ringor.Api.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Dalion.Ringor.Api.Services {
    public class ApplicationInfoProviderTests {
        private readonly ImplicitFlowAuthenticationSettings _authenticationSettings;
        private readonly Assembly _entryAssembly;
        private readonly string _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationInfoProvider _sut;

        public ApplicationInfoProviderTests() {
            _entryAssembly = typeof(Program).Assembly;
            _environment = "UnitTests";
            _authenticationSettings = new ImplicitFlowAuthenticationSettings {
                Authority = new Uri("https://ringor.eu/auth"),
                Tenant = "M2019Tests",
                ClientId = "theClientId",
                Scopes = new[] {
                    "scope1",
                    "scope2"
                }
            };
            FakeFactory.Create(out _httpContextAccessor);
            _sut = new ApplicationInfoProvider(_httpContextAccessor, _entryAssembly, _environment, _authenticationSettings);
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
            public void ReportsExpectedEmail() {
                var actual = _sut.Provide();
                actual.Email.Should().Be("info@dalion.eu");
            }

            [Fact]
            public void ReportsExpectedEnvironment() {
                var actual = _sut.Provide();
                actual.Environment.Should().Be(_environment);
            }

            [Fact]
            public void ReportsExpectedAuthenticationInfo() {
                var actual = _sut.Provide();
                var expectedAuthenticationInfo = new ApplicationInfo.ApplicationAuthenticationInfo {
                    Authority = new Uri("https://ringor.eu/auth"),
                    Tenant = "M2019Tests",
                    ClientId = "theClientId",
                    Scopes = new[] {
                        "scope1",
                        "scope2"
                    }
                };
                actual.AuthenticationInfo.Should().BeEquivalentTo(expectedAuthenticationInfo);
            }

            [Fact]
            public void ReportsExpectedUrlInfo() {
                _httpRequest.Scheme = "https";
                _httpRequest.Host = new HostString("dalion.eu:12345");
                _httpRequest.PathBase = new PathString("/api");
                var actual = _sut.Provide();
                var expectedUrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                    AppUrl = "/api",
                    SiteUrl = "https://dalion.eu:12345"
                };
                actual.UrlInfo.Should().BeEquivalentTo(expectedUrlInfo);
            }

            [Fact]
            public void GivenNoPathBase_ReportsExpectedUrlInfo() {
                _httpRequest.Scheme = "https";
                _httpRequest.Host = new HostString("dalion.eu:12345");
                _httpRequest.PathBase = null;
                var actual = _sut.Provide();
                var expectedUrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                    AppUrl = "",
                    SiteUrl = "https://dalion.eu:12345"
                };
                actual.UrlInfo.Should().BeEquivalentTo(expectedUrlInfo);
            }
        }
    }
}